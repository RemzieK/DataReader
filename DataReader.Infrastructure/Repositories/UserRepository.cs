using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using DataReader.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Infrastructure.Repositories
{
    
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DatabaseConnection.DatabaseConnection _dbConnection;
        public UserRepository(DatabaseConnection.DatabaseConnection dbConnection)
            : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }
        protected override string TableName => "Users";
        public async Task CreateAsync(User users)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"INSERT INTO {TableName} (Username, Password, RoleId) VALUES (@Username, @Password, @RoleId)", connection);
                command.Parameters.AddWithValue("@Username", users.Username);
                command.Parameters.AddWithValue("@Password", users.Password);
                command.Parameters.AddWithValue("@RoleId", users.RoleId);
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task UpdateAsync(User users)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"UPDATE {TableName} SET UserName = @Username, Password = @Password, RoleId=@RoleId WHERE UserID = @UserID", connection);
                command.Parameters.AddWithValue("@UserId", users.UserId);
                command.Parameters.AddWithValue("@Username", users.Username);
                command.Parameters.AddWithValue("@Password", users.Password);
                command.Parameters.AddWithValue("@RoleId", users.RoleId);
                await command.ExecuteNonQueryAsync();
            }
        }
        protected override User Map(SqlDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                Username = reader.GetString(reader.GetOrdinal("UserName")),
                Password = reader.GetString(reader.GetOrdinal("UserPassword")),
                RoleId = reader.GetInt32(reader.GetOrdinal("RoleId"))
            };
        }
        
        public async Task<User> GetByUsername(string username)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand($"SELECT * FROM {TableName} WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

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
        public override async Task SoftDeleteAsync(int userId)
        {
            var user = await GetByIdAsync(userId);

            if (user != null)
            {
                user.IsDeleted = true;
                await UpdateAsync(user);
            }
        }
    }
}
