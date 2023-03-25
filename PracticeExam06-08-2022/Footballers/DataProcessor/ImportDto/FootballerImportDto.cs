

using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class FootballerImportDto
    {
        [XmlElement("Name")]
        [MinLength(2),MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; } = null!;
        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; } = null!;
        [XmlElement("BestSkillType")]
        [Required]

        public int BestSkillType { get; set; }
        [XmlElement("PositionType")]
        [Required]

        public int PositionType { get; set; } 
    }
}
