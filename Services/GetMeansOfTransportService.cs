using c_sharp_playground.Models;
using System.Threading.Tasks;

namespace c_sharp_playground.Services
{
    public class GetMeansOfTransportService : BaseService
    {
        private static readonly string _hostname = "http://www.labs.skanetrafiken.se/v2.2/trafficmeans.asp";
        public GetMeansOfTransportService() : base(_hostname)
        {
        }

        public async Task<GetMeansOfTransportResult> GetMeansOfTransport()
        {
            var response = await GetResponse();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent = ExtractContent<GetMeansOfTransportResult>(responseContent);
            return Deserialize<GetMeansOfTransportResult>(responseContent);
        }
    }
}
