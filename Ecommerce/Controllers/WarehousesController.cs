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
    [Authorize(Roles = "User")]
    public class WarehousesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        //[Authorize(Roles = "User")]
        // GET: Warehouses
        public ActionResult Index()
        {
            var user = db.Users
                .Where(u => u.UserName == User.Identity.Name.ToString())
                .FirstOrDefault();
            var warehouses = db.Warehouses.Include(w => w.City)
                .Include(w => w.Departament)
                .Where(w => w.CompanyID == user.CompanyID);
            return View(warehouses.ToList());
        }

        //[Authorize(Roles = "User")]
        // GET: Warehouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        //[Authorize(Roles = "Admin")]
        // GET: Warehouses/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name");
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");

            var user = db.Users
                .Where(u => u.UserName == User.Identity.Name.ToString())
                .FirstOrDefault();
            var warehouse = new Warehouse { CompanyID = user.CompanyID, };

            return View(warehouse);
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Warehouses.Add(warehouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", warehouse.CityID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", warehouse.DepartamentID);
            return View(warehouse);
        }

        //[Authorize(Roles = "Admin")]
        // GET: Warehouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", warehouse.CityID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", warehouse.DepartamentID);

            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", warehouse.CityID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", warehouse.DepartamentID);
            return View(warehouse);
        }

        //[Authorize(Roles = "Admin")]
        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse warehouse = db.Warehouses.Find(id);
            db.Warehouses.Remove(warehouse);
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
