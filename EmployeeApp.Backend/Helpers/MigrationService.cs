using FluentMigrator.Runner;

namespace EmployeeApp.Helpers;

public class MigrationService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(string connectionString)
    {
        _serviceProvider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres() // Используем PostgreSQL
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationService).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider();
    }

    public void MigrateUp()
    {
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        Console.WriteLine("✅ Все миграции применены!");
    }
}