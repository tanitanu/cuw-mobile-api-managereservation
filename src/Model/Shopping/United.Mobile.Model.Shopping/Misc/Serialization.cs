using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class Serialization
    {
        public static string Serialize(object obj, Type type)
        {
            string xml = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, obj);
            xml = sb.ToString();

            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            xml = xml.Replace("\r\n", "");

            return xml;
        }

        public static object Deserialize(string xml, Type type)
        {
            object obj = null;

            XmlSerializer xmlSerializer = new XmlSerializer(type);
            obj = xmlSerializer.Deserialize(new XmlTextReader(new StringReader(xml)));

            return obj;
        }
    }
}
