using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ExportDto
{
    public class PrisonerExportDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CellNumber { get; set; }
        public OfficerExportDto[] Officers { get; set; }
        public decimal TotalOfficerSalary { get; set; }
    }
}
