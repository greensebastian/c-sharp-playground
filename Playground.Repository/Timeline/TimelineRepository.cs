using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Playground.Models.Timeline;
using Playground.Models.Timeline.Data;
using Playground.Models.User;
using Playground.Repository.Data;

namespace Playground.Repository.Timeline
{
    public class TimelineRepository
    {
        private readonly PlaygroundDatabaseContext _dbContext;

        public TimelineRepository(PlaygroundDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTimelineData(IEnumerable<Timelineobject> objects, PlaygroundUser sessionUser)
        {
            // Add user to tracked entities in database context by finding from id
            var user = _dbContext.Users.Find(sessionUser.Id);

            // Find or create timeline data link
            var timelineData = user.TimelineData;
            if (timelineData == null)
            {
                timelineData = new TimelineData();
                timelineData.ActivitySegments = new List<DbActivitySegment>();
                timelineData.PlaceVisits = new List<DbPlaceVisit>();
                user.TimelineData = timelineData;
            }

            // Retrieve activity segments and place visits already stored in the user
            var activitySegmentHashes = timelineData.ActivitySegments.Select(segment => segment.Hash) ?? new int[0];
            var placeVisitHashes = timelineData.PlaceVisits.Select(visit => visit.Hash) ?? new int[0];

            // Find and process any new visits or segments not already in the database
            var processor = new TimelineProcessor(_dbContext.Locations, activitySegmentHashes, placeVisitHashes);
            var (newPlaceVisits, newActivitySegments) = processor.Process(objects);

            // Add to users list of items and save to database
            timelineData.ActivitySegments.AddRange(newActivitySegments);
            timelineData.PlaceVisits.AddRange(newPlaceVisits);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveTimelineData(PlaygroundUser sessionUser)
        {
            // Add user to tracked entities in database context by finding from id
            var user = _dbContext.Users.Find(sessionUser.Id);

            var timelineData = user?.TimelineData;
            if (timelineData == null) return;

            foreach (var visit in timelineData.PlaceVisits) RemovePlaceVisit(visit);
            foreach (var segment in timelineData.ActivitySegments) RemoveActivitySegment(segment);
            _dbContext.TimelineData.Remove(timelineData);
            await _dbContext.SaveChangesAsync();
        }

        private void RemovePlaceVisit(DbPlaceVisit placeVisit)
        {
            if (placeVisit == null) return;
            if (placeVisit.ChildVisits != null && placeVisit.ChildVisits.Count > 0)
            {
                foreach (var childVisit in placeVisit.ChildVisits) RemovePlaceVisit(childVisit);
            }
            _dbContext.PlaceVisits.Remove(placeVisit);
            _dbContext.LocationVisits.Remove(placeVisit.LocationVisit);
        }

        private void RemoveActivitySegment(DbActivitySegment activitySegment)
        {
            if (activitySegment == null) return;
            TryRemoveWaypoint(activitySegment.StartWaypoint);
            TryRemoveWaypoint(activitySegment.EndWaypoint);
            TryRemoveWaypoints(activitySegment.Waypoints);
            TryRemoveLocationVisits(activitySegment.TransitLocationVisits);
            _dbContext.ActivitySegments.Remove(activitySegment);
        }

        private void TryRemoveWaypoints(IEnumerable<DbWaypoint> waypoints)
        {
            if (waypoints == null) return;
            _dbContext.Waypoints.RemoveRange(waypoints);
        }

        private void TryRemoveWaypoint(DbWaypoint waypoint)
        {
            if (waypoint == null) return;
            _dbContext.Waypoints.Remove(waypoint);
        }

        private void TryRemoveLocationVisits(IEnumerable<DbLocationVisit> locationVisits)
        {
            if (locationVisits == null) return;
            _dbContext.LocationVisits.RemoveRange(locationVisits);
        }
    }
}
