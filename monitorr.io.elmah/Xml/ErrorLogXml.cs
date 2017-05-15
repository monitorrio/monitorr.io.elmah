using System.Xml.Serialization;

namespace monitorr.io.elmah.Xml
{
    [XmlRoot("errorLog")]
    public class ErrorLogXml
    {
        [XmlAttribute("LogId")]
        public string LogId { get; set; }
    }
}