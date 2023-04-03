using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {

        [XmlElement("FirstName")]
        [MinLength(2),MaxLength(7)]
        [Required]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        [MinLength(2),MaxLength(7)]
        [Required]
        public string LastName { get; set; }

        [XmlArray("Boardgames")]
        public ImportBoardgameDto[] Boardgames { get; set; }
    }
}


//< Creator >
//		< FirstName > Debra </ FirstName >
//		< LastName > Edwards </ LastName >
//		< Boardgames >
//			< Boardgame >
//				< Name > 4 Gods </ Name >
//				< Rating > 7.28 </ Rating >
//				< YearPublished > 2017 </ YearPublished >
//				< CategoryType > 4 </ CategoryType >
//				< Mechanics > Area Majority / Influence, Hand Management, Set Collection, Simultaneous Action Selection, Worker Placement</Mechanics>
//			</Boardgame>