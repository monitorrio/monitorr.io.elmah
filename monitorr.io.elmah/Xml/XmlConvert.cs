using System;
using System.IO;
using System.Xml.Serialization;

namespace monitorr.io.elmah.Xml
{
    public class XmlConvert
    {
        public static T DeserializeObject<T>(string xml)
            where T : new()
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new T();
            }
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }
    }
}