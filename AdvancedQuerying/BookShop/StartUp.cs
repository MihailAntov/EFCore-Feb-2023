namespace BookShop
{
    using Data;
    using Models;
    using Models.Enums;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //Problem 2:
            //string command = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, command));

            //Problem 3:
            //Console.WriteLine(GetGoldenBooks(db));

            //Problem 4:
            //Console.WriteLine(GetBooksByPrice(db));


            //Problem 5:
            //Console.WriteLine(GetBooksNotReleasedIn(db, 2000));


            //Problem 6:
            //string category = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, category));


            //Problem 7:
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));


            //Problem 8:
            //string name = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, name));


            //Problem 9:
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, input));


            //Problem 10:
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, input));


            //Problem 11:
            //int input = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, input));


            //Problem 12:
            //Console.WriteLine(CountCopiesByAuthor(db));


            //Problem 13:
            //Console.WriteLine(GetTotalProfitByCategory(db));


            //Problem 14:
            //Console.WriteLine(GetMostRecentBooks(db));


            //Problem 15:
            //IncreasePrices(db);


            //Problem 16:
            //Console.WriteLine(RemoveBooks(db));





        }

        //problem 2
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (AgeRestriction.TryParse(command.ToLower(), true, out AgeRestriction ageRestriction))
            {
                var books = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();

                return string.Join(Environment.NewLine, books);
            }
            return string.Empty;
        }
        //probmel 3
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            return String.Join(Environment.NewLine, books);


        }
        //problem 4
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}");

            return String.Join(Environment.NewLine, books);
        }

        //problem 5
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b=>b.Title);

            return String.Join(Environment.NewLine, books);
        }

        //problem 6
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(" ");

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            return String.Join(Environment.NewLine, books);
        }

        //problem 7
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime releaseDate = DateTime.ParseExact(date, "dd-MM-yyyy",CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate < releaseDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}");
            return String.Join(Environment.NewLine, books);
        }

        //problem 8
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {

            var authorsNames = context.Authors
                .Select(a => new { a.FirstName, a.LastName })
                .ToList()
                .Where(a=> a.FirstName.EndsWith(input))
                .Select(a=>$"{a.FirstName} {a.LastName}")
                .OrderBy(a=>a);
                

            return String.Join(Environment.NewLine, authorsNames);
        }

        //problem 9
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            return String.Join(Environment.NewLine,books);
        }

        //problem 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Include(b => b.Author)
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");



                return String.Join(Environment.NewLine, books);
        }

        //problem 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int result = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();
            return result;
        }

        //problem 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .OrderByDescending(a=> a.Books.Select(b => b.Copies).Sum())
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books.Select(b=>b.Copies).Sum()
                })
                .Select(a=> $"{a.FirstName} {a.LastName} - {a.TotalCopies}");

            return String.Join(Environment.NewLine, authors);
        }

        //problem 13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfits = c.CategoryBooks.Select(b => b.Book.Price * b.Book.Copies).Sum()
                })
                .OrderByDescending(c => c.TotalProfits)
                .ThenBy(c => c.Name)
                .Select(c => $"{c.Name} ${c.TotalProfits:f2}");

            return String.Join(Environment.NewLine, categories);
                
        }

        //problem 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks.OrderByDescending(b => b.Book.ReleaseDate)
                    .Take(3)
                    .Select(b => new
                    {
                        b.Book.Title,
                        Year = b.Book.ReleaseDate.Value.Year

                    })
                });

            StringBuilder sb = new StringBuilder();

            foreach(var category in categories)
            {
                sb.AppendLine($"--{category.Name}");
                foreach(var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //problem 15
        public static void IncreasePrices(BookShopContext context)
        {
            List<Book> books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach(Book book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //problem 16
        public static int RemoveBooks(BookShopContext context)
        {
            List<Book> booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            int result = booksToRemove.Count;

            foreach(Book book in booksToRemove)
            {
                context.Books.Remove(book);
            }
            context.SaveChanges();

            

            return result;
        }


    }

}


