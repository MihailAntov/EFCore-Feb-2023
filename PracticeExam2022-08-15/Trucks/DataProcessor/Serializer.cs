namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    using Trucks.DataProcessor.ExportDto;

    

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = context.Despatchers
                .Where(d => d.Trucks.Any())
                .Select(d => new DespatcherDto()
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count,
                    Trucks = d.Trucks.Select(t => new TruckDto()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    }).OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            return XmlHelper.Serialize(despatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var despatchers = context.Clients
                .Where(d => d.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .Select(d => new
                {
                    Name = d.Name,
                    Trucks = d.ClientsTrucks
                    .Where(t => t.Truck.TankCapacity >= capacity)
                    .OrderBy(t => t.Truck.MakeType)
                    .ThenByDescending(t => t.Truck.CargoCapacity)
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        VinNumber = t.Truck.VinNumber,
                        TankCapacity = t.Truck.TankCapacity,
                        CargoCapacity = t.Truck.CargoCapacity,
                        CategoryType = t.Truck.CategoryType.ToString(),
                        MakeType = t.Truck.MakeType.ToString()
                    })
                    .ToArray()

                })
                .OrderByDescending(d => d.Trucks.Length)
                .ThenBy(d=>d.Name)
                .Take(10);

            return JsonConvert.SerializeObject(despatchers,Formatting.Indented);
        }
    }
}
