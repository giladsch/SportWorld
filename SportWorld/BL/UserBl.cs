using System;
using System.Collections.Generic;
using SportWorld.DAL;
using SportWorld.Data;
using SportWorld.Models;

namespace SportWorld.BL
{
    public class UserBl
    {
        private readonly UserDataAccess _userDal;

        public UserBl(SportWorldContext context)
        {
            _userDal = new UserDataAccess(context);
        }

        public void AddUser(User user)
        {
            if (_userDal.IsUserExist(user.UserName))
                return;

            _userDal.AddUser(user);
        }

        public bool IsUserExist(string username)
        {
            return _userDal.IsUserExist(username);
        }

        public void UpdateUser(User user)
        {
            _userDal.UpdateUser(user);
        }

        public void DeleteUser(User user)
        {
            _userDal.DeletUser(user);
        }

        public User GetById(string username)
        {
            return _userDal.GetUser(username);
        }

        public List<User> GetAllUsers()
        {
            return _userDal.GetAllUsers();
        }

        public int GetHowManyAdmins()
        {
            return _userDal.GetHowManyAdmins();
        }
    }
}
