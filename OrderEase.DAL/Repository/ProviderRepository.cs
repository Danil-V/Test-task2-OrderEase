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

        public Provider Get(int id)
        {
            return _db.Providers.Find(id);
        }

        public void Create(Provider provider)
        {
            _db.Providers.Add(provider);
        }

        public void Update(Provider provider)
        {
            _db.Entry(provider).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Provider provider = _db.Providers.Find(id);
            if (provider != null)
                _db.Providers.Remove(provider);
        }
    }
}
