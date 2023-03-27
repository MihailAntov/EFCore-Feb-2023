using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.Data.Models
{
    public class Tag
    {
        public Tag()
        {
            GameTags = new HashSet<GameTag>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<GameTag> GameTags { get; set; }
    }
}
