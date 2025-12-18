using API.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.DTOs.Request;
using WebApplication1.Models.DTOs.Response;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ReferenceController : ControllerBase
    {
        private readonly MyCoffeeCupContext _context;

        public ReferenceController(MyCoffeeCupContext context)
        {
            _context = context;
        }
        [HttpGet("AllJobTitle")]
        public async Task<IActionResult>
        GetJobTittle()
        {
            var obj = await _context.JobTitle.ToListAsync();
            return  Ok(obj.Adapt<List<JobTitleEmployee>>());
        }

        [HttpGet("AllWorkPlace")]
        public async Task<IActionResult>
            GetWorkPlace()
        {
            var obj = await _context.WorkPlace.ToListAsync();
            return Ok(obj.Adapt<List<AllWorkPlaces>>());
        }

        //аванс за выбранный период
        [HttpGet("avans/{employeeId}/period")]
        public async Task<IActionResult> GetAvansByEmployeeAndPeriod(
           int employeeId,
           [FromQuery] DateOnly startDate,
           [FromQuery] DateOnly endDate)
        {
            try
            {
                var avansList = await _context.Avans
                    .Where(a => a.IdEmployee == employeeId
                        && a.Date >= startDate
                        && a.Date <= endDate
                        && a.IsDelete != true)
                    .OrderBy(a => a.Date)
                    .ToListAsync();

                return Ok(avansList.Adapt<List<AllAvans>>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка получения авансов: {ex.Message}");
            }
        }

        [HttpGet("shifts/{employeeId}/period")]
        [ProducesResponseType(typeof(List<AllShifts>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetShiftsByEmployeeAndPeriod(
            int employeeId,
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest("Дата начала не может быть позже даты окончания");
                }

                var shifts = await _context.Shift
                    .Include(s => s.IdWorkplaceNavigation)
                    .Include(s => s.IdEmployeeNavigation)
                    .Where(s => s.IdEmployee == employeeId
                        && s.Date >= startDate
                        && s.Date <= endDate
                        && s.IsDelete != true)
                    .OrderBy(s => s.Date)
                    .ProjectToType<AllShifts>()
                    .ToListAsync();

                return Ok(shifts.Adapt<List<AllShifts>>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка получения смен: {ex.Message}");
            }
        }

        [HttpPost("AddAvans")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAvans([FromBody] AddAvans dto)
        {
            try
            {
                // Валидация
                if (dto.Amount <= 0)
                {
                    return BadRequest("Сумма аванса должна быть больше 0");
                }

                if (dto.Date > DateOnly.FromDateTime(DateTime.Now))
                {
                    return BadRequest("Дата аванса не может быть в будущем");
                }

                // Проверяем существование сотрудника
                var employee = await _context.Employee
                    .FirstOrDefaultAsync(e => e.IdEmployee == dto.IdEmployee && e.IsDelete != true);

                if (employee == null)
                {
                    return NotFound("Сотрудник не найден");
                }

                // Создаем аванс
                var avans = new Avans
                {
                    IdEmployee = dto.IdEmployee,
                    Date = dto.Date,
                    Amount = dto.Amount,
                    IsDelete = false,
                    
                };

                _context.Avans.Add(avans);
                await _context.SaveChangesAsync();

                // Маппим в DTO для ответа
                var response = avans.Adapt<AddAvans>();

                return Ok(new
                {
                    avans.IdAvans,
                    Message = "Аванс успешно создан",
                    Avans = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Ошибка создания аванса: {ex.Message}" });
            }
        }

        [HttpPut("avans/{avansId}/update")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAvans(int id, [FromBody] UpdateAvansRequest dto)
        {
            try
            {
                // Находим аванс
                var avans = await _context.Avans
                    .FirstOrDefaultAsync(a => a.IdAvans == id && a.IsDelete != true);

                if (avans == null)
                {
                    return NotFound(new { Error = $"Аванс с ID {id} не найден" });
                }

                // Валидация
                if (dto.Amount.HasValue && dto.Amount <= 0)
                {
                    return BadRequest("Сумма аванса должна быть больше 0");
                }

                if (dto.Date.HasValue && dto.Date > DateOnly.FromDateTime(DateTime.Now))
                {
                    return BadRequest("Дата аванса не может быть в будущем");
                }

                // Обновляем поля (только если они переданы)
                if (dto.Date.HasValue)
                {
                    avans.Date = dto.Date.Value;
                }

                if (dto.Amount.HasValue)
                {
                    avans.Amount = dto.Amount.Value;
                }

                _context.Avans.Update(avans);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Аванс успешно обновлен",
                    IdAvans = avans.IdAvans
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Ошибка обновления аванса: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteAvans/{IdAvans}")]
        public async Task<IActionResult>
            DeleteAvans(int IdAvans)
        {
            var obj = await _context.Avans.FindAsync(IdAvans);
            if (obj == null)
            {
                return NotFound("Сотрудник не найден");
            }
            obj.IsDelete = true;
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Сотрудник успешно удалён");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }



    }
}
