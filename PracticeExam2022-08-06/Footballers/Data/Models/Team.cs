﻿
using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
    public class Team
    {
        public Team()
        {
            TeamsFootballers = new HashSet<TeamFootballer>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        [MaxLength(40)]
        [Required]
        public string Nationality { get; set; } = null!;
        public int Trophies { get; set; }
        public ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
