using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        [XmlElement("ManufacturerName")]
        [MinLength(4),MaxLength(40)]
        [Required]
        public string ManufacturerName { get; set; }

        [XmlElement("Founded")]
        [MinLength(10), MaxLength(100)]
        [Required]
        public string Founded { get; set; }
    }
}
