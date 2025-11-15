using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities
{
    public class Cities
    {
        [Key]
        public int IdCity { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<WorkPlace> WorkPlaces { get; set; } = new List<WorkPlace>();
    }
}
