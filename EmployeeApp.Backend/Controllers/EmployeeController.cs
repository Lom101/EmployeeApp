using EmployeeApp.Dto.Employee.Request;
using EmployeeApp.Serivce.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddEmployeeAsync([FromBody] CreateEmployeeRequest request)
    {
        if (request == null)
            return BadRequest("Employee cannot be null");

        var employeeId = await employeeService.AddEmployeeAsync(request);
        if (employeeId <= 0)
            return BadRequest("Failed to create employee");

        return Ok(employeeId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeAsync(int id)
    {
        var isDeleted = await employeeService.DeleteEmployeeAsync(id);
        return isDeleted ? NoContent() : NotFound("Employee not found");
    }
    
    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetEmployeesByCompanyIdAsync(int companyId)
    {
        var employees = await employeeService.GetEmployeesByCompanyIdAsync(companyId);
        return employees.Any() ? Ok(employees) : NotFound("No employees found for this company");

    }

    [HttpGet("department/{departmentId}")]
    public async Task<IActionResult> GetEmployeesByDepartmentIdAsync(int departmentId)
    {
        var employees = await employeeService.GetEmployeesByDepartmentIdAsync(departmentId);
        return employees.Any() ? Ok(employees) : NotFound("No employees found for this department");
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync(int id, [FromBody] UpdateEmployeeRequest request)
    {
        if (request == null)
            return BadRequest("Employee data is required");

        var isUpdated = await employeeService.UpdateEmployeeAsync(id, request);
        return isUpdated ? NoContent() : NotFound("Employee not found or no changes made");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(int id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        return employee != null ? Ok(employee) : NotFound("Employee not found");
    }
}