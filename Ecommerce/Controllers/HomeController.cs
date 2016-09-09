using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {

        private EcommerceContext db = new EcommerceContext();
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).FirstOrDefault();
            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}