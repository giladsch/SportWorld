using SportWorld.Data;
using SportWorld.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportWorld.DAL
{
    public class ProductDataAccess
    {
        private readonly SportWorldContext _sportWorldContext;

        public ProductDataAccess(SportWorldContext sportWorldContext)
        {
            _sportWorldContext = sportWorldContext;
        }

        public List<Product> GetAllProducts() => _sportWorldContext
                .Product
                .Include(p => p.Comments)
                .ThenInclude(c => c.Publisher)
                .ToList();

        public Product GetProductById(int productId)
        {
            return _sportWorldContext.Product
                                .Include(p => p.Comments)
                                .ThenInclude(c => c.Publisher)
                                .FirstOrDefault(x => x.ID == productId);
        }

        public IEnumerable<Product> GetProductsByCategory(Category category)
        {
            return _sportWorldContext.Product
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.Publisher)
                    .Where(p => p.Category == category)
                    .AsEnumerable();
        }

        public void SaveProducts(IEnumerable<Product> products)
        {  
            _sportWorldContext.Product.AddRange(products);
            _sportWorldContext.SaveChanges();
        }

        public void AddComment(int productId, Comment comment)
        {
            this.GetProductById(productId).Comments.Add(comment);
            _sportWorldContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _sportWorldContext.Product.Update(product);
            _sportWorldContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            var productToDelete = _sportWorldContext
                .Product
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.ID == product.ID);

            if(productToDelete == null)
            {
                throw new 
                    Exception(string.Format("could not find product with id {0}", product.ID));
            }

            _sportWorldContext.Comment.RemoveRange(productToDelete.Comments);
            _sportWorldContext.Product.Remove(productToDelete);
            _sportWorldContext.SaveChanges();
        }
    }
}
