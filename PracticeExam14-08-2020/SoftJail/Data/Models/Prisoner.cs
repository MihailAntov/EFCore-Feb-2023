
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            Mails = new HashSet<Mail>();
            PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Nickname { get; set; } = null!;
        [Range(18, 65)]
        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime IncarcerationDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? Bail { get; set; }

        [ForeignKey(nameof(Cell))]
        public int? CellId { get; set; }
        public virtual Cell? Cell { get; set; }

        public virtual ICollection<Mail> Mails { get; set; }
        public virtual ICollection<OfficerPrisoner> PrisonerOfficers {get; set;}
    }
}
