using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
            OfficerPrisoners = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public Position Position { get; set; }
        [Required]
        public Weapon Weapon { get; set; }

        [ForeignKey(nameof(Department))]
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }
    }
}
