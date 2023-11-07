using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEase.Data;

namespace PicBox.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDataContext _db;
        public AccountController(AppDataContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> HomePageAsync()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("StartPage", "Home");
        }
    }
}