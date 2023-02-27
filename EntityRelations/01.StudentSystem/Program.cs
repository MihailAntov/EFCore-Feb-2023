using System;
using Microsoft.EntityFrameworkCore;

using P01_StudentSystem.Data;


namespace P01_StudentSystem;

internal class Startup
{
    static void Main(string[] args)
    {
        StudentSystemContext context = new StudentSystemContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("Done");
    }
}
