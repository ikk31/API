namespace WebApplication1.Models.DTOs.Request
{
    public class AddEmployees
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public DateOnly? HireDate { get; set; }
        
        public int? IdJobTittle { get; set; }

        public bool? IsDelete { get; set; }

        public int IdEmployee { get; set; }

        public string? PhotoPath { get; set; }
    }
}
