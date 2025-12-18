using API.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities
{
    [Table("ShiftPayout")]
    public class ShiftPayouts
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(Shift))]
        public int IdShift { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey(nameof(Payouts))]
        public int IdPayouts { get; set; }

        public virtual Shift? Shift { get; set; }
        public virtual Payouts? Payouts { get; set; }
    }
}
