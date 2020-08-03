using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.BL;
using Hydra.DAL;
using Hydra.Data;
using Hydra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreBl _storeBl;

        public StoreController(HydraContext hydraContext)
        {
            _storeBl = new StoreBl(hydraContext);
        }


        // GET: Store
        public ActionResult Index()
        {
            return View(_storeBl.GetAllStores());
        }

        public ActionResult ById(int id)
        {
            return View("Views/Store/ByStoreId.cshtml", _storeBl.GetStoreById(id));
        }

        // GET: Store/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }

            return View();
        }

        // POST: Store/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind("Name", "Lontitude", "Latitude", "OpeningHour", "ClosingHour")] Store store)
        {
            try
            {
                var errorMessage = GetErrorIfInvalid(store);

                if(!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }

                _storeBl.AddStore(store);
                return RedirectToAction("Index", "About");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }
            try
            {
                var store = _storeBl.GetStoreById(id);
                if (store == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find store with id {0}", id) });
                }

                return View(store);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: Store/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Name", "Lontitude", "Latitude", "OpeningHour", "ClosingHour")] Store store)
        {
            try
            {
                var storeToEdit = _storeBl.GetStoreById(id);

                if (storeToEdit == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find store with id {0}", id) });
                }

                var errorMessage = GetErrorIfInvalid(store);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }
               
                storeToEdit.Latitude = store.Latitude;
                storeToEdit.Lontitude = store.Lontitude;
                storeToEdit.Name = store.Name;
                storeToEdit.OpeningHour = store.OpeningHour;
                storeToEdit.ClosingHour = store.ClosingHour;

                _storeBl.UpdateStore(storeToEdit);
                return View(storeToEdit);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Store/Delete/5
        public ActionResult Delete(int id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }

            try
            {
                var store = _storeBl.GetStoreById(id);
                return store == null
                    ? RedirectToAction("Index", "Error", new { error = string.Format("Could not find store with id {0}", id) })
                    : (ActionResult)View(store);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: Store/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var store = _storeBl.GetStoreById(id);
                try
                {
                    _storeBl.DeleteStore(store);
                }
                catch
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find store with id {0}", id) });
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("Oops! failed to delete product with id {0}", id) });
            }
        }

        private string GetErrorIfInvalid(Store store)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(store.Name))
            {
                error = "Store name cant be null";
            }

            if (store.Lontitude <= 0 || store.Latitude <= 0)
            {
                error = "Lontitude or latitude are zero or less";
            }

            if (string.IsNullOrEmpty(store.OpeningHour))
            {
                error = "Opening hours cant be null";
            }

            if (string.IsNullOrEmpty(store.ClosingHour))
            {
                error = "Closing hours name cant be null";
            }

            var closingHh = int.Parse(store.ClosingHour.Split(':')[0]);
            var openingHh = int.Parse(store.OpeningHour.Split(':')[0]);

            if (closingHh < openingHh)
            {
                error = "Closing hour cant be before opening hours";
            }

            return error;
        }

        private bool IsAdminConnected()
        {
            var isAdminConnected = HttpContext.Session.GetInt32("IsAdminConnected") ?? 0;
            return isAdminConnected == 1 ? true : false;
        }
    }
}