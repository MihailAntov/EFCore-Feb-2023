// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Text;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var projectDtos = XmlHelper.Deserialize<ProjectImportDto[]>(xmlString, "Projects");
            ICollection<Project> validProjects = new HashSet<Project>();
            StringBuilder sb = new StringBuilder();
            foreach(var projectDto in projectDtos)
            {
                if(!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!DateTime.TryParseExact(projectDto.OpenDate,"dd/MM/yyyy",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                Project project = new Project();
                if(String.IsNullOrEmpty(projectDto.DueDate))
                {
                    project.DueDate = null;
                }
                else
                {
                    if (!DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    project.DueDate = validDate;
                }


                
                project.Name = projectDto.Name;
                project.OpenDate = openDate;
                

                ICollection<Task> validTasks = new HashSet<Task>();
                foreach(var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskDueDate < taskOpenDate || taskOpenDate < project.OpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(project.DueDate.HasValue && taskDueDate > project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    

                    Task task = new Task();
                    task.Name = taskDto.Name;
                    task.OpenDate = taskOpenDate;
                    task.DueDate = taskDueDate;
                    task.ExecutionType = (ExecutionType)taskDto.ExecutionType;
                    task.LabelType = (LabelType)taskDto.LabelType;

                    validTasks.Add(task);
                }

                project.Tasks = validTasks;
                validProjects.Add(project);

                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, validTasks.Count));
            }

            context.Projects.AddRange(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
            
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeeDtos = JsonConvert.DeserializeObject<EmployeeImportDto[]>(jsonString);
            ICollection<Employee> validEmployees = new HashSet<Employee>();
            int[] existingTasks = context.Tasks.Select(t => t.Id).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var employeeDto in employeeDtos)
            {
                if(!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Employee employee = new Employee();
                employee.Username = employeeDto.Username;
                employee.Phone = employeeDto.Phone;
                employee.Email = employeeDto.Email;
                ICollection<int> tasks = new HashSet<int>();

                foreach(int taskId in employeeDto.Tasks.Distinct())
                {
                    if(!existingTasks.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    tasks.Add(taskId);
                }

                employee.EmployeesTasks = tasks.Select(t => new EmployeeTask()
                {
                    TaskId = t
                }).ToArray();

                validEmployees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, tasks.Count));
            }

            context.AddRange(validEmployees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

           
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        
    }
}