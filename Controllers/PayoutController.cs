using API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.DTOs.Request;
using WebApplication1.Models.DTOs.Response;
using WebApplication1.Models.Entities;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    public class PayoutController : ControllerBase
    {
        private readonly MyCoffeeCupContext _context;

        public PayoutController(MyCoffeeCupContext context)
        {
            _context = context;
        }

        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPayouts()
        {
            try
            {
                var query = _context.Payouts
                    .Include(p => p.IdEmployeeNavigation)
                    .Include(p => p.ShiftPayouts)
                    .Include(p => p.AvansPayouts)
                    .AsQueryable();

               

                var payouts = await query
                    .OrderByDescending(p => p.PeriodStart)
                    .Select(p => new AllPayout
                    {
                        IdPayouts = p.IdPayouts,
                        IdEmployee = p.IdEmployee,
                        EmployeeName = $"{p.IdEmployeeNavigation.Name} {p.IdEmployeeNavigation.LastName}",
                        PeriodStart = p.PeriodStart,
                        PeriodEnd = p.PeriodEnd,
                        PeriodName = p.PeriodName,
                        TotalHours = p.TotalHours,
                        TotalAmount = p.TotalAmount,
                        PaidAt = p.PaidAt,
                        ShiftCount = p.ShiftPayouts.Count,
                        AvansCount = p.AvansPayouts.Count
                    })
                    .ToListAsync();

                return Ok(payouts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Ошибка получения выплат: {ex.Message}" });
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetEmployeePayouts(int employeeId)
        {
            try
            {
                var payouts = await _context.Payouts
                    .Include(p => p.IdEmployeeNavigation)
                    .Include(p => p.ShiftPayouts)
                    .ThenInclude(sp => sp.Shift)
                    .Include(p => p.AvansPayouts)
                    .ThenInclude(ap => ap.Avans)
                    .Where(p => p.IdEmployee == employeeId)
                    .OrderByDescending(p => p.PeriodStart)
                    .Select(p => new
                    {
                        p.IdPayouts,
                        p.PeriodStart,
                        p.PeriodEnd,
                        p.PeriodName,
                        p.TotalHours,
                        p.TotalAmount,
                        
                        p.PaidAt,
                        p.Note,
                        
                        Shifts = p.ShiftPayouts.Select(sp => new
                        {
                            sp.Shift.IdShifts,
                            sp.Shift.Date,
                            sp.Shift.TotalEarned
                        }),
                        Avans = p.AvansPayouts.Select(ap => new
                        {
                            ap.Avans.IdAvans,
                            ap.Avans.Date,
                            ap.Avans.Amount
                        })
                    })
                    .ToListAsync();

                return Ok(payouts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Ошибка получения выплат сотрудника: {ex.Message}" });
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePayout([FromBody] AddPayout dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Валидация
                if (dto.PeriodStart > dto.PeriodEnd)
                {
                    return BadRequest("Дата начала периода не может быть позже даты окончания");
                }

                if (dto.TotalAmount <= 0)
                {
                    return BadRequest("Сумма выплаты должна быть больше 0");
                }

                if (dto.ShiftIds == null || !dto.ShiftIds.Any())
                {
                    return BadRequest("Не выбраны смены для выплаты");
                }

                // 2. Проверяем существование сотрудника
                var employee = await _context.Employee
                    .FirstOrDefaultAsync(e => e.IdEmployee == dto.IdEmployee && e.IsDelete != true);

                if (employee == null)
                {
                    return NotFound("Сотрудник не найден");
                }

                // 3. Проверяем, не создана ли уже выплата за этот период
                var existingPayout = await _context.Payouts
                    .FirstOrDefaultAsync(p => p.IdEmployee == dto.IdEmployee
                        && p.PeriodStart == dto.PeriodStart
                        && p.PeriodEnd == dto.PeriodEnd
                        && p.IsDelete != true);

                if (existingPayout != null)
                {
                    return BadRequest($"Выплата за период {dto.PeriodStart:dd.MM.yyyy} - {dto.PeriodEnd:dd.MM.yyyy} уже существует");
                }

                // 4. Создаем выплату
                var payout = new Payouts
                {
                    IdEmployee = dto.IdEmployee,
                    PeriodStart = dto.PeriodStart,
                    PeriodEnd = dto.PeriodEnd,
                    PeriodName = dto.PeriodName,
                    TotalAmount = dto.TotalAmount, // В вашей модели int
                    TotalHours = dto.TotalHours,
                    PaidAt = dto.PaidAt,
                    Note = dto.Notes,
                    IsDelete = false
                };

                _context.Payouts.Add(payout);
                await _context.SaveChangesAsync(); // Сохраняем чтобы получить IdPayouts

                // 5. Связываем смены с выплатой
                foreach (var shiftId in dto.ShiftIds)
                {
                    // Проверяем существование смены
                    var shift = await _context.Shift
                        .FirstOrDefaultAsync(s => s.IdShifts == shiftId
                            && s.IdEmployee == dto.IdEmployee
                            && s.IsDelete != true);

                    if (shift == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Смена с ID {shiftId} не найдена или не принадлежит сотруднику");
                    }

                    var shiftPayout = new ShiftPayouts
                    {
                        IdShift = shiftId,
                        IdPayouts = payout.IdPayouts,
                        
                    };

                    _context.ShiftPayout.Add(shiftPayout);
                }

                // 6. Связываем авансы с выплатой
                foreach (var avansId in dto.AvansIds)
                {
                    // Проверяем существование аванса
                    var avans = await _context.Avans
                        .FirstOrDefaultAsync(a => a.IdAvans == avansId
                            && a.IdEmployee == dto.IdEmployee
                            && a.IsDelete != true);

                    if (avans == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Аванс с ID {avansId} не найден или не принадлежит сотруднику");
                    }

                    var avansPayout = new AvansPayouts
                    {
                        IdAvans = avansId,
                        IdPayouts = payout.IdPayouts,
                    };

                    _context.AvansPayout.Add(avansPayout);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 7. Возвращаем ответ
                return Ok(new
                {
                    Success = true,
                    IdPayout = payout.IdPayouts,
                    Message = "Выплата успешно создана",
                    Period = $"{dto.PeriodStart:dd.MM.yyyy} - {dto.PeriodEnd:dd.MM.yyyy}",
                    TotalAmount = dto.TotalAmount,
                    ShiftCount = dto.ShiftIds.Count,
                    AvansCount = dto.AvansIds.Count
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    Success = false,
                    Error = $"Ошибка создания выплаты: {ex.Message}"
                });
            }
        }
    }
}
