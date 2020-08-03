using Hydra.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Controllers
{


    public class StatisticsController : Controller
    {
        private readonly HydraContext _hydraContext;

        public StatisticsController(HydraContext hydraContext)
        {
            _hydraContext = hydraContext;
        }

        public PartialViewResult GetStatistics()
        {

            var usersByGender = _hydraContext.User
                .GroupBy(g => g.Gender)
                .Select(x => new { gender = x.Key.ToString(), count = x.Count() })
                .ToList();
            
            var prodcutsByCategory = _hydraContext.Product
                .GroupBy(p => p.Category)
                .Select(x => new { category = x.Key.ToString(), count = x.Count() })
                .ToList();

            var commentsByGender = _hydraContext.Comment
                .GroupBy(c => c.Publisher.Gender)
                .Select(x => new { gender = x.Key.ToString(), count = x.Count() });

            ViewBag.prodcutsByCategory = JsonConvert.SerializeObject(prodcutsByCategory);
            ViewBag.usersByGender = JsonConvert.SerializeObject(usersByGender);
            ViewBag.commentsByGender = JsonConvert.SerializeObject(commentsByGender);

            return PartialView("Views/Statistics/Statistics.cshtml");
        }
    }
}