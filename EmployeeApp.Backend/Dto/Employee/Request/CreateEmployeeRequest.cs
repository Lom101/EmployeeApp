using System.ComponentModel;

namespace EmployeeApp.Dto.Employee.Request;

public class CreateEmployeeRequest
{
    [DefaultValue("Bulat")]
    public string Name { get; set; }

    [DefaultValue("Shakirov")]
    public string Surname { get; set; }

    [DefaultValue("+75553339999")]
    public string Phone { get; set; }

    [DefaultValue(1)]
    public int CompanyId { get; set; }

    [DefaultValue(1)]
    public int PassportId { get; set; }

    [DefaultValue(1)]
    public int DepartmentId { get; set; }
}