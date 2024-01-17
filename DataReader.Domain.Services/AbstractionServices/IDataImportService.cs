using DataReader.Domain.Entities;
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
        Task ImportDataAsync(string jsonData, SqlConnection connection);
      
    }

}



