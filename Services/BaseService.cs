using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace c_sharp_playground.Services
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

        protected string Serialize<T>(T input) where T : new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, input);
                    return stringWriter.ToString();
                }
            }
        }

        protected T Deserialize<T>(string input) where T : new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (var stringReader = new StringReader(input))
            {
                return (T)serializer.Deserialize(stringReader);
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
