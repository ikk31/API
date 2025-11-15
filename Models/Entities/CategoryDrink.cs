using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public partial class CategoryDrink
{
    [Key]
    public int IdCarDrink { get; set; }

    public string? NameCategoryDrink { get; set; }

    public virtual ICollection<TechnicalMap> TechnicalMaps { get; set; } = new List<TechnicalMap>();
}
