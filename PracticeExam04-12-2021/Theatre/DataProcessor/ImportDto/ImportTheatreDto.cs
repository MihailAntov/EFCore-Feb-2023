using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatreDto
    {
        [MinLength(4),MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [Range(1,10)]
        [Required]
        public sbyte NumberOfHalls { get; set; }

        [MinLength(4),MaxLength(30)]
        [Required]
        public string Director { get; set; }


        public ImportTicketDto[] Tickets { get; set; }
    }
}
