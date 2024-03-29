﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunDto
    {
        [Required]
        public int ManufacturerId { get; set; }


        [Range(100,1_350_000)]
        [Required]
        public int GunWeight { get; set; }

        [Range(2.00,35.00)]
        [Required]
        public double BarrelLength { get; set; }


        public int? NumberBuild { get; set; }

        [Range(1,100_000)]
        [Required]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; }

        [Required]
        public int ShellId { get; set; }

        public ImportCountryGunDto[] Countries { get; set; }
    }
}
