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
        private readonly CommentDataAccess _commentDal;
        private readonly ProductDataAccess _productDal;
        private readonly UserDataAccess _userDal;

        public CommentBl(SportWorldContext sportWorldContext)
        {
            _commentDal = new CommentDataAccess(sportWorldContext);
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
            return _commentDal.GetById(commentId);
        }

        public List<Comment> GetAll()
        {
            return _commentDal.GetAll();
        }

        public void Delete(int commentId)
        {
            _commentDal.Delete(commentId);
        }

        public void Update(Comment comment)
        {
            _commentDal.Update(comment);
        }
    }
}
