using Microsoft.AspNetCore.Mvc;

namespace OrderEase.WebServer.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
