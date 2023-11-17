using OrderEase.BLL.DTO;

namespace OrderEase.BLL.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetOrders();
        IEnumerable<OrderItemDTO> GetOrderItems();
        Task MakeOrderAsync(ViewOrderDTO viewOrder);
        Task<ViewOrderDTO> GetOrderAsync(string item);
        Task UpdateOrderAsync(ViewOrderDTO viewOrder);
        Task DeleteOrderAsync(string item);

    }
}
