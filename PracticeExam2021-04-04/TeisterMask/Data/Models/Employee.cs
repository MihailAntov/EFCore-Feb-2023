using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeisterMask.Data.Models
{
    public class Employee
    {

        public Employee()
        {
            EmployeesTasks = new HashSet<EmployeeTask>();
        }
        
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; }
    }
}
