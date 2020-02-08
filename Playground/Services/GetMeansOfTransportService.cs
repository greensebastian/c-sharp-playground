using System.Threading.Tasks;
using Playground.Helpers;
using Playground.Models.Skanetrafiken;
using System.Runtime.Caching;
using System;

namespace Playground.Services
{
    public class GetMeansOfTransportService : BaseService
    {
        private static readonly string _hostname = "http://www.labs.skanetrafiken.se/v2.2/trafficmeans.asp";
        private static readonly MemoryCache _cache = new MemoryCache("GetMeansOfTransportServiceCache");
        private const string GetMeansOfTransportResultCacheKey = "GetMeansOfTransportResult";
        public GetMeansOfTransportService() : base(_hostname)
        {
        }

        private static CacheItemPolicy CachePolicy
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60)
                };
            }
        }

        public async Task<GetMeansOfTransportResult> GetMeansOfTransport()
        {
            try
            {
                var cachedValue = _cache.Get(GetMeansOfTransportResultCacheKey);
                if (cachedValue != null)
                {
                    return cachedValue as GetMeansOfTransportResult;
                }
            } catch (Exception)
            {
                // Memory cache retrieval failed, continue with retrieving and updating the value from external service
            }
            var response = await GetResponse();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent = ExtractContent<GetMeansOfTransportResult>(responseContent);
            var deserializedResponse = XmlHelper.Deserialize<GetMeansOfTransportResult>(responseContent);
            _cache.Add(GetMeansOfTransportResultCacheKey, deserializedResponse, CachePolicy);

            return deserializedResponse;
        }
    }
}
