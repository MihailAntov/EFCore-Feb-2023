using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VillainNames
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            SqlConnection con = new SqlConnection(@"Server=.;Database=MinionsDB;Trusted_Connection=True;TrustServerCertificate=true;");

            con.Open();
            using (con)
            {
                //await GetVillainsWithMinionCount(con);
                //await GetMinionsByVillainId(con);
                //await AddMinion(con);
                //await ChangeTownNamesCasing(con); 
                //await RemoveVillain(con);
                await PrintAllMinionNames(con);

                



            }
        }

        public async static Task GetVillainsWithMinionCount(SqlConnection con)
        {
            SqlCommand command = new SqlCommand(SQLQueries.GetVillainsWithMinionCount, con);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string villainName = (string)reader["Name"];
                int minionCount = (int)reader["MinionsCount"];

                Console.WriteLine($"{villainName} - {minionCount}");
            }
        }

        public async static Task GetMinionsByVillainId(SqlConnection con)
        {
            SqlCommand command = new SqlCommand(SQLQueries.GetVillainNameById, con);
            int villainId = int.Parse(Console.ReadLine());
            command.Parameters.AddWithValue("@Id", villainId);

            object villainReader = await command.ExecuteScalarAsync();

            if (villainReader != null)
            {
                Console.WriteLine($"Villain: {(string)villainReader}");
            }
            else
            {
                Console.WriteLine($"No villain with ID {villainId} exists in the database.");
            }

            SqlCommand minionCommand = new SqlCommand(SQLQueries.GetMinionsByVillainId, con);
            minionCommand.Parameters.AddWithValue("@Id", villainId);

            SqlDataReader minionReader = await minionCommand.ExecuteReaderAsync();
            if (!minionReader.HasRows)
            {
                Console.WriteLine("(no minions)");
            }

            while (minionReader.Read())
            {
                long rowNum = (long)minionReader["RowNum"];
                string minionName = (string)minionReader["Name"];
                int minionAge = (int)minionReader["Age"];

                Console.WriteLine($"{rowNum}. {minionName} {minionAge}");
            }
        }

        public async static Task AddMinion(SqlConnection con)
        {
            SqlTransaction transaction = con.BeginTransaction();
            
            string[] minionArgs = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string[] VillainArgs = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string minionName = minionArgs[1];
            int minionAge = int.Parse(minionArgs[2]);
            string minionTown = minionArgs[3];

            string villainName = VillainArgs[1];

            SqlCommand checkVillain = new SqlCommand(SQLQueries.CheckIfVillainExists, con, transaction);
            checkVillain.Parameters.AddWithValue("@villainName", villainName);
            int villainCount = (int)await checkVillain.ExecuteScalarAsync();

            if(villainCount == 0)
            {
                SqlCommand addVillain = new SqlCommand(SQLQueries.AddVillainByName, con, transaction);
                addVillain.Parameters.AddWithValue("@villainName", villainName);
                await addVillain.ExecuteNonQueryAsync();
                Console.WriteLine($"Villain {villainName} was added to the database.");
            }

            SqlCommand checkTown = new SqlCommand(SQLQueries.CheckIfTownExists, con, transaction);
            checkTown.Parameters.AddWithValue("@townName", minionTown);

            int townCount = (int)await checkTown.ExecuteScalarAsync();
            if(townCount == 0)
            {
                SqlCommand addTown = new SqlCommand(SQLQueries.InsertTownByName, con, transaction);
                addTown.Parameters.AddWithValue("@townName", minionTown);
                await addTown.ExecuteNonQueryAsync();
                Console.WriteLine($"Town {minionTown} was added to the database.");
            }

            SqlCommand getTownId = new SqlCommand(SQLQueries.GetTownId, con, transaction);
            getTownId.Parameters.AddWithValue("@townName", minionTown);
            int townId = (int)await getTownId.ExecuteScalarAsync();

            SqlCommand getVillainId = new SqlCommand(SQLQueries.GetVillainId, con, transaction);
            getVillainId.Parameters.AddWithValue("@villainName", villainName);
            int villainId = (int)await getVillainId.ExecuteScalarAsync();

            SqlCommand addMinion = new SqlCommand(SQLQueries.AddMinion, con, transaction);
            addMinion.Parameters.AddWithValue("@name", minionName);
            addMinion.Parameters.AddWithValue("@age", minionAge);
            addMinion.Parameters.AddWithValue("@townId", townId);
            await addMinion.ExecuteNonQueryAsync();

            SqlCommand getMinionId = new SqlCommand(SQLQueries.GetMinionId, con, transaction);
            getMinionId.Parameters.AddWithValue("@minionName", minionName);
            int minionId = (int)await getMinionId.ExecuteScalarAsync();


            SqlCommand addMinionVillain = new SqlCommand(SQLQueries.AddMinionVillain, con, transaction);
            addMinionVillain.Parameters.AddWithValue("@villainId", villainId);
            addMinionVillain.Parameters.AddWithValue("@minionId", minionId);

            await addMinionVillain.ExecuteNonQueryAsync();
            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");

            transaction.Commit();


        }

        public async static Task ChangeTownNamesCasing(SqlConnection con)
        {
            string countryName = Console.ReadLine();

            //SqlCommand checkIfCountryExists = new SqlCommand(SQLQueries.CheckIfCountryExists, con);
            //checkIfCountryExists.Parameters.AddWithValue("@countryname", countryName);
            //int count = (int) await checkIfCountryExists.ExecuteScalarAsync();

            

            SqlCommand findCountryCodeByName = new SqlCommand(SQLQueries.findCountryCodeByName, con);
            findCountryCodeByName.Parameters.AddWithValue("@countryName", countryName);
            int code = 0;
            try
            {
                code = (int)await findCountryCodeByName.ExecuteScalarAsync();
                
            }
            catch
            {
                Console.WriteLine("No town names were affected.");
                return;
            }

            SqlCommand upcaseTownNames = new SqlCommand(SQLQueries.UpcaseTownNames, con);
            upcaseTownNames.Parameters.AddWithValue("@countryCode", code);

            await upcaseTownNames.ExecuteNonQueryAsync();

            SqlCommand findTownCount = new SqlCommand(SQLQueries.FindTownCountByCountryCode, con);
            findTownCount.Parameters.AddWithValue("@countryCode", code);
            int count = (int)await findTownCount.ExecuteScalarAsync();

            Console.WriteLine($"{count} town names were affected.");

            List<string> towns = new List<string>();
            SqlCommand findTownNames = new SqlCommand(SQLQueries.FindTownNamesByCountryCode, con);
            findTownNames.Parameters.AddWithValue("@countryCode", code);
            SqlDataReader townNames = await findTownNames.ExecuteReaderAsync();
            while(townNames.Read())
            {
                towns.Add((string)townNames["Name"]);
            }

            Console.WriteLine($"[{string.Join(",",towns)}]");
        }

        public async static Task RemoveVillain(SqlConnection con)
        {
            SqlTransaction transaction = con.BeginTransaction();

            int villainId = int.Parse(Console.ReadLine());



            SqlCommand getName = new SqlCommand(SQLQueries.FindVillainById,con, transaction);
            getName.Parameters.AddWithValue("@villainId", villainId);
            SqlDataReader villainNameReader = await getName.ExecuteReaderAsync();
            
            if(!villainNameReader.Read())
            {
                Console.WriteLine("No such villain was found.");
                return;
            }
            else
            {
                string villainName = (string)villainNameReader["Name"];
                Console.WriteLine($"{villainName} was deleted");
            }
            await villainNameReader.CloseAsync();

            SqlCommand getCount = new SqlCommand(SQLQueries.FindMinionCountByVillainId,con, transaction);
            getCount.Parameters.AddWithValue("@villainId", villainId);
            int count = (int)await getCount.ExecuteScalarAsync();
            Console.WriteLine($"{count} minions were released.");


            SqlCommand releaseMinions = new SqlCommand(SQLQueries.ReleaseMinionsById, con, transaction);
            releaseMinions.Parameters.AddWithValue("@villainId", villainId);
            await releaseMinions.ExecuteNonQueryAsync();

            SqlCommand deleteVillain = new SqlCommand(SQLQueries.DeleteVillainById, con, transaction);
            deleteVillain.Parameters.AddWithValue("@villainId", villainId);
            await deleteVillain.ExecuteScalarAsync();

            transaction.Commit();



        }

        //TODO
        public async static Task PrintAllMinionNames(SqlConnection con)
        {
            List<string> minionNames = new List<string>();

            SqlCommand getAllMinionNames = new SqlCommand(SQLQueries.GetAllMinionNames, con);

            SqlDataReader reader = await getAllMinionNames.ExecuteReaderAsync();

            while(reader.Read())
            {
                minionNames.Add((string)reader["Name"]);
            }
            await reader.CloseAsync();

            Console.WriteLine(String.Join(Environment.NewLine,minionNames));
        }
    }
}