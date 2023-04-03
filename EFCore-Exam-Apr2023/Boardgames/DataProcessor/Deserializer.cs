namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Creator> validCreators = new HashSet<Creator>();
            var creatorDtos = XmlHelper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            foreach(var creatorDto in creatorDtos)
            {
                if(!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName
                };
                ICollection<Boardgame> validBoardgames = new HashSet<Boardgame>();
                foreach(var boardgameDto in creatorDto.Boardgames)
                {
                    if(!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validBoardgames.Add(new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType)boardgameDto.CategoryType,
                        Mechanics = boardgameDto.Mechanics
                    });


                }

                creator.Boardgames = validBoardgames;

                validCreators.Add(creator);
                sb.AppendLine(String.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, validBoardgames.Count));
            }
            context.Creators.AddRange(validCreators);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sellerDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);
            ICollection<Seller> validSellers = new HashSet<Seller>();
            StringBuilder sb = new StringBuilder();
            int[] existingBoardgames = context.Boardgames.Select(b => b.Id).ToArray();
            foreach(var sellerDto in sellerDtos)
            {
                if(!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<BoardgameSeller> validBoardgames = new HashSet<BoardgameSeller>();
                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website
                };

                foreach(int boardgameId in sellerDto.Boardgames.Distinct())
                {
                    if(!existingBoardgames.Contains(boardgameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validBoardgames.Add(new BoardgameSeller()
                    {
                        Seller = seller,
                        BoardgameId = boardgameId
                    });
                }

                seller.BoardgamesSellers = validBoardgames;
                validSellers.Add(seller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, validBoardgames.Count));

            }

            context.Sellers.AddRange(validSellers);
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
