using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieLand.Models;
using MovieLand.BL;
using MovieLand.Data;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MovieLand.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductBl _productBl;

        public HomeController(MovieLandContext movieLandContext)
        {
            _productBl = new ProductBl(movieLandContext);
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
