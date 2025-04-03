using Dapper;
using EmployeeApp.Entity;
using EmployeeApp.Helpers;
using EmployeeApp.Repository.Interfaces;

namespace EmployeeApp.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    // Инжектим DbConnectionFactory через конструктор
    public EmployeeRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
        SELECT e.id,
           e.name,
           e.surname,
           e.phone,
           e.company_id    AS CompanyId,
           e.passport_id    AS PassportId,
           e.department_id    AS DepartmentId,
           -- passport
           p.id            AS Id,
           p.type          AS Type,
           p.number        AS Number,
           -- department
           d.id            AS Id,
           d.name          AS Name,
           d.phone         AS Phone
        FROM employees e
                 LEFT JOIN passports p ON e.passport_id = p.id
                 LEFT JOIN departments d ON e.department_id = d.id
        WHERE e.id = @id;
    ";

        var result = await connection.QueryAsync<Employee, Passport, Department, Employee>(
            sql,
            (employee, passport, department) =>
            {
                employee.Passport = passport;
                employee.Department =  department;
                return employee;
            },
            new { id },
            splitOn: "Id, Id"
        );

        return result.FirstOrDefault();
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

        var sql = @"
        SELECT e.id,
               e.name,
               e.surname,
               e.phone,
               e.company_id    AS CompanyId,
               e.passport_id    AS PassportId,
               e.department_id    AS DepartmentId,
               -- passport
               p.id            AS Id,
               p.type          AS Type,
               p.number        AS Number,
               -- department
               d.id            AS Id,
               d.name          AS Name,
               d.phone         AS Phone
        FROM employees e
                 LEFT JOIN passports p ON e.passport_id = p.id
                 LEFT JOIN departments d ON e.department_id = d.id
        WHERE e.company_id = @id;
    ";

        var result = await connection.QueryAsync<Employee, Passport, Department, Employee>(
            sql,
            (employee, passport, department) =>
            {
                employee.Passport = passport;
                employee.Department = department;
                return employee;
            },
            new { companyId },
            splitOn: "Id, Id"
        );

        return result;
    }
    
    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
        SELECT e.id,
               e.name,
               e.surname,
               e.phone,
               e.company_id    AS CompanyId,
               e.passport_id    AS PassportId,
               e.department_id    AS DepartmentId,
               -- passport
               p.id            AS Id,
               p.type          AS Type,
               p.number        AS Number,
               -- department
               d.id            AS Id,
               d.name          AS Name,
               d.phone         AS Phone
        FROM employees e
                 LEFT JOIN passports p ON e.passport_id = p.id
                 LEFT JOIN departments d ON e.department_id = d.id
        WHERE e.department_id = @id;
    ";

        var result = await connection.QueryAsync<Employee, Passport, Department, Employee>(
            sql,
            (employee, passport, department) =>
            {
                employee.Passport = passport ?? null;
                employee.Department = department ?? null;
                return employee;
            },
            new { departmentId },
            splitOn: "Id, Id"
        );
        
        return result;
    }

}