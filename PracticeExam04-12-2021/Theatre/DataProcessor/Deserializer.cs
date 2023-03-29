namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var playDtos = XmlHelper.Deserialize<ImportPlayDto[]>(xmlString, "Plays");
            StringBuilder sb = new StringBuilder();
            ICollection<Play> validPlays = new HashSet<Play>();
            foreach(var playDto in playDtos)
            {
                if(!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture,out TimeSpan duration))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!Enum.TryParse(typeof(Genre),playDto.Genre,out Object genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play validPlay = new Play()
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = playDto.Rating,
                    Genre = (Genre)genre,
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                validPlays.Add(validPlay);
                sb.AppendLine(string.Format(SuccessfulImportPlay, validPlay.Title, validPlay.Genre, validPlay.Rating));
            }

            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var castsDto = XmlHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");
            StringBuilder sb = new StringBuilder();
            ICollection<Cast> validCasts = new HashSet<Cast>();
            foreach(var castDto in castsDto)
            {
                if(!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast validCast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };
                string characterType = validCast.IsMainCharacter ? "main" : "lesser";
                validCasts.Add(validCast);
                sb.AppendLine(string.Format(SuccessfulImportActor, validCast.FullName, characterType));
            }

            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var theaterDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            ICollection<Theatre> validTheatres = new HashSet<Theatre>();

            foreach(var theatreDto in theaterDtos)
            {
                if(!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Ticket> validTickets = new HashSet<Ticket>();

                foreach(var ticketDto in theatreDto.Tickets)
                {
                    if(!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validTickets.Add(new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId
                    });
                }

                Theatre validTheatre = new Theatre()
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director,
                    Tickets = validTickets
                };
                validTheatres.Add(validTheatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, validTheatre.Name, validTickets.Count));

            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
