using System.Collections.Generic;
using System.Threading.Tasks;
using Playground.Helpers;
using Playground.Models.Skanetrafiken;

namespace Playground.Services
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
            return XmlHelper.Deserialize<GetStartEndPointResult>(responseContent);
        }
    }
}
