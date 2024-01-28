using System.Data.SqlClient;
using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;

namespace DataReader.Infrastructure.Repositories
{
    public class IndustryRepository : Repository<Industry>, IIndustryRepository
    {
        private readonly DatabaseConnection.DatabaseConnection _dbConnection;

        public IndustryRepository(DatabaseConnection.DatabaseConnection dbConnection)
            : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        protected override string TableName => "Industries";

        public async Task CreateAsync(Industry industry)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"INSERT INTO {TableName} (IndustryName) VALUES (@IndustryName)", connection);
                command.Parameters.AddWithValue("@IndustryName", industry.IndustryName);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Industry industry)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"UPDATE {TableName} SET IndustryName = @IndustryName WHERE IndustryId = @IndustryId", connection);
                command.Parameters.AddWithValue("@IndustryId", industry.IndustryId);
                command.Parameters.AddWithValue("@IndustryName", industry.IndustryName);
                await command.ExecuteNonQueryAsync();
            }
        }

        protected override Industry Map(SqlDataReader reader)
        {
            return new Industry
            {
                IndustryId = reader.GetInt32(reader.GetOrdinal("IndustryId")),
                IndustryName = reader.GetString(reader.GetOrdinal("IndustryName"))
            };
        }

        public override async Task SoftDeleteAsync(int industryId)
        {
            var industry = await GetByIdAsync(industryId);

            if (industry != null)
            {
                industry.IsDeleted = true;
                await UpdateAsync(industry);
            }
        }

    }
}
