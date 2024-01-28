using DataReader.Domain.Entities;
using DataReader.Infrastructure.DatabaseConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly DatabaseConnection databaseConnection;

        public DataImportService(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public async Task ImportDataAsync(string jsonData, SqlConnection connection)
        {
           
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonData);

            foreach (var item in data)
            {
                string countryId, industryId;

               
                string selectCountryCommand = "SELECT CountryId FROM Country WHERE CountryName = @CountryName;";
                using (SqlCommand command = new SqlCommand(selectCountryCommand, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", item["Country"]);
                    countryId = (await command.ExecuteScalarAsync())?.ToString();
                }

                
                if (countryId == null)
                {
                    string insertCountryCommand = "INSERT INTO Country (CountryName) OUTPUT INSERTED.CountryId VALUES (@CountryName);";
                    using (SqlCommand command = new SqlCommand(insertCountryCommand, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", item["Country"]);
                        countryId = (await command.ExecuteScalarAsync()).ToString();
                    }
                }

               
                string selectIndustryCommand = "SELECT IndustryId FROM Industry WHERE IndustryName = @IndustryName;";
                using (SqlCommand command = new SqlCommand(selectIndustryCommand, connection))
                {
                    command.Parameters.AddWithValue("@IndustryName", item["Industry"]);
                    industryId = (await command.ExecuteScalarAsync())?.ToString();
                }
                if (industryId == null)
                {
                    string insertIndustryCommand = "INSERT INTO Industry (IndustryName) OUTPUT INSERTED.IndustryId VALUES (@IndustryName);";
                    using (SqlCommand command = new SqlCommand(insertIndustryCommand, connection))
                    {
                        command.Parameters.AddWithValue("@IndustryName", item["Industry"]);
                        industryId = (await command.ExecuteScalarAsync()).ToString();
                    }
                }

                
                string insertOrganizationCommand = @"
            INSERT INTO Organization (OrganizationId, Name, Website, CountryId, Description, Founded, IndustryId, NumberOfEmployees) 
            VALUES (@OrganizationId, @Name, @Website, @CountryId, @Description, @Founded, @IndustryId, @NumberOfEmployees);";
                using (SqlCommand command = new SqlCommand(insertOrganizationCommand, connection))
                {
                    command.Parameters.AddWithValue("@OrganizationId", item["Organization Id"]);
                    command.Parameters.AddWithValue("@Name", item["Name"]);
                    command.Parameters.AddWithValue("@Website", item["Website"]);
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    command.Parameters.AddWithValue("@Description", item["Description"]);
                    command.Parameters.AddWithValue("@Founded", item["Founded"]);
                    command.Parameters.AddWithValue("@IndustryId", industryId);
                    command.Parameters.AddWithValue("@NumberOfEmployees", item["Number of employees"]);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }

}
