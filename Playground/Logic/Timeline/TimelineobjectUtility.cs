using Playground.Models.Timeline;

namespace Playground.Logic.Timeline
{
    public static class TimelineobjectUtility
    {
        public static bool IsActivitySegment(Timelineobject timelineObject)
        {
            return timelineObject.activitySegment != null;
        }

        public static bool IsPlaceVisit(Timelineobject timelineObject)
        {
            return timelineObject.placeVisit != null;
        }

        /// <summary>
        /// Retrieve the EventType of the timeline object
        /// </summary>
        public static EventType GetEventType(Timelineobject timelineObject)
        {
            var typeSum = 0;
            if (IsActivitySegment(timelineObject))
            {
                typeSum += (int)EventType.ActivitySegment;
            }
            if (IsPlaceVisit(timelineObject))
            {
                typeSum += (int)EventType.PlaceVisit;
            }
            return (EventType)typeSum;
        }

        public enum EventType
        {
            Unknown = 0,
            ActivitySegment = 1,
            PlaceVisit = 2,
            Both = 3
        }
    }

    public enum SemanticType
    {
        TYPE_HOME = 0,
        TYPE_WORK = 1
    }
}
