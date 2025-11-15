namespace WebApplication1.Models.DTOs.Response
{
    public class AllEmployees
    {
        public int IdEmployee { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? PhotoPath { get; set; }
        public bool? IsDelete {  get; set; }
    }
}
