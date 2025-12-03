//using API.Models.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml;
//using WebApplication1.Data;

//namespace WebApplication1.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ReportsController : ControllerBase
//    {
//        private readonly MyCoffeeCupContext _context;
//        public ReportsController(MyCoffeeCupContext context) { _context = context; }

//        [HttpGet("shifts")]
//        public async Task<IActionResult>
//            ExportShiftsReport([FromQuery] DateTime? startDate,
//        [FromQuery] DateTime? endDate,
//        [FromQuery] int? employeeId)
//        {
//            var query = _context.Shift
//       .Include(s => s.IdEmployee)
//       .Include(s => s.IdWorkplace)
//       .AsQueryable();

//            // Для сравнения DateOnly используем только даты
//            if (startDate.HasValue)
//            {
//                var startDateValue = startDate.Value;
//                query = query.Where(s => s.Date >= startDateValue);
//            }

//            if (endDate.HasValue)
//            {
//                var endDateValue = endDate.Value;
//                query = query.Where(s => s.Date <= endDateValue);
//            }

//            if (employeeId.HasValue)
//            {
//                query = query.Where(s => s.EmployeeId == employeeId.Value);
//            }

//            var shifts = await query
//                .OrderBy(s => s.Date)
//                .ThenBy(s => s.IdEmployee.Name) // Исправлено: Employee вместо idEmployeeMaxigation
//                .ToListAsync();

//            // Генерация Excel отчета
//            var reportBytes = await GenerateShiftsExcelReport(shifts);

//            return File(reportBytes,
//                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                $"shifts_report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
//        }

//        private byte[] GenerateShiftsExcelReport(List<Shift> shifts)
//        {
//            using var package = new ExcelPackage();
//            var worksheet = package.Workbook.Worksheets.Add("Отчет по сменам");

//            // Заголовки
//            worksheet.Cells[1, 1].Value = "Сотрудник";
//            worksheet.Cells[1, 2].Value = "Дата";
//            worksheet.Cells[1, 3].Value = "Рабочая точка";
//            worksheet.Cells[1, 4].Value = "Начало";
//            worksheet.Cells[1, 5].Value = "Окончание";
//            worksheet.Cells[1, 6].Value = "Перерыв";
//            worksheet.Cells[1, 7].Value = "Отработано";
//            worksheet.Cells[1, 8].Value = "Ставка";
//            worksheet.Cells[1, 9].Value = "Сумма";
//            worksheet.Cells[1, 10].Value = "Комментарий";

//            // Данные
//            for (int i = 0; i < shifts.Count; i++)
//            {
//                var shift = shifts[i];
//                var row = i + 2;

//                worksheet.Cells[row, 1].Value = shift.Employee?.FullName;
//                worksheet.Cells[row, 2].Value = shift.Date.ToString("dd.MM.yyyy");
//                worksheet.Cells[row, 3].Value = shift.Workplace?.Name;
//                worksheet.Cells[row, 4].Value = shift.StartTime.ToString(@"hh\:mm");
//                worksheet.Cells[row, 5].Value = shift.EndTime.ToString(@"hh\:mm");
//                worksheet.Cells[row, 6].Value = shift.BreakDuration.ToString(@"hh\:mm");
//                worksheet.Cells[row, 7].Value = shift.WorkedHours;
//                worksheet.Cells[row, 8].Value = shift.HourlyRate;
//                worksheet.Cells[row, 9].Value = shift.WorkedHours * (double)shift.HourlyRate;
//                worksheet.Cells[row, 10].Value = shift.Notes;
//            }

//            // Авторазмер колонок
//            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

//            return package.GetAsByteArray();
//        }
//    }
//}
