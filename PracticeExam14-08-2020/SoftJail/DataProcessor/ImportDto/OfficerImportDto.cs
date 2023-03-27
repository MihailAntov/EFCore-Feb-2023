using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerImportDto
    {
        [XmlElement("Name")]

        public string Name { get; set; }
        [XmlElement("Money")]

        public decimal Money { get; set; }
        [XmlElement("Position")]

        public string Position { get; set; }
        [XmlElement("Weapon")]

        public string Weapon { get; set; }
        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }
        [XmlArray("Prisoners")]
        public OfficerPrisonerImportDto[] Prisoners { get; set; }
    }
}


//< Officer >
//		< Name > Minerva Kitchingman </ Name >
//		< Money > 2582 </ Money >
//		< Position > Invalid </ Position >
//		< Weapon > ChainRifle </ Weapon >
//		< DepartmentId > 2 </ DepartmentId >
//		< Prisoners >
//			< Prisoner id = "15" />
//		</ Prisoners >