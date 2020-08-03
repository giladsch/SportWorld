using System;
using Hydra.BL;
using Hydra.Data;
using Hydra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers
{
    public class UserController: Controller
    {
        private readonly UserBl _userBl;

        public UserController(HydraContext context)
        {
            _userBl = new UserBl(context);
        }

        public ActionResult Create()
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }

            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind("ID", "Name", "Gender")] User user)
        {
            try
            {
                var errorMessage = GetErrorIfInvalid(user);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }

                var userToAdd = new User
                {
                    ID = user.ID,
                    Name = user.Name,
                    Gender = user.Gender
                };

                _userBl.AddUser(user.ID, user.Name, user.Gender);

                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        } 

        // GET: User/Edit/
        public ActionResult Edit(string id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }
            try
            {
                User user = _userBl.GetById(id);
                if (user == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with id {0}", id) });
                }

                return View(user);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("ID", "Name", "Gender")] User user)
        {
            try
            {
                var errorMessage = GetErrorIfInvalid(user);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return RedirectToAction("Index", "Error", new { error = errorMessage });
                }

                User userToEdit = _userBl.GetById(user.ID);

                if (userToEdit == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with id {0}", id) });
                }

                userToEdit.Name = user.Name;
                userToEdit.Gender = user.Gender;

                _userBl.UpdateUser(userToEdit);

                return RedirectToAction("Details", "User");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: User/Delete/
        public ActionResult Delete(String id)
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to edit. please go to admin page" });
            }

            try
            {
                User user = _userBl.GetById(id);
                if (user == null)
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with id {0}", id) });
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
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                User userToDelete = _userBl.GetById(id);
                try
                {
                    _userBl.DeleteUser(userToDelete);
                }
                catch
                {
                    return RedirectToAction("Index", "Error", new { error = string.Format("Could not find user with id {0}", id) });
                }

                return RedirectToAction("Details", "User");
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { error = string.Format("Oops! failed to delete product with id {0}", id) });
            }
        }

        public ActionResult Details()
        {
            if (!IsAdminConnected())
            {
                return RedirectToAction("Index", "Error", new { error = "You must be an admin to view users data. please go to admin page" });
            }

            return View(_userBl.GetAllUsers());
        }

        private string GetErrorIfInvalid(User user)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                error = "user name cant be empty or null";
            }

            if (user.Gender.ToString().Length < 1)
            {
                error = "Please Select Gender";
            }
            
            return error;
        }

        [HttpGet]
        public void Login(string id, string name)
        {
            _userBl.AddUser(id, name, GetRandom());

            SetUserInSession(id);
        }

        [HttpGet]
        public void Logout()
        {
            SetUserInSession();
        }

        private void SetUserInSession(string id = "")
        {
            HttpContext.Session.SetString("ConnectedUserId", id);
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
            var isAdminConnected = HttpContext.Session.GetInt32("IsAdminConnected") ?? 0;
            return isAdminConnected == 1 ? true : false;
        }
    }
}
