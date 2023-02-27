using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using P01_StudentSystem.Data.Models;



namespace P01_StudentSystem.Data;

public class StudentsSystemContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Homework> Homeworks { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCourse> StudentsCourses { get; set; }

    public StudentsSystemContext(DbContextOptions options) : base(options)
    {

    }

    public StudentsSystemContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=StudentSystem;Trusted_Connection=True;Integrated Security=True;");
        }
    }

}

