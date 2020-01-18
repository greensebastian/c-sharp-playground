using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using c_sharp_playground.Models.Timeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace c_sharp_playground.Logic
{
    public class TimelineLogic
    {
        public static SortedSet<Timelineobject> CreateTimelineSet()
        {
            return new SortedSet<Timelineobject>(new TimelineobjectComparer());
        }

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
        private class TimelineobjectComparer : IComparer<Timelineobject>
        {
            public int Compare([AllowNull] Timelineobject x, [AllowNull] Timelineobject y)
            {
                return (int)(ExtractDate(y) - ExtractDate(x));
            }

            private long ExtractDate(Timelineobject timelineObject)
            {
                if (timelineObject.activitySegment != null)
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
