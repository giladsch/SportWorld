using Hydra.Data;
using Hydra.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.DAL
{
    public class AdminDataAccess
    {
        private readonly HydraContext _hydraContext;

        public AdminDataAccess(HydraContext hydraContext)
        {
            _hydraContext = hydraContext;
        }

        public IEnumerable<Comment> GetByUserIdInDateRange(DateTime start, DateTime end, string userId)
        {
            var comments = _hydraContext.Comment
                .Include(usr => usr.Publisher)
                .Where(c => c.Date >= start &&
                       c.Date <= end &&
                       c.Publisher.ID == userId);
            return comments;
        }
    }
}
