using Microsoft.AspNetCore.Mvc;
using OrderEase.BLL.DTO;
using OrderEase.DAL.Data.EF;
using OrderEase.WebClient.PL.Models;
using OrderEase.WebServer.PL.Services.AuthService;

namespace OrderEase.WebServer.PL.Controllers
{
    public class AuthorizationController : Controller
    {
        private AuthService _authUser;

        public AuthorizationController(AppDataContext db)
        {
            _authUser = new AuthService(db);
        }


        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationAsync(RegisterViewModel model)
        {
            var dto = new RegisterDTO();
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    dto.Email = model.Email;
                    dto.Password = model.Password;
                    dto.ConfirmPassword = model.ConfirmPassword;

                    await _authUser.RegistrationAsync(dto);
                    return RedirectToAction("Login");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
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
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            var dto = new LoginDTO();

            if (ModelState.IsValid)
            {
                dto.Email = model.Email;
                dto.Password = model.Password;
                var user = await _authUser.LoginAsync(dto);
                if (user != null)
                {
                    await _authUser.AuthenticateAsync(user, HttpContext); // аутентификация
                    return RedirectToAction("HomePage", "Account");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
    }
}
