﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using c_sharp_playground.Logic;
using c_sharp_playground.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        private GetJourneyService JourneyService
        {
            get
            {
                return new GetJourneyService();
            }
        }

        private QuerystationService QueryStationService
        {
            get
            {
                return new QuerystationService();
            }
        }

        private StationresultsService StationResultsService
        {
            get
            {
                return new StationresultsService();
            }
        }

        private GetMeansOfTransportService MeansOfTransportService
        {
            get
            {
                return new GetMeansOfTransportService();
            }
        }



        [HttpGet]
        public ActionResult QueryJournies([FromQuery] string startPoint, [FromQuery] string endPoint, [FromQuery] DateTime? dateTime = null, [FromQuery] int numberResults = 5, [FromQuery] int lineTypeSum = 2047)
        {
            try
            {
                var response = JourneyService.SearchForJourney(startPoint, endPoint, dateTime ?? DateTime.Now, numberResults, lineTypeSum).Result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public ActionResult Querystation([FromQuery] string station)
        {
            try
            {
                var response = QueryStationService.SearchForStation(station).Result;
                return Ok(response);
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
                var response = StationResultsService.GetStationDetails(stationId, dateTime ?? DateTime.Now, direction).Result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public ActionResult MeansOfTransport()
        {
            try
            {
                var response = MeansOfTransportService.GetMeansOfTransport().Result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public ActionResult TrainDelays([FromQuery] string startPoint, [FromQuery] string endPoint, [FromQuery] DateTime? dateTime = null, [FromQuery] int numberResults = 5, [FromQuery] int lineTypeSum = 2047)
        {
            try
            {
                var meansOfTransportServiceResult = MeansOfTransportService.GetMeansOfTransport().Result;
                var searchWords = new string[]
                {
                    "Tåg", "Buss, kommersiell"
                };
                // Sum all line type ids where the line name exists in search words
                lineTypeSum = meansOfTransportServiceResult.TransportModes.Where(line => searchWords.Any(word => line.Name.Contains(word, StringComparison.InvariantCultureIgnoreCase))).Sum(line => line.Id);
                var journeyServiceResult = JourneyService.SearchForJourney(startPoint, endPoint, dateTime ?? DateTime.Now, numberResults, lineTypeSum).Result;
                
                if (journeyServiceResult == null)
                {
                    return StatusCode((int)HttpStatusCode.NoContent, "No response from Skånetrafiken for specified request");
                }

                var response = SkanetrafikenLogic.GetDelaysResponse(journeyServiceResult, meansOfTransportServiceResult);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
