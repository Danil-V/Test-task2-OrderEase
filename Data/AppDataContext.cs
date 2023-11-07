using Microsoft.EntityFrameworkCore;
using OrderEase.Models.Data;


namespace OrderEase.Data
{
    public class AppDataContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Order>? Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Provider> Providers => Set<Provider>();


        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
            try
            {
                bool isCreated = Database.EnsureCreated();
            }
            catch (Exception ex) 
            { Console.WriteLine(ex); }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // добавляем роли
            Role admin = new Role { Id = 1, Name = "admin" };
            Role user = new Role { Id = 2, Name = "user" };

            modelBuilder.Entity<Role>().HasData(new Role[] { admin, user });

            modelBuilder.Entity<User>().HasData(
                    new User { Id = Guid.NewGuid().GetHashCode(), Email = "admin@gmail.com", Password = "12345", RoleId = admin.Id },
                    new User { Id = Guid.NewGuid().GetHashCode(), Email = "bob@gmail.com", Password = "55555", RoleId = user.Id }
            );
        }
    }
}
