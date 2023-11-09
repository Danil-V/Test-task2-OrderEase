using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin, user")]
        public IActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrderAsync(OrderModel model)
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
            else
                ModelState.AddModelError("", "Заполните все поля");
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> ReadOrder(string id)
        {
            // Формируем условие для поиска в БД по id:
            var orderId = int.Parse(id);

            // Формируем нужные нам данные для передачи в представление:
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            var orderItem = await _db.OrderItems.FirstOrDefaultAsync(x => x.OrderId == order.Id);
            var provider = await _db.Providers.FirstOrDefaultAsync(x => x.Id == order.ProviderId);

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
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> UpdateOrder(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderAsync(OrderModel model, string id)
        {
            if (ModelState.IsValid)
            {
                var provider = await _db.Providers.FirstOrDefaultAsync(p => p.Name == model.ProviderName);
                var order = await _db.Orders.FirstOrDefaultAsync(o => o.Number == id);
                var orderItem = await _db.OrderItems.FirstOrDefaultAsync(i => i.OrderId == order.Id);

                order.ProviderId = provider.Id;
                order.Date = DateTime.Now;
                orderItem.Name = model.ProductName;
                orderItem.Quantity = model.Quantity;
                orderItem.Unit = model.Unit;

                await _db.SaveChangesAsync();
                return RedirectToAction("HomePage", "Account");
            }
            else
                ModelState.AddModelError("", "Заполните все поля");
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> DeleateOrder(string id)
        {
            var orderId = int.Parse(id);

            var order = _db.Orders.FirstOrDefault(x => x.Id == orderId);
            var orderItem = _db.OrderItems.FirstOrDefault(x => x.OrderId == order.Id);
            _db.Orders.Remove(order);
            _db.OrderItems.Remove(orderItem);
            _db.SaveChanges();
            return RedirectToAction("HomePage", "Account");
        }
    }
}
