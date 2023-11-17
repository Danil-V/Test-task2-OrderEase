using AutoMapper;
using OrderEase.BLL.DTO;
using OrderEase.BLL.Interfaces;
using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;
using OrderEase.DAL.Repository;

namespace OrderEase.BLL.Services
{
    public class OrderService : IOrderService
    {

        private EFUnitOfWork _Database;

        public OrderService(AppDataContext db)
        {
            _Database = new EFUnitOfWork(db);
        }

        public IEnumerable<OrderDTO> GetOrders()
        {
            // применяем автомаппер для проекции одной коллекции на другую
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Order>, List<OrderDTO>>(_Database.Orders.GetAll());
        }

        public IEnumerable<OrderItemDTO> GetOrderItems()
        {
            // применяем автомаппер для проекции одной коллекции на другую
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrderItem, OrderItemDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<OrderItem>, List<OrderItemDTO>>(_Database.OrderItems.GetAll());
        }

        public async Task MakeOrderAsync(ViewOrderDTO viewOrderDTO)
        {
            if (viewOrderDTO != null)
            {
                // Получаем логин пользователя:
                var user = await _Database.Users.GetAsync(viewOrderDTO.UserEmail);
                var provider = await _Database.Providers.GetAsync(viewOrderDTO.ProviderName);

                // Создаем новый заказ и добавляем его в бд:
                var order = new Order
                {
                    Id = (int)viewOrderDTO.OrderId,
                    Number = viewOrderDTO.OrderNumber,
                    Date = DateTime.Now,
                    UserEmail = user.Email,
                    ProviderId = provider.Id
                };

                var orderItem = new OrderItem
                {
                    Id = (int)viewOrderDTO.OrderItemId,
                    Name = viewOrderDTO.ProductName,
                    Quantity = viewOrderDTO.Quantity,
                    Unit = viewOrderDTO.Unit,
                    OrderId = order.Id
                };

                await _Database.Orders.CreateAsync(order);
                await _Database.OrderItems.CreateAsync(orderItem);
                await _Database.Save();
            }
        }

        public async Task<ViewOrderDTO> GetOrderAsync(string item)
        {
            var viewOrder = new ViewOrderDTO();

            // Формируем нужные нам данные для передачи в представление:
            var order = await _Database.Orders.GetAsync(item);
            var orderItem = await _Database.OrderItems.GetAsync(item);
            var provider = await _Database.Providers.GetAsync(order.ProviderId.ToString());

            viewOrder.OrderNumber = order.Number;
            viewOrder.Date = order.Date;
            viewOrder.ProductName = orderItem.Name;
            viewOrder.Quantity = orderItem.Quantity;
            viewOrder.Unit = orderItem.Unit;
            viewOrder.ProviderName = provider.Name;

            return viewOrder;
        }

        public async Task UpdateOrderAsync(ViewOrderDTO viewOrder)
        {
            var provider = await _Database.Providers.GetAsync(viewOrder.ProviderName);
            var order = await _Database.Orders.GetAsync(viewOrder.OrderNumber);
            var orderItem = await _Database.OrderItems.GetAsync(order.Id.ToString());

            order.ProviderId = provider.Id;
            order.Date = DateTime.Now;
            orderItem.Name = viewOrder.ProductName;
            orderItem.Quantity = viewOrder.Quantity;
            orderItem.Unit = viewOrder.Unit;

            await _Database.Save();
        }

        public async Task DeleteOrderAsync(string item)
        {
            Order order = await _Database.Orders.GetAsync(item);
            OrderItem orderItem = await _Database.OrderItems.GetAsync(item);

            await _Database.Orders.DeleteAsync(order.Id);
            await _Database.OrderItems.DeleteAsync(orderItem.Id);
            await _Database.Save();
        }
    }
}
