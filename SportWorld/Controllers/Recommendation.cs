using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SportWorld.BL;
using SportWorld.DAL;
using SportWorld.Data;

namespace SportWorld.Controllers
{
    public class Recommendation : Controller
    {
        private readonly ProductBl _productBl;
        private readonly CommentBl _commentBl;
        private readonly UserBl _userBl;
        public Recommendation(SportWorldContext sportWorldContext)
        {
            _productBl = new ProductBl(sportWorldContext);
            _commentBl = new CommentBl(sportWorldContext);
            _userBl = new UserBl(sportWorldContext);
        }

        // GET: Recommendation
        public ActionResult Index()
        {
            double min = int.MaxValue;
            var closestUserName = "";
            string userName = HttpContext.Session.GetString("ConnectedUserId");
            if (String.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("Could not find current user") });
            }

            try
            {
                var user = _userBl.GetById(userName);

                // dictionary of score of user
                var usersScore = _commentBl.GetAll().GroupBy(c => c.Publisher.UserName)
                             .Select(g => new
                             {
                                 g.Key,
                                 Value = g.Sum(x => x.Rating)
                             }).ToDictionary(x => x.Key, x => x.Value);

                // get current user score
                var userScore = usersScore[userName];

                usersScore.Remove(userName);

                // get closest user for current user
                foreach (var item in usersScore)
                {
                    if (min > Math.Abs(userScore - item.Value))
                    {
                        min = userScore - item.Value;
                        closestUserName = item.Key;
                    }
                }

                var allProducts = _productBl.GetAllProducts();

                var comment = allProducts.FindAll(p => p.Comments.Any(c => c.Publisher != user && c.Publisher.UserName == closestUserName)).SelectMany(p => p.Comments).ToList().FindAll(c => c.Publisher.UserName == closestUserName).OrderByDescending(c => c.Rating).FirstOrDefault();

                if(comment == null)
                {
                    var highestComment = allProducts.SelectMany(p => p.Comments).ToList().OrderByDescending(c => c.Rating).FirstOrDefault();
                    var recommendedHeightestProduct = allProducts.First(p => p.Comments.ToList().Contains(comment));
                    return View(recommendedHeightestProduct);
                }

                var recommendedProduct = allProducts.First(p => p.Comments.ToList().Contains(comment));

                return View(recommendedProduct);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("an error while get recommendation please try again") });
            }
        }
    }
}
