using System;
using SportWorld.BL;
using SportWorld.Data;
using SportWorld.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SportWorld.Controllers
{
    public class UserController : Controller
    {
        private readonly UserBl _userBl;

        public UserController(SportWorldContext context)
        {
            _userBl = new UserBl(context);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind("Gender", "UserName", "Password", "Email")] User user)
        {
            try
            {
                var errorMessage = GetErrorIfInvalid(user, true);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }

                var userToAdd = new User
                {
                    Gender = user.Gender,
                    UserName = user.UserName,
                    Password = user.Password,
                    Email = user.Email,
                    IsAdmin = false
                };

                _userBl.AddUser(userToAdd);
                SetUserInSession(userToAdd.UserName, user.IsAdmin);

                return RedirectToAction("Index", "About");

            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: User/Edit/
        public ActionResult Edit(string UserName)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit." });
            }
            try
            {
                User user = _userBl.GetById(UserName);
                if (user == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with id {0}", UserName) });
                }

                return View(user);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: User/Edit/name
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string username, [Bind("Gender", "UserName", "Email, IsAdmin")] User user)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit." });
            }

            try
            {
                var errorMessage = GetErrorIfInvalid(user, false);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }

                User userToEdit = _userBl.GetById(username);

                if (userToEdit == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with username {0}", username) });
                }

                // in case of last admin make himself not admin
                if (_userBl.GetHowManyAdmins() == 1 && HttpContext.Session.GetString("ConnectedUserId") == user.UserName && user.IsAdmin == false)
                {
                    user.IsAdmin = true;
                }

                userToEdit.Gender = user.Gender;
                userToEdit.UserName = user.UserName;
                userToEdit.Email = user.Email;
                userToEdit.IsAdmin = user.IsAdmin;

                _userBl.UpdateUser(userToEdit);

                return RedirectToAction("Details", "User");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: User/Delete/
        public ActionResult Delete(string username)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit." });
            }

            try
            {
                User user = _userBl.GetById(username);
                if (user == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with username {0}", username) });
                }
                return View(user);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string username, IFormCollection collection)
        {
            try
            {
                User userToDelete = _userBl.GetById(username);
                try
                {
                    _userBl.DeleteUser(userToDelete);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with id {0}", username) });
                }

                return RedirectToAction("Details", "User");
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("Oops! failed to delete product with id {0}", username) });
            }
        }

        public ActionResult Details()
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to view users data." });
            }

            return View(_userBl.GetAllUsers());
        }

        private string GetErrorIfInvalid(User user, bool shouldCheckUsername)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                error = "user name cant be empty or null";
            }
            if (shouldCheckUsername && _userBl.IsUserExist(user.UserName))
            {
                error = "user name already exist";
            }

            if (user.Gender.ToString().Length < 1)
            {
                error = "Please Select Gender";
            }

            return error;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateLogin(string username, string password)
        {
            var user = _userBl.GetById(username);
            if (user == null)
            {
                return RedirectToAction("Index", "Error", new { error = "User dosent exist" });
            }
            else
            {
                if (user.Password != password)
                {
                    return RedirectToAction("Index", "Error", new { error = "username or password wrong" });
                }
                SetUserInSession(user.UserName, user.IsAdmin);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            SetUserInSession("", false);
            return RedirectToAction("Index", "Home");
        }

        private void SetUserInSession(string username, bool isAdmin)
        {

            HttpContext.Session.SetString("IsAdminConnected", isAdmin.ToString().ToLower());
            HttpContext.Session.SetString("ConnectedUserId", username);
        }

        private Gender GetRandom()
        {
            var num = new Random().Next(1, 4);

            return num == 1
                ? Gender.Male
                : num == 2
                    ? Gender.Female
                    : Gender.Other;
        }

        private bool IsAdminConnected()
        {
            return HttpContext.Session.GetString("IsAdminConnected") == "true" ? true : false;
        }
    }
}
