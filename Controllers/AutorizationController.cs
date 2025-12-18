using API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.DTOs.Request;
using WebApplication1.Instruments;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Mapster;
using WebApplication1.Models.DTOs.Response;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    public class AutorizationController : ControllerBase
    {
        private readonly MyCoffeeCupContext _context;
        private readonly IConfiguration _configuration;
        private readonly ManagerPassword _managerPassword;
        public AutorizationController(MyCoffeeCupContext context, IConfiguration configuration, ManagerPassword managerPassword)
        {
            _context = context;
            _configuration = configuration;
            _managerPassword = managerPassword;
        }

        [HttpPost("Autorization")]
        public async Task<IActionResult>
    PostAutorization([FromBody] UserPasswordDto userPassword)
        {
            if (string.IsNullOrEmpty(userPassword.Password) && string.IsNullOrEmpty(userPassword.Name))
                return BadRequest("Не указан логин или пароль");
            var obj = await _context.User.Include(x => x.IdRoleNavigation).SingleOrDefaultAsync(x => x.Name!.ToLower() == userPassword.Name!.ToLower());
            if (obj == null)
                return Unauthorized("Невереное имя пользователя или пароль");
            return _managerPassword.VerifyHashedPassword(obj, obj.Password!, userPassword.Password!) == PasswordVerificationResult.Success ? Ok(new AllRoleToken { Name = obj.IdRoleNavigation!.Name, Token = GenerateToken(obj)}) : Unauthorized("Невереное имя пользователя или пароль");

            
        }
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name!),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.IdRole.ToString()!),
                new Claim(ClaimTypes.Hash, user.Password!)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokensSettings:Key"]!));
            var crets = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _configuration["TokensSettings:Issuer"], audience: _configuration["TokensSettings:Audience"], claims: claims, signingCredentials: crets, expires: DateTime.Now.AddHours(3));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

   
   
}
