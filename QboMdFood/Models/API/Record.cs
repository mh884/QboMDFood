using System.Xml.Serialization;

namespace QboMdFood.Models.API
{
    [XmlRoot(ElementName = "record")]
    public class Record
    {
        [XmlElement(ElementName = "amount")]
        public string Amount { get; set; }
        [XmlElement(ElementName = "customer_id")]
        public string Customer_id { get; set; }
        [XmlElement(ElementName = "customer_name")]
        public string Customer_name { get; set; }
        [XmlElement(ElementName = "gst_amount")]
        public string Gst_amount { get; set; }
        [XmlElement(ElementName = "invoice_date")]
        public string Invoice_date { get; set; }
        [XmlElement(ElementName = "invoice_number")]
        public string Invoice_number { get; set; }
    }


}