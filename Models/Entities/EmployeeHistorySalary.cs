using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities;

public partial class EmployeeHistorySalary
{
    [Key]
    public int IdHistorySalary { get; set; }

    [ForeignKey(nameof(IdJobTittleNavigation))]
    public int? IdJobTittle { get; set; }

    public double? OldSalaryRate { get; set; }

    public double? NewSalaryRate { get; set; }

    public DateOnly? Changedate { get; set; }

    public string? Reason { get; set; }

    public virtual JobTittle? IdJobTittleNavigation { get; set; }
}
