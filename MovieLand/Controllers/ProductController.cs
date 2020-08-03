using System;
using Hydra.BL;
using Hydra.Data;
using Hydra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductBl _productBl;

        public ProductController(HydraContext hydraContext)
        {
            _productBl = new ProductBl(hydraContext);
        }

        // GET: Product
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Error");
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            ViewData["UserId"] =
                HttpContext.Session.GetString("ConnectedUserId") ??
                string.Empty;

            return View(_productBl.GetProductById(id));
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        public ActionResult Create(int id, [Bind("ID", "Name", "Price", "ImageUrl", "Category", "Description")] Product product)
        {
            try
            {
                var errorMessage = GetErrorIfInvalid(product);

                if(!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }

                var productToAdd = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Category = product.Category
                };

                _productBl.SaveProdcut(productToAdd);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }
            try
            {
                var product = _productBl.GetProductById(id);
                if (product == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find product with id {0}", id) });
                }

                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("ID", "Name", "Price", "ImageUrl", "Category", "Description")] Product product)
        {
            try
            {
                var productToEdit = _productBl.GetProductById(id);

                if(productToEdit == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find product with id {0}", id) });

                }

                var errorMessage = GetErrorIfInvalid(product);
                
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }

                productToEdit.Description = product.Description;
                productToEdit.Category = product.Category;
                productToEdit.ImageUrl = product.ImageUrl;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                _productBl.UpdateProduct(productToEdit);
                return View(productToEdit);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }

            try
            {
                var product = _productBl.GetProductById(id);
                if (product == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find product with id {0}", id) });
                }
                return View(product);
            }
            catch 
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var product = _productBl.GetProductById(id);
                try
                {
                    _productBl.DeleteProduct(product);
                }
                catch
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find product with id {0}", id) });
                }

                return RedirectToAction("Index", "Catalog");
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("Oops! failed to delete product with id {0}", id) });
            }
        }

        private string GetErrorIfInvalid(Product product)
        {
            var error = string.Empty;

            if (product.Price <= 0)
            {
                error = "Product price cant be negative or zero";
            }
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                error = "Product name cant be empty or null";
            }

            if (string.IsNullOrWhiteSpace(product.ImageUrl))
            {
                error = "Product image url cant be empty or null";
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                error = "Product description cant be empty or null";
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