using Microsoft.AspNetCore.Mvc;
namespace OrderEase.Controllers
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
