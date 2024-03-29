﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            Songs = new HashSet<Song>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public int? ProducerId { get; set; }
        public Producer? Producer { get; set; }
        public ICollection<Song> Songs { get; set; }

        [NotMapped]
        public decimal Price { get { return Songs.Select(s => s.Price).Sum(); } }
    }
}
