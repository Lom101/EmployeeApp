﻿namespace EmployeeApp.Entity;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }   
    public int CompanyId { get; set; } // может быть отрицательным и 0, так как в тз не было условий по поводу этого
    public int? PassportId { get; set; }
    public Passport? Passport { get; set; }
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
}