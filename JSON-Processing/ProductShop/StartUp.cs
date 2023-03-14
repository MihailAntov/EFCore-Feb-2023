using Newtonsoft.Json;
//using System.Text.Json;

using ProductShop.Data;
using ProductShop.Models;
using ProductShop.DTOs.Import;
namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();


            //problem 1
            //string usersAsJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usersAsJson));

            //problem2
            //string productsAsJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productsAsJson));

            //problem3
            //string categoriesAsJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesAsJson));


            //problem4
            //string categoriesProductsAsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context,categoriesProductsAsJson));

            //problem5
            //Console.WriteLine(GetProductsInRange(context));

            //problem6
            //Console.WriteLine(GetSoldProducts(context));

            //problem7 -- NOT WORKING CORRECTLY
            Console.WriteLine(GetCategoriesByProductsCount(context));

            //problem8 -- INCORRECT PROBLEM DESCRIPTION
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {


            var categories = JsonConvert.DeserializeObject<CategoryDto[]>(inputJson)
                .Select(c => new Category()
                {
                    Name = c.Name
                });
            context.Categories.AddRange(categories.Where(c => c.Name != null));
            context.SaveChanges();

            return $"Successfully imported {categories.Where(c => c.Name != null).Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {


            var categoryProducts = JsonConvert.DeserializeObject<CategoryProductDto[]>(inputJson)
                .Select(cp => new CategoryProduct()
                {
                    CategoryId = cp.CategoryId,
                    ProductId = cp.ProductId
                });

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {

            var products = JsonConvert.DeserializeObject<List<ProductDto>>(inputJson)
                .Select(p => new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerId = p.SellerId,
                    BuyerId = p.BuyerId,
                });

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            var users = JsonConvert.DeserializeObject<UserDto[]>(inputJson)
                .Select(u => new User
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age
                });
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var results = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                });

            return JsonConvert.SerializeObject(results);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    })
                });



            return JsonConvert.SerializeObject(products);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count())
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count(),
                    //averagePrice = decimal.Round(c.CategoriesProducts.Average(cp => cp.Product.Price),2),
                    //totalRevenue = decimal.Round(c.CategoriesProducts.Sum(cp => cp.Product.Price),2)
                    averagePrice = c.CategoriesProducts.Any() ? $"{c.CategoriesProducts.Average(p => p.Product.Price):f2}" : "0.00",
                    totalRevenue = c.CategoriesProducts.Any() ? $"{c.CategoriesProducts.Sum(p=>p.Product.Price):f2}" : "0.00"
                });

            return JsonConvert.SerializeObject(categories,Formatting.Indented);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = new
            {

                usersCount = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null)).Count(),
                users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Where(p => p.Buyer != null).Count())
                .Select(u => new
                {

                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                       count = u.ProductsSold
                    .Where(ps => ps.Buyer != null).Count(),

                       products = u.ProductsSold
                    .Where(ps => ps.Buyer != null)
                    .Select(ps => new
                    {
                        name = ps.Name,
                        price = ps.Price
                    })
                    }
                })
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };


            return JsonConvert.SerializeObject(users, settings);

        }
    }
}