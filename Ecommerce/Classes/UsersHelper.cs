﻿using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Ecommerce.Classes
{
    public class UsersHelper : IDisposable
    {
//        USER HELPER
//1.	Add the user helper class:
            private static ApplicationDbContext userContext = new ApplicationDbContext();
            private static EcommerceContext db = new EcommerceContext();

        public static bool DeleteUser(string UserName, string RolName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(UserName);
            if (userASP == null)
            {
                return false;
            }
            var Response = userManager.RemoveFromRole(userASP.Id, RolName);
            return Response.Succeeded;
        }

        public static bool UpdateUserName(string CurrentUserName, string NewUserName) {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(CurrentUserName);
            if (userASP == null)
            {
                return false;
            }
            userASP.UserName = NewUserName;
            userASP.Email = NewUserName;
            var Response = userManager.Update(userASP);
            return Response.Succeeded;
        }

        public static bool UpdateUserNameCustomer(string CurrentUserName, string NewUserName)
        {
            var CustomerManager = new Customer();
            var userASP = CustomerManager.UserName;
            if (userASP == null)
            {
                return false;
            }
            //var Response = CustomerManager.Update(userASP);
            return true; //Response.Succeeded;
        }

        public static void CheckRole(string roleName)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

                // Check to see if Role Exists, if not create it
                if (!roleManager.RoleExists(roleName))
                {
                    roleManager.Create(new IdentityRole(roleName));
                }
            }

            public static void CheckSuperUser()
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var email = WebConfigurationManager.AppSettings["AdminUser"];
                var password = WebConfigurationManager.AppSettings["AdminPassWord"];
                var userASP = userManager.FindByName(email);
                if (userASP == null)
                {
                    CreateUserASP(email, "Admin", password);
                    return;
                }

                userManager.AddToRole(userASP.Id, "Admin");
            }

            public static void CreateUserASP(string email, string roleName)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var userASP = userManager.FindByEmail(email);
                if(userASP == null)
                { 
                        userASP = new ApplicationUser
                        {
                        Email = email,
                        UserName = email,
                        };
                    userManager.Create(userASP, email);
                 }//fin de UserASP == null
            
            userManager.AddToRole(userASP.Id, roleName);
            }//fin de static void

            public static void CreateUserASP(string email, string roleName, string password)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

                var userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userASP, password);
                userManager.AddToRole(userASP.Id, roleName);
            }

            public static async Task PasswordRecovery(string email)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var userASP = userManager.FindByEmail(email);
                if (userASP == null)
                {
                    return;
                }

                var user = db.Users.Where(tp => tp.UserName == email).FirstOrDefault();
                if (user == null)
                {
                    return;
                }

                var random = new Random();
                var newPassword = string.Format("{0}{1}{2:04}*",
                    user.FirstName.Trim().ToUpper().Substring(0, 1),
                    user.LastName.Trim().ToLower(),
                    random.Next(10000));

                userManager.RemovePassword(userASP.Id);
                userManager.AddPassword(userASP.Id, newPassword);

                var subject = "Taxes Password Recovery";
                var body = string.Format(@"
                <h1>Taxes Password Recovery</h1>
                <p>Yor new password is: <strong>{0}</strong></p>
                <p>Please change it for one, that you remember easyly",
                    newPassword);

                await MailHelper.SendMail(email, subject, body);
            }

            public void Dispose()
            {
                userContext.Dispose();
                db.Dispose();
            }

    }
}