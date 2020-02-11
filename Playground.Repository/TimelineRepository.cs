using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Playground.Models.Timeline;
using Playground.Models.Timeline.Data;
using Playground.Models.User;
using Playground.Repository.Data;

namespace Playground.Repository
{
    public class TimelineRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly User _user;

        public TimelineRepository(DatabaseContext dbContext, User user)
        {
            _dbContext = dbContext;
            _user = user;
        }

        public async Task AddTimelineData(IEnumerable<Timelineobject> objects, User user)
        {
            var userData = await _dbContext.Users.FindAsync(user.Id);
            if (userData.TimelineData == null) userData.TimelineData = new TimelineData();
            var timelineData = userData.TimelineData;
            await _dbContext.SaveChangesAsync();
            foreach (var timelineObject in objects)
            {
                if (timelineObject.activitySegment != null)
                    await AddActivitySegment(timelineObject.activitySegment, timelineData);
                if (timelineObject.placeVisit != null)
                    await AddPlaceVisit(timelineObject.placeVisit, timelineData);
            }
        }

        private async Task AddActivitySegment(Activitysegment segment, TimelineData timelineData)
        {
            var hash = ActivitySegmentHash(segment);
            var dbSegment = _dbContext.ActivitySegments.FirstOrDefault(dbSegment => dbSegment.Hash == hash);
            if (dbSegment == null)
            {
                var dbSegent = new DbActivitySegment
                {
                    Hash = hash,
                    ActivityType = segment.activityType,
                    Confidence = segment.confidence,
                    Distance = segment.distance,
                    EndDateTime = FromJavascriptMs(segment.duration.endTimestampMs),
                    StartDateTime = FromJavascriptMs(segment.duration.startTimestampMs),
                    StartWaypoint = GetWaypoint(segment.startLocation),
                    EndWaypoint = GetWaypoint(segment.endLocation),
                    Waypoints = segment.waypointPath?.waypoints.Select(GetWaypoint).ToList(),
                    TimelineData = timelineData,
                };
                dbSegent.TransitLocationVisits = segment.transitPath?.transitStops.Select(stop => GetLocationVisit(stop, dbSegent)).ToList();

                await _dbContext.ActivitySegments.AddAsync(dbSegent);
                await _dbContext.SaveChangesAsync();
            }
        }

        private DateTime FromJavascriptMs(long ms)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(ms / 1000).DateTime;
        }

        private DbLocationVisit GetLocationVisit(Location location, DbPlaceVisit placeVisit)
        {
            var dbLocation = FindLocation(location) ?? CreateAndAddLocation(location);

            if (string.IsNullOrEmpty(dbLocation.Address))
                dbLocation.Address = location.address;

            var dbLocationVisit = new DbLocationVisit
            {
                Location = dbLocation,
                PlaceVisit = placeVisit,
                SemanticType = location.semanticType
            };
            _dbContext.Add(dbLocationVisit);
            return dbLocationVisit;
        }

        private DbLocationVisit GetLocationVisit(Transitstop stop, DbActivitySegment activitySegment)
        {
            var proxyLocation = new Location
            {
                latitudeE7 = stop.latitudeE7,
                longitudeE7 = stop.longitudeE7,
                name = stop.name,
                placeId = stop.placeId
            };

            var dbLocation = FindLocation(proxyLocation) ?? CreateAndAddLocation(proxyLocation);

            var dbLocationVisit = new DbLocationVisit
            {
                Location = dbLocation,
                ActivitySegment = activitySegment
            };
            _dbContext.Add(dbLocationVisit);
            return dbLocationVisit;
        }

        private DbLocation FindLocation(Location location)
        {
            return _dbContext.Locations.FirstOrDefault(dbLocation => dbLocation.PlaceId == location.placeId);
        }

        private DbLocation CreateAndAddLocation(Location location)
        {
            var dbLocation = new DbLocation
            {
                LatitudeE7 = location.latitudeE7,
                LongitudeE7 = location.longitudeE7,
                Name = location.name,
                PlaceId = location.placeId
            };
            _dbContext.Locations.Add(dbLocation);
            return dbLocation;
        }

        private DbWaypoint GetWaypoint(Location location)
        {
            var point = new DbWaypoint
            {
                LatitudeE7 = location.latitudeE7,
                LongitudeE7 = location.longitudeE7
            };
            _dbContext.Add(point);
            return point;
        }

        private DbWaypoint GetWaypoint(Waypoint wayPoint)
        {
            var point = new DbWaypoint
            {
                LatitudeE7 = wayPoint.latE7,
                LongitudeE7 = wayPoint.lngE7
            };
            _dbContext.Add(point);
            return point;
        }

        private async Task AddPlaceVisit(Placevisit visit, TimelineData timelineData)
        {
            var hash = PlaceVisitHash(visit);
            var dbVisit = _dbContext.PlaceVisits.FirstOrDefault(dbVisit => dbVisit.Hash == hash);
            if (dbVisit == null)
            {
                dbVisit = new DbPlaceVisit
                {
                    CenterLatE7 = visit.centerLatE7,
                    CenterLngE7 = visit.centerLngE7,
                    Confidence = visit.placeConfidence,
                    EndDateTime = FromJavascriptMs(visit.duration.endTimestampMs),
                    StartDateTime = FromJavascriptMs(visit.duration.startTimestampMs),
                    TimelineData = timelineData
                };
                dbVisit.LocationVisit = GetLocationVisit(visit.location, dbVisit);
                dbVisit.ChildVisits = visit.childVisits?.Select(childVisit => MapChildVisit(childVisit)).ToList();

                await _dbContext.PlaceVisits.AddAsync(dbVisit);
                await _dbContext.SaveChangesAsync();
            }
        }

        private DbPlaceVisit MapChildVisit(Childvisit visit)
        {
            var dbVisit = new DbPlaceVisit
            {
                CenterLatE7 = visit.centerLatE7,
                CenterLngE7 = visit.centerLngE7,
                Confidence = visit.placeConfidence,
                EndDateTime = FromJavascriptMs(visit.duration.endTimestampMs),
                StartDateTime = FromJavascriptMs(visit.duration.startTimestampMs)
            };
            dbVisit.LocationVisit = GetLocationVisit(visit.location, dbVisit);

            _dbContext.PlaceVisits.AddAsync(dbVisit);
            _dbContext.SaveChangesAsync();
            return dbVisit;
        }

        private string PlaceVisitHash(Placevisit placeVisit)
        {
            return _user.UserName + placeVisit.duration.startTimestampMs.ToString() + placeVisit.duration.endTimestampMs.ToString();
        }

        private string ActivitySegmentHash(Activitysegment activitySegment)
        {
            return _user.UserName + activitySegment.duration.startTimestampMs.ToString() + activitySegment.duration.endTimestampMs.ToString();
        }
    }
}
