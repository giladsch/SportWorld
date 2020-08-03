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
        private readonly SportWorldContext _SportWorldContext;

        public ProductDataAccess(SportWorldContext SportWorldContext)
        {
            _SportWorldContext = SportWorldContext;
        }

        public List<Product> GetAllProducts()
        {
            return _SportWorldContext
                .Product
                .Include(p => p.Comments)
                .ThenInclude(c => c.Publisher)
                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _SportWorldContext.Product
                                .Include(p => p.Comments)
                                .ThenInclude(c => c.Publisher)
                                .FirstOrDefault(x => x.ID == productId);
        }

        public IEnumerable<Product> GetProductsByCategory(Category category)
        {
            return _SportWorldContext.Product
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.Publisher)
                    .Where(p => p.Category == category)
                    .AsEnumerable();
        }

        public void SaveProducts(IEnumerable<Product> products)
        {  
            _SportWorldContext.Product.AddRange(products);
            _SportWorldContext.SaveChanges();
        }

        public void AddComment(int productId, Comment comment)
        {
            this.GetProductById(productId).Comments.Add(comment);
            _SportWorldContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _SportWorldContext.Product.Update(product);
            _SportWorldContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            var productToDelete = _SportWorldContext
                .Product
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.ID == product.ID);

            if(productToDelete == null)
            {
                throw new 
                    Exception(string.Format("could not find product with id {0}", product.ID));
            }

            _SportWorldContext.Comment.RemoveRange(productToDelete.Comments);
            _SportWorldContext.Product.Remove(productToDelete);
            _SportWorldContext.SaveChanges();
        }
    }
}
