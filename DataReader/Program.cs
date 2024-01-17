using System;
using System.IO;
using System.Text.RegularExpressions;
using DataReader;
using DataReader.Domain.Services;
using DataReader.Domain.Services.AbstractionServices;
using DataReader.Infrastructure.DatabaseConnection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;



class Program
{
    static void Main()
    {
       
        var connectionString = "Server=localhost;Database=DataReader;User Id=sa;Password=sqldocker2022;";

        var databaseConnection = new DatabaseConnection(connectionString);
        var dataImportService = new DataImportService(databaseConnection);
        var dataReaderService = new DataReaderService(dataImportService, databaseConnection);


        var consoleManager = new ConsoleLogic(dataReaderService);

        consoleManager.Run().GetAwaiter().GetResult();
    }
}





