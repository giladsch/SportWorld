using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Models
{
	public class Product
	{
        public int ID { get; set; }

		public string Name { get; set; }

		public double Price { get; set; }

        public string ImageUrl { get; set; }

        public Category Category { get; set; }

        public string Description { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}