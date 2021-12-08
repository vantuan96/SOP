using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using SOP.Models;
using SOP.FunctionLibrary;
using Ext.Net.MVC;
using Ext.Net;
using SOP.FunctionLibrary;
namespace SOP.Controllers
{
    public class OrganizationRatingController : Controller
    {
        private SOPEntities db = new SOPEntities();
        // GET: /OrganizationRating/
        public ActionResult Index(int? CBCS)
        {
            
            if (CBCS == null)
            {
                return View(db.Organizations.Where(u => u.Organization_Active == true).ToList());
            }
            else
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                User user = db.Users.Find(UserId);
                return View(db.Organizations.Where(u => u.Organization_Active == true && u.Organization_Id==user.User_CurrentOrganizationId).ToList());
            }
        }
        public ActionResult MOrganizationRatingdirect(int? Month, int? Year, int OrganizationId)
        {
            if (Month == null)
            {
                return RedirectToAction("MNowOrganizationRating", new { OrganizationId = OrganizationId });
            }
            else
            {
                return RedirectToAction("MOrganizationRating", new { Month = Month, Year = Year, OrganizationId = OrganizationId });
            }
        }
        public ActionResult MNowOrganizationRating(int OrganizationId)
        {
            var CurrentRatingOrganization = (from u in db.RatingResults
                                     join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                     join o in db.Users on u.RatingResult__UserId equals o.User_Id
                                     where o.User_CurrentOrganizationId == OrganizationId && u.RatingResult_CreatedOn.Value.Month == DateTime.Now.Month && u.RatingResult_CreatedOn.Value.Year == DateTime.Now.Year
                                     group new { u, ug } by new { ug.Rating_Name }
                                         into grp
                                         select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            ViewBag.Month = DateTime.Now.Month.ToString();
            ViewBag.Year = DateTime.Now.Year.ToString();
            ViewBag.OrganizationId = OrganizationId.ToString();
            return View("MOrganizationRating", CurrentRatingOrganization);
        }
        public ActionResult MOrganizationRating(int Month, int Year, int OrganizationId)
        {
            ViewBag.Month = Month.ToString();
            ViewBag.Year = Year.ToString();
            ViewBag.OrganizationId = OrganizationId.ToString();
            var CurrentRatingOrganization = (from u in db.RatingResults
                                     join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                     join o in db.Users on u.RatingResult__UserId equals o.User_Id
                                     where o.User_CurrentOrganizationId == OrganizationId && (u.RatingResult_CreatedOn.Value.Month == Month || Month == 0) && (u.RatingResult_CreatedOn.Value.Year == Year || Year == 0)
                                     group new { u, ug } by new { ug.Rating_Name }
                                         into grp
                                         select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            return View("MOrganizationRating", CurrentRatingOrganization);
        }
        public ActionResult MOrganizationRatingPrint(int Month, int Year, int OrganizationId)
        {
            var CurrentRatingOrganization = (from u in db.RatingResults
                                             join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                             join o in db.Users on u.RatingResult__UserId equals o.User_Id
                                             where o.User_CurrentOrganizationId == OrganizationId && (u.RatingResult_CreatedOn.Value.Month == Month || Month == 0) && (u.RatingResult_CreatedOn.Value.Year == Year || Year == 0)
                                             group new { u, ug } by new { ug.Rating_Name }
                                             into grp
                                                 select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            ViewBag.Organizations = db.Organizations.Find(OrganizationId).Organization_Name;
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
            return View("MOrganizationRatingPrint", CurrentRatingOrganization.ToList());
        }
        //-----------------------------------------------------------------------------
        public ActionResult YOrganizationRatingdirect(int? Year, int OrganizationId)
        {
            if (Year == null)
            {
                return RedirectToAction("YNowOrganizationlRating", new { OrganizationId = OrganizationId });
            }
            else
            {
                return RedirectToAction("YOrganizationRating", new { Year = Year, OrganizationId = OrganizationId });
            }
        }
        public ActionResult YNowOrganizationlRating(int OrganizationId)
        {
            ViewBag.Time = DateTime.Now.Year.ToString();
            ViewBag.OrganizationId = OrganizationId.ToString();
            return View("YOrganizationRating", db.SOP_GetYOrganizationRating_ByYear(DateTime.Now.Year, OrganizationId));
        }
        public ActionResult YOrganizationRating(int Year, int OrganizationId)
        {
            ViewBag.OrganizationId = OrganizationId.ToString();
            ViewBag.Time = Year.ToString();
            return View("YOrganizationRating", db.SOP_GetYOrganizationRating_ByYear(Year, OrganizationId));
        }
        public ActionResult YOrganizationPrint(int Year, int OrganizationId)
        {
            ViewBag.Time = Year.ToString();
            ViewBag.Organizations = db.Organizations.Find(OrganizationId).Organization_Name;
            return View(db.SOP_GetYOrganizationRating_ByYear(Year, OrganizationId).ToList());

        }
        //--------------------------------------------------------------------------------------------------------------
        public ActionResult YCompareOrganizationRating(int OrganizationId)
        {
            ViewBag.OrganizationId = OrganizationId.ToString();
            return View(db.SOP_GetYCompareOrganizationRating(OrganizationId));
        }
        public ActionResult YCompareOrganizationPrint(int OrganizationId)
        {
            ViewBag.Organizations = db.Organizations.Find(OrganizationId).Organization_Name;
            return View(db.SOP_GetYCompareOrganizationRating(OrganizationId).ToList());
        }
        //--------------------------------------------------------------------------------------------------------------
        public ActionResult DetailOrganizationRating(DateTime? FromTime, DateTime? ToTime,int OrganizationId)
        {
            ViewBag.OrganizationId = OrganizationId.ToString();
            if(FromTime==null)
            {

            FromTime  = new DateTime(DateTime.Now.Year, 1, 1);
            ViewBag.FromTime = FromTime;
            ToTime = DateTime.Now;
            ViewBag.ToTime = ToTime;
            return View(db.SOP_GetDetailOrganizationRating(FromTime, ToTime, OrganizationId).ToList());
            }
            else
            {
                return RedirectToAction("DetailOrganizationRatingRedirect", new { OrganizationId = OrganizationId, FromTime = FromTime, ToTime = ToTime });
            }

        }
        public ActionResult DetailOrganizationRatingRedirect(int OrganizationId, DateTime FromTime, DateTime ToTime)
        {
            ToTime= ToTime.AddHours(23);
            ViewBag.OrganizationId = OrganizationId.ToString();
            ViewBag.FromTime = FromTime;
            ViewBag.ToTime = ToTime;
            return View("DetailOrganizationRating", db.SOP_GetDetailOrganizationRating(FromTime, ToTime, OrganizationId).ToList());

        }
        public ActionResult DetailOrganizationRatingPrint(int OrganizationId, DateTime FromTime, DateTime ToTime)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.Organization = db.Organizations.Find(OrganizationId).Organization_Name;
            ViewBag.FromTime = FromTime.ToString("dd/M/yyyy");
            ViewBag.ToTime = ToTime.ToString("dd/M/yyyy");
            return View(db.SOP_GetDetailOrganizationRating(FromTime, ToTime, OrganizationId).ToList());

        }

        //-----------------------------------------------------
        public ActionResult CompareTwoOrganizationRating(DateTime? FromTime, DateTime? ToTime, int? OrganizationId1, int? OrganizationId2)
        {
            if (FromTime == null || ToTime == null || OrganizationId1==null || OrganizationId2==null)
            {
                ViewBag.OrganizationId1 = "";
                ViewBag.OrganizationId2 = "";
                ViewBag.OrganizationName1 = "Chưa chọn cơ quan";
                ViewBag.OrganizationName2 = "Chưa chọn cơ quan";
                ViewBag.OrganizationrList = Function.OrganizationrList();
                FromTime = new DateTime(DateTime.Now.Year, 1, 1);
                ViewBag.FromTime = FromTime;
                ToTime = DateTime.Now;
                ViewBag.ToTime = ToTime;
                return View();
            }
            else
            {
                return RedirectToAction("CompareTwoOrganizationRatingRedirect", new { FromTime = FromTime, ToTime = ToTime, OrganizationId1 = OrganizationId1, OrganizationId2 = OrganizationId2 });
            }
        }
        public ActionResult CompareTwoOrganizationRatingRedirect(DateTime FromTime, DateTime ToTime, int OrganizationId1, int OrganizationId2)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.OrganizationrList = Function.OrganizationrList();
            ViewBag.FromTime = FromTime;
            ViewBag.ToTime = ToTime;
            ViewBag.OrganizationId1 = OrganizationId1.ToString();
            ViewBag.OrganizationId2 = OrganizationId2.ToString();
            ViewBag.OrganizationName1 = db.Organizations.Find(OrganizationId1).Organization_Name;
            ViewBag.OrganizationName2 = db.Organizations.Find(OrganizationId2).Organization_Name;
            return View("CompareTwoOrganizationRating",db.SOP_GetCompareTwoOrganizationRating(FromTime, ToTime, OrganizationId1, OrganizationId2).ToList());
        }
        //---------------------------------------------------------------
        public ActionResult ListOrganizationRating(DateTime? FromTime, DateTime? ToTime)
        {
            if (FromTime == null)
            {
                FromTime = new DateTime(DateTime.Now.Year, 1, 1);
                ViewBag.FromTime = FromTime;
                ToTime = DateTime.Now;
                ViewBag.ToTime = ToTime;
                return View(db.SOP_GetListOrganizationRating(FromTime, ToTime).ToList());
            }
            else
            {

                return RedirectToAction("ListOrganizationRatingDirect", new { FromTime = FromTime, ToTime = ToTime });
            }
        }
        public ActionResult ListOrganizationRatingDirect(DateTime FromTime, DateTime ToTime)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.FromTime = FromTime;
            ViewBag.ToTime = ToTime;
            return View("ListOrganizationRating", db.SOP_GetListOrganizationRating(FromTime, ToTime).ToList());
        }
        public ActionResult ListOrganizationRatingPrint(DateTime FromTime, DateTime ToTime)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.FromTime = FromTime.ToString("dd/M/yyyy");
            ViewBag.ToTime = ToTime.ToString("dd/M/yyyy");
            return View( db.SOP_GetListOrganizationRating(FromTime, ToTime).ToList());
        }
        
    }
}