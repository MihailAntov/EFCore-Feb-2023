using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellDto
    {
        [Range(1,1000)]
        public int CellNumber { get; set; }
        
        public bool HasWindow { get; set; }

    }
}
