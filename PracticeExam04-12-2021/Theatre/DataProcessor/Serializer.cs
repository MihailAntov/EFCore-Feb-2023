namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theaters = context.Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    Tickets = t.Tickets
                        .Where(t => t.RowNumber <= 5)
                        .Select(t => new
                        {
                            Price = t.Price,
                            RowNumber = t.RowNumber
                        })
                        .OrderByDescending(t => t.Price)
                        .ToArray()
                })
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.Halls,
                    TotalIncome = t.Tickets.Sum(t => t.Price),
                    Tickets = t.Tickets
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonConvert.SerializeObject(theaters,Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {

            const string mainCharacterMessage = "Plays main character in '{0}'.";

            var plays = context.Plays
                .Where(p => p.Rating <= raiting)
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .Select(p => new ExportPlayDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                        .Where(c => c.IsMainCharacter)
                        .OrderByDescending(c => c.FullName)
                        .Select(c => new ExportCastDto()
                        {
                            FullName = c.FullName,
                            MainCharacter = String.Format(mainCharacterMessage, p.Title)
                        }).ToArray()
                }).ToArray();

            return XmlHelper.Serialize<ExportPlayDto>(plays, "Plays");
        }
    }
}
