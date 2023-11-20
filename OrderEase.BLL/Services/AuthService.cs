using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OrderEase.BLL.DTO;
using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;
using OrderEase.DAL.Repository;
using System.Security.Claims;

namespace OrderEase.WebServer.PL.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private EFUnitOfWork _database;
        public AuthService(AppDataContext db)
        {
            _database = new EFUnitOfWork(db);
        }

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

        public async Task<User?> LoginAsync(LoginDTO model)
        {
            var user = await _database.Users.GetAsync(model.Email);
            user.Role = await _database.Roles.GetAsync(user.RoleId.ToString());

            if (user != null && user.Email == model.Email && user.Password == model.Password)
            {
                return user;
            }
            else
                return null;
        }
        public async Task RegistrationAsync(RegisterDTO model)
        {
            var user = await _database.Users.GetAsync(model.Email);

            if (user == null)
            {
                // Добавляем пользователя в бд:
                user = new User { Id = Guid.NewGuid().GetHashCode(), Email = model.Email, Password = model.Password };
                var userRole = await _database.Roles.GetAsync("user");

                if (userRole != null)
                    user.Role = userRole;
                await _database.Users.CreateAsync(user);
                await _database.SaveAsync();
            }
        }
        public async Task LogoutAsync(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
