﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class CarDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int TraveledDistance { get; set; }
        public int[] PartsId { get; set; }
    }
}
