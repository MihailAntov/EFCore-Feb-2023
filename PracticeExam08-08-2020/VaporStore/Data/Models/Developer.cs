using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.Data.Models
{
    public class Developer
    {
        public Developer()
        {
            Games = new HashSet<Game>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Game> Games { get; set; }
    }
}
