using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEase.BLL.DTO;
using OrderEase.BLL.Services;
using OrderEase.DAL.Data.EF;
using OrderEase.WebClient.PL.Models;
using System.Security.Claims;

namespace OrderEase.WebServer.PL.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDataContext _db;
        private OrderService _orderService;
        public OrderController(AppDataContext db)
        {
            _db = db;
            _orderService = new OrderService(_db);
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrderAsync(OrderViewModel model)
        {
            var viewOrder = new ViewOrderDTO();
            if (ModelState.IsValid)
            {
                // Получаем логин пользователя:
                var userEmail = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;

               
                    viewOrder.OrderId = Guid.NewGuid().GetHashCode();
                    viewOrder.OrderItemId = Guid.NewGuid().GetHashCode();
                    viewOrder.ProviderName = model.ProviderName;
                    viewOrder.OrderNumber = Guid.NewGuid().ToString();
                    viewOrder.Date = DateTime.Now;
                    viewOrder.UserEmail = userEmail;
                    viewOrder.ProductName = model.ProductName;
                    viewOrder.Quantity = model.Quantity;
                    viewOrder.Unit = model.Unit;
                    viewOrder.ProviderName = model.ProviderName;

                    await _orderService.MakeOrderAsync(viewOrder);

                    return RedirectToAction("HomePage", "Account");
                
            }
            else
                ModelState.AddModelError("", "Заполните все поля");
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> ReadOrder(string id)
        {
            ViewOrderDTO order = await _orderService.GetOrderAsync(id);
           
            // Передаем данные в представление:
            ViewBag.Number = order.OrderNumber;
            ViewBag.Date = order.Date;
            ViewBag.ProductName = order.ProductName;
            ViewBag.Quantity = order.Quantity;
            ViewBag.Unit = order.Unit;
            ViewBag.ProviderName = order.ProviderName;


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
        public async Task<IActionResult> UpdateOrderAsync(OrderViewModel model, string id)
        {
            var order = new ViewOrderDTO();
            if (ModelState.IsValid)
            {
 

                order.ProviderName = model.ProviderName;
                order.OrderNumber = id;
                order.Date = DateTime.Now;
                order.ProductName = model.ProductName;
                order.Quantity = model.Quantity;
                order.Unit = model.Unit;

                _orderService.UpdateOrderAsync(order);
                
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
            await _orderService.DeleteOrderAsync(id);
           
            return RedirectToAction("HomePage", "Account");
        }
    }
}
