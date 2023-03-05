using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillainNames
{
    public static class SQLQueries
    {
        public static string GetVillainsWithMinionCount =
            @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                FROM Villains AS v
                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
            GROUP BY v.Id, v.Name
              HAVING COUNT(mv.VillainId) > 3 
            ORDER BY COUNT(mv.VillainId)";


        public const string GetVillainNameById = @"SELECT Name FROM Villains WHERE Id = @Id";

        public static string GetMinionsByVillainId =
            @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                                 m.Name, 
                                                 m.Age
                                            FROM MinionsVillains AS mv
                                            JOIN Minions As m ON mv.MinionId = m.Id
                                           WHERE mv.VillainId = @Id
                                        ORDER BY m.Name";

        public static string CheckIfVillainExists =
            @"SELECT 
                    COUNT(*)
                FROM Villains 
                WHERE Name = @villainName";

        public static string AddVillainByName =
            @"INSERT INTO Villains 
               (Name, EvilnessFactorId)
                VALUES
                (@villainName, 4)";

        public static string CheckIfTownExists =
            @"SELECT 
                    COUNT(*)
                FROM Towns 
                WHERE Name = @townName";

        public static string CheckIfCountryExists =
            @"SELECT 
                    COUNT(*)
                FROM Countries 
                WHERE Name = @countryName";

        public static string findCountryCodeByName =
            @"SELECT TOP(1)
                c.Id
            FROM Towns AS t
            JOIN Countries AS c ON t.CountryCode = c.Id
			WHERE c.Name = @countryName";

        public static string FindTownNamesByCountryCode =
            @"SELECT
                Name
            FROM Towns
            WHERE CountryCode = @countryCode ";

        public static string FindTownCountByCountryCode =
            @"SELECT
                COUNT(*)
            FROM Towns
            WHERE CountryCode = @countryCode ";

        public static string UpcaseTownNames =
            @"UPDATE Towns
                SET Name = UPPER(Name)
                WHERE CountryCode = @countryCode";

        public static string InsertTownByName =
            @"INSERT INTO Towns 
               (Name)
                VALUES
                (@townName)";

        public static string GetVillainId =
            @"SELECT 
                Id
            FROM Villains
            WHERE Name = @villainName";

        public static string GetTownId =
            @"SELECT 
                Id
            FROM Towns
            WHERE Name = @townName";

        public static string GetMinionId =
            @"SELECT TOP(1)
                Id
            FROM Minions
            WHERE Name = @minionName
            ORDER BY Id DESC";

        public static string AddMinion =
            @"INSERT INTO Minions
                (Name,Age, TownId)
                VALUES
                (@name, @age, @townId)";
        public static string AddMinionVillain = 
            @"INSERT INTO MinionsVillains
                (MinionId, VillainId)
                VALUES
                (@minionId, @villainId)";

        public static string FindVillainById =
            @"SELECT 
                Name
                FROM Villains 
                WHERE Id = @villainId";

        public static string FindMinionCountByVillainId =
            @"SELECT 
                COUNT(*)
                FROM Villains AS v
				JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                WHERE v.Id = @villainId";

        public static string DeleteVillainById =
            @"DELETE
                FROM Villains
                WHERE Id = @villainId";

        public static string ReleaseMinionsById =
            @"DELETE FROM MinionsVillains                
                WHERE VillainId = @villainId";

        public static string GetAllMinionNames =
            @"SELECT Name FROM Minions";

        public static string IncreaseAgeAndLowerCaseName =
            @"UPDATE Minions
	SET Name = LOWER(SUBSTRING(Name,1,1))+SUBSTRING(Name,2,LEN(Name)),
		Age = Age +1
		WHERE Id = @minionId";

        public static string GetAllMinionsAndAges =
            @"SELECT
                Name, Age
            FROM Minions";

        public static string IncreaseAgeWithUsp =
            @"EXEC usp_GetOlder @minionId";

        public static string GetMinionById =
            @"SELECT 
                Name,
                Age
            FROM Minions
            WHERE Id = @minionId";

    }
}
