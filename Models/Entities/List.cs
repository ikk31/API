using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities;

public partial class List
{
    [Key]
    public int IdList { get; set; }

    public string? Name { get; set; }

    public double? Count { get; set; }

    public DateOnly? Date { get; set; }

    [ForeignKey(nameof(IdShiftNavigation))]
    public int? IdShift { get; set; }

    public double? MinStock { get; set; }

    [ForeignKey(nameof(IdCategoryNavigation))]
    public int? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Shift? IdShiftNavigation { get; set; }
}
