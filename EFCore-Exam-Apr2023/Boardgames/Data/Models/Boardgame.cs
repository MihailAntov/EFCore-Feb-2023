﻿using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class Boardgame
    {

        public Boardgame()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }
        
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; } = null!;


        public double Rating { get; set; }


        public int YearPublished { get; set; }


        public CategoryType CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }

        [Required]
        public virtual Creator Creator { get; set; } = null!;


        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}
