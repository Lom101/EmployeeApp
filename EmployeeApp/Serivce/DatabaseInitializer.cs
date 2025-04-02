using Dapper;
using EmployeeApp.Helpers;
using Npgsql;

namespace EmployeeApp.Serivce;

public class DatabaseInitializer
{
    private readonly DbConnectionFactory _connectionFactory;

    public DatabaseInitializer(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public void InitializeDatabase()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        var dropTablesSql = @"
            DROP TABLE IF EXISTS employees CASCADE;
            DROP TABLE IF EXISTS departments CASCADE;
            DROP TABLE IF EXISTS passports CASCADE;
        ";

        // Выполняем удаление таблиц
        connection.Execute(dropTablesSql);
        
        var createTableSql = @"
                CREATE TABLE IF NOT EXISTS passports (
                    id SERIAL PRIMARY KEY,
                    type VARCHAR(50) NOT NULL,
                    number VARCHAR(50) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS departments (
                    id SERIAL PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    phone VARCHAR(20) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS employees (
                    id SERIAL PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    surname VARCHAR(100) NOT NULL,
                    phone VARCHAR(100) NOT NULL,
                    company_id INT NOT NULL,
                    passport_id INT,
                    department_id INT,
                    FOREIGN KEY (passport_id) REFERENCES passports(id),
                    FOREIGN KEY (department_id) REFERENCES departments(id)
                );
        ";

        connection.Execute(createTableSql);
        
        // Вставка тестовых данных в таблицы
        var insertDataSql = @"
                INSERT INTO passports (type, number) VALUES -- Вставляем данные в таблицу passports
                ('Passport', '123456789'),
                ('ID Card', '987654321');

                -- Вставляем данные в таблицу departments
                INSERT INTO departments (name, phone) VALUES
                ('HR', '123-456-7890'),
                ('IT', '987-654-3210');

                -- Вставляем данные в таблицу employees
                -- INSERT INTO employees (name, surname, phone, company_id, passport_id, department_id) VALUES
                -- ('John', 'Doe', '555-555-5555', 1, 1, 1),
                -- ('Jane', 'Doe', '555-555-5556', 1, 2, 2);
            ";

        // Выполняем вставку данных
        connection.Execute(insertDataSql);
    }
}