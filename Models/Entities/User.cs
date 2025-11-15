using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities;

public partial class User
{
    [Key]
    public int IdUsers { get; set; }

    public string? Name { get; set; }

    [ForeignKey(nameof(IdRoleNavigation))]
    public int? IdRole { get; set; }

    public string? Password { get; set; }

    public virtual Role? IdRoleNavigation { get; set; }
}
