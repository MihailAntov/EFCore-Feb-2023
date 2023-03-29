namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var countryDtos = XmlHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");
            StringBuilder sb = new StringBuilder();
            ICollection<Country> validCountries = new HashSet<Country>();
            foreach (var countryDto in countryDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country validCountry = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };

                validCountries.Add(validCountry);
                sb.AppendLine(String.Format(SuccessfulImportCountry, validCountry.CountryName, validCountry.ArmySize));
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var manufacturerDtos = XmlHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");
            StringBuilder sb = new StringBuilder();
            ICollection<Manufacturer> validManufacturers = new HashSet<Manufacturer>();

            foreach (var manufacturerDto in manufacturerDtos)
            {
                if (!IsValid(manufacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (validManufacturers.Any(m => m.ManufacturerName == manufacturerDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string[] foundedArgs = manufacturerDto.Founded.Split(", ");

                if(foundedArgs.Length < 3)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string town = foundedArgs[foundedArgs.Length - 2];
                string country = foundedArgs[foundedArgs.Length - 1];
                string foundedLocation = $"{town}, {country}";

                Manufacturer validManufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };

                

                validManufacturers.Add(validManufacturer);
                sb.AppendLine(string.Format(SuccessfulImportManufacturer,validManufacturer.ManufacturerName, foundedLocation));
            }

            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var shellDtos = XmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");
            StringBuilder sb = new StringBuilder();
            ICollection<Shell> validShells = new HashSet<Shell>();

            foreach (var shellDto in shellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell validShell = new Shell()
                {
                    Caliber = shellDto.Caliber,
                    ShellWeight = shellDto.ShellWeight
                };

                validShells.Add(validShell);
                sb.AppendLine(string.Format(SuccessfulImportShell, validShell.Caliber, validShell.ShellWeight));


            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var gunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            ICollection<Gun> validGuns = new HashSet<Gun>();
            int[] existingCountries = context.Countries.Select(c => c.Id).ToArray();
            int[] existingShells = context.Shells.Select(s => s.Id).ToArray();
            foreach (var gunDto in gunDtos)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!Enum.TryParse<GunType>(gunDto.GunType, out GunType gunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //if(!existingShells.Contains(gunDto.ShellId))
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                Gun validGun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = gunType,
                    ShellId = gunDto.ShellId,
                    CountriesGuns = new HashSet<CountryGun>()
                };

                ICollection<CountryGun> validCountriesGuns = new HashSet<CountryGun>();

                foreach(var countryGunDto in gunDto.Countries)
                {
                    //if(!existingCountries.Contains(countryGunDto.Id))
                    //{
                    //    sb.AppendLine(ErrorMessage);
                    //    continue;
                    //}

                    validGun.CountriesGuns.Add(new CountryGun()
                    {
                        CountryId = countryGunDto.Id
                    });
                }

                validGuns.Add(validGun);
                sb.AppendLine(string.Format(SuccessfulImportGun, validGun.GunType, validGun.GunWeight, validGun.BarrelLength));


            }

            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}