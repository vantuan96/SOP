using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP.Models;
using Ext.Net.MVC;
using Ext.Net;

namespace SOP.Controllers
{
    public class DepartmentController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // GET: /Department/
        public ActionResult Index()
        {
            var departments = db.Departments.Include(d => d.Organization);
            return View(departments.ToList());
        }

        // GET: /Department/Create
        public ActionResult Create()
        {
            List<Ext.Net.ListItem> listOrganizations = new List<Ext.Net.ListItem>();
            foreach (var i in db.Organizations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Organization_Name, i.Organization_Id);
                listOrganizations.Add(a);
            }
            ViewBag.Department_OrganizationId = listOrganizations;
            //ViewBag.Department_OrganizationId = (from x in db.Organizations select x).ToList();
            return this.PartialExtView();
        }

        // GET: /Department/Edit/5
        public ActionResult Edit(int? Department_Id)
        {
            if (Department_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(Department_Id);
            if (department == null)
            {
                return HttpNotFound();
            }
            List<Ext.Net.ListItem> listOrganizations = new List<Ext.Net.ListItem>();
            foreach (var i in db.Organizations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Organization_Name, i.Organization_Id);
                listOrganizations.Add(a);
            }
            ViewBag.Department_OrganizationId = listOrganizations;
            //ViewBag.Department_OrganizationId = new SelectList(db.Organizations, "Organization_Id", "Organization_Name", department.Department_OrganizationId);
            return this.PartialExtView("Edit", department);
        }

        // POST: /Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ActionName("Save")]
        public ActionResult Save([Bind(Include = "Department_Id,Department_Name,Department_OrganizationId,Department_Information,Department_PhoneNumber,Department_Fax,Department_Email,Department_Active,Department_CreatedOn")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.Departments.Add(department);
                db.SaveChanges();
                return this.Store(db.Departments.Include(o => o.Organization).ToList());
            }

            return View("Create");
        }

        public ActionResult EditSave([Bind(Include = "Department_Id,Department_Name,Department_OrganizationId,Department_Information,Department_PhoneNumber,Department_Fax,Department_Email,Department_Active,Department_CreatedOn")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return this.Store(db.Departments.Include(o => o.Organization).ToList());
            }
            return View("Edit");
        }


        // GET: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int? Department_Id)
        {
            Department department = db.Departments.Find(Department_Id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return this.Store(db.Departments.Include(o => o.Organization).ToList());
        }

        // POST: /Department/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Department department = db.Departments.Find(id);
        //    db.Departments.Remove(department);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
