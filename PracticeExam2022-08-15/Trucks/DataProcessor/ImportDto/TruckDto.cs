
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class TruckDto
    {
        [StringLength(8)]
        [RegularExpression(@"[A-Z]{2}\d{4}[A-Z]{2}")]
        [Required]
        [XmlElement("RegistrationNumber")]
        public string RegistrationNumber { get; set; } = null!;

        [StringLength(17)]
        [Required]
        [XmlElement("VinNumber")]

        public string VinNumber { get; set; } = null!;

        [Range(950,1420)]
        [XmlElement("TankCapacity")]
        public int TankCapacity { get; set; }

        [Range(5000, 29000)]
        [XmlElement("CargoCapacity")]
        public int CargoCapacity { get; set; }
        [XmlElement("CategoryType")]

        public int CategoryType { get; set; }
        [XmlElement("MakeType")]

        public int MakeType { get; set; }
    }
}
