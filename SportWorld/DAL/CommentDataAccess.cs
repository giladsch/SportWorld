using Microsoft.EntityFrameworkCore;
using SportWorld.Data;
using SportWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportWorld.DAL
{
    public class CommentDataAccess
    {
        private readonly SportWorldContext _sportWorldContext;

        public CommentDataAccess(SportWorldContext sportWorldContext)
        {
            _sportWorldContext = sportWorldContext;
        }

        public Comment GetById(int commentId)
        {
            return _sportWorldContext.Comment.Include(usr => usr.Publisher).First(c => c.ID == commentId);
        }

        public List<Comment> GetAll()
        {
            return _sportWorldContext.Comment.Include(usr => usr.Publisher).OrderByDescending(p=>p.Rating).ToList();
        }

        public void Delete(int commentId)
        {
            var comment = _sportWorldContext.Comment.First(c => c.ID == commentId);

            _sportWorldContext.Comment.Remove(comment);
            _sportWorldContext.SaveChanges();
        }

        public void Update(Comment comment)
        {
            _sportWorldContext.Comment.Update(comment);
            _sportWorldContext.SaveChanges();
        }
    }
}
