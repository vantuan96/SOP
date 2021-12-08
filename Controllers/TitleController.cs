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
    public class TitleController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // Action Load Index
        // GET: /Title/
        public ActionResult Index()
        {
            return View(db.Titles.ToList());
        }

        // Action Search Title By Name
        //public ActionResult Find_Name()
        //{
        //    TextField text = X.GetCmp<TextField>("TitleFind");
        //    return this.Store(db.Ethnic_Find(text.Value.ToString()).ToList());
        //}
        // Action Load Detail Title

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Title title = db.Titles.Find(id);
            if (title == null)
            {
                return HttpNotFound();
            }
            return View(title);
        }

        // Action Load Form Create Title
        // GET: /Title/Create
        public ActionResult Create()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Create" };
        }

        // Action Create Title
        [HttpPost]
        public ActionResult Save(Title title)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lstTitle = db.Titles.Where(x => x.Title_Name.Equals(title.Title_Name));
                    int intTitle = lstTitle.Count();
                    if (title.Title_Name == null)
                    {
                        X.Msg.Alert("Thông Báo", "Chức Danh không được để trống").Show();
                        return this.Direct();
                    }
                    else if (intTitle > 0)
                    {
                        X.Msg.Alert("Thông Báo", "Chức Danh không được trùng").Show();
                        return this.Direct();
                    }
                    else
                    {
                        db.Titles.Add(title);
                        db.SaveChanges();
                        return this.Store(db.Titles.ToList());
                    }
                }
                else return this.Direct();

            }
            catch (Exception)
            {

                throw;
            }
        }

        // Action Load Form Edit Title
        public ActionResult Edit(int? Title_Id, string Title_Name, bool? Title_Active)
        {
            try
            {
                if (Title_Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Title title = db.Titles.Find(Title_Id);
                title.Title_Name = Title_Name;
                title.Title_Active = Title_Active;
                if (title == null)
                {
                    return HttpNotFound();
                }
                return this.PartialExtView("Edit", title);

            }
            catch (Exception)
            {

                throw;
            }
        }

        // Action Edit Title
        [HttpPost]
        public ActionResult EditSave([Bind(Include = "Title_Id,Title_Name,Title_Active")] Title title)
        {
            if (ModelState.IsValid)
            {
                var titleList = db.Titles.Where(x => x.Title_Name.Equals(title.Title_Name));
                titleList = titleList.Where(x => x.Title_Id != title.Title_Id);
                int intTitle = titleList.Count();
                if (title.Title_Name == null)
                {
                    X.Msg.Alert("Thông Báo", "Chức danh không được để trống").Show();
                    return this.Direct();
                }
                if (intTitle > 0)
                {
                    X.Msg.Alert("Thông Báo", "Chức Danh không được trùng").Show();
                    return this.Direct();
                }
                else
                {
                    db.Entry(title).State = EntityState.Modified;
                    db.SaveChanges();
                    return this.Store(db.Titles.ToList());
                }
            }
            else return this.Direct();
        }

        // Action Delete Title
        public ActionResult Delete(int? id)
        {
            try
            {
                Title title = db.Titles.Find(id);
                db.Titles.Remove(title);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Title title = db.Titles.Find(id);
        //    db.Titles.Remove(title);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

    }
}
