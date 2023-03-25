namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Trucks.DataProcessor;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        
        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            ImportCoachDto[] coaches = XmlHelper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");
            IList<Coach> validCoaches = new List<Coach>();
            IList<FootballerImportDto> viewFootballers = new List<FootballerImportDto>();
            StringBuilder sb = new StringBuilder();
            foreach(var coach in coaches)
            {
                if(!IsValid(coach))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<FootballerImportDto> validFootballers = new List<FootballerImportDto>();

                foreach(var footballer in coach.Footballers)
                {
                    if(!IsValid(footballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var contractStartDate = DateTime.ParseExact(footballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var contractEndDate = DateTime.ParseExact(footballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (contractEndDate < contractStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    validFootballers.Add(footballer);
                    viewFootballers.Add(footballer);
                }
                
                
                validCoaches.Add(new Coach()
                {
                    Name = coach.Name,
                    Nationality = coach.Nationality,
                    Footballers = validFootballers
                    .Select(f =>
                    new Footballer()
                    {
                        Name = f.Name,
                        ContractStartDate = DateTime.ParseExact(f.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ContractEndDate = DateTime.ParseExact(f.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BestSkillType = (BestSkillType)f.BestSkillType,
                        PositionType = (PositionType)f.PositionType
                    })
                    .ToArray()
                }); ;
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, validFootballers.Count));

            }
            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
            
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teams = JsonConvert.DeserializeObject<TeamImportDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            HashSet<int> existingPlayers = context.Footballers.Select(f => f.Id).ToHashSet();
            List<Team> validTeams = new List<Team>();

            foreach(var team in teams)
            {
                if(!IsValid(team) || team.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<int> validPlayers = new List<int>();
                foreach(int playerId in team.Footballers.Distinct())
                {
                    if(!existingPlayers.Contains(playerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    validPlayers.Add(playerId);
                }

                validTeams.Add(new Team()
                {
                    Name = team.Name,
                    Nationality = team.Nationality,
                    TeamsFootballers = validPlayers
                    .Select(p => new TeamFootballer()
                    {
                        FootballerId = p
                    }).ToArray()
                });

                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, validPlayers.Count));
                
            }

            context.Teams.AddRange(validTeams);
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
