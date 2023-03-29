using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        public Card()
        {
            Purchases = new HashSet<Purchase>();
        }
        
        public int Id { get; set; }
        [MaxLength(19)]
        public string Number { get; set; } = null!;
        [MaxLength(3)]
        public string Cvc { get; set; } = null!;

        public CardType Type { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
