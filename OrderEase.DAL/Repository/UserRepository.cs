using Microsoft.EntityFrameworkCore;
using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;
using OrderEase.DAL.Interfaces;

namespace OrderEase.DAL.Repository
{
    public class UserRepository : IRepository<User>
    {
        private AppDataContext _db;

        public UserRepository(AppDataContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users;
        }

        public async Task<User> GetAsync(string item)
        {
            bool result = int.TryParse(item, out var id);
            if (result == true)
                return await _db.Users.FindAsync(id);
            else
                return await _db.Users.FirstOrDefaultAsync(x => x.Email == item);
        }

        public async Task CreateAsync(User user)
        {
            _db.Users.Add(user);
        }

        public async Task UpdateAsync(User user)
        {
            _db.Update(user);
        }

        public async Task DeleteAsync(int id)
        {
            User user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
                _db.Users.Remove(user);
        }
    }
}
