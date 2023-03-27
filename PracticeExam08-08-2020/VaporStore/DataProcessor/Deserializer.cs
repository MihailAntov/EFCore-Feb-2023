namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var games = JsonConvert.DeserializeObject<GameImportDto[]>(jsonString);
            List<Game> validGames = new List<Game>();
            StringBuilder sb = new StringBuilder();
            foreach(var game in games)
            {
                if(!IsValid(game))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game validGame = new Game();
                validGame.Name = game.Name;
                validGame.Price = game.Price;
                if(!DateTime.TryParse(game.ReleaseDate, out DateTime valid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validGame.ReleaseDate = valid;
                

                Developer? developer = validGames.Select(g => g.Developer)
                    .FirstOrDefault(d => d.Name == game.Developer);

                if(developer == null)
                {
                    developer = new Developer()
                    {
                        Name = game.Developer
                    };
                }
                validGame.Developer = developer;

                Genre? genre = validGames.Select(g => g.Genre)
                    .FirstOrDefault(g => g.Name == game.Genre);

                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = game.Genre
                    };
                }
                validGame.Genre = genre;

                List<Tag> validTags = new List<Tag>();

                foreach(string tag in game.Tags)
                {
                    Tag? validTag = validGames.SelectMany(g => g.GameTags.Select(gt => gt.Tag))
                        .FirstOrDefault(t => t.Name == tag);

                    if(validTag == null)
                    {
                        validTag = new Tag()
                        {
                            Name = tag
                        };
                    }

                    validTags.Add(validTag);
                }

                validGame.GameTags = validTags
                    .Select(t => new GameTag()
                    {
                        Game = validGame,
                        Tag = t
                    }).ToArray();


                validGames.Add(validGame);
                sb.AppendLine(String.Format(SuccessfullyImportedGame,validGame.Name, validGame.Genre.Name, validGame.GameTags.Count));

            }

            context.AddRange(validGames);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var users = JsonConvert.DeserializeObject<UserImportDto[]>(jsonString);
            List<User> validUsers = new List<User>();
            StringBuilder sb = new StringBuilder();

            foreach(var user in users)
            {
                if(!IsValid(user))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User validUser = new User();

                validUser.Username = user.Username;
                validUser.FullName = user.FullName;
                validUser.Age = user.Age;
                validUser.Email = user.Email;

                
                if(user.Cards.Any(c=> Enum.Parse(typeof(CardType),c.Type) == null))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validUser.Cards = user.Cards.Select(c => new Card()
                {
                    Number = c.Number,
                    Cvc = c.CVC,
                    Type = (CardType)Enum.Parse(typeof(CardType), c.Type)
                }).ToArray();

                validUsers.Add(validUser);
                sb.AppendLine(String.Format(SuccessfullyImportedUser, validUser.Username, validUser.Cards.Count));

            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var purchases = XmlHelper.Deserialize<PurchaseImportDto[]>(xmlString, "Purchases");
            List<Purchase> validPurchases = new List<Purchase>();
            foreach(var purchase in purchases)
            {
                if(!IsValid(purchase))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                Purchase validPurchase = new Purchase();
                Game? game = context.Games
                    .FirstOrDefault(g => g.Name == purchase.Title);
                if(game == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
        

                Card? card = context.Cards
                    .FirstOrDefault(c => c.Number == purchase.Card);
                if(card == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!Enum.TryParse(typeof(PurchaseType), purchase.Type, out var type))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!DateTime.TryParseExact(purchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var time))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validPurchase.Game = game;
                validPurchase.Card = card;
                validPurchase.Type = (PurchaseType)type;
                validPurchase.Date = time;
                validPurchase.ProductKey = purchase.Key;

                validPurchases.Add(validPurchase);
                sb.AppendLine(String.Format(SuccessfullyImportedPurchase, validPurchase.Game.Name, validPurchase.Card.User.Username));



            }

            context.AddRange(validPurchases);
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