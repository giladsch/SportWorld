using MovieLand.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLand.Controllers
{


    public class StatisticsController : Controller
    {
        private readonly MovieLandContext _movieLandContext;

        public StatisticsController(MovieLandContext movieLandContext)
        {
            _movieLandContext = movieLandContext;
        }

        public PartialViewResult GetStatistics()
        {

            var usersByGender = _movieLandContext.User
                .GroupBy(g => g.Gender)
                .Select(x => new { gender = x.Key.ToString(), count = x.Count() })
                .ToList();
            
            var prodcutsByCategory = _movieLandContext.Product
                .GroupBy(p => p.Category)
                .Select(x => new { category = x.Key.ToString(), count = x.Count() })
                .ToList();

            var commentsByGender = _movieLandContext.Comment
                .GroupBy(c => c.Publisher.Gender)
                .Select(x => new { gender = x.Key.ToString(), count = x.Count() });

            ViewBag.prodcutsByCategory = JsonConvert.SerializeObject(prodcutsByCategory);
            ViewBag.usersByGender = JsonConvert.SerializeObject(usersByGender);
            ViewBag.commentsByGender = JsonConvert.SerializeObject(commentsByGender);

            return PartialView("Views/Statistics/Statistics.cshtml");
        }
    }
}