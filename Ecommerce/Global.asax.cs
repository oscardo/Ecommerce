﻿using Ecommerce.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ecommerce
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //4.In the Global.asax, add the following line:
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Models.EcommerceContext, 
                Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            CheckRolesAndSuperUser();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");
            UsersHelper.CheckRole("Customer");
            UsersHelper.CheckSuperUser();
        }
    }
}
