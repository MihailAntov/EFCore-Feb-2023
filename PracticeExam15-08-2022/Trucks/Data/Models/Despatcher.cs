﻿

using System.ComponentModel.DataAnnotations;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        public Despatcher()
        {
            Trucks = new HashSet<Truck>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        public string Position { get; set; } = null!;

        public virtual ICollection<Truck> Trucks { get; set; }
    }
}