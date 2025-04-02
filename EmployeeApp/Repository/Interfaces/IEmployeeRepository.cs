using EmployeeApp.Entity;

namespace EmployeeApp.Repository.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<int> AddEmployeeAsync(Employee employee);
    Task<int> UpdateEmployeeAsync(Employee employee);
    Task<int> DeleteEmployeeAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(int companyId);
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId);
}