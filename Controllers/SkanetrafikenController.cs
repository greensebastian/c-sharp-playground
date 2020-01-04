﻿using c_sharp_playground.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace c_sharp_playground.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SkanetrafikenController : ControllerBase
    {
        private readonly ILogger<SkanetrafikenController> _logger;

        public SkanetrafikenController(ILogger<SkanetrafikenController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Querystation([FromQuery] string station)
        {
            try
            {
                var service = new QuerystationService();
                return Ok(service.SearchForStation(station).Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public ActionResult Stationresults([FromQuery] int stationId, [FromQuery] DateTime? dateTime = null, [FromQuery] Direction direction = Direction.Both)
        {
            try
            {
                var service = new StationresultsService();
                return Ok(service.GetStationDetails(stationId, dateTime ?? DateTime.Now, direction).Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}