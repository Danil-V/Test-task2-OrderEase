using Microsoft.AspNetCore.Mvc;
using OrderEase.Core.Models.Data;
using OrderEase.Core.Models.Web;
using OrderEase.DBEntity;
using OrderEase.WebServer.Services.AuthService;

namespace OrderEase.WebServer.Controllers
{
    public class AuthorizationController : Controller
    {
        private AuthService _authUser = new();
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
                    await _authUser.RegistrationAsync(model, _db);
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
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _authUser.LoginAsync(model, _db);
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
