using System;
using SportWorld.DAL;
using SportWorld.Data;

namespace SportWorld.BL
{
    public class CommentBl
    {
        private readonly ProductDataAccess _productDal;
        private readonly UserDataAccess _userDal;

        public CommentBl(SportWorldContext context)
        {
            _productDal = new ProductDataAccess(context);
            _userDal = new UserDataAccess(context);
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
    }
}
