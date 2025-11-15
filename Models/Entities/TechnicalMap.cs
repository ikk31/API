using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities;

public partial class TechnicalMap
{
    [Key]
    public int IdTech { get; set; }

    public string? NameDrink { get; set; }

    public string? Recipe { get; set; }

    [ForeignKey(nameof(IdCategorydrinkNavigation))]
    public int? IdCategorydrink { get; set; }

    public virtual CategoryDrink? IdCategorydrinkNavigation { get; set; }
}
