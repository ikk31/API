using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Entities;

namespace API.Models.Entities;

public partial class Payouts
{
    [Key]
    public int IdPayouts { get; set; }

    [ForeignKey(nameof(IdEmployeeNavigation))]
    public int IdEmployee { get; set; }

    
    public DateOnly? PeriodStart { get; set; }
    public DateOnly? PeriodEnd { get; set; }

    public string? PeriodName { get; set; }

    [ForeignKey(nameof(IdAvansNavigation))]
    public int IdAvans { get; set; }

    public int? TotalHours { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateOnly? PaidAt { get; set; }

    public bool? IsDelete { get; set; }

    public string? Note { get; set; }
    

    public virtual Employee? IdEmployeeNavigation { get; set; }
    public virtual Avans? IdAvansNavigation { get; set; }

    public virtual ICollection<ShiftPayouts> ShiftPayouts { get; set; } = new List<ShiftPayouts>();
    public virtual ICollection<AvansPayouts> AvansPayouts { get; set; } = new List<AvansPayouts>();


}
