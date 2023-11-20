using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;
using OrderEase.DAL.Interfaces;

namespace OrderEase.DAL.Repository
{
    public class EFUnitOfWork : Interfaces.IUnitOfWork
    {
        private AppDataContext _db;
        private OrderRepository _orderRepository;
        private OrderItemRepository _orderItemRepository;
        private ProviderRepository _providerRepository;
        private UserRepository _userRepository;
        private RoleRepository _roleRepository;

        public EFUnitOfWork(AppDataContext db)
        {
            _db = db;
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository(_db);
                return _orderRepository;
            }
        }
        public IRepository<OrderItem> OrderItems
        {
            get
            {
                if (_orderItemRepository == null)
                    _orderItemRepository = new OrderItemRepository(_db);
                return _orderItemRepository;
            }
        }

        public IRepository<Provider> Providers
        {
            get
            {
                if (_providerRepository == null)
                    _providerRepository = new ProviderRepository(_db);
                return _providerRepository;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_db);
                return _userRepository;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_db);
                return _roleRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
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
