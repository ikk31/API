using API.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
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


    }
}
