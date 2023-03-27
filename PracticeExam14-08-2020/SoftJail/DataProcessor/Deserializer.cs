namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var departments = JsonConvert.DeserializeObject<DepartmentImportDto[]>(jsonString);
            List<Department> validDepartments = new List<Department>();

            foreach(DepartmentImportDto department in departments)
            {
                if(!IsValid(department)
                    || !department.Cells.Any()
                    || department.Cells.Any(c=>!IsValid(c)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department validDepartment = new Department()
                {
                    Name = department.Name,
                    Cells = department.Cells.Where(c => IsValid(c))
                    .Select(c => new Cell()
                    {
                        CellNumber = c.CellNumber,
                        HasWindow = c.HasWindow,
                    }).ToArray()
                };
                sb.AppendLine(String.Format(SuccessfullyImportedDepartment, validDepartment.Name, validDepartment.Cells.Count));
                validDepartments.Add(validDepartment); 

            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
            
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisoners = JsonConvert.DeserializeObject<PrisonerImportDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Prisoner> validPrisoners = new List<Prisoner>();
            foreach(var prisoner in prisoners)
            {
                if(!IsValid(prisoner))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(prisoner.Mails.Any(m=>!IsValid(m)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime incarcerationDate;
                DateTime? releaseDate = null;
                try
                {
                    incarcerationDate = DateTime.ParseExact(
                        prisoner.IncarcerationDate,"dd/MM/yyyy",CultureInfo.InvariantCulture
                        );
                }
                catch (Exception ex)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(prisoner.ReleaseDate != null)
                {
                    try
                    {
                        releaseDate = DateTime.ParseExact(
                            prisoner.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture
                            );
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                }

                List<MailImportDto> validMails = prisoner.Mails
                    .Where(m => IsValid(m))
                    .ToList();

                Prisoner validPrisoner = new Prisoner();
                validPrisoner.FullName = prisoner.Fullname;
                validPrisoner.Nickname = prisoner.Nickname;
                validPrisoner.Age = prisoner.Age;
                validPrisoner.IncarcerationDate = incarcerationDate;
                validPrisoner.ReleaseDate = releaseDate;
                validPrisoner.CellId = prisoner.CellId;
                validPrisoner.Bail = prisoner.Bail;
                validPrisoner.Mails = validMails.Select(m=>
                new Mail()
                { 
                    Description = m.Description,
                    Sender = m.Sender,
                    Address = m.Address
                }).ToArray();

                sb.AppendLine(String.Format(SuccessfullyImportedPrisoner, validPrisoner.FullName, validPrisoner.Age));
                
                validPrisoners.Add(validPrisoner);
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var officers = XmlHelper.Deserialize<OfficerImportDto[]>(xmlString, "Officers");


            List<Officer> validOfficers = new List<Officer>();
            List<OfficerPrisoner> validOfficersPrisoners = new List<OfficerPrisoner>();
            StringBuilder sb = new StringBuilder();
            int[] existingPrisoners = context.Prisoners.Select(p => p.Id).ToArray();
            foreach(var officer in officers)
            {
                if(!IsValid(officer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Officer validOfficer = new Officer();

                try
                {
                    validOfficer.FullName = officer.Name;
                    validOfficer.Salary = officer.Money;
                    validOfficer.Position = (Position)Enum.Parse(typeof(Position), officer.Position);
                    validOfficer.Weapon = (Weapon)Enum.Parse(typeof(Weapon), officer.Weapon);
                    validOfficer.DepartmentId = officer.DepartmentId;

                    

                    validOfficer.OfficerPrisoners = officer.Prisoners
                        .Select(p => new OfficerPrisoner()
                        {
                            Officer = validOfficer,
                            PrisonerId = p.Id
                        }).ToArray();

                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validOfficers.Add(validOfficer);

                sb.AppendLine(string.Format(SuccessfullyImportedOfficer, validOfficer.FullName, validOfficer.OfficerPrisoners.Count));
                

            }
            context.Officers.AddRange(validOfficers);
            context.SaveChanges();


            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}