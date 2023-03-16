using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using AutoMapper.QueryableExtensions;

using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.DTOs.Export;
using ProductShop.Models;
using AutoMapper;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //problem 1
                //string usersXml = "../../../Datasets/users.xml";
                //Console.WriteLine(ImportUsers(context, usersXml));

                //problem2
                //string productsXml = File.ReadAllText("../../../Datasets/products.xml");
                //Console.WriteLine(ImportProducts(context, productsXml));


                //problem3
                //string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
                //Console.WriteLine(ImportCategories(context, categoriesXml));


                //problem4
                //string cateogiresProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");
                //Console.WriteLine(ImportCategoryProducts(context, cateogiresProductsXml));

                //problem5
                //Console.WriteLine(GetProductsInRange(context));

                //problem6
                //Console.WriteLine(GetSoldProducts(context));

                //problem7
                //Console.WriteLine(GetCategoriesByProductsCount(context));

                //problem8
                Console.WriteLine(GetUsersWithProducts(context));

            }
        }



        public static string ImportUsers(ProductShopContext context, string inputXml)
        {





            XDocument usersXml = XDocument.Load(inputXml);
            ICollection<User> users = new HashSet<User>();
            foreach (var user in usersXml.Root.Elements())
            {
                string firstName = user.Element("firstName").Value;
                string lastName = user.Element("lastName").Value;
                int age = int.Parse(user.Element("age").Value);

                users.Add(new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";


        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            StringReader reader = new StringReader(inputXml);

            XDocument productsXml = XDocument.Load(reader);

            ICollection<Product> products = new HashSet<Product>();

            foreach (var product in productsXml.Root.Elements())
            {
                string name = product.Element("name").Value;
                decimal price = decimal.Parse(product.Element("price").Value);
                int sellerId = int.Parse(product.Element("sellerId").Value);
                int? buyerId = null;
                if (product.Element("buyerId") != null)
                {
                    buyerId = int.Parse(product.Element("buyerId").Value);
                }


                products.Add(new Product()
                {
                    Name = name,
                    Price = price,
                    SellerId = sellerId,
                    BuyerId = buyerId
                });
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";


        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {

            StringReader reader = new StringReader(inputXml);

            XDocument categoriesXml = XDocument.Load(reader);
            ICollection<Category> categories = new HashSet<Category>();
            foreach (var category in categoriesXml.Root.Elements())
            {
                string name = category.Element("name").Value;
                categories.Add(new Category()
                {
                    Name = name
                });
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            StringReader reader = new StringReader(inputXml);

            XDocument categoriesProductsXml = XDocument.Load(reader);
            ICollection<CategoryProduct> categoriesProducts = new HashSet<CategoryProduct>();
            foreach (var categoryProduct in categoriesProductsXml.Root.Elements())
            {
                if (categoryProduct.Element("CategoryId") == null ||
                   categoryProduct.Element("ProductId") == null)
                {
                    continue;
                }


                int categoryId = int.Parse(categoryProduct.Element("CategoryId").Value);
                int productId = int.Parse(categoryProduct.Element("ProductId").Value);

                categoriesProducts.Add(new CategoryProduct()
                {
                    CategoryId = categoryId,
                    ProductId = productId
                });
            }

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ExportProductDto()
                {
                    ProductName = p.Name,
                    Price = p.Price,
                    BuyerFullName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .Take(10)
                .ToArray();

            return Serializer<ExportProductDto[]>(products, "Products");

        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new ExportUserWithSoldProductDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Products = u.ProductsSold.Select(p => new ExportSoldProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToArray()
                })
                .ToArray();

            return Serializer<ExportUserWithSoldProductDto[]>(users, "Users");
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoryDto()
                {
                    Name = c.Name,
                    ProductCount = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)

                })
                .OrderByDescending(c => c.ProductCount)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return Serializer<CategoryDto[]>(categories, "Categories");
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            UsersWithProductsDto root = new UsersWithProductsDto()
            {
                Users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count())
                .Take(10)
                .Select(u => new UserWithProductDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Products = new SoldProductsDto()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold                        
                        .Select(p =>
                        new ExportSoldProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }

                })
                .ToArray(),

                Count = context.Users
                .Where(u => u.ProductsSold.Any())
                .Count()
            };

            return Serializer<UsersWithProductsDto>(root, "Users");
        }
        private static string Serializer<T>(T dataTransferObjects, string xmlRootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));

            StringBuilder sb = new StringBuilder();
            using var write = new StringWriter(sb);

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(write, dataTransferObjects, xmlNamespaces);

            return sb.ToString();
        }
    }
}