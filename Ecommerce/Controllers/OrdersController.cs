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
    [Authorize(Roles ="User")]
    public class OrdersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Orders
        public ActionResult Index()
        {
            var user = GetUser();

            var orders = db.Orders
                .Where(o => o.CompanyID == user.CompanyID)
                .Include(o => o.Customer)
                .Include(o => o.State);
            return View(orders.ToList());
        }

        private User GetUser()
        {
            return db.Users
                    .Where(u => u.UserName == User.Identity.Name.ToString())
                    .FirstOrDefault();
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = GetUser();

            ViewBag.CustomerID = new SelectList(CombosHelper.GetCustomers(user.CompanyID), "CustomerID", "FullName");
            var view = new NewOrderView
            {
                Date = DateTime.Now,
                Details = db.OrderDetailTmps.Where(ODT => ODT.UserName == User.Identity.Name).ToList()
            };
            return View(view);
        }

        public ActionResult DeleteProduct(int? id)
        {
            var user = GetUser();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderdetailtmps = db.OrderDetailTmps.Where(
                odt => odt.UserName == user.UserName 
                && odt.ProductID == id).FirstOrDefault();
            if (orderdetailtmps == null)
            {
                return HttpNotFound();
            }
            db.OrderDetailTmps.Remove(orderdetailtmps);
            db.SaveChanges();
            return RedirectToAction("Create");
        }


        [HttpPost]
        public ActionResult AddProduct(AddProductView view)
        {
            var user = GetUser();
            if (ModelState.IsValid)
            {
                var orderDetailTmp = db.OrderDetailTmps.Where(
                    odt => odt.UserName == user.UserName
                    && odt.ProductID == view.ProductID).FirstOrDefault();
                if(orderDetailTmp == null)
                { 
                var product = db.Products.Find(view.ProductID);

                orderDetailTmp = new OrderDetailTmp
                {
                    Description = product.Description,
                    Price = product.Price,
                    ProductID = product.ProductID,
                    Quantity =  view.Quantity,
                    TaxRate = product.Tax.Rate,                   
                    UserName = User.Identity.Name,
                };
                db.OrderDetailTmps.Add(orderDetailTmp);
                }
                else
                {
                    orderDetailTmp.Quantity += view.Quantity;
                    db.Entry(orderDetailTmp).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.ProductID = new SelectList(CombosHelper.GetProducts(user.CompanyID), "ProductID", "Description");
            return PartialView(view);
        }



        public ActionResult AddProduct()
        {
            var user = GetUser();
            ViewBag.ProductID = new SelectList(CombosHelper.GetProducts(user.CompanyID), "ProductID", "Description");
            return PartialView();
        }


        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewOrderView view)
        {
            if (ModelState.IsValid)
            {
                var response = MovementsHelper.NewOrder(view, User.Identity.Name);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                }
                
            }

            var user = GetUser();
            ViewBag.CustomerID = new SelectList(CombosHelper.GetCustomers(user.CompanyID), "CustomerID", "FullName");
            view.Details = db.OrderDetailTmps.Where(ODT => ODT.UserName == User.Identity.Name).ToList();
            return View(view);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = GetUser();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerID = new SelectList(CombosHelper.GetCustomers(user.CompanyID), "CustomerID", "UserName", order.CustomerID);
            ViewBag.StateId = new SelectList(CombosHelper.GetStates(), "StateId", "Description", order.StateId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,StateId,Date,Remarks")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", order.StateId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
