using EmployeeApp.Dto.Employee.Request;
using EmployeeApp.Entity;
using EmployeeApp.Helpers.Mapper;
using EmployeeApp.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployeeAsync([FromBody] CreateEmployeeRequest request)
    {
        if (request == null)
            return BadRequest("Employee cannot be null");
        
        var employee = request.ToEmployee(); // маппим DTO в сущность
        
        var employeeId = await _employeeRepository.AddEmployeeAsync(employee);
        
        if (employeeId <= 0)
            return StatusCode(500, "Failed to create employee");
        
        return Ok(employeeId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeAsync(int id)
    {
        var affectedRows = await _employeeRepository.DeleteEmployeeAsync(id);
        if(affectedRows == 0)
            return NotFound("Employee not found");
        
        return NoContent();
    }
    
    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetEmployeesByCompanyIdAsync(int companyId)
    {
        var employees = await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);
        if (!employees.Any())
            return NotFound("No employees found for this company");

        var employeeDtos = employees.Select(e => e.ToResponseDto());
        return Ok(employeeDtos);
    }

    [HttpGet("department/{departmentId}")]
    public async Task<IActionResult> GetEmployeesByDepartmentIdAsync(int departmentId)
    {
        var employees = await _employeeRepository.GetEmployeesByDepartmentIdAsync(departmentId);
        if (!employees.Any())
            return NotFound("No employees found for this department");

        var employeeDtos = employees.Select(e => e.ToResponseDto());
        return Ok(employeeDtos);

    }
    
    // 5. Обновить данные сотрудника
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync(int id, [FromBody] UpdateEmployeeRequest request)
    {
        if (request == null)
            return BadRequest("Employee data is required");

        if (id != request.Id)
            return BadRequest("Employee ID mismatch");

        var employee = request.ToEmployee(id);

        var affectedRows = await _employeeRepository.UpdateEmployeeAsync(employee);
        if (affectedRows == 0)
            return NotFound("Employee not found or no changes made");

        return NoContent();  // Успешно обновлено
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if(employee == null)
            return NotFound("Employee not found");
        
        return Ok(employee.ToResponseDto());
    }
}