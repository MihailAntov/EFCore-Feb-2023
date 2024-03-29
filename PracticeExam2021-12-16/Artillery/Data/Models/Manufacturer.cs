﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        
        [MaxLength(40)]
        public string ManufacturerName { get; set; } = null!;


        [MaxLength(100)]
        public string Founded { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }

    }
}
