using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDto
    {
        [XmlElement("CountryName")]
        [MinLength(4),MaxLength(60)]
        [Required]
        public string CountryName { get; set; }

        [XmlElement("ArmySize")]
        [Range(50_000,10_000_000)]
        [Required]
        public int ArmySize { get; set; }
    }
}
