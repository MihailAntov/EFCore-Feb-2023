using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

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

                //string suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
                //Console.WriteLine(ImportSuppliers(context, suppliersXml));

                //string partsXml = File.ReadAllText("../../../Datasets/parts.xml");
                //Console.WriteLine(ImportParts(context, partsXml));

                //string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
                //Console.WriteLine(ImportCars(context, carsXml));

                //string customersXml = File.ReadAllText("../../../Datasets/customers.xml");
                //Console.WriteLine(ImportCustomers(context, customersXml));

                //string salesXml = File.ReadAllText("../../../Datasets/sales.xml");
                //Console.WriteLine(ImportSales(context, salesXml));

                //Console.WriteLine(GetCarsWithDistance(context));

                //Console.WriteLine(GetCarsFromMakeBmw(context));

                //Console.WriteLine(GetLocalSuppliers(context));

                //Console.WriteLine(GetCarsWithTheirListOfParts(context));

                //Console.WriteLine(GetTotalSalesByCustomer(context));

                Console.WriteLine(GetSalesWithAppliedDiscount(context));

            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            TextReader reader = new StringReader(inputXml);

            XDocument xDocument = XDocument.Load(reader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (var supplier in xDocument.Root.Elements())
            {
                suppliers.Add(new Supplier()
                {
                    Name = supplier.Element("name").Value,
                    IsImporter = bool.Parse(supplier.Element("isImporter").Value)

                });
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";

        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            TextReader textReader = new StringReader(inputXml);
            XDocument xDocument = XDocument.Load(textReader);
            ICollection<Part> parts = new HashSet<Part>();

            ICollection<int> suppliers = context.Suppliers
                .Select(s => s.Id)
                .ToHashSet();

            foreach (var part in xDocument.Root.Elements())
            {
                int supplierId = int.Parse(part.Element("supplierId").Value);
                if (!suppliers.Contains(supplierId))
                {
                    continue;
                }
                string name = part.Element("name").Value;
                decimal price = decimal.Parse(part.Element("price").Value);
                int quantity = int.Parse(part.Element("quantity").Value);


                parts.Add(new Part()
                {
                    Name = name,
                    Price = price,
                    Quantity = quantity,
                    SupplierId = supplierId
                });


            }

            context.Parts.AddRange(parts.Where(p => suppliers.Contains(p.SupplierId)));
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";

        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            TextReader textReader = new StringReader(inputXml);
            XDocument carsXml = XDocument.Load(textReader);
            List<int> partIds = context.Parts.Select(p => p.Id).ToList();

            ICollection<Car> cars = new HashSet<Car>();

            foreach(var car in carsXml.Root.Elements())
            {
                string make = car.Element("make").Value;
                string model = car.Element("model").Value;
                int travelDistance = int.Parse(car.Element("traveledDistance").Value);

                var inputPartIds = car.Element("parts").Elements();
                List<int> validPartIds = new List<int>();
                foreach (var partId in inputPartIds)
                {
                    XAttribute attr = partId.Attribute("id");
                    int value = int.Parse(attr.Value);
                    if(partIds.Contains(value) && !validPartIds.Contains(value))
                    {
                        validPartIds.Add(value);
                    }
                }


                

                cars.Add(new Car()
                {
                    Make = make,
                    Model = model,
                    TraveledDistance = travelDistance,
                    PartsCars = validPartIds.Select(id => new PartCar()
                    {
                        PartId = id
                    })
                    .ToList()
                });

                
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";

        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            TextReader reader = new StringReader(inputXml);
            XDocument doc = XDocument.Load(reader);
            ICollection<Customer> customers = new HashSet<Customer>();

            foreach(var customer in doc.Root.Elements())
            {
                string name = customer.Element("name").Value;
                DateTime birthDate = DateTime.Parse(customer.Element("birthDate").Value);
                bool isYoungDriver = bool.Parse(customer.Element("isYoungDriver").Value);

                customers.Add(new Customer()
                {
                    Name = name,
                    BirthDate = birthDate,
                    IsYoungDriver = isYoungDriver
                });
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();



            return $"Successfully imported {customers.Count}";
        }


        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            TextReader reader = new StringReader(inputXml);
            XDocument doc = XDocument.Load(reader);
            HashSet<int> existingCarIds = context
                .Cars
                .AsNoTracking()
                .Select(c => c.Id)
                .ToHashSet();
            ICollection<Sale> sales = new HashSet<Sale>();
            foreach(var saleXml in doc.Root.Elements())
            {
                int carId = int.Parse(saleXml.Element("carId").Value);
                if(!existingCarIds.Contains(carId))
                {
                    continue;
                }

                int customerId = int.Parse(saleXml.Element("customerId").Value);
                int discount = int.Parse(saleXml.Element("discount").Value);

                Sale sale = new Sale()
                {
                    CarId = carId,
                    CustomerId = customerId,
                    Discount = discount
                };
                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TraveledDistance > 2_000_000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new CarDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TraveledDistance
                }).ToArray();

            return Serializer<CarDto[]>(cars, "cars");
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c=> new CarBmwDto()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TraveledDistance
                })
                .ToArray();

            return Serializer<CarBmwDto[]>(cars, "cars");
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new LocalSupplierDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();

            return Serializer<LocalSupplierDto[]>(suppliers, "suppliers");
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .Select(c => new CarWithListOfPartsDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TraveledDistance,
                    Parts = c.PartsCars
                    .Select(pc => new PartDto()
                    {
                       Name = pc.Part.Name,
                       Price = pc.Part.Price
                    })
                    .OrderByDescending(p=>p.Price)
                    .ToArray()
                }).ToArray();

            return Serializer<CarWithListOfPartsDto[]>(cars, "cars");
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersTemp = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    Name = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SalesAmounts = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver
                        ? s.Car.PartsCars.Sum(pc => pc.Part.Price * 0.95M)
                        : s.Car.PartsCars.Sum(pc => pc.Part.Price)
                    })
                }).ToArray();

            var customers = customersTemp
                .OrderByDescending(c => c.SalesAmounts.Sum(s => s.Prices))
                .Select(c => new CustomerDto()
                {
                    Name = c.Name,
                    BoughtCars = c.BoughtCars,
                    SpentMoney = Math.Round(c.SalesAmounts.Sum(sa => sa.Prices), 2).ToString("f2")
                }).ToArray();
                

            return Serializer<CustomerDto[]>(customers, "customers");
                
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SalesWithDiscountDto()
                {
                    CarSaleDto = new CarSaleDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TraveledDistance
                    },
                    Discount = (int)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(pc=>pc.Part.Price),
                    PriceWithDiscount = Math.Round(s.Car.PartsCars.Sum(pc => pc.Part.Price) * (100m - s.Discount) / 100m,4)

                }).ToArray();

            return Serializer<SalesWithDiscountDto[]>(sales, "sales");
        }

        public static string Serializer<T>(T dataTransferObject, string rootElementName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));

            StringBuilder sb = new StringBuilder();
            using var write = new StringWriter(sb);

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(write, dataTransferObject, xmlNamespaces);

            return sb.ToString();
        }
    }
}