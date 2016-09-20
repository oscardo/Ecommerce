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
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.City).Include(u => u.Company).Include(u => u.Departament);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name");
            ViewBag.CompanyID = new SelectList(CombosHelper.GetCompanies(), "CompanyID", "Name");
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    UsersHelper.CreateUserASP(user.UserName, "User");

                    if (user.PhotoFile != null)
                    {

                        var folder = "~/Content/users";
                        var file = string.Format("{0}_{1}.jpg", user.FullName, user.UserID);
                        var response = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                        if (response)
                        {
                            user.Photo = string.Format("{0}/{1}", folder, file);
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.InnerException.Message.ToString());
                }
            }

            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(CombosHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", user.DepartamentID);

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(CombosHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", user.DepartamentID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (user.PhotoFile != null)
                    {

                        var folder = "~/Content/users";
                        var file = string.Format("{0}_{1}.jpg", user.FullName, user.UserID);
                        var response = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                        if (response)
                        {
                            user.Photo = string.Format("{0}/{1}", folder, file);
                        }
                    }
                    var db2 = new EcommerceContext();
                    var currentUser = db2.Users.Find(user.UserID);
                    if (currentUser.UserName != user.UserName)
                    {
                        UsersHelper.UpdateUserName(currentUser.UserName, user.UserName);
                    }
                    db2.Dispose();
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.InnerException.Message.ToString());
                }
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(CombosHelper.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", user.DepartamentID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            UsersHelper.DeleteUser(user.UserName);
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
