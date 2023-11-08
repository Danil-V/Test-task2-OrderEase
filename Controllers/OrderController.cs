using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderEase.Data;
using OrderEase.Models.Data;
using OrderEase.Models.Web;
using System.Security.Claims;

namespace OrderEase.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDataContext _db;
        public OrderController(AppDataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                // Получаем логин пользователя:
                var userEmail = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
                var provider = await _db.Providers.FirstOrDefaultAsync(u => u.Name == model.ProviderName);
                if (model != null && provider != null)
                {
                    // Создаем новый заказ и добавляем его в бд:
                    var newOrder = new Order { Id = Guid.NewGuid().GetHashCode(), Number = Guid.NewGuid().ToString(), Date = DateTime.Now, UserEmail = userEmail, ProviderId = provider.Id };
                    var newOrderItem = new OrderItem { Id = Guid.NewGuid().GetHashCode(), Name = model.ProductName, Quantity = model.Quantity, Unit = model.Unit, OrderId = newOrder.Id };

                    _db.Orders.Add(newOrder);
                    _db.OrderItems.Add(newOrderItem);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("HomePage", "Account");
                }
            }
            return RedirectToAction("CreateOrder", "Order");
        }

        [HttpGet]
        public IActionResult ReadOrder(string id)
        {
            // Формируем условие для поиска в БД по id:
            var orderId = int.Parse(id);

            // Формируем нужные нам данные для передачи в представление:
            var order = _db.Orders.FirstOrDefault(x => x.Id == orderId);
            var orderItem = _db.OrderItems.FirstOrDefault(x => x.OrderId == order.Id);
            var provider = _db.Providers.FirstOrDefault(x => x.Id == order.ProviderId);

            // Передаем данные в представление:
            ViewBag.Number = order.Number;
            ViewBag.Date = order.Date;
            ViewBag.ProductName = orderItem.Name;
            ViewBag.Quantity = orderItem.Quantity;
            ViewBag.Unit = orderItem.Unit;
            ViewBag.ProviderName = provider.Name;

            return View(ViewBag);
        }

        [HttpGet]
        public IActionResult UpdateOrder(string id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleateOrder(string id)
        {
            return View();
        }
    }
}
