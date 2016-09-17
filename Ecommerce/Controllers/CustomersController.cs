using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;
using Ecommerce.Classes;

namespace Ecommerce.Controllers
{
    public class CustomersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Customers
        public ActionResult Index()
        {
            var user = GetUser();
            var customers = db.Customers
                .Include(c => c.Department)
                .Include(c => c.City)
                .Where(c => c.CompanyID == user.CompanyID);
            return View(customers.ToList());
        }

        private User GetUser()
        {
            return db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            var user = GetUser();
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name");
            var customer = new Customer() { CompanyID = user.CompanyID, };
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid == false)
            {
                customer.DepartmentID = 2;
                db.Customers.Add(customer);
                db.SaveChanges();
                UsersHelper.CreateUserASP(customer.UserName, "Customer");
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", customer.CityID);
            //ViewBag.CompanyID = new SelectList(CombosHelper.GetCompanies(), "CompanyID", "Name", customer.CompanyID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", customer.CityID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                // TODO: Hace falta  desarrollo
                //var user = GetUser(); 
                //var db2 = new EcommerceContext();
                //var currentUser = db2.Users.Find(user.UserID);
                //if (currentUser.UserName != user.UserName)
                //{
                //    UsersHelper.UpdateUserName(currentUser.UserName, user.UserName);
                //}
                //db2.Dispose();
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", customer.CityID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
