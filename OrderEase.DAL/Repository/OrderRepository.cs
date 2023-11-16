using Microsoft.EntityFrameworkCore;
using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;
using OrderEase.DAL.Interfaces;


namespace OrderEase.DAL.Repository
{
    public class OrderRepository : IRepository<Order>
    {
        private AppDataContext _db;

        public OrderRepository(AppDataContext db)
        {
            _db = db;
        }

        public IEnumerable<Order> GetAll()
        {
            return _db.Orders;
        }

        public Order Get(int id)
        {
            return _db.Orders.Find(id);
        }

        public void Create(Order order)
        {
            _db.Orders.Add(order);
        }

        public void Update(Order order)
        {
            _db.Entry(order).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Order order = _db.Orders.Find(id);
            if (order != null)
                _db.Orders.Remove(order);
        }
    }
}
