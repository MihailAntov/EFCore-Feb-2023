namespace TeisterMask.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context.Projects
                .Where(p => p.Tasks.Any())
                .OrderByDescending(p => p.Tasks.Count)
                .ThenBy(p => p.Name)
                .Select(p => new ProjectExportDto()
                {
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate == null ? "No" : "Yes",
                    TasksCount = p.Tasks.Count,
                    Tasks = p.Tasks.Select(t => new TaskExportDto()
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                }).ToArray();

            return XmlHelper.Serialize<ProjectExportDto>(projects, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e =>
                e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date)
                    .OrderByDescending(et => et.Task.DueDate)
                    .ThenBy(et => et.Task.Name)
                    .Select(et => new
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("MM/dd/yyyy"),
                        DueDate = et.Task.DueDate.ToString("MM/dd/yyyy"),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    })
                    
                    .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees,Formatting.Indented);
                
                    
        }
    }
}