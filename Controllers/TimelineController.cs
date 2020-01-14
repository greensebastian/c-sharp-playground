using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using c_sharp_playground.Models.Timeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;

namespace c_sharp_playground.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IFileInfo _demoFileInfo;

        public TimelineController(IWebHostEnvironment webHostEnvironment)
        {
            _demoFileInfo = webHostEnvironment.ContentRootFileProvider.GetFileInfo("App_Data/demo/demo_timeline.json");
        }

        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files, [FromForm] bool showDemo = false)
        {
            // Create temporary file to use as buffer for demo file, if needed
            var tempFile = showDemo ? Path.GetTempFileName() : null;
            Stream tempFileStream = null;
            try
            {
                if (showDemo)
                {
                    // Add a demo file to the list of files
                    System.IO.File.Copy(_demoFileInfo.PhysicalPath, tempFile, true);
                    tempFileStream = System.IO.File.OpenRead(tempFile);
                    files.Add(new FormFile(tempFileStream, 0, _demoFileInfo.Length, Path.GetFileNameWithoutExtension(tempFile), Path.GetFileName(tempFile)));
                }

                long size = files.Sum(file => file.Length);
                var timelineObjects = new SortedSet<Timelineobject>(new TimelineobjectComparer());

                foreach (var file in files)
                {
                    // Validate request file somewhat
                    var extension = Path.GetExtension(file.FileName);
                    if (!extension.Contains("json", StringComparison.InvariantCultureIgnoreCase) && !extension.Contains("tmp", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return BadRequest(new { error = "Invalid file extension, expected json." });
                    }
                    if (file.Length <= 0)
                    {
                        continue;
                    }

                    // Read, deserialize, and add resulting timeline objects to response model
                    using (var stream = file.OpenReadStream())
                    {
                        var content = await DeserializeFromStream<SemanticTimeline>(stream);
                        timelineObjects.UnionWith(content?.timelineObjects ?? new List<Timelineobject>());
                    }
                }

                var results = new { fileCount = files.Count, size, resultCount = timelineObjects.Count, results = timelineObjects };

                return Ok(results);
            } catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            } finally
            {
                // Clean up temporary files
                if (tempFile != null && tempFileStream != null)
                {
                    tempFileStream.Dispose();
                    System.IO.File.Delete(tempFile);
                }
            }
        }

        // Deserialize a json stream to a generic type
        private async static Task<T> DeserializeFromStream<T>(Stream stream) where T: class, new()
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
            } catch (Exception)
            {
                return null;
            }
        }

        // Comparer for parsing and sorting the relevant dates from the timeline objects
        private class TimelineobjectComparer : IComparer<Timelineobject>
        {
            public int Compare([AllowNull] Timelineobject x, [AllowNull] Timelineobject y)
            {
                return (int) (ExtractDate(y) - ExtractDate(x));
            }

            private long ExtractDate(Timelineobject timelineObject){
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
