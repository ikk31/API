using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
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
            var obj = await _context.Shift.Include(x => x.IdEmployeeNavigation).ToListAsync();
            return Ok(obj.Adapt<List<AllShifts>>());
        }
    }
}
