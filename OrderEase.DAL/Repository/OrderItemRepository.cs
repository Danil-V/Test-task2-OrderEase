using Microsoft.EntityFrameworkCore;
using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;
using OrderEase.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEase.DAL.Repository
{
    public class OrderItemRepository : IRepository<OrderItem>
    {
        private AppDataContext _db;

        public OrderItemRepository(AppDataContext db)
        {
            _db = db;
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return _db.OrderItems;
        }

        public async Task<OrderItem> GetAsync(string item)
        {
            bool result = int.TryParse(item, out var id);
            if (result == true)
                return await _db.OrderItems.FirstOrDefaultAsync(x => x.OrderId == id);
            else
                return await _db.OrderItems.FirstOrDefaultAsync(x => x.Name == item);
        }

        public async Task CreateAsync(OrderItem orderItem)
        {
            _db.OrderItems.Add(orderItem);
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _db.Update(orderItem);
        }

        public async Task DeleteAsync(int id)
        {
            var orderItem = await _db.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
            //if (orderItem != null)
                _db.OrderItems.Remove(orderItem);
        }
    }
}
