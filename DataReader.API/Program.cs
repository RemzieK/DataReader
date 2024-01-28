using DataReader.Domain.Interfaces;
using DataReader.Domain.Services;
using DataReader.Domain.Services.AbstractionServices;
using DataReader.Domain.Services.EntityServices;
using DataReader.Infrastructure.DatabaseConnection;
using DataReader.Infrastructure.Repositories;
using Domain.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DataReader");
builder.Services.AddScoped<DatabaseConnection>(_ => new DatabaseConnection(connectionString));


builder.Services.AddScoped<IDataReaderService, DataReaderService>();
builder.Services.AddScoped<IDataImportService, DataImportService>();
builder.Services.AddScoped<IPasswordHashing, PasswordHashing>();
builder.Services.AddScoped<IJwtServices, JwtService>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationService>();
builder.Services.AddScoped<IAuthorizationServices, AuthorizationService>();
builder.Services.AddScoped<IDownloadPdf, DownloadPdf>();
builder.Services.AddScoped<IGeneralStatsDailyJson, GeneralStatsDailyJson>();

builder.Services.AddScoped<IUserServices, UserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IIndustryRepository, IndustryRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();






// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<IpAddressFilterMiddleware>(IPAddress.Parse("127.0.0.1"));

app.UseAuthorization();

app.MapControllers();

app.Run();
