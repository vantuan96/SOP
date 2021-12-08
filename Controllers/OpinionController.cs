using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOP.Models;
using Ext.Net.MVC;
using Ext.Net;
using System.Data;
using System.Data.Entity;

namespace SOP.Controllers
{
    public class OpinionController : Controller
    {
        private SOPEntities db = new SOPEntities();
        // GET: /Opinion/
        public ActionResult Index()
        {  
            return View();
        }
        public ActionResult Index2(int UserId)
        {
            ViewBag.Username = db.Users.Find(UserId).User_UserName;
            return View();
        }
        public ActionResult Opinion_Send(FormCollection values)
        {
            string Email = values["Email"];
            User user = db.Users.Where(u=>u.User_UserName==Email).First();
            Opinion opinio = new Opinion();
            opinio.Opinion_Date = DateTime.Now;
            opinio.Opinion_From = Convert.ToInt32(Session["UserId"].ToString());
            opinio.Opinion_To = user.User_Id;
            opinio.Opinion_Title = values["Opinion_Title"];
            opinio.Opinion_Content = values["Opinion_Content"];
            db.Opinions.Add(opinio);
            db.SaveChanges();
            return RedirectToAction("Opinion_Sent");
        }
        public JsonResult CheckUser(string UserName)
        {

            UserName.Trim();
            bool check = false;
            foreach (var item in db.Users.ToList())
            {
                if (UserName == item.User_UserName) check = true;
            };

                if (check == true)
                {
                    return Json(new { valid = true }, JsonRequestBehavior.AllowGet); 
                }
                else
                    return Json(new { valid = false, message = "Email không tồn tại" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Opinion_Received()
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var opinion = (from u in db.Opinions
                           join ug in db.Users on u.Opinion_From equals ug.User_Id
                           where u.Opinion_To == UserId
                           select new { u.Opinion_To, u.Opinion_Title, u.Opinion_Date, u.Opinion_Id, u.Opinion_From, u.Opinion_Content, ug.User_FullName, ug.User_UserName }).ToList();
            return View(opinion);
        }
        public ActionResult Opinion_Sent()
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var opinion = (from u in db.Opinions
                           join ug in db.Users on u.Opinion_To equals ug.User_Id
                           where u.Opinion_From == UserId
                           select new { u.Opinion_To, u.Opinion_Title, u.Opinion_Date, u.Opinion_Id, u.Opinion_From, u.Opinion_Content, ug.User_FullName, ug.User_UserName }).ToList();
            return View(opinion);
        }
	}
}