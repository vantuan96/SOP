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
    public class OrganizationController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // GET: /Organization/
        public ActionResult Index()
        {
            var organizations = db.Organizations.Include(o => o.OrganizationType);
            return View(organizations.ToList());
        }

        // GET: /Organization/Create
        public ActionResult Create()
        {
            List<Ext.Net.ListItem> listOrganizationTypes = new List<Ext.Net.ListItem>();
            foreach (var i in db.OrganizationTypes.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.OrganizationType_Name, i.OrganizationType_Id);
                listOrganizationTypes.Add(a);
            }
            ViewBag.OrganizationOrganizationType = listOrganizationTypes;

            List<Ext.Net.ListItem> listOrganization = new List<Ext.Net.ListItem>();
            foreach (var i in db.Organizations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Organization_Name, i.Organization_Id);
                listOrganization.Add(a);
            }
            ViewBag.Organization = listOrganization;

            List<Ext.Net.ListItem> listOrganizationFieldOperation = new List<Ext.Net.ListItem>();
            foreach (var i in db.FieldOperations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.FieldOperation_Name, i.FieldOperation_Name);
                listOrganizationFieldOperation.Add(a);
            }
            ViewBag.OrganizationFieldOperation = listOrganizationFieldOperation;
            return this.PartialExtView("Create");
        }

        // POST: /Organization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public ActionResult Save([Bind(Include = "Organization_Name,Organization_Superior,Organization_Information,Organization_Address,Organization_PhoneNumber,Organization_Fax,Organization_Email,Organization_Website,Organization_Active,Organization_CreatedOn,Organization_OrganizationTypeId,Organization_FieldOperation,Organization_DirectlyUnder,Organization_Specialized")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Organizations.Add(organization);
                db.SaveChanges();
                return this.Store(db.Organizations.Include(d => d.OrganizationType).ToList());
            }
            return View("Create");
        }

        // GET: /Organization/Edit/5
        public ActionResult Edit(int? Organization_Id)
        {

            List<Ext.Net.ListItem> listOrganizationTypes = new List<Ext.Net.ListItem>();
            foreach (var i in db.OrganizationTypes.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.OrganizationType_Name, i.OrganizationType_Id);
                listOrganizationTypes.Add(a);
            }
            ViewBag.OrganizationOrganizationType = listOrganizationTypes;


            List<Ext.Net.ListItem> listOrganization = new List<Ext.Net.ListItem>();
            foreach (var i in db.Organizations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Organization_Name, i.Organization_Id);
                listOrganization.Add(a);
            }
            ViewBag.Organization = listOrganization;

            List<Ext.Net.ListItem> listOrganizationFieldOperation = new List<Ext.Net.ListItem>();
            foreach (var i in db.FieldOperations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.FieldOperation_Name, i.FieldOperation_Name);
                listOrganizationFieldOperation.Add(a);
            }
            ViewBag.OrganizationFieldOperation = listOrganizationFieldOperation;
            if (Organization_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organizations.Find(Organization_Id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return this.PartialExtView("Edit", organization);
        }

        // POST: /Organization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult EditSave([Bind(Include = "Organization_Id,Organization_Name,Organization_Superior,Organization_Information,Organization_Address,Organization_PhoneNumber,Organization_Fax,Organization_Email,Organization_Website,Organization_Active,Organization_CreatedOn,Organization_OrganizationTypeId,Organization_FieldOperation,Organization_DirectlyUnder,Organization_Specialized")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return this.Store(db.Organizations.Include(d => d.OrganizationType).ToList());
            }
            return View("Edit");
        }

        // POST: /Organization/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int Organization_Id)
        {
            Organization organization = db.Organizations.Find(Organization_Id);
            if (organization.Departments.Count() > 0)
            {
                X.MessageBox.Alert("Thông Báo!", "Cơ quan không thể xóa được").Show();
                return this.Direct();
            }
            else
            {
                db.Organizations.Remove(organization);
                db.SaveChanges();
                return this.Store(db.Organizations.ToList());
            }
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
