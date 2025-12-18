namespace WebApplication1.Models.DTOs.Response
{
    public class AllAvans
    {
        public int IdAvans { get; set; }
        public int IdEmployee { get; set; }
        public DateOnly? Date { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsDelete { get; set; }

        public EmployeeDto? IdEmployeeNavigation { get; set; }
    }
}
