using System;
using Microsoft.EntityFrameworkCore;

using P01_StudentSystem.Data;


namespace P01_StudentSystem;

internal class Startup
{
    static void Main(string[] args)
    {
        StudentsSystemContext context = new StudentsSystemContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("Done");
    }
}
