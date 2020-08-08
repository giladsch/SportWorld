using SportWorld.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportWorld.Controllers
{


    public class StatisticsController : Controller
    {
        private readonly SportWorldContext _SportWorldContext;

        public StatisticsController(SportWorldContext SportWorldContext)
        {
            _SportWorldContext = SportWorldContext;
        }

        public PartialViewResult GetStatistics()
        {

            var usersByGender = _SportWorldContext.User
                .GroupBy(user => user.Gender)
                .Select(x => new { gender = x.Key.ToString(), count = x.Count() })
                .ToList();
            
            var prodcutsByCategory = _SportWorldContext.Product
                .GroupBy(product => product.Category)
                .Select(x => new { category = x.Key.ToString(), count = x.Count() })
                .ToList();

            var commentsByGender = _SportWorldContext.Comment
                .GroupBy(comment => comment.Publisher.Gender)
                .Select(x => new { gender = x.Key.ToString(), count = x.Count() });

            ViewBag.prodcutsByCategory = JsonConvert.SerializeObject(prodcutsByCategory);
            ViewBag.usersByGender = JsonConvert.SerializeObject(usersByGender);
            ViewBag.commentsByGender = JsonConvert.SerializeObject(commentsByGender);

            return PartialView("Views/Statistics/Statistics.cshtml");
        }
    }
}