using System;
using System.Linq;
using DataLayer.Backend.Data;
using DataLayer.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Services
{
    public class Login
    {
        private IDbContextFactory<EpicFoodRescueDBContext> factory;
        public Login(IDbContextFactory<EpicFoodRescueDBContext> factory)
        {
            this.factory = factory;
        }

        public User User(string username, string password)
        {
            using var context =factory.CreateDbContext();
            bool exist = false;
            bool passwordMatch = false;

            var query = context.Users
                .Include(u => u.UserPrivateInfo)
                .Where(u => u.UserPrivateInfo.Username == username);

            var user = query.FirstOrDefault();

            if (user != null)
            {
                exist = true;
                {
                    if (user.UserPrivateInfo.Password == password)
                    {
                        passwordMatch = true;
                    }
                }
            }

            if (exist && passwordMatch)
            {
                return user;
            }

            return null;
        }

        public User ChangeUsername(User userInput, string username, string password)
        {
            using var context = factory.CreateDbContext();

            //query user
            var query = context.Users
                .Where(u => u.Id == userInput.Id)
                .Include(u => u.UserPrivateInfo);

            //find user
            var userToChange = query.First();

            //password match
            if (userToChange.UserPrivateInfo.Password != password) throw new Exception("Invalid password");

            //change user
            userToChange.UserPrivateInfo.Username = username;
        
            context.SaveChanges();

            return userToChange;
        }

        public User ChangePassword(User userInput, string oldPassword, string newPassword, string newPasswordRepeat)
        {
            using var context = factory.CreateDbContext();

            //new password match
            if (newPassword != newPasswordRepeat) throw new Exception("passwords don't match!");

            //find user
            var query = context.Users.Where(u=>u.Id == userInput.Id)
                .Include(u=>u.UserPrivateInfo);
            var userNewPassword = query.First();
            //old password ok
            if (userNewPassword.UserPrivateInfo.Password != oldPassword) throw new Exception("Invalid !"); ;

            //change password
            userNewPassword.UserPrivateInfo.Password = newPassword;

            context.SaveChanges();
            return userNewPassword;


        }

    }
}
