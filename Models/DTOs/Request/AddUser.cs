using API.Models.Entities;

namespace WebApplication1.Models.DTOs.Request
{
    public class AddUser
    {
        public int IdUsers { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public int IdRole { get; set; }

       
    }
}
