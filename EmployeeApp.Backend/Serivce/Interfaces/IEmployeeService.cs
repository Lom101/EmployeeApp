using EmployeeApp.Dto.Employee.Request;
using EmployeeApp.Dto.Employee.Response;

namespace EmployeeApp.Serivce.Interfaces;

public interface IEmployeeService
{
    Task<int> AddEmployeeAsync(CreateEmployeeRequest request);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<IEnumerable<EmployeeResponse>> GetEmployeesByCompanyIdAsync(int companyId);
    Task<IEnumerable<EmployeeResponse>> GetEmployeesByDepartmentIdAsync(int departmentId);
    Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request);
    Task<EmployeeResponse?> GetEmployeeByIdAsync(int id);
}