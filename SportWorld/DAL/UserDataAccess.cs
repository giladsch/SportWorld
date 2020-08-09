using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportWorld.Data;
using SportWorld.Models;

namespace SportWorld.DAL
{
    public class UserDataAccess
    {
        private readonly SportWorldContext _sportWorldContext;

        public UserDataAccess(SportWorldContext sportWorldContext)
        {
            _sportWorldContext = sportWorldContext;
        }

        public void AddUser(User user)
        {
            _sportWorldContext.User.Add(user);

            _sportWorldContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _sportWorldContext.User.Update(user);
            _sportWorldContext.SaveChanges();
        }

        public void DeletUser(User user)
        {
            var userToDelete = _sportWorldContext
                .User
                .SingleOrDefault(u => u.UserName == user.UserName);

            if (userToDelete == null)
            {
                throw new
                    Exception(string.Format("could not find user with userName {0}", user.UserName));
            }

            userToDelete.IsDeleted = true;

            _sportWorldContext.User.Update(user);
            _sportWorldContext.SaveChanges();
        }

        public bool IsUserExist(string username)
        {
            return _sportWorldContext.User.Any(u => u.UserName == username);
        }

        public User GetUser(string username)
        {
            return _sportWorldContext.User.FirstOrDefault(u => u.UserName == username);
        }

        public List<User> GetAllUsers()
        {
            return _sportWorldContext.User.ToList();
        }

        public int GetHowManyAdmins()
        {
            return _sportWorldContext.User.Count(u => u.IsAdmin ==true);
        }
    }
}
