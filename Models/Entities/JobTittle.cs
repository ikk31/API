using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public partial class JobTittle
{
    public string? Name { get; set; }

    public double? BaseRate { get; set; }

    [Key]
    public int IdJobTittle { get; set; }

    public virtual ICollection<EmployeeHistorySalary> EmployeeHistorySalaries { get; set; } = new List<EmployeeHistorySalary>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
