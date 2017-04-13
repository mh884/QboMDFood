using System.Collections.Generic;
using System.Xml.Serialization;

namespace QboMdFood.Models.API
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "sales")]
    public class Sales
    {
        [XmlElement(ElementName = "record")]
        public List<Record> Record { get; set; }
        public Sales()
        {
            Record = new List<Record>();
        }


    }


}