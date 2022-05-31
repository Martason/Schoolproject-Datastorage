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
    public class AdminBackend
    {
        private IDbContextFactory<EpicFoodRescueDBContext> factory;
        public AdminBackend(IDbContextFactory<EpicFoodRescueDBContext> factory)
        {
            this.factory = factory;
        }

        public List<string> PrintAllCustomers() //2
        {
            using var context = factory.CreateDbContext();
            var returnList = new List<string>();

            var query = context.Customers.Include(c=>c.User);
            foreach (var info in query)
            {
                returnList.Add(
                    $"Username: {info.Username} Fullname: {info.FullName} Email: {info.Email} Registers At: {info.User.FirstRegisteredAt}");
            }

            return returnList;
        }
        public string DeleteUser(string username) // 3
        {
            using var context = factory.CreateDbContext();
            var query = context.UserPrivateInfos.Where(c => c.Username == username);

            if (query.Any())
            {
                var userPrivateInfo = query.First();
                try
                {
                    context.UserPrivateInfos.Remove(context.Customers.Find(userPrivateInfo.Id));
                    context.SaveChanges();
                    return $" {username} has been removed";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine();
                    Console.ReadKey();
                    return "Something went wrong! Call your IT department";
                }
            }

            return $"Username {username} could not be found";
        }

        public List<string> PrintAllResturants() //4
        {
            using var context = factory.CreateDbContext();
            var returnList = new List<string>();

            var query = context.Restaurants.Include(r=>r.User);
            foreach (var resturant in query)
            {
                returnList.Add(
                    $"Name: {resturant.CompanyName} Username: {resturant.Username} Id: {resturant.Id} Phonenumber: {resturant.PhoneNumber} Registered at: {resturant.User.FirstRegisteredAt}");
            }

            return returnList;
        }
        /// <summary>
        /// WARNING! Change username och password at once for user objekt.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="organisationsnummer"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="type"></param>
        /// <returns> Returns a User objekt with automated username and password OR null</returns>
        public User AddNewResturant(string name, string organisationsnummer, string phoneNumber, string type = "") // 5
        {
            using var context = factory.CreateDbContext();

            var user = new User{ FirstRegisteredAt = DateTime.Now, PasswordLastChangedAt = DateTime.Now};
            var resturant = new Restaurant 
                { 
                    CompanyName = name, 
                    Username = "Restaurant" + user.Id, 
                    Password = "Password" + user.Id,
                    Organisationsnummer = organisationsnummer,
                    PhoneNumber = phoneNumber, 
                    Type = type, 
                    User = user};

            context.Add(user);
            context.Add(resturant);
            context.SaveChanges();


            return user;
        }


    }
}
