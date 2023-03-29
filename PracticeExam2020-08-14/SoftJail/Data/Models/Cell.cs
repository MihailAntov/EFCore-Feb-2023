using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public Cell()
        {
            Prisoners = new HashSet<Prisoner>();
        }
        [Key]
        public int Id { get; set; }
        [Range(1,1000)]
        [Required]
        public int CellNumber { get; set; }
        [Required]
        public bool HasWindow { get; set; }

        [ForeignKey(nameof(Department))]
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<Prisoner> Prisoners { get; set; }
    }
}
