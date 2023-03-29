using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public Play()
        {
            Casts = new HashSet<Cast>();
            Tickets = new HashSet<Ticket>();
        }
        
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; } = null!;

        public TimeSpan Duration { get; set; }

        public float Rating { get; set; }

        public Genre Genre { get; set; }

        [MaxLength(700)]
        public string Description { get; set; } = null!;

        [MaxLength(30)]
        public string Screenwriter { get; set; } = null!;

        public virtual ICollection<Cast> Casts { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
