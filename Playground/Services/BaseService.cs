using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Playground.Services
{
    public abstract class BaseService
    {
        protected readonly string Hostname;
        protected BaseService(string hostname)
        {
            Hostname = hostname;
        }

        protected async Task<HttpResponseMessage> GetResponse(Dictionary<string, string> queryParameters = null, List<string> escapedChars = null)
        {
            string url = Hostname;

            if (queryParameters != null && queryParameters.Count > 0)
            {
                var parameters = new List<string>();
                foreach (var item in queryParameters)
                {
                    parameters.Add(HttpUtility.UrlPathEncode(item.Key) + "=" + HttpUtility.UrlPathEncode(item.Value));
                }
                url += "?" + string.Join('&', parameters);

                // Undo escape on certain characters
                if (escapedChars != null && escapedChars.Count > 0)
                {
                    foreach (var escapedChar in escapedChars)
                    {
                        url = url.Replace(HttpUtility.UrlPathEncode(escapedChar), escapedChar);
                    }
                }
            }
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        protected static string ExtractContent<T>(string input)
        {
            var contentTag = typeof(T).Name.ToString();
            var startTag = $"<{contentTag}>";
            var endTag = $"</{contentTag}>";

            var startIndex = input.IndexOf(startTag);
            var endIndex = input.IndexOf(endTag) + endTag.Length;

            return input.Substring(startIndex, endIndex - startIndex);
        }
    }
}
