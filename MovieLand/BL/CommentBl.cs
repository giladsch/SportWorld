using System;
using MovieLand.DAL;
using MovieLand.Data;

namespace MovieLand.BL
{
    public class CommentBl
    {
        private readonly ProductDataAccess _productDal;
        private readonly UserDataAccess _userDal;

        public CommentBl(MovieLandContext context)
        {
            _productDal = new ProductDataAccess(context);
            _userDal = new UserDataAccess(context);
        }

        public void Add(string productId, string text, string publisherId)
        {
            var comment = new Models.Comment
            {
                Text = text,
                Date = DateTime.Now,
                Publisher = _userDal.GetUser(publisherId)
            };

            _productDal.AddComment(int.Parse(productId), comment);
        }
    }
}
