using System.Xml.Serialization;

namespace QboMdFood.Models.API
{

    [XmlRoot(ElementName = "response")]
    public class Response
    {
        [XmlAttribute(AttributeName = "Dttm")]
        public string Dttm { get; set; }
        [XmlElement(ElementName = "sales")]
        public Sales Sales { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }


}