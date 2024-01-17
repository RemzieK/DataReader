using DataReader.Domain.Interfaces;
using DataReader.Domain.Services;
using DataReader.Domain.Services.AbstractionServices;
using DataReader.Domain.Services.EntityServices;
using DataReader.Infrastructure.DatabaseConnection;
using DataReader.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DataReader");
builder.Services.AddScoped<DatabaseConnection>(_ => new DatabaseConnection(connectionString));


builder.Services.AddScoped<IDataReaderService, DataReaderService>();
builder.Services.AddScoped<IDataImportService, DataImportService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPasswordHashing, PasswordHashing>();
builder.Services.AddScoped<IJwtServices, JwtService>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationService>();
builder.Services.AddScoped<IAuthorizationServices, AuthorizationService>();

builder.Services.AddScoped<IUserServices, UserService>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
