using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class EmployeeImportDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\d]{3,}$")]
        [MinLength(3),MaxLength(40)]
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }


        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        [Required]
        public string Phone { get; set; }
        public int[] Tasks { get; set; }
    }
}
