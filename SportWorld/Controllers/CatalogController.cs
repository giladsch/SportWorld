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

        public ActionResult Search(Category category, double from = 0, double to = double.MaxValue)
        {
            try
            {
                if (from < 0 || to <= 0 || from > to)
                {
                    return RedirectToAction("Index", "Error", new { error = "Oops! You have entered an invalid range." });
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

        // GET: Catalog
        public ActionResult Index()
        {
            return View(_productBl.GetAllProducts());
        }
    }
}