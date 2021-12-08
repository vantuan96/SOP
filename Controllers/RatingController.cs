using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using SOP.Models;
using Ext.Net.MVC;
using Ext.Net;



namespace SOP.Controllers
{
    public class RatingController : Controller
    {
        private SOPEntities db = new SOPEntities();
        //
        // GET: /Rating/
        /// <summary>
        /// Reate
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            int UserId =Convert.ToInt32(Session["UserId"].ToString());
            var CurrentRatingUser = (from u in db.RatingResults
                                     join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                     where u.RatingResult__UserId == UserId && u.RatingResult_CreatedOn.Value.Month == DateTime.Now.Month && u.RatingResult_CreatedOn.Value.Year == DateTime.Now.Year
                                     group new { u, ug } by new { ug.Rating_Name }
                                         into grp
                                         select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            ViewBag.Month = DateTime.Now.Month.ToString();
            ViewBag.Year = DateTime.Now.Year.ToString();
            return View(CurrentRatingUser);
        }
        public ActionResult GetDataPrint(int Month, int Year)
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var CurrentRatingUser = (from u in db.RatingResults
                                     join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                     where u.RatingResult__UserId == UserId && (u.RatingResult_CreatedOn.Value.Month == Month || Month == 0) && (u.RatingResult_CreatedOn.Value.Year == Year || Year == 0)
                                     group new { u, ug } by new { ug.Rating_Name }
                                     into grp
                                     select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            ViewBag.User = db.Users.Find(UserId);
            ViewBag.Organizations = db.Organizations.Find(ViewBag.User.User_CurrentOrganizationId).Organization_Name;
            string M;
            string Y;
            if (Month == 0)
            {
                M = "";
            }
            else M = "THÁNG " + Month.ToString();
            if (Year == 0)
            {
                Y = "";
            }
            else Y = Year.ToString();

            ViewBag.Time = M + "/" + Y;
            return View("MonthPrint", CurrentRatingUser);
        }
        //-------------------------
        public ActionResult GetDataredirect(int Month, int Year)
        {
            return RedirectToAction("GetData", new { Month = Month, Year = Year });
        }
        public ActionResult GetData(int Month, int Year)
        {
            ViewBag.Month = Month.ToString();
            ViewBag.Year = Year.ToString();
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var CurrentRatingUser = (from u in db.RatingResults
                                     join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                     where u.RatingResult__UserId == UserId && (u.RatingResult_CreatedOn.Value.Month == Month || Month==0) && (u.RatingResult_CreatedOn.Value.Year == Year || Year==0)
                                     group new { u, ug } by new { ug.Rating_Name }
                                     into grp
                                     select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            return View("Index",CurrentRatingUser);
        }
        //-------------------------
        public ActionResult MCompareRating(int? Year )
        {
            ViewBag.Time = DateTime.Now.Year.ToString();
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            if (Year == null)
            {
                
                return View(db.SOP_GetMCompareRating_ByYear(DateTime.Now.Year, UserId));
            }
            else
            {
                return RedirectToAction("MCompareRatingRedirect", new { Year = Year, UserId = UserId });
            }
        }
        public ActionResult MCompareRatingRedirect(int Year, int UserId)
        {
            ViewBag.Time = Year.ToString();
            return View("MCompareRating", db.SOP_GetMCompareRating_ByYear(Year, UserId));
        }
        public ActionResult MComparePrint(int? Year)
        {
            ViewBag.Time = Year.ToString();
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            ViewBag.User = db.Users.Find(UserId);
            ViewBag.Organizations = db.Organizations.Find(ViewBag.User.User_CurrentOrganizationId).Organization_Name;
            return View(db.SOP_GetMCompareRating_ByYear(Year, UserId).ToList());
           
        }
        public ActionResult YCompareRating()
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            return View(db.SOP_GetMCompareRating(UserId));   
        }
        public ActionResult YComparePrint()
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            ViewBag.User = db.Users.Find(UserId);
            ViewBag.Organizations = db.Organizations.Find(ViewBag.User.User_CurrentOrganizationId).Organization_Name;
            return View(db.SOP_GetMCompareRating(UserId).ToList());
        }
	}
}
