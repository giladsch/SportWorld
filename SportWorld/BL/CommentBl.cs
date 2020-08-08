using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportWorld.DAL;
using SportWorld.Data;
using SportWorld.Models;

namespace SportWorld.BL
{
    public class CommentBl
    {
        private readonly SportWorldContext _sportWorldContext;
        private readonly ProductDataAccess _productDal;
        private readonly UserDataAccess _userDal;

        public CommentBl(SportWorldContext sportWorldContext)
        {
            _sportWorldContext = sportWorldContext;
            _productDal = new ProductDataAccess(sportWorldContext);
            _userDal = new UserDataAccess(sportWorldContext);
        }

        public void Add(string productId, string text, string publisherId, double rating)
        {
            double newRating = rating < 0 ? 0 : rating > 10 ? 10 : rating;
            var comment = new Models.Comment
            {
                Text = text,
                Date = DateTime.Now,
                Publisher = _userDal.GetUser(publisherId),
                Rating = newRating
            };

            _productDal.AddComment(int.Parse(productId), comment);
        }

        public Comment GetById(int commentId)
        {
            return _sportWorldContext.Comment.Include(usr => usr.Publisher).First(c => c.ID == commentId);
        }

        public List<Comment> GetAll()
        {
            return _sportWorldContext.Comment.Include(usr => usr.Publisher).ToList();
        }

        public void Delete(int commentId)
        {
            var allProduct = _productDal.GetAllProducts();

            var comment = _sportWorldContext.Comment.First(c => c.ID == commentId);

            _sportWorldContext.Comment.Remove(comment);
            _sportWorldContext.SaveChanges();
        }

        public void UpdateProduct(Comment comment)
        {
            _sportWorldContext.Comment.Update(comment);
            _sportWorldContext.SaveChanges();
        }
    }
}
