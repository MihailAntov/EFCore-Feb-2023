using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ImportDto
{
    public class TeamImportDto
    {
        [Required]
        [MinLength(3),MaxLength(40)]
        [RegularExpression(@"[\d\w\.\-\s]+")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(2),MaxLength(40)]
        public string Nationality { get; set; } = null!;
        [Range(1,int.MaxValue)]
        public int Trophies { get; set; }
        public int[] Footballers { get; set; }
    }
}


//"Name": "Chelsea F.C.",
//    "Nationality": "The United Kingdom",
//    "Trophies": "34",
//    "Footballers": [
//      38,
//      39,
//      59,
//      62,
//      57,
//      56
//    ]