using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        public enum ResourceTypes { Video, Presentation, Document, Other }
        public int ResourceId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [DataType("varchar")]
        public string Url { get; set; }
        public ResourceTypes ResourceType { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
