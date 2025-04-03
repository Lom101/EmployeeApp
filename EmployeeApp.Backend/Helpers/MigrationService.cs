using FluentMigrator.Runner;
using Npgsql;

namespace EmployeeApp.Helpers;

public class MigrationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _connectionString;

    public MigrationService(string connectionString)
    {
        _connectionString = connectionString;

        _serviceProvider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres() // Используем PostgreSQL
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationService).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider();
    }

    public void EnsureDatabaseCreated()
    {
        var builder = new NpgsqlConnectionStringBuilder(_connectionString);
        var databaseName = builder.Database;
        builder.Database = "postgres";

        try
        {
            using var connection = new NpgsqlConnection(builder.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";
            var exists = command.ExecuteScalar();

            if (exists == null)
            {
                command.CommandText = $"CREATE DATABASE \"{databaseName}\"";
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании базы данных '{databaseName}': {ex.Message}");
            throw;
        }
    }

    public void DropDatabase()
    {
        var builder = new NpgsqlConnectionStringBuilder(_connectionString);
        var databaseName = builder.Database;
        builder.Database = "postgres";

        try
        {
            using var connection = new NpgsqlConnection(builder.ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseName}'";
            command.ExecuteNonQuery();

            command.CommandText = $"DROP DATABASE IF EXISTS \"{databaseName}\"";
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении базы данных '{databaseName}': {ex.Message}");
            throw;
        }
    }

    public void MigrateUp()
    {
        EnsureDatabaseCreated();

        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        Console.WriteLine("Все миграции применены!");
    }
}
