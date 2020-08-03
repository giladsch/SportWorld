using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MovieLand.Data;
using MovieLand.Models;

namespace MovieLand.DAL
{
    public class UserDataAccess
    {
        private readonly MovieLandContext _context;

        public UserDataAccess(MovieLandContext context)
        {
            _context = context;
        }

        public void AddUser(string id, string name, Gender gender)
        {
             _context.User.Add(new User
            {
                ID = id,
                Name = name,
                Gender = gender
            });

            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void DeletUser(User user)
        {
            var  userToDelete = _context
                .User
                .SingleOrDefault(u => u.ID == user.ID);

            if (userToDelete == null)
            {
                throw new
                    Exception(string.Format("could not find user with id {0}", user.ID));
            }

            _context.User.Remove(userToDelete);
            _context.SaveChanges();
        }

        public bool IsUserExist(string id)
        {
            return _context.User.Any(u => u.ID == id);
        }

        public User GetUser(string id) 
        {
            return _context.User.FirstOrDefault(u => u.ID == id);
        }

        public List<User> GetAllUsers()
        {
            return _context.User.ToList();
        }
    }
}
