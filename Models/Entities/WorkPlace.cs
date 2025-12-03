using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities;

public partial class WorkPlace
{
    [Key]
    public int IdWorkPlace { get; set; }

    public string? Name { get; set; }

    [ForeignKey(nameof(Cities))]
    public int? IdCity { get; set; }

    public virtual Cities? Cities { get; set; }
    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
