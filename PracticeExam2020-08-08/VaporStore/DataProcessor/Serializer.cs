namespace VaporStore.DataProcessor
{ 
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ExportDto;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                 .Where(g => genreNames.Contains(g.Name))
                 .Select(g => new GenreExportDto()
                 {
                     Id = g.Id,
                     Genre = g.Name,
                     Games = g.Games
                        .Where(g => g.Purchases.Any())
                        .Select(g => new GameExportDto()
                        {
                            Id = g.Id,
                            Title = g.Name,
                            Developer = g.Developer.Name,
                            Tags = String.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
                            Players = g.Purchases.Count
                        })
                        .OrderByDescending(g => g.Players)
                        .ThenBy(g => g.Id)
                        .ToArray(),

                     TotalPlayers = g.Games.Sum(g => g.Purchases.Count())
                 }).OrderByDescending(g => g.TotalPlayers)
                    .ThenBy(g => g.Id)
                    .ToArray();
            
            return JsonConvert.SerializeObject(genres, Formatting.Indented);
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            PurchaseType type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseType);

            var users = context.Users

                .Where(u => u.Cards.Select(c => c.Purchases.Where(p=> p.Type == type)).Any())
                
                .Select(u => new UserExportDto()
                {
                    UserName = u.Username,
                    Purchases = u.Cards.SelectMany(c => c.Purchases.Where(p=>p.Type == type))
                                        .OrderBy(p=>p.Date)
                                        .Select(p => new PurchaseExportDto()
                                        {
                                            Card = p.Card.Number,
                                            Cvc = p.Card.Cvc,
                                            Date = p.Date.ToString("yyyy-MM-dd HH:mm"),
                                            Game = new GamePurchaseExportDto()
                                            {
                                                Title = p.Game.Name,
                                                Genre = p.Game.Genre.Name,
                                                Price = p.Game.Price
                                            }
                                        })
                                        
                                        .ToArray(),
                    
                    
                })
                
                .ToArray();

            users = users
                .Where(u=>u.Purchases.Any())
                .Select(u=> new UserExportDto()
                {
                    UserName = u.UserName,
                    Purchases = u.Purchases,
                    TotalSpent = u.Purchases.Sum(p=>p.Game.Price)
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.UserName)
                .ToArray();


            return XmlHelper.Serialize<UserExportDto>(users, "Users");

        }
    }
}