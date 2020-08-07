using SportWorld.DAL;
using SportWorld.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SportWorld.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly AdminDataAccess _adminDataAccess;

        public AdminController(SportWorldContext SportWorldContext)
        {
            _adminDataAccess = new AdminDataAccess(SportWorldContext);
            _userDataAccess = new UserDataAccess(SportWorldContext);
        }

        public ActionResult Index()
        {

            if (IsAdminConnected())
            {
                ViewBag.users = _userDataAccess.GetAllUsers();
                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult Search(DateTime start, DateTime end, string username)
        {
            try
            {
                var comments = _adminDataAccess.GetByUserIdInDateRange(start, end, username);
                return View(comments);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        private bool IsAdminConnected()
        {
            return HttpContext.Session.GetString("IsAdminConnected") == "true" ? true : false;
        }
    }
}
