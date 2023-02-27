using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        public enum ContentTypes { Application, Pdf, Zip}
        public int HomeworkId { get; set; }
        [DataType("varchar")]
        public string Content { get; set; }
        public ContentTypes ContentType { get; set; }
        public DateTime SubmissionTime { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
