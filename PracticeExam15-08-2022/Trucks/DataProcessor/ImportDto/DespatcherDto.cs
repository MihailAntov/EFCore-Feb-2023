using System.ComponentModel.DataAnnotations;

using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
	public class DespatcherDto
	{
        [MinLength(2),MaxLength(40)]
		[XmlElement("Name")]
		public string Name { get; set; } = null!;
        [Required]
		[XmlElement("Position")]
		public string Position { get; set; } = null!;
        [XmlArray("Trucks")]
		public List<TruckDto> Trucks { get; set; }


	}
}
