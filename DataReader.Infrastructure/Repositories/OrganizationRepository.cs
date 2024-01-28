using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using System.Data.SqlClient;

namespace DataReader.Infrastructure.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        private readonly DatabaseConnection.DatabaseConnection _dbConnection;

        public OrganizationRepository(DatabaseConnection.DatabaseConnection dbConnection)
            : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        protected override string TableName => "Organizations";

        public async Task CreateAsync(Organization organization)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"INSERT INTO {TableName} (Name, Website, CountryId, Description, Founded, IndustryId, NumberOfEmployees) " +
                                            "VALUES (@Name, @Website, @CountryId, @Description, @Founded, @IndustryId, @NumberOfEmployees)", connection);

                command.Parameters.AddWithValue("@Name", organization.Name);
                command.Parameters.AddWithValue("@Website", organization.Website);
                command.Parameters.AddWithValue("@CountryId", organization.CountryId);
                command.Parameters.AddWithValue("@Description", organization.Description);
                command.Parameters.AddWithValue("@Founded", organization.Founded);
                command.Parameters.AddWithValue("@IndustryId", organization.IndustryId);
                command.Parameters.AddWithValue("@NumberOfEmployees", organization.NumberOfEmployees);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Organization organization)
        {
            using (var connection = _dbConnection.Connect())
            {
                await connection.OpenAsync();
                var command = new SqlCommand($"UPDATE {TableName} SET Name = @Name, Website = @Website, CountryId = @CountryId, " +
                                            "Description = @Description, Founded = @Founded, IndustryId = @IndustryId, NumberOfEmployees = @NumberOfEmployees " +
                                            "WHERE OrganizationId = @OrganizationId", connection);

                command.Parameters.AddWithValue("@OrganizationId", organization.OrganizationId);
                command.Parameters.AddWithValue("@Name", organization.Name);
                command.Parameters.AddWithValue("@Website", organization.Website);
                command.Parameters.AddWithValue("@CountryId", organization.CountryId);
                command.Parameters.AddWithValue("@Description", organization.Description);
                command.Parameters.AddWithValue("@Founded", organization.Founded);
                command.Parameters.AddWithValue("@IndustryId", organization.IndustryId);
                command.Parameters.AddWithValue("@NumberOfEmployees", organization.NumberOfEmployees);

                await command.ExecuteNonQueryAsync();
            }
        }

        protected override Organization Map(SqlDataReader reader)
        {
            return new Organization
            {
                OrganizationId = reader.IsDBNull(reader.GetOrdinal("OrganizationId")) ? null : reader.GetString(reader.GetOrdinal("OrganizationId")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Website = reader.IsDBNull(reader.GetOrdinal("Website")) ? null : reader.GetString(reader.GetOrdinal("Website")),
                CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Founded = reader.GetInt32(reader.GetOrdinal("Founded")),
                IndustryId = reader.GetInt32(reader.GetOrdinal("IndustryId")),
                NumberOfEmployees = reader.GetInt32(reader.GetOrdinal("NumberOfEmployees")),
            };
        }

        public override async Task SoftDeleteAsync(int organizationId)
        {
            var organization = await GetByIdAsync(organizationId);

            if (organization != null)
            {
                organization.IsDeleted = true;
                await UpdateAsync(organization);
            }
        }

    }
}

