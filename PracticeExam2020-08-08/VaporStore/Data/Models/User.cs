using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            Cards = new HashSet<Card>();
        }
        
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Username { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;  
        public int Age { get; set; }
        public virtual ICollection<Card> Cards { get; set; }

    }
}
