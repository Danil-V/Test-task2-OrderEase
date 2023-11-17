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
    public class RoleRepository : IRepository<Role>
    {
        private AppDataContext _db;

        public RoleRepository(AppDataContext db)
        {
            _db = db;
        }

        public IEnumerable<Role> GetAll()
        {
            return _db.Roles;
        }

        public async Task<Role> GetAsync(string item)
        {
            bool result = int.TryParse(item, out var id);
            if (result == true)
                return await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);
            else
                return await _db.Roles.FirstOrDefaultAsync(x => x.Name == item);
        }

        public async Task CreateAsync(Role role)
        {
            _db.Roles.Add(role);
        }

        public async Task UpdateAsync(Role role)
        {
            _db.Update(role);
        }

        public async Task DeleteAsync(int id)
        {
            Role role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role != null)
                _db.Roles.Remove(role);
        }
    }
}
