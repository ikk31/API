using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Instruments;
using WebApplication1.Models.DTOs.Request;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly MyCoffeeCupContext _context;
        private readonly ManagerPassword _managerPassword;

        public UserController(MyCoffeeCupContext context, ManagerPassword managerPassword)
        {
            _context = context;
            _managerPassword = managerPassword;

        }

        [HttpPut("EditUser/{IdUser}")]
        public async Task<IActionResult>
            PutUser([FromBody] AddUser addUser, int IdUser)
        {
            if (addUser == null)
                return BadRequest("Пустое тело запроса");
            if (IdUser != addUser.IdUsers)
                return NotFound("Пользователь не найден");

            var obj = await _context.User.FindAsync(addUser.IdUsers);
            if (obj == null)
                return NotFound("Пользователь не найден");
            obj.Name = addUser.Name;
            obj.IdRole = addUser.IdRole;
            obj.Password = _managerPassword.HashPassword(obj, addUser.Password!);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Пользователь успешно изменён");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
