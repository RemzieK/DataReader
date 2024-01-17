using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using DataReader.Infrastructure.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataReader.Infrastructure.Repositories
{
    //add all of the properties from the user model and check how were the names in the database
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DatabaseConnection.DatabaseConnection _dbConnection;
        public UserRepository(DatabaseConnection.DatabaseConnection dbConnection)
            : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }
        protected override string TableName => "Users";
        public async Task CreateAsync(User entity)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"INSERT INTO {TableName} (UserName, UserPassword, Email, Birthday) VALUES (@UserName, @UserPassword, @Email, @Birthday)", connection);
                command.Parameters.AddWithValue("@Username", entity.Username);
                command.Parameters.AddWithValue("@Password", entity.Password);
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task UpdateAsync(User users)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"UPDATE {TableName} SET UserName = @UserName, UserPassword = @UserPassword, Email = @Email, Birthday = @Birthday WHERE UserID = @UserID", connection);
                command.Parameters.AddWithValue("@UserId", users.UserId);
                command.Parameters.AddWithValue("@Username", users.Username);
                command.Parameters.AddWithValue("@Password", users.Password);
                await command.ExecuteNonQueryAsync();
            }
        }
        protected override User Map(SqlDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                Username = reader.GetString(reader.GetOrdinal("UserName")),
                //add the other 
            };
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            List<User> users = new List<User>();

            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand($"SELECT * FROM {TableName}", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = Map(reader);
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
        public async Task<User> GetByIdAsync(int ID)
        {
            User user = null;
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand($"SELECT * FROM {TableName} WHERE UserID = @UserID", connection))
                {
                    command.Parameters.AddWithValue("@UserID", ID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = Map(reader);
                        }
                    }
                }
            }
            return user;
        }
        public async Task<User> GetByUsername(string username)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand($"SELECT * FROM {TableName} WHERE UserName = @UserName", connection))
                {
                    command.Parameters.AddWithValue("@UserName", username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return Map(reader);
                        }
                    }
                }
            }
            return null;
        }
        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            User user = null;

            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand($"SELECT * FROM {TableName} WHERE UserName = @UserName AND UserPassword = @UserPassword", connection))
                {
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@UserPassword", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = Map(reader);
                        }
                    }
                }
            }
            return user;

        }
    }
}
