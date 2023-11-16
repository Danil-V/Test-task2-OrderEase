using OrderEase.DAL.Data.Models.Data;

namespace OrderEase.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<Provider> Providers { get; } 
        void Save();
    }
}
