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

        private EFUnitOfWork _database;

        public OrderService(AppDataContext db)
        {
            _database = new EFUnitOfWork(db);
        }

        public IEnumerable<OrderDTO> GetOrders()
        {
            // применяем автомаппер для проекции одной коллекции на другую
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Order>, List<OrderDTO>>(_database.Orders.GetAll());
        }

        public IEnumerable<OrderItemDTO> GetOrderItems()
        {
            // применяем автомаппер для проекции одной коллекции на другую
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrderItem, OrderItemDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<OrderItem>, List<OrderItemDTO>>(_database.OrderItems.GetAll());
        }

        public async Task MakeOrderAsync(ViewOrderDTO viewOrderDTO)
        {
            if (viewOrderDTO != null)
            {
                // Получаем логин пользователя:
                var user = await _database.Users.GetAsync(viewOrderDTO.UserEmail);
                var provider = await _database.Providers.GetAsync(viewOrderDTO.ProviderName);

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

                await _database.Orders.CreateAsync(order);
                await _database.OrderItems.CreateAsync(orderItem);
                await _database.SaveAsync();
            }
        }

        public async Task<ViewOrderDTO> GetOrderAsync(string item)
        {
            var viewOrder = new ViewOrderDTO();

            // Формируем нужные нам данные для передачи в представление:
            var order = await _database.Orders.GetAsync(item);
            var orderItem = await _database.OrderItems.GetAsync(item);
            var provider = await _database.Providers.GetAsync(order.ProviderId.ToString());

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
            var provider = await _database.Providers.GetAsync(viewOrder.ProviderName);
            var order = await _database.Orders.GetAsync(viewOrder.OrderNumber);
            var orderItem = await _database.OrderItems.GetAsync(order.Id.ToString());

            order.ProviderId = provider.Id;
            order.Date = DateTime.Now;
            orderItem.Name = viewOrder.ProductName;
            orderItem.Quantity = viewOrder.Quantity;
            orderItem.Unit = viewOrder.Unit;

            await _database.SaveAsync();
        }

        public async Task DeleteOrderAsync(string item)
        {
            Order order = await _database.Orders.GetAsync(item);
            OrderItem orderItem = await _database.OrderItems.GetAsync(item);

            await _database.Orders.DeleteAsync(order.Id);
            await _database.OrderItems.DeleteAsync(orderItem.Id);
            await _database.SaveAsync();
        }
    }
}
