using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Entities;

namespace API.Models.Entities
{
    public partial class Avans
    {
        [Key]
        public int IdAvans {  get; set; }

        [ForeignKey(nameof(IdEmployeeNavigation))]
        public int IdEmployee { get; set; }

        public decimal? Amount { get; set; }

        public DateOnly? Date { get; set; }

        public bool? IsDelete { get; set; }

        public virtual Employee? IdEmployeeNavigation { get; set; }

        public virtual ICollection<AvansPayouts> AvansPayouts { get; set; } = new List<AvansPayouts>();


    }
}
