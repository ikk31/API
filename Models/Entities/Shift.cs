using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Entities;

namespace API.Models.Entities;

public partial class Shift
{
    [Key]
    public int IdShifts { get; set; }

    [ForeignKey(nameof(IdEmployeeNavigation))]
    public int? IdEmployee { get; set; }

    public DateOnly? Date { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public int? BreakDuration { get; set; }

    public int? ActualDuration { get; set; }

    public int? HourlyRate { get; set; }

    public decimal? TotalEarned { get; set; }

    [ForeignKey(nameof(IdWorkplaceNavigation))]
    public int? IdWorkplace { get; set; }

    public string? Notes { get; set; }

    public double? WorkHours { get; set; }


    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual WorkPlace? IdWorkplaceNavigation { get; set; }

    public virtual ICollection<ShiftPayout> ShiftPayouts { get; set; } = new List<ShiftPayout>();
    public virtual ICollection<List> Lists { get; set; } = new List<List>();
}
