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
    public class ProductsController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Products
        public ActionResult Index()
        {
            var user = db.Users
                .Where(u => u.UserName == User.Identity.Name.ToString())
                .FirstOrDefault();

            var products = db.Products
                .Include(p => p.Category)
                .Include(p => p.Tax)
                .Where(p => p.CompanyID == user.CompanyID);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            User user = Users();

            ViewBag.CategoryID = new SelectList(CombosHelper.GetCategories(user.CompanyID), "CategoryID", "Description");
            ViewBag.TaxID = new SelectList(CombosHelper.GetTaxes(user.CompanyID), "TaxID", "Description");
            var product = new Product { CompanyID = user.CompanyID, };
            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();

                if (product.ImageFile != null)
                {

                    var folder = "~/Content/products";
                    var file = string.Format("{0}.jpg", product.ProductID);
                    var response = FilesHelper.UploadPhoto(product.ImageFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        product.Image = pic;
                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

             
                return RedirectToAction("Index");
            }

            User user = Users();

            ViewBag.CategoryID = new SelectList(CombosHelper.GetCategories(user.CompanyID), "CategoryID", "Description", product.CategoryID);
            ViewBag.TaxID = new SelectList(db.Taxes, "TaxID", "Description", product.TaxID);
            return View(product);
        }

        private User Users()
        {
            return db.Users
                .Where(u => u.UserName == User.Identity.Name.ToString())
                .FirstOrDefault();
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            User user = Users();

            ViewBag.CategoryID = new SelectList(CombosHelper.GetCategories(user.CompanyID), "CategoryID", "Description", product.CategoryID);
            ViewBag.TaxID = new SelectList(db.Taxes, "TaxID", "Description", product.TaxID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {

                    var folder = "~/Content/products";
                    var file = string.Format("{0}.jpg", product.ProductID);
                    var response = FilesHelper.UploadPhoto(product.ImageFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        product.Image = pic;
                    }
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            User user = Users();

            ViewBag.CategoryID = new SelectList(CombosHelper.GetCategories(user.CompanyID), "CategoryID", "Description", product.CategoryID);
            ViewBag.TaxID = new SelectList(db.Taxes, "TaxID", "Description", product.TaxID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
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
