using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.ImportDto
{
    public class CardImportDto
    {
        [RegularExpression(@"\d{4} \d{4} \d{4} \d{4}")]
        [Required]
        public string? Number { get; set; }

        [Required]
        [RegularExpression(@"\d{3}")]
        public string? CVC { get; set; }

        [Required]
        [EnumDataType(typeof(CardType))]
        public string? Type { get; set; }
    }
}
