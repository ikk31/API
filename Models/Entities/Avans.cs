using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    public partial class Avans
    {
        [Key]
        public int idAvans {  get; set; }

        [ForeignKey(nameof(Employee))]
        public int? IdEmployee { get; set; }

        public int? Amount { get; set; }

        public DateOnly? date { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
