using SportWorld.DAL;
using SportWorld.Data;
using SportWorld.Models;
using System.Collections.Generic;
using System.Linq;

namespace SportWorld.BL
{
    public class StoreBl
    {
        private readonly StoreDataAccess _storeDataAccess;

        public StoreBl(SportWorldContext SportWorldContext)
        {
            _storeDataAccess = new StoreDataAccess(SportWorldContext);
        }

        public List<Store> GetAllStores()
        {
            return _storeDataAccess.GetAllStores();
        }

        public IEnumerable<Store> GetStoreByName(string name)
        {
            return _storeDataAccess.GetStoreByName(name);
        }

        public void AddStore(Store store)
        {
            _storeDataAccess.AddStore(store);
        }

        public Store GetStoreById(int id)
        {
            return _storeDataAccess.GetStoreById(id);
        }

        public void UpdateStore(Store storeToUpdate)
        {
            _storeDataAccess.UpdateStore(storeToUpdate);
        }

        public void DeleteStore(Store store)
        {
            _storeDataAccess.DeleteStore(store);
        }
    }
}
