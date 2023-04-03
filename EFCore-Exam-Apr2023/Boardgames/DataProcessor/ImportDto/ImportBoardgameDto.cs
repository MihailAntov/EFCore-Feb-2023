using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [Required]
        [MinLength(10), MaxLength(20)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Range(1,10)]
        [XmlElement("Rating")]
        public double Rating { get; set; }

        [Range(2018,2023)]
        [XmlElement("YearPublished")]
        public int YearPublished { get; set; }

        [Range(0,4)]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; }
    }
}


//< Boardgame >
////				< Name > 4 Gods </ Name >
////				< Rating > 7.28 </ Rating >
////				< YearPublished > 2017 </ YearPublished >
////				< CategoryType > 4 </ CategoryType >
////				< Mechanics > Area Majority / Influence, Hand Management, Set Collection, Simultaneous Action Selection, Worker Placement</Mechanics>
////			</Boardgame>