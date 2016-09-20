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
    public class CompaniesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies
                .Include(c => c.City)
                .Include(c => c.Departament);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name");
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    db.Companies.Add(company);
                    db.SaveChanges();

                    if (company.LogoFile != null)
                    {
                        
                        var folder = "~/Content/logos";
                        var file = string.Format("{0}.jpg", company.CompanyID);
                        var response = FilesHelper.UploadPhoto(company.LogoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}.jgp", folder, file);
                            company.Logo = pic;
                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if(ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "No puede estar duplicado o error en datos");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message.ToString());
                    }
                }
            }

            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", company.CityID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", company.DepartamentID);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", company.CityID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", company.DepartamentID);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                try
                {
                    var folder = "~/Content/logos";
                    var file = string.Format("{0}.jpg", company.CompanyID);
                    if (company.LogoFile != null)
                    {
                        var response = FilesHelper.UploadPhoto(company.LogoFile, folder, file);
                        var pic = string.Format("{0}/{1}.jpg", folder, file);
                        company.Logo = pic;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                        if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "No puede estar duplicado o error en datos");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.Message.ToString());
                        }
                    }
            }
            ViewBag.CityID = new SelectList(CombosHelper.GetCities(), "CityID", "Name", company.CityID);
            ViewBag.DepartamentID = new SelectList(CombosHelper.GetDepartament(), "DepartamentID", "Name", company.DepartamentID);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            try
            {
                db.Companies.Remove(company);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "No puede estar duplicado o error en datos");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message.ToString());
                    }
            }
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
