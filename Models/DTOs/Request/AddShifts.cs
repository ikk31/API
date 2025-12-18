using System.Data.SqlTypes;

namespace WebApplication1.Models.DTOs.Request
{
    public class AddShifts
    {
        public int IdShifts { get; set; }
        public int IdEmployee { get; set; }
        public DateOnly? Date {  get; set; }
        public int IdWorkplace {  get; set; }
        public int? HourlyRate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Notes { get; set; }
        public int? BreakDuration { get; set; }

        public decimal? TotalEarned { get; set; }
        public double? WorkHours { get; set; }
        public bool? IsDelete { get; set; }

    }


}
