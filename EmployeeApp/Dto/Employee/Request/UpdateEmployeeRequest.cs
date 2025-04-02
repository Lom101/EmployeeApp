namespace EmployeeApp.Dto.Employee.Request;

public class UpdateEmployeeRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int CompanyId { get; set; }
    public int PassportId { get; set; }
    public int DepartmentId { get; set; }
}