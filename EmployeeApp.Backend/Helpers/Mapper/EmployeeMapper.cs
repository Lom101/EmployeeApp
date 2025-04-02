using EmployeeApp.Dto.Employee.Request;
using EmployeeApp.Dto.Employee.Response;
using EmployeeApp.Entity;

namespace EmployeeApp.Helpers.Mapper;

public static class EmployeeMapper
{
    public static Employee ToEmployee(this CreateEmployeeRequest request)
    {
        return new Employee
        {
            Name = request.Name,
            Surname = request.Surname,
            Phone = request.Phone,
            CompanyId = request.CompanyId,
            DepartmentId = request.DepartmentId,
            PassportId = request.PassportId
        };
    }
    
    public static Employee ToEmployee(this UpdateEmployeeRequest request, int employeeId)
    {
        return new Employee
        {
            Id = employeeId,
            Name = request.Name,
            Surname = request.Surname,
            Phone = request.Phone,
            CompanyId = request.CompanyId,
            DepartmentId = request.DepartmentId,
            PassportId = request.PassportId
        };
    }

    public static EmployeeResponse ToResponseDto(this Employee employee)
    {
        return new EmployeeResponse
        {
            Id = employee.Id,
            Name = employee.Name,
            Surname = employee.Surname,
            Phone = employee.Phone,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            Department = employee.Department,
            PassportId = employee.PassportId,
            Passport = employee.Passport
        };
    }
}