using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Entities;

namespace API.Models.Entities;

public partial class Payout
{
    [Key]
    public int IdPayouts { get; set; }

    [ForeignKey(nameof(IdEmployeeNavigation))]
    public int? IdEmployee { get; set; }

    
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }

    [ForeignKey(nameof(IdAvansNavigation))]
    public int? IdAvans { get; set; }

    public int? TotalHours { get; set; }

    public int? TotalAmount { get; set; }

    public DateOnly? PaidAt { get; set; }

    

    public virtual Employee? IdEmployeeNavigation { get; set; }
    public virtual Avans? IdAvansNavigation { get; set; }

    public virtual ICollection<ShiftPayout> ShiftPayouts { get; set; } = new List<ShiftPayout>();
    public virtual ICollection<AvansPayout> AvansPayouts { get; set; } = new List<AvansPayout>();


}
