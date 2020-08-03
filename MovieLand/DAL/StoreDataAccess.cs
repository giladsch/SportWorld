using MovieLand.Data;
using MovieLand.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MovieLand.DAL
{
    public class StoreDataAccess
    {
        private readonly MovieLandContext _movieLandContext;

        public StoreDataAccess(MovieLandContext movieLandContext)
        {
            _movieLandContext = movieLandContext;
        }

        public void UpdateStore(Store storeToUpdate)
        {
            _movieLandContext.Store.Update(storeToUpdate);
            _movieLandContext.SaveChanges();
        }

        public Store GetStoreById(int id)
        {
            return _movieLandContext.Store
                .SingleOrDefault(x => x.ID == id);
        }

        public List<Store> GetAllStores()
        {
            return _movieLandContext.Store
                .ToList();
        }

        public IEnumerable<Store> GetStoreByName(string name)
        {
            return _movieLandContext.Store
                                .Where(s => s.Name.Contains(name))
                                .ToList();
        }

        public Store GetStroeById(int storeId)
        {
            return _movieLandContext.Store
                .SingleOrDefault(store => store.ID == storeId);
        }

        public void AddStore(Store store)
        {
            _movieLandContext.Store.Add(store);
            _movieLandContext.SaveChanges();
        }

        public void DeleteStore(Store store)
        {
            _movieLandContext.Store.Remove(store);
            _movieLandContext.SaveChanges();
        }
    }
}
