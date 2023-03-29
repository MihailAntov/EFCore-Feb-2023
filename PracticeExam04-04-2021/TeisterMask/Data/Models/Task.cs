using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.Data.Models
{
    public class Task
    {
        public Task()
        {
            EmployeesTasks = new HashSet<EmployeeTask>();
        }
        
        [Key]
        public int Id { get; set; }

        [MaxLength(40)]
        [Required]
        public string Name { get; set; }

        public DateTime OpenDate { get; set; }

        
        public DateTime DueDate { get; set; }

        public ExecutionType ExecutionType { get; set; }
        public LabelType LabelType { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        [Required]
        public virtual Project Project { get; set; } = null!;

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; }


    }
}
