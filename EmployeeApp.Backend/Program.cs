using EmployeeApp.Helpers;
using EmployeeApp.Middleware;
using EmployeeApp.Repository;
using EmployeeApp.Repository.Interfaces;
using EmployeeApp.Serivce;
using EmployeeApp.Serivce.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new Exception("No db connection string found");

// Запускаем миграции при старте приложения
var migrationService = new MigrationService(connectionString);
migrationService.MigrateUp();

// Регистрируем фабрику подключений
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee API", Version = "v1" });
});

// Регистрируем обработчик завершения работы приложения
AppDomain.CurrentDomain.ProcessExit += (s, e) =>
{
    Console.WriteLine("Приложение закрывается...");
    migrationService.DropDatabase(); // Здесь выполняется удаление базы данных
};

var app = builder.Build();


app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();