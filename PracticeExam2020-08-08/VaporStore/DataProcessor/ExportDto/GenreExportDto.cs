﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.DataProcessor.ExportDto
{
    public class GenreExportDto
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public GameExportDto[] Games { get; set; }
        public int TotalPlayers { get; set; }
    }
}
