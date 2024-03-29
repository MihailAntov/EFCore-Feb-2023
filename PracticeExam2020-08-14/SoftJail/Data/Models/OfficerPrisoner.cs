﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        [ForeignKey(nameof(Prisoner))]
        public int PrisonerId { get; set; }
        [Required]
        public virtual Prisoner Prisoner { get; set; } = null!;

        [ForeignKey(nameof(Officer))]
        public int OfficerId { get; set; }
        [Required]
        public virtual Officer Officer { get; set; } = null!;
    }
}
