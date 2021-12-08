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
using Ext.Net.MVC;

namespace SOP.Controllers
{
    public class OrganizationtypeController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // GET: /Organizationtype/
        public ActionResult Index()
        {
            return View(db.OrganizationTypes.ToList());
        }

        // GET: /Organizationtype/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationType organizationtype = db.OrganizationTypes.Find(id);
            if (organizationtype == null)
            {
                return HttpNotFound();
            }
            return View(organizationtype);
        }

        // GET: /Organizationtype/Create
        public ActionResult Create()
        {
            return this.PartialExtView();
        }

        // POST: /Organizationtype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganizationType_Id,OrganizationType_Name,OrganizationType_Active")] OrganizationType organizationtype)
        {
            if (ModelState.IsValid)
            {
                db.OrganizationTypes.Add(organizationtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organizationtype);
        }
        public ActionResult Save(OrganizationType organizationType)
        {
            if (ModelState.IsValid)
            {
                var organizationTypeList = db.OrganizationTypes.Where(x => x.OrganizationType_Name.Equals(organizationType.OrganizationType_Name));
                int intOrganizationType = organizationTypeList.Count();
                if (organizationType.OrganizationType_Name == null)
                {
                    X.Msg.Alert("Thông Báo", "Loại tổ chức không được để trống").Show();
                    return this.Direct();
                }
                else if (intOrganizationType > 0)
                {
                    X.Msg.Alert("Thông Báo", "Loại tổ chức không được trùng").Show();
                    return this.Direct();
                }
                else
                {
                    db.OrganizationTypes.Add(organizationType);
                    db.SaveChanges();
                    return this.Store(db.OrganizationTypes.ToList());
                }
            }
            else return this.Direct();
        }

        // GET: /Organizationtype/Edit/5
        public ActionResult Edit(int? OrganizationType_Id, string OrganizationType_Name, bool? OrganizationType_Active)
        {
            if (OrganizationType_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationType organizationType = db.OrganizationTypes.Find(OrganizationType_Id);
            organizationType.OrganizationType_Name = OrganizationType_Name;
            organizationType.OrganizationType_Active = OrganizationType_Active;
            if (organizationType == null)
            {
                return HttpNotFound();
            }
            return this.PartialExtView("Edit", organizationType);

            //return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = noteBook };
        }

        // POST: /Organizationtype/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult EditSave([Bind(Include = "OrganizationType_Id,OrganizationType_Name,OrganizationType_Active")] OrganizationType organizationtype)
        {
            if (ModelState.IsValid)
            {
                var organizationtypeList = db.OrganizationTypes.Where(x => x.OrganizationType_Name.Equals(organizationtype.OrganizationType_Name));
                organizationtypeList = organizationtypeList.Where(x => x.OrganizationType_Id != organizationtype.OrganizationType_Id);
                int intOrganizationtype = organizationtypeList.Count();
                if (organizationtype.OrganizationType_Name == null)
                {
                    X.Msg.Alert("Thông Báo", "Loại tổ chức không được để trống").Show();
                    return this.Direct();
                }
                else if (intOrganizationtype > 0)
                {
                    X.Msg.Alert("Thông Báo", "Loại tổ chức không được trùng").Show();
                    return this.Direct();
                }
                else
                {
                    db.Entry(organizationtype).State = EntityState.Modified;
                    db.SaveChanges();
                    return this.Store(db.OrganizationTypes.ToList());
                }
            }
            return View(organizationtype);
        }

        // POST: /Organizationtype/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int OrganizationType_Id)
        {
            OrganizationType organizationtype = db.OrganizationTypes.Find(OrganizationType_Id);
            db.OrganizationTypes.Remove(organizationtype);
            db.SaveChanges();
            return this.Store(db.OrganizationTypes.ToList());
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
