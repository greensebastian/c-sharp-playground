using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Playground.Extensions;
using Playground.Models.Timeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Playground.Logic.Timeline
{
    public class TimelineLogic
    {
        /// <summary>
        /// Create and empty sorted timeline set with a specific comparer
        /// </summary>
        /// <returns>An empty SortedSet</returns>
        public static SortedSet<Timelineobject> CreateTimelineSet()
        {
            return new SortedSet<Timelineobject>(new StartTimestampComparer());
        }

        /// <summary>
        /// Create a sorted set of timeline objects from a form file
        /// </summary>
        /// <param name="setToPopulate">The set to add data to</param>
        /// <param name="file">The file to extract data from</param>
        /// <returns>void Task</returns>
        public async static Task PopulateFromFile(SortedSet<Timelineobject> setToPopulate, IFormFile file)
        {
            // Validate request file somewhat
            var extension = Path.GetExtension(file.FileName);
            if (!extension.Contains("json", StringComparison.InvariantCultureIgnoreCase) && !extension.Contains("tmp", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"File format is not valid, expected JSON", "file");
            }
            if (file.Length <= 0)
            {
                return;
            }

            // Read, deserialize, and add resulting timeline objects to response model
            using (var stream = file.OpenReadStream())
            {
                var content = await DeserializeFromStream<SemanticTimeline>(stream);
                setToPopulate.UnionWith(content?.timelineObjects ?? new List<Timelineobject>());
            }
        }

        /// <summary>
        /// Builds a dictionary response set from the specified timeline objects
        /// </summary>
        /// <param name="timelineObjects">Input enumerable of timeline objects</param>
        /// <returns>Dictionary with response content</returns>
        public static Dictionary<string, object> CreateResponseContent(IEnumerable<Timelineobject> timelineObjects)
        {
            var response = new Dictionary<string, object>();

            response.Add("activitySegmentResults", Process(timelineObjects.Where(TimelineobjectUtility.IsActivitySegment).Select(timelineObject => timelineObject.activitySegment)));

            response.Add("placeVisitResults", Process(timelineObjects.Where(TimelineobjectUtility.IsPlaceVisit).Select(timelineObject => timelineObject.placeVisit)));

            return response;
        }

        /// <summary>
        /// Process a list of activity segments into a result object
        /// </summary>
        /// <param name="activitySegmentSet">List of activities to process</param>
        /// <returns>An object with result data</returns>
        private static object Process(IEnumerable<Activitysegment> activitySegmentSet)
        {
            // Means of travel by distance
            var distanceDistribution = new ValueSortedSet("m");
            // Means of travel by trip count
            var countDistribution = new ValueSortedSet("times");
            // Means of travel by time spent
            var timeDistribution = new ValueSortedSet("ms");

            foreach (var activitySegment in activitySegmentSet)
            {
                var name = activitySegment.activityType.WithoutUnderscores().ToTitleCase(true);
                distanceDistribution.Put(activitySegment.activityType, activitySegment.distance, name);
                countDistribution.Put(activitySegment.activityType, 1, name);
                timeDistribution.Put(activitySegment.activityType, (int)(activitySegment.duration.endTimestampMs - activitySegment.duration.startTimestampMs), name);
            }

            var results = new Dictionary<string, ValueSortedSet>();
            results.Add("Distance", distanceDistribution);
            results.Add("Count", countDistribution);
            results.Add("Time", timeDistribution);

            // Total distance travelled
            // Average time spent travelling
            // Average commute between work and home
            // Longest distance for one day

            foreach (var result in results)
            {
                result.Value.PostProcess();
            }

            return results;
        }

        /// <summary>
        /// Process a list of place visits into a result object
        /// </summary>
        /// <param name="placeVisitSet">List of place visits to process</param>
        /// <returns>An object with result data</returns>
        private static object Process(IEnumerable<Placevisit> placeVisitSet)
        {
            // Means of travel by trip count
            var countDistribution = new ValueSortedSet("times");
            // Means of travel by time spent
            var timeDistribution = new ValueSortedSet("ms");

            var homeDistribution = new ValueSortedSet("times");
            var workDistribution = new ValueSortedSet("times");
            var daysAtWork = new HashSet<DateTime>();

            string key;
            string name;
            string semanticTypeKey;
            foreach (var placeVisit in placeVisitSet)
            {
                var startDate = DateTimeOffset.FromUnixTimeMilliseconds(placeVisit.duration.startTimestampMs).UtcDateTime;
                var endDate = DateTimeOffset.FromUnixTimeMilliseconds(placeVisit.duration.endTimestampMs).UtcDateTime;
                key = placeVisit.location.placeId;
                name = placeVisit.location.name;
                countDistribution.Put(key, 1, name);
                timeDistribution.Put(key, (ulong)(placeVisit.duration.endTimestampMs - placeVisit.duration.startTimestampMs), name);

                // Figure out the semantic type of the visit, and append to counter
                semanticTypeKey = placeVisit.location.semanticType;
                if (semanticTypeKey != null)
                {
                    if (Enum.TryParse(semanticTypeKey, true, out SemanticType semanticType)) {
                        switch (semanticType)
                        {
                            case SemanticType.TYPE_WORK:
                                workDistribution.Put(key, 1, name);
                                foreach (var date in startDate.DaysToAndIncluding(endDate)) daysAtWork.Add(date);
                                break;
                            case SemanticType.TYPE_HOME:
                                homeDistribution.Put(key, 1, name);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            // Post-process any lists that need it
            var toProcess = new Dictionary<string, ValueSortedSet>();
            toProcess.Add("Home", homeDistribution);
            toProcess.Add("Work", workDistribution);
            toProcess.Add("Count", countDistribution);
            toProcess.Add("Time", timeDistribution);
            foreach (var result in toProcess)
            {
                result.Value.PostProcess();
            }

            // Most visited
            // Number of visits in total
            // "Known locations" home, work etc
            // Time spent at home/work vs other places
            // Unique number of places visited
            // Furthest away from home

            var home = homeDistribution.DataSet.FirstOrDefault();
            var work = workDistribution.DataSet.FirstOrDefault();
            var numberOfDaysAtWork = daysAtWork.Count();

            // Set up results to return
            var results = new Dictionary<string, object>();
            results.Add("Count", countDistribution);
            results.Add("Time", timeDistribution);
            results.Add("Home", home?.Name ?? "");
            results.Add("Work", work?.Name ?? "");
            results.Add("DaysAtWork", numberOfDaysAtWork);

            return results;
        }

        // Deserialize a json stream to a generic type
        private async static Task<T> DeserializeFromStream<T>(Stream stream) where T : class, new()
        {
            try
            {
                var serializer = new JsonSerializer();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return await Task.Run(() => serializer.Deserialize(jsonTextReader, typeof(T)) as T);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Comparer for parsing and sorting the relevant dates from the timeline objects
        private class StartTimestampComparer : IComparer<Timelineobject>
        {
            public int Compare([AllowNull] Timelineobject x, [AllowNull] Timelineobject y)
            {
                return (int)(ExtractDate(y) - ExtractDate(x));
            }

            private long ExtractDate(Timelineobject timelineObject)
            {
                if (TimelineobjectUtility.IsActivitySegment(timelineObject))
                {
                    return timelineObject.activitySegment.duration.startTimestampMs;
                }
                else
                {
                    return timelineObject.placeVisit.duration.startTimestampMs;
                }
            }
        }
    }
}
