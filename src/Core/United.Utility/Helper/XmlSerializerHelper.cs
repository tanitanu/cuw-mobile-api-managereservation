using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace United.Utility.Helper
{
    public static class XmlSerializerHelper
    {
        public static string Serialize<T>(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }

            string xml = string.Empty;
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                xmlSerializer.Serialize(writer, t);
                xml = writer.ToString();
                writer.Flush();
                writer.Close();
            }
            return xml;
        }

        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }

            T t = default(T);
            using (StringReader reader = new StringReader(xml))
            {
                //var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                //t = (T)xmlSerializer.Deserialize(reader);
                reader.Close();
            }

            return t;
        }

        public static T GetObjectFromXmlData<T>(string xmlSessionData)
        {
            if (string.IsNullOrEmpty(xmlSessionData))
                return default(T);

            //var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(T));
            //var xmlResult = (T)xmlSerialize.Deserialize(new StringReader(xmlSessionData));
            T xmlResult = default(T);
            StringReader memoryStream = new StringReader(xmlSessionData);
            var xmlSerializer = new XmlSerializer(typeof(T));
            xmlResult = (T)xmlSerializer.Deserialize(memoryStream);
            if (xmlResult != null)
                return xmlResult;
            else
                return default(T);
        }

        public static string SaveObjectFromXmlData<T>(T xmlSessionData)
        {
            var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(T));
            StringBuilder xmlResult = new StringBuilder();
            var swWriter = new StringWriter(xmlResult);
            xmlSerialize.Serialize(swWriter, xmlSessionData);

            return xmlResult.ToString();
        }

        public static string SaveObjectFromXml<T>(T xmlSessionData)
        {
            using (StringWriter memoryStream = new StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(memoryStream, xmlSessionData);

                return memoryStream.ToString();

            }

            return string.Empty;
        }
    }
}