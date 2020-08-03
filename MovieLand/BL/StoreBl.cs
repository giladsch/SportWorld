using MovieLand.DAL;
using MovieLand.Data;
using MovieLand.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieLand.BL
{
    public class StoreBl
    {
        private readonly StoreDataAccess _storeDataAccess;

        public StoreBl(MovieLandContext movieLandContext)
        {
            _storeDataAccess = new StoreDataAccess(movieLandContext);
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
