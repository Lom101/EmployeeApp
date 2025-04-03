using EmployeeApp.Dto.Employee.Request;
using EmployeeApp.Dto.Employee.Response;
using EmployeeApp.Helpers.Mapper;
using EmployeeApp.Repository.Interfaces;
using EmployeeApp.Serivce.Interfaces;

namespace EmployeeApp.Serivce;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task<int> AddEmployeeAsync(CreateEmployeeRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Employee cannot be null");

        var employee = request.ToEmployee();
        return await employeeRepository.AddEmployeeAsync(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var affectedRows = await employeeRepository.DeleteEmployeeAsync(id);
        return affectedRows > 0;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetEmployeesByCompanyIdAsync(int companyId)
    {
        var employees = await employeeRepository.GetEmployeesByCompanyIdAsync(companyId);
        return employees.Select(e => e.ToResponseDto());
    }

    public async Task<IEnumerable<EmployeeResponse>> GetEmployeesByDepartmentIdAsync(int departmentId)
    {
        var employees = await employeeRepository.GetEmployeesByDepartmentIdAsync(departmentId);
        return employees.Select(e => e.ToResponseDto());
    }

    public async Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Employee data is required");

        if (id != request.Id)
            throw new ArgumentException("Employee ID mismatch");

        //var employee = request.ToEmployee(id);
        var affectedRows = await employeeRepository.UpdateEmployeeAsync(id, request);
        return affectedRows > 0;
    }

    public async Task<EmployeeResponse?> GetEmployeeByIdAsync(int id)
    {
        var employee = await employeeRepository.GetEmployeeByIdAsync(id);
        return employee?.ToResponseDto();
    }
}