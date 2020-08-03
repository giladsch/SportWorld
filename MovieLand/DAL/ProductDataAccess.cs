using Hydra.Data;
using Hydra.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.DAL
{
    public class ProductDataAccess
    {
        private readonly HydraContext _hydraContext;

        public ProductDataAccess(HydraContext hydraContext)
        {
            _hydraContext = hydraContext;
        }

        public List<Product> GetAllProducts()
        {
            return _hydraContext
                .Product
                .Include(p => p.Comments)
                .ThenInclude(c => c.Publisher)
                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _hydraContext.Product
                                .Include(p => p.Comments)
                                .ThenInclude(c => c.Publisher)
                                .FirstOrDefault(x => x.ID == productId);
        }

        public IEnumerable<Product> GetProductsByCategory(Category category)
        {
            return _hydraContext.Product
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.Publisher)
                    .Where(p => p.Category == category)
                    .AsEnumerable();
        }

        public void SaveProducts(IEnumerable<Product> products)
        {  
            _hydraContext.Product.AddRange(products);
            _hydraContext.SaveChanges();
        }

        public void AddComment(int productId, Comment comment)
        {
            this.GetProductById(productId).Comments.Add(comment);
            _hydraContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _hydraContext.Product.Update(product);
            _hydraContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            var productToDelete = _hydraContext
                .Product
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.ID == product.ID);

            if(productToDelete == null)
            {
                throw new 
                    Exception(string.Format("could not find product with id {0}", product.ID));
            }

            _hydraContext.Comment.RemoveRange(productToDelete.Comments);
            _hydraContext.Product.Remove(productToDelete);
            _hydraContext.SaveChanges();
        }
    }
}
