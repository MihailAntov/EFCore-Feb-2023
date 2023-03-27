
using System.ComponentModel.DataAnnotations;


namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerImportDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Fullname { get; set; }
        [Required]
        [RegularExpression(@"The [A-Z]{1}[a-zA-Z]+$")]
        public string Nickname { get; set; }
        [Required]
        [Range(18,65)]
        public int Age { get; set; }
        [Required]
        public string IncarcerationDate { get; set; }
        public string? ReleaseDate { get; set; }
        [Range(0,Double.MaxValue)]
        public decimal? Bail { get; set; }
        public int? CellId { get; set; }
        public MailImportDto[] Mails { get; set; }
    }
}
