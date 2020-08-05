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
        private readonly SportWorldContext _context;

        public UserDataAccess(SportWorldContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.User.Add(user);

            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void DeletUser(User user)
        {
            var userToDelete = _context
                .User
                .SingleOrDefault(u => u.UserName == user.UserName);

            if (userToDelete == null)
            {
                throw new
                    Exception(string.Format("could not find user with userName {0}", user.UserName));
            }

            userToDelete.IsDeleted = true;

            _context.User.Update(user);
            _context.SaveChanges();
        }

        public bool IsUserExist(string username)
        {
            return _context.User.Any(u => u.UserName == username);
        }

        public User GetUser(string username)
        {
            return _context.User.FirstOrDefault(u => u.UserName == username);
        }

        public List<User> GetAllUsers()
        {
            return _context.User.ToList();
        }
    }
}
