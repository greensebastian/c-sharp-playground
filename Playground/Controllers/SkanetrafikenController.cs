using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Playground.Logic;
using Playground.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Playground.Controllers
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
        public async Task<ActionResult> QueryJournies([FromQuery] string startPoint, [FromQuery] string endPoint, [FromQuery] DateTime? dateTime = null, [FromQuery] int numberResults = 5, [FromQuery] int lineTypeSum = 2047)
        {
            try
            {
                var response = await JourneyService.SearchForJourney(startPoint, endPoint, dateTime ?? DateTime.Now, numberResults, lineTypeSum);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Querystation([FromQuery] string station)
        {
            try
            {
                var response = await QueryStationService.SearchForStation(station);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Stationresults([FromQuery] int stationId, [FromQuery] DateTime? dateTime = null, [FromQuery] Direction direction = Direction.Both)
        {
            try
            {
                var response = await StationResultsService.GetStationDetails(stationId, dateTime ?? DateTime.Now, direction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> MeansOfTransport()
        {
            try
            {
                var response = await MeansOfTransportService.GetMeansOfTransport();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> TrainDelays([FromQuery] string startPoint, [FromQuery] string endPoint, [FromQuery] DateTime? dateTime = null, [FromQuery] int numberResults = 5, [FromQuery] int lineTypeSum = 2047)
        {
            try
            {
                var meansOfTransportServiceResult = await MeansOfTransportService.GetMeansOfTransport();
                var searchWords = new string[]
                {
                    "Tåg", "Buss, kommersiell"
                };
                // Sum all line type ids where the line name exists in search words
                lineTypeSum = meansOfTransportServiceResult.TransportModes.Where(line => searchWords.Any(word => line.Name.Contains(word, StringComparison.InvariantCultureIgnoreCase))).Sum(line => line.Id);
                var journeyServiceResult = await JourneyService.SearchForJourney(startPoint, endPoint, dateTime ?? DateTime.Now, numberResults, lineTypeSum);
                
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
