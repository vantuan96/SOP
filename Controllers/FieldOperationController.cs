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
    public class FieldOperationController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // GET: /FieldOperation/
        public ActionResult Index()
        {
            return View(db.FieldOperations.ToList());
        }

        // GET: /FieldOperation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldOperation fieldoperation = db.FieldOperations.Find(id);
            if (fieldoperation == null)
            {
                return HttpNotFound();
            }
            return View(fieldoperation);
        }

        // GET: /FieldOperation/Create
        public ActionResult Create()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Create" };
        }

        [HttpPost]
        public ActionResult Save([Bind(Include = "FieldOperation_Name,FieldOperation_Name,FieldOperation_Active")] FieldOperation fieldOperation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (fieldOperation.FieldOperation_Active == null)
                    {
                        fieldOperation.FieldOperation_Active = false;
                    }
                    var lstFieldOperation = db.FieldOperations.Where(x => x.FieldOperation_Name.Equals(fieldOperation.FieldOperation_Name));
                    int intFieldOperation = lstFieldOperation.Count();
                    if (fieldOperation.FieldOperation_Name == null)
                    {
                        X.Msg.Alert("Thông Báo", "Lĩnh Vực không được để trống").Show();
                        return this.Direct();
                    }
                    else if (intFieldOperation > 0)
                    {
                        X.Msg.Alert("Thông Báo", "Lĩnh vực không được trùng").Show();
                        return this.Direct();
                    }
                    else
                    {
                        db.FieldOperations.Add(fieldOperation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                return View(fieldOperation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /FieldOperation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="FieldOperation_Id,FieldOperation_Name,FieldOperation_Active")] FieldOperation fieldoperation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.FieldOperations.Add(fieldoperation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(fieldoperation);
        //}

        // GET: /FieldOperation/Edit/5
        public ActionResult Edit(int? FieldOperation_Id, string FieldOperation_Name, bool FieldOperation_Active)
        {
            try
            {
                if (FieldOperation_Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FieldOperation item = db.FieldOperations.Find(FieldOperation_Id);
                item.FieldOperation_Name = FieldOperation_Name;
                item.FieldOperation_Active = FieldOperation_Active;
                if (item == null)
                {
                    return HttpNotFound();
                }
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = item };
            }
            catch (Exception)
            {

                throw;
            };
        }

        // POST: /FieldOperation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult EditSave([Bind(Include = "FieldOperation_Id,FieldOperation_Name,FieldOperation_Active")] FieldOperation fieldoperation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (fieldoperation.FieldOperation_Active == null)
                    {
                        fieldoperation.FieldOperation_Active = false;
                    }
                    fieldoperation.FieldOperation_Active = true;
                    var fieldOperationList = db.FieldOperations.Where(x => x.FieldOperation_Name.Equals(fieldoperation.FieldOperation_Name));
                    fieldOperationList = fieldOperationList.Where(x => x.FieldOperation_Id != fieldoperation.FieldOperation_Id);
                    int intFieldOperation = fieldOperationList.Count();
                    if (fieldoperation.FieldOperation_Name == null)
                    {
                        X.Msg.Alert("Thông Báo", "Lĩnh vực không được để trống").Show();
                        return this.Direct();
                    }
                    else if (intFieldOperation > 0)
                    {
                        X.Msg.Alert("Thông Báo", "Lĩnh vực không được trùng").Show();
                        return this.Direct();
                    }
                    else
                    {
                        db.Entry(fieldoperation).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View("Edit");
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /FieldOperation/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                FieldOperation fieldoperation = db.FieldOperations.Find(id);
                db.FieldOperations.Remove(fieldoperation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
