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


        public CommentController(SportWorldContext context)
        {
            _commentBl = new CommentBl(context);
        }

        public ActionResult Details‬()
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to view comments data." });
            }

            return View(_commentBl.GetAll());
        }

        [HttpPost]
        public ActionResult Add(IFormCollection form)
        {
            _commentBl.Add(form["ProductId"], form["Comment"], HttpContext.Session.GetString("ConnectedUserId"), double.Parse(form["Rating"]));

            return RedirectToAction("Details", "Product", new { id = form["ProductId"] });
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit." });
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
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit." });
            }
            try
            {
                try
                {
                    _commentBl.Delete(int.Parse(id));
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format( "error while deleting comment")});
                }

                return RedirectToAction("Details", "Comment");
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("error while deleting comment") });
            }
        }

        private bool IsAdminConnected()
        {
            return HttpContext.Session.GetString("IsAdminConnected") == "true" ? true : false;
        }
    }
}
