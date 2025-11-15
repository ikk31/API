using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public partial class PayRollPeriod
{
    [Key]
    public int IdPayPeriod { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<Payout> Payouts { get; set; } = new List<Payout>();
}
