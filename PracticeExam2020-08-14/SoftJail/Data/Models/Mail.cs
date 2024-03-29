﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Sender { get; set; } = null!;
        [Required]

        public string Address { get; set; } = null!;

        [ForeignKey(nameof(Prisoner))]
        [Required]
        public int PrisonerId { get; set; }
        [Required]
        public virtual Prisoner Prisoner { get; set; } = null!;
    }
}
