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
    public class ProviderRepository : IRepository<Provider>
    {
        private AppDataContext _db;

        public ProviderRepository(AppDataContext db)
        {
            _db = db;
        }

        public IEnumerable<Provider> GetAll()
        {
            return _db.Providers;
        }

        public async Task<Provider> GetAsync(string item)
        {
            bool result = int.TryParse(item, out var id);
            if (result == true)
                return await _db.Providers.FirstOrDefaultAsync(x => x.Id == id);
            else
                return await _db.Providers.FirstOrDefaultAsync(x => x.Name == item);
        }

        public async Task CreateAsync(Provider provider)
        {
            _db.Providers.Add(provider);
        }

        public async Task UpdateAsync(Provider provider)
        {
            _db.Update(provider);
        }

        public async Task DeleteAsync(int id)
        {
            Provider provider = await _db.Providers.FindAsync(id);
            if (provider != null)
                _db.Providers.Remove(provider);
        }
    }
}
