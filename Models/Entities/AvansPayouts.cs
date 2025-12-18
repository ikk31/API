using API.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities
{
    [Table("AvansPayouts")]
    public class AvansPayouts
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(Avans))]
        public int IdAvans { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey(nameof(Payouts))]
        public int IdPayouts { get; set; }

        // Навигационные свойства
        public virtual Avans? Avans { get; set; }
        public virtual Payouts? Payouts { get; set; }
    }
}
