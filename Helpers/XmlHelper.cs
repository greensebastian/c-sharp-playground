using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace c_sharp_playground.Helpers
{
    public static class XmlHelper
    {
        public static string Serialize<T>(T input) where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, input);
                    return stringWriter.ToString();
                }
            }
        }

        public static T Deserialize<T>(string input) where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var stringReader = new StringReader(input))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
