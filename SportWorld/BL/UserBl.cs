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

        public void AddUser(string id, string name, Gender gender)
        {
            if (_userDal.IsUserExist(id))
                return;

            _userDal.AddUser(id, name, gender);
        }

        public void UpdateUser(User user)
        {
            _userDal.UpdateUser(user);
        }

        public void DeleteUser(User user)
        {
            _userDal.DeletUser(user);
        }

        public User GetById(string id)
        {
            return _userDal.GetUser(id);
        }

        public List<User> GetAllUsers()
        {
            return _userDal.GetAllUsers();
        }
    }
}
