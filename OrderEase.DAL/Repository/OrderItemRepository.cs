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

        public OrderItem Get(int id)
        {
            return _db.OrderItems.Find(id);
        }

        public void Create(OrderItem orderItem)
        {
            _db.OrderItems.Add(orderItem);
        }

        public void Update(OrderItem orderItem)
        {
            _db.Entry(orderItem).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            OrderItem orderItem = _db.OrderItems.Find(id);
            if (orderItem != null)
                _db.OrderItems.Remove(orderItem);
        }
    }
}
