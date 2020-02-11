using System.Linq;
using System.Collections.Generic;
using Playground.Models.Timeline;
using Playground.Models.Timeline.Data;
using System;
using System.Collections.Concurrent;

namespace Playground.Repository
{
    public class TimelineProcessor
    {
        private readonly ConcurrentDictionary<int, DbLocation> _dbLocations = new ConcurrentDictionary<int, DbLocation>();
        private readonly HashSet<int> _dbActivitySegments = new HashSet<int>();
        private readonly HashSet<int> _dbPlaceVisits = new HashSet<int>();

        public TimelineProcessor(IEnumerable<DbLocation> dbLocations, IEnumerable<int> dbActivitySegmentHashes, IEnumerable<int> dbPlaceVisitHashes)
        {
            foreach (var dbLocation in dbLocations) _dbLocations[dbLocation.GetHashCode()] = dbLocation;
            foreach (var dbActivitySegmentHash in dbActivitySegmentHashes) _dbActivitySegments.Add(dbActivitySegmentHash);
            foreach (var dbPlaceVisitHash in dbPlaceVisitHashes) _dbPlaceVisits.Add(dbPlaceVisitHash);
        }

        public (List<DbPlaceVisit>, List<DbActivitySegment>) Process(IEnumerable<Timelineobject> timelineObjects)
        {
            var placeVisits = new List<DbPlaceVisit>();
            var activitySegments = new List<DbActivitySegment>();

            foreach (var timelineObject in timelineObjects)
            {
                if (timelineObject.placeVisit != null)
                {
                    var placeVisit = GetPlaceVisit(timelineObject.placeVisit);
                    if (placeVisit != null) placeVisits.Add(placeVisit);
                }
                if (timelineObject.activitySegment != null)
                {
                    var activitySegment = GetActivitySegment(timelineObject.activitySegment);
                    if (activitySegment != null) activitySegments.Add(activitySegment);
                }
            }
            return (placeVisits, activitySegments);
        }

        private DbActivitySegment GetActivitySegment(Activitysegment segment)
        {
            var hashableObject = new DbActivitySegment
            {
                StartDateTime = FromJavascriptMs(segment.duration.startTimestampMs),
                EndDateTime = FromJavascriptMs(segment.duration.endTimestampMs)
            };
            var hash = hashableObject.GetHashCode();

            // Check if the activity segment already exists, otherwise register and create object
            if (_dbActivitySegments.Contains(hash)) return null;
            _dbActivitySegments.Add(hash);

            var dbSegment = new DbActivitySegment
            {
                Hash = hash,
                ActivityType = segment.activityType,
                Confidence = segment.confidence,
                Distance = segment.distance,
                EndDateTime = hashableObject.EndDateTime,
                StartDateTime = hashableObject.StartDateTime,
                StartWaypoint = GetWaypoint(segment.startLocation),
                EndWaypoint = GetWaypoint(segment.endLocation),
                Waypoints = segment.waypointPath?.waypoints.Select(GetWaypoint).ToList(),
                TransitLocationVisits = segment.transitPath?.transitStops.Select(GetLocationVisit).ToList()
            };
            return dbSegment;
        }

        private DbPlaceVisit GetPlaceVisit(Placevisit visit)
        {
            var hashableObject = new DbPlaceVisit
            {
                StartDateTime = FromJavascriptMs(visit.duration.startTimestampMs),
                EndDateTime = FromJavascriptMs(visit.duration.endTimestampMs)
            };
            var hash = hashableObject.GetHashCode();

            // Check if the activity segment already exists, otherwise register and create object
            if (_dbPlaceVisits.Contains(hash)) return null;
            _dbPlaceVisits.Add(hash);

            var dbVisit = new DbPlaceVisit
            {
                Hash = hash,
                CenterLatE7 = visit.centerLatE7,
                CenterLngE7 = visit.centerLngE7,
                Confidence = visit.placeConfidence,
                EndDateTime = FromJavascriptMs(visit.duration.endTimestampMs),
                StartDateTime = FromJavascriptMs(visit.duration.startTimestampMs),
                LocationVisit = GetLocationVisit(visit.location),
                ChildVisits = visit.childVisits?.Select(childVisit => MapChildVisit(childVisit)).ToList()
            };
            return dbVisit;
        }

        private DateTime FromJavascriptMs(long ms)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(ms / 1000).DateTime;
        }

        private DbLocationVisit GetLocationVisit(Location location)
        {
            var dbLocation = FindOrCreateLocation(location);

            if (string.IsNullOrEmpty(dbLocation.Address))
                dbLocation.Address = location.address;

            var dbLocationVisit = new DbLocationVisit
            {
                Location = dbLocation,
                SemanticType = location.semanticType
            };
            return dbLocationVisit;
        }

        private DbLocationVisit GetLocationVisit(Transitstop stop)
        {
            var proxyLocation = new Location
            {
                latitudeE7 = stop.latitudeE7,
                longitudeE7 = stop.longitudeE7,
                name = stop.name,
                placeId = stop.placeId
            };

            var dbLocation = FindOrCreateLocation(proxyLocation);

            var dbLocationVisit = new DbLocationVisit
            {
                Location = dbLocation
            };
            return dbLocationVisit;
        }

        private DbLocation FindOrCreateLocation(Location loc)
        {
            DbLocation dbLocation;
            var hash = loc.placeId?.GetHashCode();
            if (hash != null && hash.HasValue && _dbLocations.ContainsKey(hash.Value))
                dbLocation = _dbLocations[hash.Value];
            else
            {
                dbLocation = CreateLocation(loc);
                _dbLocations[dbLocation.GetHashCode()] = dbLocation;
            }
            
            return dbLocation;
        }

        private DbLocation CreateLocation(Location location)
        {
            var dbLocation = new DbLocation
            {
                LatitudeE7 = location.latitudeE7,
                LongitudeE7 = location.longitudeE7,
                Name = location.name,
                PlaceId = location.placeId
            };
            return dbLocation;
        }

        private DbWaypoint GetWaypoint(Location location)
        {
            var point = new DbWaypoint
            {
                LatitudeE7 = location.latitudeE7,
                LongitudeE7 = location.longitudeE7
            };
            return point;
        }

        private DbWaypoint GetWaypoint(Waypoint wayPoint)
        {
            var point = new DbWaypoint
            {
                LatitudeE7 = wayPoint.latE7,
                LongitudeE7 = wayPoint.lngE7
            };
            return point;
        }

        private DbPlaceVisit MapChildVisit(Childvisit visit)
        {
            var dbVisit = new DbPlaceVisit
            {
                CenterLatE7 = visit.centerLatE7,
                CenterLngE7 = visit.centerLngE7,
                Confidence = visit.placeConfidence,
                EndDateTime = FromJavascriptMs(visit.duration.endTimestampMs),
                StartDateTime = FromJavascriptMs(visit.duration.startTimestampMs),
                LocationVisit = GetLocationVisit(visit.location)
            };
            return dbVisit;
        }
    }
}
