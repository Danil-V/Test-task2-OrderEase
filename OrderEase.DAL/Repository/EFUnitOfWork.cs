using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;
using OrderEase.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrderEase.DAL.Repository
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private AppDataContext _db;
        private OrderRepository orderRepository;
        private OrderItemRepository orderItemRepository;
        private ProviderRepository providerRepository;

        public EFUnitOfWork(AppDataContext db)
        {
            _db = db;
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(_db);
                return orderRepository;
            }
        }
        public IRepository<OrderItem> OrderItems
        {
            get
            {
                if (orderItemRepository == null)
                    orderItemRepository = new OrderItemRepository(_db);
                return orderItemRepository;
            }
        }

        public IRepository<Provider> Providers
        {
            get
            {
                if (providerRepository == null)
                    providerRepository = new ProviderRepository(_db);
                return providerRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
