
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    public class TeamFootballer
    {
        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }
        [Required]
        public Team Team { get; set; } = null!;

        [ForeignKey(nameof(Footballer))]
        public int FootballerId { get; set; }
        [Required]
        public Footballer Footballer { get; set; } = null!;

    }
}
