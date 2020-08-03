﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MovieLand.BL;
using MovieLand.Data;
using MovieLand.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieLand.Controllers
{
    public class AboutController : Controller
    {
        private readonly StoreBl _storeBl;
        private readonly ISecretSettings _secrets;

        public AboutController(MovieLandContext movieLandContext, [FromServices]ISecretSettings secrets)
        {
            _storeBl = new StoreBl(movieLandContext);
            _secrets = secrets;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "MovieLand pop";
            ViewData["MapCredantials"] = _secrets.MapCredantials;

            return View();
        }

        [HttpGet]
        public IEnumerable<Store> GetStoresByName(string name)
        {
            try
            {
                return name == null 
                    ? _storeBl.GetAllStores() 
                    : _storeBl.GetStoreByName(name);
            }
            catch
            {
                throw new Exception("failed getting store by name");
            }
        }

        [HttpGet]
        public async Task<string> GetTemprature(double lon, double lat)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var tempUrl = $"http://api.openweathermap.org/data/2.5/weather?units=metric&lat={lat}&lon={lon}&APPID={_secrets.WeatherKey}";
                    var res = await client.GetAsync(tempUrl);
                    res.EnsureSuccessStatusCode();
                    var content = await res.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JObject>(content);
                    return json?["main"]?["temp"]?.ToString();
                }
                catch
                {
                    throw new Exception("failed getting the weather");
                }
            }
        }
    }
}