using MovieLand.Data;
using MovieLand.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLand.DAL
{
    public class ProductDataAccess
    {
        private readonly MovieLandContext _movieLandContext;

        public ProductDataAccess(MovieLandContext movieLandContext)
        {
            _movieLandContext = movieLandContext;
        }

        public List<Product> GetAllProducts()
        {
            return _movieLandContext
                .Product
                .Include(p => p.Comments)
                .ThenInclude(c => c.Publisher)
                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _movieLandContext.Product
                                .Include(p => p.Comments)
                                .ThenInclude(c => c.Publisher)
                                .FirstOrDefault(x => x.ID == productId);
        }

        public IEnumerable<Product> GetProductsByCategory(Category category)
        {
            return _movieLandContext.Product
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.Publisher)
                    .Where(p => p.Category == category)
                    .AsEnumerable();
        }

        public void SaveProducts(IEnumerable<Product> products)
        {  
            _movieLandContext.Product.AddRange(products);
            _movieLandContext.SaveChanges();
        }

        public void AddComment(int productId, Comment comment)
        {
            this.GetProductById(productId).Comments.Add(comment);
            _movieLandContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _movieLandContext.Product.Update(product);
            _movieLandContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            var productToDelete = _movieLandContext
                .Product
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.ID == product.ID);

            if(productToDelete == null)
            {
                throw new 
                    Exception(string.Format("could not find product with id {0}", product.ID));
            }

            _movieLandContext.Comment.RemoveRange(productToDelete.Comments);
            _movieLandContext.Product.Remove(productToDelete);
            _movieLandContext.SaveChanges();
        }
    }
}
