using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Playground.Helpers;
using Playground.Models.Skanetrafiken;

namespace Playground.Services
{
    public class GetJourneyService : BaseService
    {
        private static readonly string _hostname = "http://www.labs.skanetrafiken.se/v2.2/resultspage.asp";
        public GetJourneyService() : base(_hostname)
        {
        }

        /// <summary>
        /// Searches for a journey
        /// </summary>
        /// <param name="startPointName">Name of start station</param>
        /// <param name="endPointName">Name of end station</param>
        /// <param name="dateTime">Time to start the search from</param>
        /// <param name="numberResults">Number of possible journeys to return</param>
        /// <param name="lineTypeSum">Sum of line types to be used. Refer to GetMeansOfTransport service</param>
        /// <returns>Possible journeys given the input</returns>
        public async Task<GetJourneyResult> SearchForJourney(string startPointName, string endPointName, DateTime dateTime, int numberResults, int lineTypeSum)
        {
            var querystationService = new QuerystationService();
            var startPointTask = querystationService.SearchForStation(startPointName);
            var endPointTask = querystationService.SearchForStation(endPointName);

            await Task.WhenAll(startPointTask, endPointTask);

            var startPoint = startPointTask.Result.StartPoints[0];
            var endPoint = endPointTask.Result.StartPoints[0];

            var queryParameters = new Dictionary<string, string>()
            {
                { "selPointFr", BuildPointParameter(startPoint) },
                { "selPointTo", BuildPointParameter(endPoint) },
                { "inpDate", dateTime.ToString("yyyy-MM-dd") },
                { "inpTime", dateTime.ToString("HH:mm") },
                { "NoOf", numberResults.ToString() },
                { "transportMode", lineTypeSum.ToString() },
                { "cmdAction", CommandActionType.Search.ToString().ToLower() }
            };
            var escapedChars = new List<string> { "|" };
            var response = await GetResponse(queryParameters, escapedChars);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent = ExtractContent<GetJourneyResult>(responseContent);
            return XmlHelper.Deserialize<GetJourneyResult>(responseContent);
        }

        private string BuildPointParameter(Point point)
        {
            return $"{point.Name}|{point.Id}|{(int)point.Type}";
        }
    }

    public enum CommandActionType
    {
        Search = 0,
        Next = 1,
        Previous = 2
    }
}
