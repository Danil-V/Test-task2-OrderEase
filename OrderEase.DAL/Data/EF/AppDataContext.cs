using Microsoft.EntityFrameworkCore;
using OrderEase.DAL.Data.Models.Data;

namespace OrderEase.DAL.Data.EF
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
            // Добавляем роли:
            Role admin = new Role { Id = 1, Name = "admin" };
            Role user = new Role { Id = 2, Name = "user" };

            modelBuilder.Entity<Role>().HasData(new Role[] { admin, user });

            // Добавляем поставщиков:
            Provider twigo = new Provider { Id = 1, Name = "Twigo" };
            Provider mvideo = new Provider { Id = 2, Name = "М.Видео" };
            Provider dns = new Provider { Id = 3, Name = "DNS" };
            Provider eldorado = new Provider { Id = 4, Name = "Эльдорадо" };

            modelBuilder.Entity<Provider>().HasData(new Provider[] { twigo, mvideo, dns, eldorado });

            // Добавляем первого пользователя (администратора):
            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Email = "admin@gmail.com", Password = "12345", RoleId = admin.Id });
        }
    }
}
