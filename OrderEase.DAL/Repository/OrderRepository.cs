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

        public async Task<Order> GetAsync(string item)
        {
            bool result = int.TryParse(item, out var id);

            if (result == true) 
                return await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            else
                return await _db.Orders.FirstOrDefaultAsync(x => x.Number == item);
        }

        public async Task CreateAsync(Order order)
        {
            _db.Orders.Add(order);
        }

        public async Task UpdateAsync(Order order)
        {
            _db.Update(order);
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order != null)
                _db.Orders.Remove(order);
        }
    }
}
