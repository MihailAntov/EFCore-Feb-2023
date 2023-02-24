using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftUni;

public class StartUp
{
    
    public static void Main(string[] args)
    {
        using (SoftUniContext context = new SoftUniContext())
        {
            //GetEmployeesFullInformation(context);
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            //Console.WriteLine(AddNewAddressToEmployee(context));
            Console.WriteLine(GetEmployeesInPeriod(context));  //------------ incomplete
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context)); //------------- incomplete
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProjects(context));
            //Console.WriteLine(IncreaseSalaries(context));
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
            //Console.WriteLine(DeleteProjectById(context)); //------------incomplete
            //Console.WriteLine(RemoveTown(context));
        }
    }

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        
        
            List<Employee> employees = context.Employees.OrderBy(e => e.EmployeeId).ToList();
            StringBuilder str = new StringBuilder();
            foreach ( Employee e in employees )
            {
                str.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }
            return str.ToString().TrimEnd();
              
    }

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        List<Employee> employees = context.Employees
            .Where(e => e.Salary > 50000)
            .OrderBy(e => e.FirstName)
            .ToList();
        StringBuilder str = new StringBuilder();
        foreach (Employee e in employees)
        {
            str.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }
        return str.ToString().TrimEnd();

    }
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        //int departmentId = context.Departments
        //    .FirstOrDefault(d => d.Name == "Research and Development")
        //    .DepartmentId;

        //var employees = context.Employees
        //    .Where(d => d.DepartmentId == departmentId)
        //    .Select(e=> new {Name = e.FirstName, DepartmentName = e.Department.Name, Salary = e.Salary})
        //    .ToList();

        var employees = context.Employees
            .Where(d => d.Department.Name == "Research and Development")
            .Select(e=> new {FirstName = e.FirstName,LastName = e.LastName, DepartmentName = e.Department.Name, Salary = e.Salary})
            .ToList();
        StringBuilder str = new StringBuilder();
        foreach (var e in employees)
        {
            str.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
        }
        return str.ToString().TrimEnd();


    }
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        Employee? nakov = context.Employees
            .Where(e => e.LastName == "Nakov")
            .FirstOrDefault();

        if(nakov != null)
        {
            nakov.Address = newAddress;
        }

        context.SaveChanges();

        List<String> addresses = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address.AddressText)
            .ToList();

        StringBuilder str = new StringBuilder();
        foreach(string address in addresses)
        {
            str.AppendLine(address);
        }

        return str.ToString().TrimEnd();

        
    }
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.Projects.Any(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003))
            .Select(e => new
            {
                Name = $"{e.FirstName} {e.LastName}",
                Manager = $"{e.Manager.FirstName} {e.Manager.LastName}",
                Projects = e.Projects
            });

        StringBuilder str = new StringBuilder();

        foreach(var e in employees)
        {
            str.AppendLine($"{e.Name} - Manager: {e.Manager}");
            foreach(Project p in e.Projects)
            {
                string start = p.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                string finish = "not finished";
                if(p.EndDate is not null)
                {
                    finish = DateTime.Parse(p.EndDate.ToString()).ToString("M/d/yyyy h:mm:ss tt");
                }
                
                str.AppendLine($"--{p.Name} - {start} - {finish}");
            }
            
        }
        return str.ToString().TrimEnd();
    }

    public static string GetAddressesByTown(SoftUniContext context)
    {
        var addresses = context.Addresses
            .Select(a => new
            {
                Text = a.AddressText,
                Town = a.Town.Name,
                Count = a.Employees.Count()
            })
            .OrderByDescending(e => e.Count)
            .ThenBy(e => e.Town)
            .ThenBy(e => e.Text)
            .Take(10);

        StringBuilder str = new StringBuilder();
        foreach(var a in addresses)
        {
            str.AppendLine($"{a.Text}, {a.Town} - {a.Count} employees");
        }

        return str.ToString().TrimEnd();
    }

    public static string GetEmployee147(SoftUniContext context)
    {
        //DOESN'T WORK IN JUDGE

        var employee147 = context.Employees
            .Where(e => e.EmployeeId == 147)
            .Select(e => new { FirstName = e.FirstName, LastName = e.LastName, JobTitle = e.JobTitle, Projects = e.Projects })
            .FirstOrDefault();

        StringBuilder str = new StringBuilder();


        str.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");
        foreach(Project project in employee147.Projects.OrderBy(p=>p.Name))
        {
            str.AppendLine(project.Name);
        }

        return str.ToString().TrimEnd();

        //DOESN'T WORK IN JUDGE
    }

    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        var departments = context.Departments
            .Select(d => new { Name = d.Name, Manager = $"{d.Manager.FirstName} {d.Manager.LastName}", Employees = d.Employees })
            .Where(d => d.Employees.Count > 5)
            .OrderBy(d => d.Employees.Count)
            .ThenBy(d => d.Name)
            .ToList();
        StringBuilder str = new StringBuilder();
        foreach(var d in departments)
        {
            str.AppendLine($"{d.Name} - {d.Manager}");
            foreach(Employee e in d.Employees.OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName))
            {
                str.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            }

        }

        return str.ToString().TrimEnd();
    }

    public static string GetLatestProjects(SoftUniContext context)
    {
        var projects = context.Projects
            .Select(p => new { Name = p.Name, Descriptoin = p.Description, StartDate = p.StartDate })
            .OrderByDescending(p => p.StartDate)
            .Take(10)
            .OrderBy(p => p.Name);

        StringBuilder str = new StringBuilder();

        foreach(var p in projects)
        {
            str.AppendLine(p.Name);
            str.AppendLine(p.Descriptoin);
            str.AppendLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
        }

        return str.ToString().TrimEnd();
    }

    public static string IncreaseSalaries(SoftUniContext context)
    {
        string[] departmentsToRaise = new string[]
        {
            "Engineering","Tool Design","Marketing","Information Services"
        };


        List<Employee> employees = context.Employees
            .Where(e => departmentsToRaise.Contains(e.Department.Name))
            .ToList();
        StringBuilder str = new StringBuilder();

        foreach(Employee e in employees.OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName))
        {
            e.Salary *= 1.12M;
            str.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
        }

        context.SaveChanges();

        return str.ToString().TrimEnd(); 
    }

    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e=>e.FirstName.Substring(0,2).ToLower() == "sa")
            .Select(e=> new {FirstName = e.FirstName, 
                            LastName = e.LastName,
                            JobTitle = e.JobTitle,
                            Salary = e.Salary})
            .OrderBy(e=>e.FirstName)
            .ThenBy(e=>e.LastName);

        StringBuilder str = new StringBuilder();
        foreach(var e in employees)
        {
            str.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
        }

        return str.ToString().TrimEnd();
    }
    public static string DeleteProjectById(SoftUniContext context)
    {
        List<Employee> employees = context.Employees
            .Where(e => e.Projects.Select(p => p.ProjectId).Contains(2))
            .ToList();

        foreach(Employee employee in employees)
        {
            employee.Projects = employee.Projects
                .Where(p => p.ProjectId != 2)
                .ToList();
        }

        Project projectToDelete = context.Projects.Find(2);

        context.Projects.Remove(projectToDelete);
        context.SaveChanges();

        List<string> projects = context.Projects
            .Select(p => p.Name)
            .Take(10).
            ToList();

        StringBuilder str = new StringBuilder();

        foreach(string name in projects)
        {
            str.AppendLine(name);
        }

        return str.ToString().TrimEnd();
        //also not working in Judge

    }

    public static string RemoveTown(SoftUniContext context)
    {
        List<Employee> employees = context.Employees
            .Where(e => e.Address.Town.Name == "Seattle")
            .ToList();

        foreach(Employee employee in employees)
        {
            employee.AddressId = null;
        }
        context.SaveChanges();

        List<Address> addresses = context.Addresses
            .Where(a => a.Town.Name == "Seattle")
            .ToList();


        int result = addresses.Count();
        context.Addresses.RemoveRange(addresses);
        context.SaveChanges();

        Town seattle = context.Towns.FirstOrDefault(t => t.Name == "Seattle");
        context.Towns.Remove(seattle);
        context.SaveChanges();

        return $"{result} addresses in Seattle were deleted";
    }

}

