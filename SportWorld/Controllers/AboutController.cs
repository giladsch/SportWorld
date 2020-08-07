using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SportWorld.BL;
using SportWorld.Data;
using SportWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SportWorld.Controllers
{
    public class AboutController : Controller
    {
        private readonly StoreBl _storeBl;
        private readonly Keys _keys;

        public AboutController(SportWorldContext SportWorldContext, Keys keys)
        {
            _storeBl = new StoreBl(SportWorldContext);
            _keys = keys;
        }

        public IActionResult Index()
        {
            ViewData["Map"] = _keys.Map;
            ViewData["Message"] = "SportWorld";
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
                    var tempUrl = $"http://api.openweathermap.org/data/2.5/weather?units=metric&lat={lat}&lon={lon}&APPID={_keys.Weather}";
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