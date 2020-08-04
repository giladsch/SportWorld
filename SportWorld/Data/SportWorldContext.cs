using Microsoft.EntityFrameworkCore;
using SportWorld.Models;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SportWorld.Data
{
	public class SportWorldContext : DbContext
	{
		public SportWorldContext(DbContextOptions<SportWorldContext> options)
			: base(options)
		{
            if (Database.EnsureCreated())
            {
                InitializeProducts(this);
            }
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Comment> Comment { get; set; }

        private void InitializeProducts(SportWorldContext SportWorldContext)
        {
            var telAviv = new Store
            {
                Name = "Tel Aviv Store",
                ClosingHour = "22:00",
                OpeningHour = "12:00",
                Latitude = 32.074031,
                Lontitude = 34.792868
            };

            var jerusalem = new Store
            {
                Name = "Jerusalem Store",
                ClosingHour = "22:00",
                OpeningHour = "12:00",
                Latitude = 31.777820,
                Lontitude = 35.209204
            };

            var eilat = new Store
            {
                Name = "Eilat Store",
                ClosingHour = "22:00",
                OpeningHour = "12:00",
                Latitude = 29.556008,
                Lontitude = 34.961806
            };

            var meirav = new User
            {
                Gender = Gender.Female,
                Name = "Meirav Shenhar"
            };

            var gal = new User
            {
                Gender = Gender.Male,
                Name = "Gal Hen"
            };

            SportWorldContext.User.AddRange(meirav, gal);
            SportWorldContext.Store.AddRange(telAviv, jerusalem, eilat);

            var fileEntries = Directory.GetFiles("./products");
            foreach (string fileName in fileEntries)
            {
                var json = File.ReadAllText(fileName);
                var figures = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                
                var figuresNoId = figures.Select(x => new Product
                {
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    Category = x.Category,
                    Description = x.Description,
                    Comments = (x.Price < 83 && x.Price > 80) ?
                            new List<Comment>{ new Comment{
                                    Publisher = x.Price % 2 == 0 ? meirav : gal,
                                    Date = DateTime.Now.AddDays(-1),
                                    Text = $"{x.Name} is the greatest!"
                            }} : null
                });               

                Product.AddRange(figuresNoId);                
            }
            SportWorldContext.SaveChanges();
        }
    }
}