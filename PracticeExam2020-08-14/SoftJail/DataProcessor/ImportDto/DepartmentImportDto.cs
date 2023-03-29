using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentImportDto
    {
        [MaxLength(25), MinLength(3)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public ImportCellDto[] Cells { get; set; } = null!;
    }
}
