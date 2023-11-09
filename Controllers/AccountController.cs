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
            DateTime date = DateTime.Now.AddMonths(-1);
            var orders = _db.Orders.Where(d => d.Date > date);
            orders = orders.OrderBy(d => d.Date);

            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> HomePageAsync(string data)
        {
            int month = int.Parse(data);
            DateTime date = DateTime.Now.AddMonths(-month);
            var orders = _db.Orders.Where(d => d.Date > date);
            orders = orders.OrderBy(d => d.Date);

            return View(orders);
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