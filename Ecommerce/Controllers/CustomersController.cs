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

            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerID equals cc.CustomerID
                       join co in db.Companies on cc.CompanyID equals co.CompanyID
                       where co.CompanyID == user.CompanyID
                       select new { cu }).ToList();

            var customers = new List<Customer>();
            foreach (var item in qry)
            {
                customers.Add(item.cu);
            }

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
            //var customer = new Customer() { CompanyID = user.CompanyID, };
            return View(); //customer
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
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        customer.DepartmentID = 2;
                        customer.CityID = 2;
                        db.Customers.Add(customer);
                        var response = DBHelper.SaveChanges(db);
                        if (!response.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, response.Message);
                            transaction.Rollback();
                            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");
                            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", customer.CityID);
                            //ViewBag.CompanyID = new SelectList(CombosHelper.GetCompanies(), "CompanyID", "Name", customer.CompanyID);
                            return View(customer);
                        }
                        UsersHelper.CreateUserASP(customer.UserName, "Customer");

                        var user = GetUser();
                        var companyCustomer = new CompanyCustomer
                        {
                            CompanyID = user.CompanyID,
                            CustomerID = customer.CustomerID,
                        };
                        db.CompanyCustomers.Add(companyCustomer);
                        db.SaveChanges();

                        transaction.Commit();
                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, ex.Message.ToString());
                    }
                }
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
            var user = GetUser();
                var companyCustomer = db.CompanyCustomers.Where(cc => cc.CompanyID == user.CompanyID &&
                cc.CustomerID == customer.CustomerID).FirstOrDefault();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.CompanyCustomers.Remove(companyCustomer);
                    db.Customers.Remove(customer);
                    var response = DBHelper.SaveChanges(db);
                    if (response.Succeeded)
                    {
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    else {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, response.Message);
                    }
                }//end try
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, ex.Message);
                } 
            }
            return View(customer);
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
