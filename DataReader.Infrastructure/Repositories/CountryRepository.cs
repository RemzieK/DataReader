﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;

namespace DataReader.Infrastructure.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly DatabaseConnection.DatabaseConnection _dbConnection;

        public CountryRepository(DatabaseConnection.DatabaseConnection dbConnection)
            : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        protected override string TableName => "Countries";

        public async Task CreateAsync(Country country)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"INSERT INTO {TableName} (CountryName) VALUES (@CountryName)", connection);
                command.Parameters.AddWithValue("@CountryName", country.CountryName);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Country country)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"UPDATE {TableName} SET CountryName = @CountryName WHERE CountryId = @CountryId", connection);
                command.Parameters.AddWithValue("@CountryId", country.CountryId);
                command.Parameters.AddWithValue("@CountryName", country.CountryName);
                await command.ExecuteNonQueryAsync();
            }
        }

        protected override Country Map(SqlDataReader reader)
        {
            return new Country
            {
                CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
                CountryName = reader.GetString(reader.GetOrdinal("CountryName"))
            };
        }

       

       

        public override async Task DeleteAsync(int countryId)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"DELETE FROM {TableName} WHERE CountryId = @CountryId", connection);
                command.Parameters.AddWithValue("@CountryId", countryId);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
