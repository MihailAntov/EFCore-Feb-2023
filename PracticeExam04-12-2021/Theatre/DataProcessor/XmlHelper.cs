using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Theatre.DataProcessor
{
    public class XmlHelper
    {
        public static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T deserializedDtos = (T)xmlSerializer.Deserialize(reader);
            return deserializedDtos;
        }

        public static string Serialize<T>(T[] objectDtos, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]),xmlRoot);
            using StringWriter writer = new StringWriter();
            xmlSerializer.Serialize(writer, objectDtos,namespaces);

            return writer.ToString();
        }
    }
}
