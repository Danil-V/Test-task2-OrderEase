using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEase.BLL.Services;
using OrderEase.DAL.Data.EF;
using OrderEase.WebServer.PL.Services.AuthService;


namespace OrderEase.WebServer.PL.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDataContext _db;
        private OrderService _orderService;
        public AccountController(AppDataContext db)
        {
            _db = db;
            _orderService = new OrderService(_db);
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> HomePageAsync()
        {
            var allOrders = _orderService.GetOrders();
            DateTime date = DateTime.Now.AddMonths(-1);
            var orders = allOrders.Where(d => d.Date > date);
            orders = orders.OrderBy(d => d.Date);

            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> HomePageAsync(string providerId, string date)
        {
            DateTime SeekDate;
            int month = int.Parse(date);
            var allOrders = _orderService.GetOrders();
            if (providerId != null)
            {
                int id = int.Parse(providerId);
                SeekDate = DateTime.Now.AddMonths(-month);
                var orders = allOrders.Where(o => o.Date > SeekDate && o.ProviderId == id);
                orders = orders.OrderBy(o => o.Date);
                return View(orders);
            }
            else
            {
                SeekDate = DateTime.Now.AddMonths(-month);
                var orders = allOrders.Where(o => o.Date > SeekDate);
                orders = orders.OrderBy(o => o.Date);
                return View(orders);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            var auth = new AuthService(_db);
            auth.LogoutAsync(HttpContext);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("StartPage", "Home");
        }
    }
}