﻿using c_sharp_playground.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_playground.Services
{
    public class QuerystationService : BaseService
    {
        private static readonly string _hostname = "http://www.labs.skanetrafiken.se/v2.2/querystation.asp";
        public QuerystationService() : base(_hostname)
        {
        }

        public async Task<GetStartEndPointResult> SearchForStation(string stationName)
        {
            var queryParameters = new Dictionary<string, string>()
            {
                { "inpPointFr", stationName }
            };
            var response = await GetResponse(queryParameters);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent = ExtractContent<GetStartEndPointResult>(responseContent);
            return Deserialize<GetStartEndPointResult>(responseContent);
        }
    }
}
