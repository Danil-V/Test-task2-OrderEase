using Microsoft.AspNetCore.Mvc;

namespace OrderEase.WebServer.PL.Controllers
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
