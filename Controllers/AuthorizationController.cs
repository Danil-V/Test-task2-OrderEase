using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderEase.Data;
using OrderEase.Models.Data;
using OrderEase.Models.Web;
using System.Security.Claims;

namespace OrderEase.Controllers
{

    public class AuthorizationController : Controller
    {
        private readonly AppDataContext _db;

        public AuthorizationController(AppDataContext db)
        {
            _db = db;
        }


        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationAsync(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    User newUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (newUser == null)
                    {
                        // Добавляем пользователя в бд:
                        newUser = new User { Id = Guid.NewGuid().GetHashCode(), Email = model.Email, Password = model.Password };
                        Role userRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                        if (userRole != null)
                            newUser.Role = userRole;

                        _db.Users.Add(newUser);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Login");
                    }
                    else
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _db.Users.Include(u => u.Role)
                                           .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await AuthenticateAsync(user); // аутентификация

                    return RedirectToAction("HomePage", "Account");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task AuthenticateAsync(User user)
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
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
