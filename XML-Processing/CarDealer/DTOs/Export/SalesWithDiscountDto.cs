
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("sale")]
    public class SalesWithDiscountDto
    {
        [XmlElement("car")]
        public CarSaleDto CarSaleDto { get; set; }
        [XmlElement("discount")]
        public int Discount { get; set; }
        [XmlElement("customer-name")]
        public string CustomerName { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }


    }
}
