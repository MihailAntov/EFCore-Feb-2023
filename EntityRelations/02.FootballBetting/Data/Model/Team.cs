﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public decimal Budget { get; set; }
        [Required]
        [StringLength(3)]
        public string Initials { get; set; }
        public int PrimaryKitColorId { get; set; }
        public Color PrimaryKitColor { get; set; }
        public int SecondaryKitColorId { get; set; }
        public Color SecondaryKitColor { get; set; }    
        public int TownId { get; set; }
        public Town Town { get; set; }

        public ICollection<Game> AwayGames { get; set; }
        public ICollection<Game> HomeGames { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
