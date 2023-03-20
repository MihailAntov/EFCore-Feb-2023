namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        
        
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {

            var despatchers = XmlHelper.Deserialize<DespatcherDto[]>(xmlString, "Despatchers");
            StringBuilder sb = new StringBuilder();

            List<Despatcher> validDespatchers = new List<Despatcher>();
            foreach(var despatcher in despatchers)
            {
                if(!IsValid(despatcher))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<Truck> validTrucks = new List<Truck>();

                foreach(TruckDto truck in despatcher.Trucks)
                {
                    if(!IsValid(truck))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    validTrucks.Add(new Truck()
                    {
                        RegistrationNumber = truck.RegistrationNumber,
                        VinNumber = truck.VinNumber,
                        TankCapacity = truck.TankCapacity,
                        CargoCapacity = truck.CargoCapacity,
                        CategoryType = (CategoryType)truck.CategoryType,
                        MakeType = (MakeType)truck.MakeType,
                        
                        
                    });

                }

                validDespatchers.Add(new Despatcher()
                {
                    Name = despatcher.Name,
                    Position = despatcher.Position,
                    Trucks = validTrucks
                });
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, validTrucks.Count));


            }

            context.Despatchers.AddRange(validDespatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            HashSet<int> existingTrucks = context.Trucks.Select(t => t.Id).ToHashSet();

            var clients = JsonConvert.DeserializeObject<ClientDto[]>(jsonString);
            List<Client> validClients = new List<Client>();
            List<int> viewTrucks = new List<int>();
            foreach(ClientDto client in clients)
            {
                if(!IsValid(client) || client.Type=="usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                List<int> validTrucks = new List<int>();
                foreach(int truckId in client.Trucks.Distinct())
                {
                    if(!existingTrucks.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    validTrucks.Add(truckId);

                }

                validClients.Add(new Client()
                {
                    Name = client.Name,
                    Nationality =client.Nationality,
                    Type = client.Type,
                    ClientsTrucks = validTrucks.Select(vt=> new ClientTruck()
                    {
                        TruckId = vt
                    }).ToArray()
                });
                viewTrucks.AddRange(validTrucks);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, validTrucks.Count));
            }

            context.Clients.AddRange(validClients);
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