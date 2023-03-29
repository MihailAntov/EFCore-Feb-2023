using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDto
    {
        [XmlElement("Title")]
        [MinLength(4),MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [XmlElement("Duration")]  
        [Required]
        public string Duration { get; set; }

        [XmlElement("Raiting")]
        [Range(0.0,10.0)]
        [Required]
        public float Rating { get; set; }

        [XmlElement("Genre")]
        [Required]
        public string Genre { get; set; }

        [XmlElement("Description")]
        [MaxLength(700)]
        [Required]
        public string Description { get; set; }

        [XmlElement("Screenwriter")]
        [MinLength(4),MaxLength(30)]
        [Required]
        public string Screenwriter { get; set; }
    }
}


//< Plays >
//  < Play >
//    < Title > The Hsdfoming </ Title >
//    < Duration > 03:40:00 </ Duration >
//    < Raiting > 8.2 </ Raiting >
//    < Genre > Action </ Genre >
//    < Description > A guyat Pinter turns into a debatable conundrum as oth ordinary and menacing. Much of this has to do with the fabled Pinter Pause, which simply mirrors the way we often respond to each other in conversation, tossing in remainders of thoughts on one subject well after having moved on to another.</Description>
//    <Screenwriter>Roger Nciotti</Screenwriter>
//  </Play>