using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OrderEase.Core.Models.Data;
using OrderEase.Core.Models.Web;
using OrderEase.DBEntity;
using System.Security.Claims;

namespace OrderEase.WebServer.Services.AuthService
{
    public class AuthService : IAuthService
    {
        public AuthService() { }

        public async Task AuthenticateAsync(User user, HttpContext context)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<User?> LoginAsync(LoginModel model, AppDataContext data)
        {
            User? user = await data.Users.Include(u => u.Role)
                                           .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                return user;
            }
            else
                return null;
        }
        public async Task RegistrationAsync(RegisterModel model, AppDataContext data)
        {
            User? user = await data.Users.Include(u => u.Role)
                                          .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                // Добавляем пользователя в бд:
                user = new User { Id = Guid.NewGuid().GetHashCode(), Email = model.Email, Password = model.Password };
                Role userRole = await data.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                if (userRole != null)
                    user.Role = userRole;
                data.Users.Add(user);
                await data.SaveChangesAsync();
            }
        }
        public async Task LogoutAsync(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
