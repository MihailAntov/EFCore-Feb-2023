﻿
using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
    public class Coach
    {
        public Coach()
        {
            Footballers = new HashSet<Footballer>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Nationality { get; set; } = null!;
        public ICollection<Footballer> Footballers { get; set; }


    }
}
