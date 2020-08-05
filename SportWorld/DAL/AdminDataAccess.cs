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
        private readonly SportWorldContext _SportWorldContext;

        public AdminDataAccess(SportWorldContext SportWorldContext)
        {
            _SportWorldContext = SportWorldContext;
        }

        public IEnumerable<Comment> GetByUserIdInDateRange(DateTime start, DateTime end, string username)
        {
            var comments = _SportWorldContext.Comment
                .Include(usr => usr.Publisher)
                .Where(c => c.Date >= start &&
                       c.Date <= end &&
                       c.Publisher.UserName == username);
            return comments;
        }
    }
}
