namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Trucks.DataProcessor;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(c => c.Footballers.Any())
                .OrderByDescending(c => c.Footballers.Count)
                .ThenBy(c => c.Name)
                .Select(c => new CoachExportDto()
                {
                    CoachName = c.Name,
                    FootballersCount = c.Footballers.Count,
                    Footballers = c.Footballers
                    .OrderBy(f=>f.Name)
                    .Select(f =>
                    new FootballerExportDto()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    }).ToArray()
                }).ToArray();

            return XmlHelper.Serialize<CoachExportDto>(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                                    .ThenBy(tf => tf.Footballer.Name)
                                    .Select(tf => new
                                    {
                                        FootballerName = tf.Footballer.Name,
                                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("MM/dd/yyyy"),
                                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("MM/dd/yyyy"),
                                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                                        PositionType = tf.Footballer.PositionType.ToString()
                                    })
                                    
                                    .ToArray()

                })
                .OrderByDescending(t=>t.Footballers.Length)
                .ThenBy(t=>t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
