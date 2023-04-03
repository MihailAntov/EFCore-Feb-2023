namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new
                {
                    CreatorName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames.Select(b => new ExportBoardgameDto()
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished
                    })
                    .OrderBy(b => b.BoardgameName)
                    .ToArray()
                })

                .Select(c => new ExportCreatorDto()
                {
                    CreatorName = c.CreatorName,
                    Boardgames = c.Boardgames,
                    BoardgamesCount = c.Boardgames.Length
                })
                .ToArray()
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName).ToArray();
                

            return XmlHelper.Serialize<ExportCreatorDto>(creators, "Creators");

        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(bs => bs.Boardgame.YearPublished >= year &&
                                                      bs.Boardgame.Rating <= rating))
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(bs => bs.Boardgame.YearPublished >= year &&
                               bs.Boardgame.Rating <= rating)
                    .Select(bs => new
                    {
                        Name = bs.Boardgame.Name,
                        Rating = bs.Boardgame.Rating,
                        Mechanics = bs.Boardgame.Mechanics,
                        Category = bs.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(b => b.Rating)
                    .ThenBy(b => b.Name)
                    .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellers,Formatting.Indented);
        }
    }
}