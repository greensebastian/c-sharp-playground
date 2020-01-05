using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using c_sharp_playground.Models;

namespace c_sharp_playground.Services
{

    public class StationresultsService : BaseService
    {
        private static readonly string _hostname = "http://www.labs.skanetrafiken.se/v2.2/stationresults.asp";

        public StationresultsService() : base(_hostname)
        {
        }

        public async Task<GetDepartureArrivalResult> GetStationDetails(int stationId, DateTime dateTime, Direction direction = Direction.Both)
        {
            var queryParameters = new Dictionary<string, string>()
            {
                { "selPointFrKey", stationId.ToString() },
                { "inpDate", dateTime.ToString("yyyy-MM-dd") },
                { "inpTime", dateTime.ToString("HH:mm") }
            };

            if (direction != Direction.Both) queryParameters.Add("selDirection", ((int)direction).ToString());

            var response = await GetResponse(queryParameters);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent = ExtractContent<GetDepartureArrivalResult>(responseContent);
            return Deserialize<GetDepartureArrivalResult>(responseContent);
        }
    }

    public enum Direction
    {
        Departures = 0,
        Arrivals = 1,
        Both = 2
    }
}
