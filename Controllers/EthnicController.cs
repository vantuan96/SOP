using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP.Models;
using Ext.Net;
using Ext.Net.MVC;

namespace SOP.Controllers
{
    public class EthnicController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // GET: /Ethnic/
        public ActionResult Index()
        {
            return View(db.Ethnics.ToList());
        }

        //public ActionResult Find_Name()
        //{
        //    TextField text = X.GetCmp<TextField>("EthnicFind");
        //    return this.Store( db.Ethnic_Find(text.Value.ToString()).ToList());
        //}

        // GET: /Ethnic/Create
        public ActionResult Create()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Create" };
        }

        // POST: /Ethnic/Create
        [HttpPost]
        public ActionResult Save(Ethnic ethnic)
        {
            if (ModelState.IsValid)
            {
                var lstEthnic = db.Ethnics.Where(x => x.Ethnic_Name.Equals(ethnic.Ethnic_Name));
                int intEthnic = lstEthnic.Count();

                if (ethnic.Ethnic_Name == null)
                {
                    X.Msg.Alert("Thông Báo", "Dân tộc không được để trống").Show();
                    return this.Direct();
                }
                else if (intEthnic > 0)
                {
                    X.Msg.Alert("Thông Báo", "Dân tộc không được trùng").Show();
                    return this.Direct();
                }
                else
                {
                    db.Ethnics.Add(ethnic);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Create");
        }

        // GET: /Ethnic/Edit/5
        public ActionResult Edit(int? Ethnic_Id, string Ethnic_Name, bool Ethnic_Active)
        {
            if (Ethnic_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ethnic ethnic = db.Ethnics.Find(Ethnic_Id);
            ethnic.Ethnic_Name = Ethnic_Name;
            ethnic.Ethnic_Active = Ethnic_Active;
            if (ethnic == null)
            {
                return HttpNotFound();
            }
            return this.PartialExtView("Edit", ethnic);
        }


        [HttpPost]
        public ActionResult EditSave([Bind(Include = "Ethnic_Id,Ethnic_Name,Ethnic_Active")] Ethnic ethnic)
        {
            if (ModelState.IsValid)
            {
                var ethnicList = db.Ethnics.Where(x => x.Ethnic_Name.Equals(ethnic.Ethnic_Name));
                ethnicList = ethnicList.Where(x => x.Ethnic_Id != ethnic.Ethnic_Id);
                int intTitle = ethnicList.Count();
                if (ethnic.Ethnic_Name == null)
                {
                    X.Msg.Alert("Thông Báo", "Dân tộc không được để trống").Show();
                    return this.Direct();
                }
                else if (intTitle > 0)
                {
                    X.Msg.Alert("Thông Báo", "Dân tộc không được trùng").Show();
                    return this.Direct();
                }
                else
                {
                    db.Entry(ethnic).State = EntityState.Modified;
                    db.SaveChanges();
                    return this.Store(db.Ethnics.ToList());
                }
            }
            return View("Edit");
        }

        // POST: /Ethnic/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int Ethnic_Id)
        {
            Ethnic ethnic = db.Ethnics.Find(Ethnic_Id);
            db.Ethnics.Remove(ethnic);
            db.SaveChanges();
            return this.Store(db.Ethnics.ToList());
            //return RedirectToAction("Index");
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
