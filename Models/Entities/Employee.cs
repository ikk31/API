using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities;

public partial class Employee
{
    public string? Name { get; set; }

    public string? LastName { get; set; }

    public DateOnly? HireDate { get; set; }

    [ForeignKey(nameof(IdJobTitleNavigation))]
    public int? IdJobTitle { get; set; }

    public bool? IsDelete { get; set; }

    [Key]
    public int IdEmployee { get; set; }

    public string? PhotoPath { get; set; }

    public virtual JobTitle? IdJobTitleNavigation { get; set; }

    public virtual ICollection<Avans> Avans { get; set; } = new List<Avans>();

    public virtual ICollection<Payout> Payouts { get; set; } = new List<Payout>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
