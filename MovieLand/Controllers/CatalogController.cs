using System.Linq;
using Hydra.BL;
using Hydra.Data;
using Hydra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductBl _productBl;
        private readonly StoreBl _storeBl;

        public CatalogController(HydraContext hydraContext)
        {
            _productBl = new ProductBl(hydraContext);
            _storeBl = new StoreBl(hydraContext);
        }

        public ActionResult ByCategory(Category category)
        {
            return View("Views/Catalog/index.cshtml", _productBl.GetProductsByCategory(category));
        }

        public ActionResult Search(Category category, double from, double to)
        {
            try
            {
                if (from <= 0 || to <= 0)
                {
                    return RedirectToAction("Index", "Error", new { error = "from or to cant be negative or zero" });
                }

                if (from > to)
                {
                    return RedirectToAction("Index", "Error", new { error = "from range cant be higer than to" });
                }

                var products = _productBl.GetProductsByCategory(category)
                    .Where(p => p.Price <= to && p.Price >= from);
                return View("Views/Catalog/index.cshtml", products);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult Predict(string name)
        {
            Category? category = _productBl.NaiveBayesFetchCategoryByName(name);
            if(category.HasValue)
            {
                ViewBag.productName = name;
                ViewBag.category = category;

                return View();
            }

            return RedirectToAction("Index", "Error", new { error = "Oops! search yeald no results" });
        }

        // GET: Catalog
        public ActionResult Index()
        {
            return View(_productBl.GetAllProducts());
        }

        // GET: Catalog/Details/
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Catalog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catalog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Catalog/Edit/
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Catalog/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Catalog/Delete/
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Catalog/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}