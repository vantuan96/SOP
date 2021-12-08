using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP.Models;

namespace SOP.Controllers
{
    public class ProvinceController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // GET: /Province/
        public ActionResult Index()
        {
            return View(db.Provinces.Include(x => x.Districts).ToList());
            //return View(db.Provinces.ToList());
        }

        // GET: /Province/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = db.Provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // GET: /Province/Create
        public ActionResult Create()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Create" };
        }

        public ActionResult Save(Province province)
        {
            if (ModelState.IsValid)
            {
                //province.Province_Active = true;
                db.Provinces.Add(province);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Create");

        }
        // POST: /Province/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Province_Id,Province_Name,Province_Active")] Province province)
        {
            if (ModelState.IsValid)
            {
                db.Provinces.Add(province);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(province);
        }

        // GET: /Province/Edit/5
        public ActionResult Edit(int? Province_Id, string Province_Name, bool? Province_Active)
        {
            if (Province_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = db.Provinces.Find(Province_Id);
            province.Province_Name = Province_Name;
            province.Province_Active = Province_Active;
            if (province == null)
            {
                return HttpNotFound();
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = province };
        }

        // POST: /Province/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult EditSave([Bind(Include = "Province_Id,Province_Name,Province_Active")] Province province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(province).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }

        // POST: /Province/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int Province_Id)
        {
            Province province = db.Provinces.Find(Province_Id);
            db.Provinces.Remove(province);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DistrictCreate(int Province_Id)
        {
            //ViewBag.Province_Id = Province_Id;
            District district = new District();
            district.District_ProvinceId = Province_Id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DistrictCreate", Model = district };
        }

        public ActionResult DistrictSave(District district)
        {
            if (ModelState.IsValid)
            {
                district.District_Active = true;
                db.Districts.Add(district);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("DistrictCreate");
        }

        // GET: /Province/Edit/5
        public ActionResult DistrictEdit(int? District_Id, int? Province_Id, string District_Name, bool? District_Active)
        {
            if (District_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(District_Id);
            district.District_Name = District_Name;
            district.District_ProvinceId = Province_Id;
            district.District_Active = District_Active;
            if (district == null)
            {
                return HttpNotFound();
            }
            Province province = new Province();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DistrictEdit", Model = district };
        }

        [HttpPost]
        public ActionResult DistrictEditSave([Bind(Include = "District_Id,District_ProvinceId,District_Name,District_Active")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }

        // POST: /Province/DistrictDelete/5
        [HttpPost, ActionName("DistrictDelete")]
        public ActionResult DistrictDeleteConfirmed(int District_Id)
        {
            District district = db.Districts.Find(District_Id);
            db.Districts.Remove(district);
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
