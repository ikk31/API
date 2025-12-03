using API.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.DTOs.Request;
using WebApplication1.Models.DTOs.Response;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    public class WorkHoursController : ControllerBase
    {
        private readonly MyCoffeeCupContext _context;

        public WorkHoursController(MyCoffeeCupContext context)
        {
            _context = context;
        }

        [HttpGet("AllShifts")]
        public async Task<IActionResult>
            GetAllShifts()
        {
            var obj = await _context.Shift.Include(x => x.IdEmployeeNavigation).Include(x => x.IdWorkplaceNavigation).ToListAsync();
            return Ok(obj.Adapt<List<AllShifts>>());
        }

        [HttpPost("AddShift")]
        public async Task<IActionResult>
            PostShifts([FromBody] AddShifts addShifts)
        {
            if (addShifts == null)
            {
                return BadRequest();
            }
            var obj = addShifts.Adapt<Shift>();
            await _context.Shift.AddAsync(obj);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Смена успешно добавлена");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetShift/{idShift}")]
        public async Task<IActionResult> GetShift(int idShift)
        {
            try
            {
                var shift = await _context.Shift
                    .Include(s => s.IdEmployeeNavigation)
                    .Include(s => s.IdWorkplaceNavigation)
                    .FirstOrDefaultAsync(s => s.IdShifts == idShift);

                if (shift == null)
                {
                    return NotFound($"Смена с ID {idShift} не найдена");
                }

                return Ok(shift.Adapt<AllShifts>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("PutShift/{IdShift}")]
        public async Task<IActionResult>
            PutEmployee([FromBody] AddShifts addShifts, int IdShift)
        {
            if (addShifts == null || IdShift != addShifts.IdShifts)
                return NotFound("Сотрудник не найден");
            var obj = await _context.Shift.FindAsync(IdShift);
            if (obj == null)
                return NotFound("Сотрнудник не найден");

            obj.IdEmployee = addShifts.IdEmployee;
            obj.Date = addShifts.Date;
            obj.StartTime = addShifts.StartTime;
            obj.EndTime = addShifts.EndTime;
            obj.IdWorkplace = addShifts.IdWorkplace;
            obj.HourlyRate = addShifts.HourlyRate;
            obj.Notes = addShifts.Notes;
            obj.BreakDuration = addShifts.BreakDuration;
            obj.WorkHours = addShifts.WorkHours;
            obj.TotalEarned = addShifts.TotalEarned;
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Информация о смене успешно изменена");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
    }
}
