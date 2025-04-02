using EmployeeApp.Helpers;
using EmployeeApp.Middleware;
using EmployeeApp.Repository;
using EmployeeApp.Repository.Interfaces;
using EmployeeApp.Serivce;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new Exception("No db connection string found");

// Регистрируем фабрику подключений
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<DatabaseInitializer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // создание бд
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        dbInitializer.InitializeDatabase();
    }
}

app.MapControllers();

app.Run();