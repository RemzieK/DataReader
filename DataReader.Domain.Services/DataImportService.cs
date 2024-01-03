using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{
    public class DataImportService: IDataImportService
    {
        private readonly string connectionString;

        public DataImportService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void ImportData(string[] lines)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (string line in lines)
                {
                    string[] data = line.Split(',');

                   
                    int countryId = InsertOrUpdateCountry(connection, data[4]);

                    InsertOrganization(connection, data, countryId);
                }
            }
        }

        public int InsertOrUpdateCountry(SqlConnection connection, string countryName)
        {
            string insertCountryQuery = @"IF NOT EXISTS (SELECT * FROM Country WHERE CountryName = @CountryName)
                                          INSERT INTO Country (CountryName) VALUES (@CountryName);
                                          SELECT CountryId FROM Country WHERE CountryName = @CountryName;";

            using (SqlCommand cmd = new SqlCommand(insertCountryQuery, connection))
            {
                cmd.Parameters.AddWithValue("@CountryName", countryName);
                return (int)cmd.ExecuteScalar();
            }
        }

        public void InsertOrganization(SqlConnection connection, string[] data, int countryId)
        {
            string insertOrgQuery = @"INSERT INTO Organization (Index, OrganizationId, Name, Website, CountryId, Description, Founded, Industry, NumberOfEmployees)
                                      VALUES (@Index, @OrganizationId, @Name, @Website, @CountryId, @Description, @Founded, @Industry, @NumberOfEmployees)";

            using (SqlCommand cmd = new SqlCommand(insertOrgQuery, connection))
            {
                cmd.Parameters.AddWithValue("@Index", data[0]);
                cmd.Parameters.AddWithValue("@OrganizationId", data[1]);
                cmd.Parameters.AddWithValue("@Name", data[2]);
                cmd.Parameters.AddWithValue("@Website", data[3]);
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                cmd.Parameters.AddWithValue("@Description", data[5]);
                cmd.Parameters.AddWithValue("@Founded", data[6]);
                cmd.Parameters.AddWithValue("@Industry", data[7]);
                cmd.Parameters.AddWithValue("@NumberOfEmployees", data[8]);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
