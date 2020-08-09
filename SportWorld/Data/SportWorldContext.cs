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

            var Gilad = new User
            {
                Gender = Gender.Male,
                UserName = "giladsch",
                Email = "12345@gmail.com",
                Password = "admin",
                IsAdmin = true,
                IsDeleted = false
            };

            var Adi = new User
            {
                Gender = Gender.Female,
                UserName = "adihahamov",
                Email = "1234@gmail.com",
                Password = "admin",
                IsAdmin = false,
                IsDeleted = false
            };

            var Noy = new User
            {
                Gender = Gender.Female,
                UserName = "noy98",
                Email = "123@gmail.com",
                Password = "admin",
                IsAdmin = false,
                IsDeleted = false
            };

            SportWorldContext.User.AddRange(Gilad, Adi, Noy);
            SportWorldContext.Store.AddRange(telAviv, jerusalem, eilat);

            var fileEntries = Directory.GetFiles("./products");
            foreach (string fileName in fileEntries)
            {
                var json = File.ReadAllText(fileName);
                var productsFromJson = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

                var productsFromCategory = productsFromJson.Select(x => new Product
                {
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    Category = x.Category,
                    Description = x.Description,
                    Comments = (x.Price < 150 && x.Price > 0) ? commentToAdd(true, x.Price, x.Name, Gilad, Adi, Noy) : commentToAdd()
                });

                Product.AddRange(productsFromCategory);
            }
            SportWorldContext.SaveChanges();
        }

        private List<Comment> commentToAdd(bool addComments = false, double price = 0, string name = "", User Gilad = null, User Adi = null, User Noy = null)
        {
            if (!addComments) return null;
            Random a = new Random();
            var num = a.Next(1, 4);
            var comment =  new Comment
            {
                Publisher = num % 3 == 0 ? Gilad : num % 3 == num ? Adi : Noy,
                Date = DateTime.Now.AddDays(-1),
                Text = $"{name} is the best sport product ever!",
                Rating = UptoTwoDecimalPoints(GetRandomNumber())
            };

            return new List<Comment>()
            {
                comment
            };
        }

        private double GetRandomNumber()
        {
            Random random = new Random();
            return random.NextDouble() * 10;
        }

        private double UptoTwoDecimalPoints(double num)
        {
            var totalCost = Convert.ToDouble(String.Format("{0:0.00}", num));
            return totalCost;
        }
    }
}