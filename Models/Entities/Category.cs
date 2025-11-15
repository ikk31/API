using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public partial class Category
{
    [Key]
    public int IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<List> Lists { get; set; } = new List<List>();
}
