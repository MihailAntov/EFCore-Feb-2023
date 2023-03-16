using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class ExportUserWithSoldProductDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]

        public string LastName { get; set; }
        [XmlArray("soldProducts")]
        public ExportSoldProductDto[] Products { get; set; }
    }
}
