using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Backend.Data;
using DataLayer.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Backend
{
    public class CustomerBackend
    {
        // 1 En metod för att få ut en lista på alla oköpta matlådor i alla restauranger, sorterade på pris lägst först.Parameter: typ av matlåda
        // 2 Metod för att registrera en ny costumer
        // 3 Metod för att köpa en matlåda
        // 4 Metod för att se custoalla mina tidiagre köp
        // 5 En metod för att se customerinfo

        private IDbContextFactory<EpicFoodRescueDBContext> factory;
        public CustomerBackend(IDbContextFactory<EpicFoodRescueDBContext> factory)
        {
            this.factory = factory;
        }

        #region AvalableFoodboxes // 1
        public List<string> AvailableFoodboxes(string category) 

        {
            using var context = factory.CreateDbContext();

            var foodboxes = new List<string>();

            var query = context.Foodboxes.Where(f => f.Category == category && f.Order == null)
                .Select(f => new
                {
                    Dishname = f.DishName, Price = f.PriceKr, Resturant = f.Restaurant.CompanyName, FoodboxID = f.Id
                })
                .OrderBy(f => f.Price)
                .ToList();

            if (!query.Any()) return null;
            foreach (var dish in query)
            {
                foodboxes.Add(
                    $"Dish: {dish.Dishname}\t Price(Kr): {dish.Price}\t Restaurant: {dish.Resturant}\t Foodbox ID: {dish.FoodboxID} ");
            }
            return foodboxes;
        }
        
        public List<string> AvailableFoodboxes()
        {
            using var context = factory.CreateDbContext();
            var foodboxes = new List<string>();

            var query = (context.Foodboxes.Where(f => f.Order == null)
                .Select(f => new
                {
                    Category = f.Category,
                    Dishname = f.DishName,
                    Price = f.PriceKr,
                    Resturant = f.Restaurant.CompanyName,
                    FoodboxID = f.Id
                })
                .OrderBy(f => f.Price));

            if (!query.Any()) return null;
            foreach (var foodbox in query)
            {
                foodboxes.Add(
                    $"Dish: {foodbox.Dishname}.\t Price(Kr): {foodbox.Price}\t Restaurant: {foodbox.Resturant}\t Category: {foodbox.Category}\t ID: {foodbox.FoodboxID} ");
            }

            return foodboxes;
        }

        #endregion

        #region RegisterNewCustomer // 2
        public Customer RegisterNewCustomer(string fullName, string Username, string password, string passwordRepeat, string email,
            string phonenumer = "")
        {
            using var context = factory.CreateDbContext();

            if (password != passwordRepeat) throw new Exception("passwords don't match!");

            var newUser = new User {FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now};
            var newCustomer = new Customer
            {
                FullName = fullName,
                Username = Username,
                Password = password,
                Email = email,
                User = newUser
            };

            context.Add(newUser);
            context.Add(newCustomer);
            context.SaveChanges();
            return newCustomer;
        }

        #endregion

        #region BuyFoodbox // 3

        public Foodbox BuyFoodbox(int foodboxId, Customer customer)
        {
            using var context = factory.CreateDbContext();
            Foodbox soldFoodox;
            Order newOrder;

            var query = context.Foodboxes.Where(f => f.Id == foodboxId)
                .Include(f=>f.Order);

            soldFoodox = query.FirstOrDefault();

            if (soldFoodox == null) return null;

            newOrder = new Order
            {
                OrderDate = DateTime.Now,
                user = customer.User
            };

            soldFoodox.Order = newOrder;
            context.Update(customer.User);
            context.Add(newOrder);

            context.SaveChanges();
            return soldFoodox;
        }

        #endregion

        #region SeeCustomerPurchaes // 4

        public List<string> PrintCustomerPurchases(Customer customer)
        {
            var foodboxes = new List<string>();

            using var context = factory.CreateDbContext();

            var query = context.Foodboxes
                .Where(f => f.Order.user.Id == customer.User.Id)
                .Include(f => f.Order)
                .Include(f => f.Restaurant).Select(f => new
            {
                f.DishName,
                Resturant = f.Restaurant.CompanyName,
                f.PriceKr,
                f.Order.OrderDate,

            });
            if (!query.Any()) return null;
            foreach (var foodbox in query)
            {
                foodboxes.Add(
                    $"Dishname: {foodbox.DishName} Restaurant: {foodbox.Resturant} Price: {foodbox.PriceKr} Order date: {foodbox.OrderDate} ");
            }
            
            return foodboxes;

        }

        #endregion

        #region Customerinfo // 5

        public string CustomerInfo(Customer customer)
        {
            return ($"Your info\n" +
                    $"Fullname: {customer.FullName}\n" +
                    $"Username: {customer.User.UserPrivateInfo.Username}\n" +
                    $"Registerd at: {customer.User.FirstRegisteredAt}\n" +
                    $"Password: {customer.User.UserPrivateInfo.Password}\n" +
                    $"Password last changed: {customer.User.PasswordLastChangedAt}");
        }

        #endregion
    }
}
