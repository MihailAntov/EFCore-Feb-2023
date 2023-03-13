﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            SongPerformers = new HashSet<SongPerformer>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }
        public Album? Album { get; set; }
        [ForeignKey(nameof(Writer))]
        public int WriterId { get; set; }
        public Writer Writer { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ICollection<SongPerformer> SongPerformers { get; set; }
    }
}