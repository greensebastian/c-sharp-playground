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
        private readonly PlaygroundDatabaseContext _dbContext;

        public TimelineRepository(PlaygroundDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddTimelineData(IEnumerable<Timelineobject> objects, PlaygroundUser user)
        {
            var timelineData = user.TimelineData;
            if (timelineData == null)
            {
                timelineData = new TimelineData();
                timelineData.ActivitySegments = new List<DbActivitySegment>();
                timelineData.PlaceVisits = new List<DbPlaceVisit>();
                user.TimelineData = timelineData;
            }

            var activitySegmentHashes = timelineData.ActivitySegments.Select(segment => segment.Hash) ?? new int[0];
            var placeVisitHashes = timelineData.PlaceVisits.Select(visit => visit.Hash) ?? new int[0];

            var processor = new TimelineProcessor(_dbContext.Locations, activitySegmentHashes, placeVisitHashes);
            var (newPlaceVisits, newActivitySegments) = processor.Process(objects);

            timelineData.ActivitySegments.AddRange(newActivitySegments);
            timelineData.PlaceVisits.AddRange(newPlaceVisits);

            await _dbContext.SaveChangesAsync();
        }

        
    }
}
