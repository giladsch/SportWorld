using SportWorld.Data;
using SportWorld.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportWorld.DAL
{
    public class AdminDataAccess
    {
        private readonly CommentDataAccess _commentDataAccess;

        public AdminDataAccess(SportWorldContext sportWorldContext)
        {
            _commentDataAccess = new CommentDataAccess(sportWorldContext);
        }

        public IEnumerable<Comment> GetByUserIdInDateRange(DateTime start, DateTime end, string username)
        {
            var comments = _commentDataAccess.GetAll()
                .Where(c => c.Date >= start &&
                       c.Date <= end &&
                       c.Publisher.UserName == username);
            return comments;
        }
    }
}
