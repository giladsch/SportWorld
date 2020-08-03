using MovieLand.Data;
using MovieLand.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLand.DAL
{
    public class AdminDataAccess
    {
        private readonly MovieLandContext _movieLandContext;

        public AdminDataAccess(MovieLandContext movieLandContext)
        {
            _movieLandContext = movieLandContext;
        }

        public IEnumerable<Comment> GetByUserIdInDateRange(DateTime start, DateTime end, string userId)
        {
            var comments = _movieLandContext.Comment
                .Include(usr => usr.Publisher)
                .Where(c => c.Date >= start &&
                       c.Date <= end &&
                       c.Publisher.ID == userId);
            return comments;
        }
    }
}
