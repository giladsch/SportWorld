using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SportWorld.Data;

namespace SportWorld.Controllers
{
    public class Recommendation : Controller
    {
        private readonly SportWorldContext _sportWorldContext;
        public Recommendation(SportWorldContext sportWorldContext)
        {
            _sportWorldContext = sportWorldContext;
        }
        // GET: Recommendation
        public ActionResult Index()
        {
            double min = int.MaxValue;
            var closestUserName = "";
            var usersScore = new Dictionary<string, double>();
            string userName = HttpContext.Session.GetString("ConnectedUserId");
            if (String.IsNullOrEmpty(userName))
            {
                return  RedirectToAction("Index", "Error", new { error = string.Format("Could not find current user") });
            }

            try
            {

                var user = _sportWorldContext.User.FirstOrDefault(u => u.UserName == userName);

                var usersComments = _sportWorldContext.Comment.GroupBy(c => c.Publisher.UserName).ToList();

                usersComments.ForEach(group =>
                {
                    usersScore[group.Key] = 0;
                    group.ToList().ForEach(c =>
                    {
                        usersScore[group.Key] += c.Rating;
                    });
                });

                var userScore = usersScore[userName];

                usersScore.Remove(userName);

                foreach (var item in usersScore)
                {
                    if (min > Math.Abs(userScore - item.Value))
                    {
                        min = userScore - item.Value;
                        closestUserName = item.Key;
                    }
                }

                var closetsUsersComment = _sportWorldContext.Comment.Where(c => c.Publisher.UserName == closestUserName).OrderByDescending(c => c.Rating).ToList().First();
                var allProducts = _sportWorldContext.Product
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.Publisher)
                    .ToList(); ;
                var recommendedProduct = allProducts.First(p => p.Comments.ToList().Contains(closetsUsersComment));

                return View(recommendedProduct);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("an error while get recommendation please try again") });
            }
        }
    }
}
