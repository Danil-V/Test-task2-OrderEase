using Microsoft.AspNetCore.Mvc;

namespace OrderEase.WebServer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult StartPage()
        {
            return View();
        }
    }
}
