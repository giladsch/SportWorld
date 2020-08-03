using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportWorld.Models;
using SportWorld.BL;
using SportWorld.Data;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SportWorld.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductBl _productBl;

        public HomeController(SportWorldContext SportWorldContext)
        {
            _productBl = new ProductBl(SportWorldContext);
        }

        public IActionResult Index()
        {
            ViewBag.Categories = Enum.GetValues(typeof(Category));
            return View(_productBl.GetAllProducts());
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Please Contact Us";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }                
    }
}
