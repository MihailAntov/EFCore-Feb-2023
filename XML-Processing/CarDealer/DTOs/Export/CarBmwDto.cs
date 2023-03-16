
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("car")]
    public class CarBmwDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("model")]

        public string Model { get; set; }
        [XmlAttribute("traveled-distance")]

        public long TravelledDistance { get; set; }
    }
}
