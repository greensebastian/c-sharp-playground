using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Playground.Logic.Timeline;
using Playground.Helpers;
using Microsoft.AspNetCore.Authorization;
using Playground.Repository;

namespace Playground.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IFileInfo _demoFileInfo;
        private readonly TimelineRepository _timelineRepository;
        private readonly UserService _userService;

        public TimelineController(IWebHostEnvironment webHostEnvironment, TimelineRepository timelineRepository, UserService userService)
        {
            _demoFileInfo = webHostEnvironment.ContentRootFileProvider.GetFileInfo("App_Data/demo/demo_timeline.json");
            _timelineRepository = timelineRepository;
            _userService = userService;
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

                var processedResults = TimelineLogic.CreateResponseContent(timelineObjects);

                var results = new { fileCount = files.Count, size, resultCount = timelineObjects.Count, processedResults };

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            try
            {
                var timelineObjects = TimelineLogic.CreateTimelineSet();
                long size;

                size = files.Sum(file => file.Length);

                foreach (var file in files)
                {
                    await TimelineLogic.PopulateFromFile(timelineObjects, file);
                }

                await _timelineRepository.AddTimelineData(timelineObjects, await _userService.CurrentUser());

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
