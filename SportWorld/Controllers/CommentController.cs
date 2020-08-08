using System;
using SportWorld.BL;
using SportWorld.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportWorld.Models;

namespace SportWorld.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentBl _commentBl;
        private readonly UserBl _userBl;


        public CommentController(SportWorldContext context)
        {
            _commentBl = new CommentBl(context);
            _userBl = new UserBl(context);
        }

        public ActionResult Details‬()
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to view comments data." });
            }

            return View(_commentBl.GetAll());
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to delete." });
            }

            try
            {
                Comment comment = _commentBl.GetById(int.Parse(id));

                return View(comment);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to delete." });
            }
            try
            {
                try
                {
                    _commentBl.Delete(int.Parse(id));
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("error while deleting comment") });
                }

                return RedirectToAction("Details", "Comment");
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("error while deleting comment") });
            }
        }

        public ActionResult Edit(string id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit." });
            }
            try
            {
                Comment comment = _commentBl.GetById(int.Parse(id));
                if (comment == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find comment with id {0}", id) });
                }
                ViewBag.users = _userBl.GetAllUsers();
                return View(comment);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string username, double rating, string text)
        {
            double newRating = rating < 0 ? 0 : rating > 10 ? 10 : rating;
            try
            {
                Comment commentToedit = _commentBl.GetById(int.Parse(id));
                User user = _userBl.GetById(username);

                if (commentToedit == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find comment with username {0}", id) });
                }

                commentToedit.Text = text;
                commentToedit.Publisher = user;
                commentToedit.Rating = newRating;
                commentToedit.Date = DateTime.Now;

                _commentBl.Update(commentToedit);

                return RedirectToAction("Details", "Comment");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        public ActionResult Add(IFormCollection form)
        {
            _commentBl.Add(form["ProductId"], form["Comment"], HttpContext.Session.GetString("ConnectedUserId"), double.Parse(form["Rating"]));

            return RedirectToAction("Details", "Product", new { id = form["ProductId"] });
        }


        private bool IsAdminConnected()
        {
            return HttpContext.Session.GetString("IsAdminConnected") == "true" ? true : false;
        }
    }
}
