using System.Linq;
using SportWorld.BL;
using SportWorld.Data;
using SportWorld.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SportWorld.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductBl _productBl;
        private readonly StoreBl _storeBl;

        public CatalogController(SportWorldContext SportWorldContext)
        {
            _productBl = new ProductBl(SportWorldContext);
            _storeBl = new StoreBl(SportWorldContext);
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
            if (category.HasValue)
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
    }
}