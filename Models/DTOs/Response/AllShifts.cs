namespace WebApplication1.Models.DTOs.Response
{
    public class AllShifts
    {
        public int idShifts { get; set; }
        public DateOnly? Date { get; set; }
        //public DateOnly? StartTime { get; set; }
        //public TimeOnly? EndTime { get; set; }
        //public int? BreakDuration { get; set; }
        //public int? ActualDuration { get; set; }
        public int? HourlyRate { get; set; }
        //public double? WorkHours { get; set; }

        public int? IdEmployee { get; set; }
        public EmployeeDto IdEmployeeNavigation { get; set; }
    }

    public class EmployeeDto
    {
        public string? Name { get; set; }
        public JobTittleDto IdJobTitleNavigation { get; set; }
    }
    public class JobTittleDto
    {
        public string? Name { get; set; }
    }
}
