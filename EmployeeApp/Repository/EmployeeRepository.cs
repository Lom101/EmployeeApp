using System.Data;
using Dapper;
using EmployeeApp.Entity;
using EmployeeApp.Helpers;
using EmployeeApp.Repository.Interfaces;
using Npgsql;

namespace EmployeeApp.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    // Инжектим DbConnectionFactory через конструктор
    public EmployeeRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Employee>(
            "SELECT * FROM employees WHERE id = @id", new { id });
    }

    public async Task<int> AddEmployeeAsync(Employee employee)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        // Проверка, существует ли паспорт с таким PassportId
        var passportExists = await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM passports WHERE id = @PassportId)",
            new { employee.PassportId });

        if (!passportExists)
            throw new ArgumentException("Passport with the provided ID does not exist.");
        
        // Проверка, существует ли отдел с таким DepartmentId
        var departmentExists = await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM departments WHERE id = @DepartmentId)",
            new { employee.DepartmentId });

        if (!departmentExists)
        {
            throw new ArgumentException("Department with the provided ID does not exist.");
        }
        
        var sql = @"
            INSERT INTO employees (name, surname, phone, company_id, passport_id, department_id) 
            VALUES (@Name, @Surname, @Phone, @CompanyId, @PassportId, @DepartmentId) 
            RETURNING id
        ";
        return await connection.ExecuteScalarAsync<int>(sql, employee);
    }

    public async Task<int> UpdateEmployeeAsync(Employee employee)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"
            UPDATE employees 
            SET name = @Name, surname = @Surname, phone = @Phone, company_id = @CompanyId, 
                passport_id = @PassportId, department_id = @DepartmentId 
            WHERE id = @Id
        ";
        return await connection.ExecuteAsync(sql, employee);
    }

    public async Task<int> DeleteEmployeeAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync("DELETE FROM employees WHERE id = @id", new { id });
    }
    
    public async Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Employee>(
            "SELECT * FROM employees WHERE company_id = @companyId", new { companyId });
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Employee>(
            "SELECT * FROM employees WHERE department_id = @departmentId", new { departmentId });
    }

}