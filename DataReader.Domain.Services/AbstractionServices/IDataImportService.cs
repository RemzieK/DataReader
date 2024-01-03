using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{
    public interface IDataImportService
    {
        void ImportData(string[] lines);
        int InsertOrUpdateCountry(SqlConnection connection, string countryName);
        void InsertOrganization(SqlConnection connection, string[] data, int countryId);
    }
}


