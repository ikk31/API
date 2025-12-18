using API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Instruments
{
    public class ManagerPassword : IPasswordHasher<User>
    {
        private readonly PasswordHasher<User> _passwordHasher;
        public ManagerPassword ()
        {
            _passwordHasher = new PasswordHasher<User>();
        }
        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
