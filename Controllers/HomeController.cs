using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using Ext.Net;
using Ext.Net.MVC;
using System.IO;
using System.Globalization;
using SOP.Models;
using SOP.FunctionLibrary;
namespace SOP.Controllers
{
    public class HomeController : BaseController
    {
        private SOPEntities db = new SOPEntities();
        // GET: /Home/
        public ActionResult Index()
        {
            bool ADMIN = true;
            bool CBSNV = true;
            bool CBMC = true;
            bool CBCS = true;
            Session["UserId"] = 10;
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            foreach (var obj in db.UserGroups.Where(user => user.UserGroup_UserId == UserId && user.UserGroup_Active==true).ToList())
            {
                if (obj.UserGroup_GroupId == Constants.ADMIN) ADMIN = false;
                if (obj.UserGroup_GroupId == Constants.CBSNV) CBSNV = false;
                if (obj.UserGroup_GroupId == Constants.CBMC) CBMC = false;
                if (obj.UserGroup_GroupId == Constants.CBCS) CBCS = false;
            }
            ViewBag.ADMIN = ADMIN;
            ViewBag.CBSNV = CBSNV;
            ViewBag.CBMC = CBMC;
            ViewBag.CBCS = CBCS;
            ViewBag.UserName = db.Users.Find(UserId).User_FullName;
            return View();


        }
        public ActionResult DashBoard()
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var obj = db.UserGroups.Where(user => user.UserGroup_UserId == UserId && user.UserGroup_Active == true).First();
            if (obj.UserGroup_GroupId == Constants.CBSNV || obj.UserGroup_GroupId == Constants.ADMIN) return View(db.SOP_GetStatusUserCMMC().ToList());
            else return View(db.SOP_GetStatusUserCMMCByOrganization(db.Users.Where(user => user.User_Id == UserId && user.User_Active==true).First().User_CurrentOrganizationId).ToList());
        }

    }
}