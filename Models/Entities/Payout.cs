using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities;

public partial class Payout
{
    [Key]
    public int IdPayouts { get; set; }

    [ForeignKey(nameof(IdEmployeeNavigation))]
    public int? IdEmployee { get; set; }

    [ForeignKey(nameof(IdPayPeriodNavigation))]
    public int? IdPayPeriod { get; set; }

    public int? TotalHours { get; set; }

    public int? TotalAmount { get; set; }

    public DateOnly? PaidAt { get; set; }

    

    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual PayRollPeriod? IdPayPeriodNavigation { get; set; }
}
