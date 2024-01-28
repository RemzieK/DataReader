using DataReader.Infrastructure.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Infrastructure.Repositories
{

    public abstract class Repository<T>
    {
        protected readonly DatabaseConnection.DatabaseConnection _dbConnection;

        public Repository(DatabaseConnection.DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        protected abstract string TableName { get; }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = _dbConnection.Connect())
            {
                connection.Open();

                using (var command = new SqlCommand($"SELECT * FROM {TableName}", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var entities = new List<T>();

                    while (await reader.ReadAsync())
                    {
                        entities.Add(Map(reader));
                    }
                    return entities;
                }
            }
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            using (var connection = _dbConnection.Connect())
            {
                connection.Open();

                using (var command = new SqlCommand($"SELECT * FROM {TableName} WHERE Id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return Map(reader);
                        }
                    }
                }
                return default;
            }
        }
        public virtual async Task SoftDeleteAsync(int ID)
        {
            using (var connection = _dbConnection.Connect())
            {
                connection.Open();

                using (var command = new SqlCommand($"UPDATE {TableName} SET IsDeleted = 1 WHERE Id = @id", connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        protected abstract T Map(SqlDataReader reader);
    }
}
