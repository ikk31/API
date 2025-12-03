using WebApplication1.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTOs.Response;
using API.Models.Entities;
using System.Net.Quic;
using WebApplication1.Models.DTOs.Request;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    public class EmployeeController:ControllerBase
    {
        private readonly MyCoffeeCupContext _context;

        public EmployeeController(MyCoffeeCupContext context)
        {
            _context = context;
        }

        
        [HttpGet("AllEmployee")]
        public async Task<IActionResult>
           GetEmployees()
        {
            var obj = await _context.Employee.Include(x => x.IdJobTitleNavigation).ToListAsync();
            return Ok(obj.Adapt<List<AllEmployees>>());
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult>
            PostEmployee([FromBody] AddEmployees addEmployees)
        {
            if (addEmployees == null)
            {
                return BadRequest("Пустое тело запроса");
            }
            var obj = addEmployees.Adapt<Employee>();
            await _context.Employee.AddAsync(obj);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Сотрудник успешно  добавлен");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteEmployee/{IdEmployee}")]
        public async Task<IActionResult>
            DeleteEmployee(int IdEmployee)
        {
            var obj = await _context.Employee.FindAsync(IdEmployee);
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

        [HttpPut("PutEmployee/{IdEmployee}")]
        public async Task<IActionResult>
            PutEmployee([FromBody] AddEmployees addEmployees, int IdEmployee)
        {
            if (addEmployees == null || IdEmployee != addEmployees.IdEmployee)
                return NotFound("Сотрудник не найден");
            var obj = await _context.Employee.FindAsync(IdEmployee);
            if (obj == null)
                return NotFound("Сотрнудник не найден");
           
            obj.Name = addEmployees.Name;
            obj.PhotoPath = addEmployees.PhotoPath;
            obj.HireDate = addEmployees.HireDate;
            obj.LastName = addEmployees.LastName;
           obj.IdJobTitle = addEmployees.IdJobTitle;
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Информация о сотруднике успешно изменена");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpGet("GetThisEmployee/{idEmployee}")]
        public async Task<IActionResult>
            GetThisEmployee(int idEmployee)
        {
            var obj = await _context.Employee.Include(x => x.IdJobTitleNavigation).FirstOrDefaultAsync(x => x.IdEmployee == idEmployee);
            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj.Adapt<ThisEmployee>());

        }

    }
}
