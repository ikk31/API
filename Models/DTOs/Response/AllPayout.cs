namespace WebApplication1.Models.DTOs.Response
{
    public class AllPayout
    {
        public int IdPayouts { get; set; }
        public int IdEmployee { get; set; }
        public string? EmployeeName { get; set; }
        public DateOnly? PeriodStart { get; set; }
        public DateOnly? PeriodEnd { get; set; }
        public string? PeriodName { get; set; }
        public int? TotalHours { get; set; }
        public decimal? TotalAmount { get; set; }
       
        public DateOnly? PaidAt { get; set; }
        public string? Note { get; set; }
       

        // Дополнительная информация
        public int ShiftCount { get; set; }
        public int AvansCount { get; set; }
        public decimal TotalEarnings { get; set; } // Сумма смен
        public decimal TotalAvans { get; set; }    // Сумма авансов
    }
}
