using SportWorld.Data;
using SportWorld.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SportWorld.DAL
{
    public class StoreDataAccess
    {
        private readonly SportWorldContext _SportWorldContext;

        public StoreDataAccess(SportWorldContext SportWorldContext)
        {
            _SportWorldContext = SportWorldContext;
        }

        public void UpdateStore(Store storeToUpdate)
        {
            _SportWorldContext.Store.Update(storeToUpdate);
            _SportWorldContext.SaveChanges();
        }

        public Store GetStoreById(int id)
        {
            return _SportWorldContext.Store
                .SingleOrDefault(x => x.ID == id);
        }

        public List<Store> GetAllStores()
        {
            return _SportWorldContext.Store
                .ToList();
        }

        public IEnumerable<Store> GetStoreByName(string name)
        {
            return _SportWorldContext.Store
                                .Where(s => s.Name.Contains(name))
                                .ToList();
        }

        public Store GetStroeById(int storeId)
        {
            return _SportWorldContext.Store
                .SingleOrDefault(store => store.ID == storeId);
        }

        public void AddStore(Store store)
        {
            _SportWorldContext.Store.Add(store);
            _SportWorldContext.SaveChanges();
        }

        public void DeleteStore(Store store)
        {
            _SportWorldContext.Store.Remove(store);
            _SportWorldContext.SaveChanges();
        }
    }
}
