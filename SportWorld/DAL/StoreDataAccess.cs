using SportWorld.Data;
using SportWorld.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SportWorld.DAL
{
    public class StoreDataAccess
    {
        private readonly SportWorldContext _sportWorldContext;

        public StoreDataAccess(SportWorldContext sportWorldContext)
        {
            _sportWorldContext = sportWorldContext;
        }

        public void UpdateStore(Store storeToUpdate)
        {
            _sportWorldContext.Store.Update(storeToUpdate);
            _sportWorldContext.SaveChanges();
        }

        public Store GetStoreById(int id)
        {
            return _sportWorldContext.Store
                .SingleOrDefault(x => x.ID == id);
        }

        public List<Store> GetAllStores()
        {
            return _sportWorldContext.Store
                .ToList();
        }

        public IEnumerable<Store> GetStoreByName(string name)
        {
            return _sportWorldContext.Store
                                .Where(s => s.Name.Contains(name))
                                .ToList();
        }

        public Store GetStroeById(int storeId)
        {
            return _sportWorldContext.Store
                .SingleOrDefault(store => store.ID == storeId);
        }

        public void AddStore(Store store)
        {
            _sportWorldContext.Store.Add(store);
            _sportWorldContext.SaveChanges();
        }

        public void DeleteStore(Store store)
        {
            _sportWorldContext.Store.Remove(store);
            _sportWorldContext.SaveChanges();
        }
    }
}
