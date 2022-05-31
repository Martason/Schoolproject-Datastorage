using System;
using System.Collections.Generic;
using DataLayer.Backend.Data;
using DataLayer.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Services
{
    public class Database
    {
        private IDbContextFactory<EpicFoodRescueDBContext> factory;
        public Database(IDbContextFactory<EpicFoodRescueDBContext> factory)
        {
            this.factory = factory;
        }
        public void SeedTestData()
        {
            using var context = factory.CreateDbContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var users = new List<User>
            {
                new() {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now},
                new() {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now},
                new() {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now},
                new() {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now},
                new() {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now},
                new() {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now},
                new() {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now}
            };
            context.AddRange(users);

            var admins = new List<Admin>
            {
                new() { FullName = "Märta Hjalmarson", Username = "admin", Password = "Qwerty", Email = "märta@hotmail.som", Employed = DateTime.Today, User = users[0]},
            };
            context.AddRange(admins);

            var resturants = new List<Restaurant>
            {
                new() {CompanyName = "Restaurant A", Username = "restaurant", Password = "Qwerty", Type = "Italian", PhoneNumber = "031 555 555", Organisationsnummer = "12456", User = users[1]},
                new() {CompanyName = "Restaurant B", Username = "ResB", Password = "Qwerty", Type = "Italian", PhoneNumber = "031 555 555", Organisationsnummer = "12454", User = users[2]},
                new() {CompanyName = "Restaurant C", Username = "ResC", Password = "Qwerty", Type = "Italian", PhoneNumber = "031 555 555", Organisationsnummer = "12344", User = users[3]},
            };
            context.AddRange(resturants);

            var customers = new List<Customer>
            {
                new() {FullName = "Pia", Username = "customer", Password = "Qwerty", Email = "pia@hotmail.com", User = users[4]},
                new() {FullName = "Kia", Username = "KimRocks", Password = "Qwerty", Email = "pff@hotmail.com", User = users[5]},
                new() {FullName = "Pim", Username = "PimRocks", Password = "Qwerty", Email = "pih@hotmail.com", User = users[6]},
            };
            context.AddRange(customers);

            var orders = new List<Order>
            {
                new Order {OrderDate = new DateTime(2021, 11, 09), user = users[4]},
                new Order {OrderDate = new DateTime(2021, 11, 09), user = users[4]},
                new Order {OrderDate = new DateTime(2021, 11, 17), user = users[4]},
            };
            context.AddRange(orders);

            var foodboxes = new List<Foodbox>
            {
                new() {DishName = "Maträtt1", PriceKr = 50, Category = "VEGO", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[0], Order = orders[2]},
                new() {DishName = "Maträtt2", PriceKr = 40, Category = "MEAT", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[0], Order = orders[2]},
                new() {DishName = "Maträtt3", PriceKr = 30, Category = "FISH", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[0], Order = orders[2]},
                new() {DishName = "Maträtt4", PriceKr = 50, Category = "VEGO", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[1]},
                new() {DishName = "Maträtt5", PriceKr = 40, Category = "MEAT", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[1]},
                new() {DishName = "Maträtt6", PriceKr = 30, Category = "FISH", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[1]},
                new() {DishName = "Maträtt7", PriceKr = 50, Category = "VEGO", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[2]},
                new() {DishName = "Maträtt8", PriceKr = 40, Category = "MEAT", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[2], Order = orders[2]},
                new() {DishName = "Maträtt9", PriceKr = 30, Category = "FISH", ExpirationDate = DateTime.Now + TimeSpan.FromDays(4), Restaurant = resturants[2]},

            };
            context.AddRange(foodboxes);
            context.SaveChanges();

        }
        public void SeedLiveUseData()
        {
            using var context = factory.CreateDbContext();
            var users = new User[]
            {
                new () {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now},
            };
            var admins = new List<Admin>
            {
                new() { FullName = "Märta Hjalmarson", Username = "admin", Password = "Qwerty", Email = "märta@hotmail.som", Employed = DateTime.Today, User = users[0]},
            };
            context.AddRange(users);
            context.AddRange(admins);

            context.SaveChanges();
        }
        public void Recreate()
        {
            using var ctx = factory.CreateDbContext();

            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
    }
}
