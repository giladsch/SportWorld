using System;
using SportWorld.BL;
using SportWorld.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SportWorld.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentBl _commentBl;


        public CommentController(SportWorldContext context)
        {
            _commentBl = new CommentBl(context);
        }

        [HttpPost]
        public ActionResult Add(IFormCollection form)
        {
            _commentBl.Add(form["ProductId"], form["Comment"], HttpContext.Session.GetString("ConnectedUserId"), double.Parse(form["Rating"]));

            return RedirectToAction("Details", "Product", new { id = form["ProductId"] });
        }
    }
}
