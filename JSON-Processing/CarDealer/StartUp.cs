using Newtonsoft.Json;


using CarDealer.Models;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.DTOs.Export;
using Microsoft.EntityFrameworkCore;


namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            
            using (CarDealerContext context = new CarDealerContext())
            {

                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();


                // problem 9
                //string suppliersAsJson = File.ReadAllText("../../../Datasets/suppliers.json");
                //Console.WriteLine(ImportSuppliers(context, suppliersAsJson));

                //problem 10
                //string partsAsJson = File.ReadAllText("../../../Datasets/parts.json");
                //Console.WriteLine(ImportParts(context, partsAsJson));

                //problem 11
                //string carsAsJson = File.ReadAllText("../../../Datasets/cars.json");
                //Console.WriteLine(ImportCars(context, carsAsJson));

                //problem 12
                //string customersAsJson = File.ReadAllText("../../../Datasets/customers.json");
                //Console.WriteLine(ImportCustomers(context, customersAsJson));

                //problem 13
                //string salesAsJson = File.ReadAllText("../../../Datasets/sales.json");
                //Console.WriteLine(ImportSales(context, salesAsJson));

                //problem 14
                //Console.WriteLine(GetOrderedCustomers(context));

                //problem 15
                //Console.WriteLine(GetCarsFromMakeToyota(context));

                //problem 16
                //Console.WriteLine(GetLocalSuppliers(context));

                //problem 17
                //Console.WriteLine(GetCarsWithTheirListOfParts(context));

                //problem 18
                Console.WriteLine(GetTotalSalesByCustomer(context));

                //problem 19
                //Console.WriteLine(GetSalesWithAppliedDiscount(context));


            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<SupplierDto[]>(inputJson)
                .Select(u => new Supplier
                {
                    Name = u.Name,
                    IsImporter = u.IsImporter,

                });
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<PartDto[]>(inputJson)
                .Select(p => new Part()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId
                })
                .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId));



            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            List<Car> cars = new List<Car>();
            List<PartCar> partsCars = new List<PartCar>();
            
            var carDtos = JsonConvert.DeserializeObject<CarDto[]>(inputJson);

            foreach (CarDto carDto in carDtos)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance
                   
                    
                };
                cars.Add(car);

                foreach (int partId in carDto.PartsId.Distinct())
                {
                    partsCars.Add(new PartCar()
                    {
                        PartId = partId,
                        Car = car
                    });
                }
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partsCars);

            context.SaveChanges();
            return $"Successfully imported {cars.Count()}.";
        }


        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            List<Customer> customers = new List<Customer>();
            var customerDtos = JsonConvert.DeserializeObject<CustomerDto[]>(inputJson);

            foreach(CustomerDto c in customerDtos)
            {
                Customer customer = new Customer()
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                };
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            List<Sale> sales = new List<Sale>();

            var saleDtos = JsonConvert.DeserializeObject<SaleDto[]>(inputJson);

            foreach(SaleDto s in saleDtos)
            {
                Sale sale = new Sale()
                {
                    Discount = s.Discount,
                    CarId = s.CarId,
                    CustomerId = s.CustomerId
                };
                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var result = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                }); 

            return JsonConvert.SerializeObject(result,Formatting.Indented);
        }


        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

           
            return JsonConvert.SerializeObject(cars,Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                });

            return JsonConvert.SerializeObject(suppliers,Formatting.Indented);

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance
                    },
                    parts = c.PartsCars.Select(p=> new
                    {
                        Name = p.Part.Name,
                        Price = $"{p.Part.Price:f2}"
                    })
                });

            return JsonConvert.SerializeObject(cars,Formatting.Indented);
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count() > 0)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenBy(c => c.boughtCars);


            return JsonConvert.SerializeObject(customers,Formatting.Indented);


        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = $"{s.Discount:f2}",
                    price = $"{s.Car.PartsCars.Sum(pc=>pc.Part.Price):f2}",
                    priceWithDiscount = $"{s.Car.PartsCars.Sum(pc => pc.Part.Price) * (100.0M - s.Discount) / 100.0M:f2}"
                });

            return JsonConvert.SerializeObject(sales,Formatting.Indented);
        }

        
    }
}