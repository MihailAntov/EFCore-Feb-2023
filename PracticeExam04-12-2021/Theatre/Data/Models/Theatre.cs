using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        public Theatre()
        {
            Tickets = new HashSet<Ticket>();
        }
        
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = null!;

        public sbyte NumberOfHalls { get; set; }

        [MaxLength(30)]
        public string Director { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
