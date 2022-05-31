using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Backend.Data;
using DataLayer.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Backend
{
    public class RestaurantBackend
    {
        private IDbContextFactory<EpicFoodRescueDBContext> factory;
        public RestaurantBackend(IDbContextFactory<EpicFoodRescueDBContext> factory)
        {
            this.factory = factory;
        }
        /*
         * 1 en metod för att få en lista över alla sålda matlådor för ett restaurang objekt
         * 2 en metod för att få se alla sålda matlådor senaste sju dagarna.
         * 3 en metod för att lägga till ett nytt matlådeobjekt för en restaurang
         * 4 en metod som visar total summa av försäljnung
         * 5 En metod som visar info (intressant så behövde jag inte databasen här //TODO bra / dåligt?
         */
        #region Soldboxes list // 1

        public List<string> SoldBoxes(Restaurant restaurant)
        {
            using var context = factory.CreateDbContext();
            var soldBoxes = new List<string>();

            var query = context.Foodboxes
                .Include(f=>f.Order)
                .Where(f => f.Order != null && f.Restaurant.Id == restaurant.Id);

            foreach (var foodbox in query)
            {
                soldBoxes.Add(
                    $"Orderdate: {foodbox.Order.OrderDate} Dishname: {foodbox.DishName} Price: {foodbox.PriceKr}");
            }

            return soldBoxes;

        }

        #endregion 

        #region Nr of sold boxes total // 2

        public int NrOfSoldBoxesLast7days(Restaurant restaurant)
        {
            using var context = factory.CreateDbContext();
            var datenow = DateTime.Now.AddDays(-7);


            var query = context.Foodboxes 
                .Where(f => f.Order != null 
                            && f.Restaurant.Id == restaurant.Id
                            && f.Order.OrderDate >= datenow);
            var querySQL = query.ToQueryString();

            return query.Count();

        }
        #endregion

        #region NewFoodBox // 3

        public Foodbox NewFoodbox(string name, decimal priceKr, Restaurant restaurant, string category = "")
        {
            using var context = factory.CreateDbContext();

            var newFoodbox = new Foodbox
            {
                DishName = name,
                PriceKr = priceKr,
                Category = category,
                ExpirationDate = DateTime.Today + TimeSpan.FromDays(3),
                Restaurant = restaurant
            };
            try
            {
                context.Update(restaurant);
                context.Add(newFoodbox);
                context.SaveChanges();
                return newFoodbox;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        #endregion

        #region MoneyEarned // 4

        public decimal MoneyEarned(Restaurant restaurant)
        {
            using var context = factory.CreateDbContext();
            var query = context.Foodboxes
                .Where(f => f.Order != null
                            && f.Restaurant.Id == restaurant.Id).Sum(f=>f.PriceKr);
            return query;
        }

        #endregion

        public string RestaurantInfo(Restaurant restaurant) // 5
        {
            return ($"Your info\n" +
                    $"Fullname: {restaurant.CompanyName}\n" +
                    $"Username: {restaurant.User.UserPrivateInfo.Username}\n" +
                    $"Registerd at: {restaurant.User.FirstRegisteredAt}\n" +
                    $"Password: {restaurant.User.UserPrivateInfo.Password}\n" +
                    $"Password last changed: {restaurant.User.PasswordLastChangedAt}");
        }
    }
}
