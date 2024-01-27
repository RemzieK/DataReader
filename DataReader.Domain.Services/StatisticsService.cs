using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DataReader.Domain.Entities;
using DataReader.Infrastructure.DatabaseConnection;

namespace Domain.Services
{
    public class StatisticsService
    {
        private readonly DatabaseConnection _databaseConnection;
        private Dictionary<string, int> _cache;

        public StatisticsService(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _cache = new Dictionary<string, int>();
        }

        private async Task<DataTable> ExecuteQuery(string query)
        {
            using (var connection = _databaseConnection.Connect())
            {
                try
                {
                

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var dataTable = new DataTable();
                            dataTable.Load(reader);

                            return dataTable;
                        }
                    }
                }
                finally
                {
                   
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }


        public async Task<DataTable> GetCompaniesWithMostEmployees()
        {
            var query = "SELECT * FROM Organization ORDER BY NumberOfEmployees DESC";
            return await ExecuteQuery(query);
        }

        public async Task<DataTable> GetTotalEmployeesByIndustry()
        {
            var query = "SELECT Industry.IndustryName, SUM(Organization.NumberOfEmployees) as TotalEmployees " +
                        "FROM Organization INNER JOIN Industry ON Organization.IndustryId = Industry.IndustryId " +
                        "GROUP BY Industry.IndustryName";
            return await ExecuteQuery(query);
        }

        public async Task<DataTable> GetGroupingByCountryAndIndustry()
        {
            var query = "SELECT Country.CountryName, Industry.IndustryName, SUM(Organization.NumberOfEmployees) as TotalEmployees " +
                        "FROM Organization INNER JOIN Country ON Organization.CountryId = Country.CountryId " +
                        "INNER JOIN Industry ON Organization.IndustryId = Industry.IndustryId " +
                        "GROUP BY Country.CountryName, Industry.IndustryName";
            return await ExecuteQuery(query);
        }

        public async Task CacheStatisticsAsync()
        {
            _cache["TotalCompanies"] = (int)(await ExecuteQuery("SELECT COUNT(*) FROM Organization")).Rows[0][0];
            _cache["TotalCountries"] = (int)(await ExecuteQuery("SELECT COUNT(*) FROM Country")).Rows[0][0];
            _cache["TotalIndustries"] = (int)(await ExecuteQuery("SELECT COUNT(*) FROM Industry")).Rows[0][0];
        }

    }
}
