using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using c_sharp_playground.Logic;
using c_sharp_playground.Helpers;

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
            try
            {
                var timelineObjects = TimelineLogic.CreateTimelineSet();
                long size;

                // Wrap in using statement to ensure temp file is created/destroyed as necessary
                using (var tempDemoFile = new TempFormFile(showDemo, _demoFileInfo))
                {
                    if (showDemo) files.Add(tempDemoFile.FormFile);

                    size = files.Sum(file => file.Length);

                    foreach (var file in files)
                    {
                        await TimelineLogic.PopulateFromFile(timelineObjects, file);
                    }
                }

                var results = new { fileCount = files.Count, size, resultCount = timelineObjects.Count, results = timelineObjects };

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
