using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("users")]
    public class UsersWithProductsDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UserWithProductDto[] Users { get; set; }

    }
}
