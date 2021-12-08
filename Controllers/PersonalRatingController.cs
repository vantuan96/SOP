using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using SOP.Models;
using SOP.FunctionLibrary;
namespace SOP.Controllers
{
    public class PersonalRatingController : Controller
    {
        private SOPEntities db = new SOPEntities();
        // GET: /PersonalRating/
        public ActionResult Index(int? CBCS)
        {
            if (CBCS == null)
            {
                var UserList = (from u in db.Users
                                join ug in db.UserGroups on u.User_Id equals ug.UserGroup_UserId
                                join o in db.Organizations on u.User_CurrentOrganizationId equals o.Organization_Id
                                where ug.UserGroup_GroupId == Constants.CBMC && ug.UserGroup_Active == true
                                select new { u.User_Id, u.User_FullName, u.User_Email, u.User_UserName, u.User_CurrentOrganizationId, o.Organization_Name }).ToList();
                return View(UserList);
            }
            else
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                User user = db.Users.Find(UserId);
                var UserList = (from u in db.Users
                                join ug in db.UserGroups on u.User_Id equals ug.UserGroup_UserId
                                join o in db.Organizations on u.User_CurrentOrganizationId equals o.Organization_Id
                                where ug.UserGroup_GroupId == Constants.CBMC && ug.UserGroup_Active == true && u.User_CurrentOrganizationId==user.User_CurrentOrganizationId
                                select new { u.User_Id, u.User_FullName, u.User_Email, u.User_UserName, u.User_CurrentOrganizationId, o.Organization_Name }).ToList();
                return View(UserList);
            }
        }
        public ActionResult MPersonalRatingdirect(int? Month, int? Year, int UserId)
        {   if(Month==null)
             {
                 return RedirectToAction("MNowPersonalRating", new { UserId = UserId });
             }
            else
             {
                 return RedirectToAction("MPersonalRating", new { Month = Month, Year = Year, UserId = UserId });
             }
        }
        public ActionResult MNowPersonalRating(int UserId)
        {
            var CurrentRatingUser = (from u in db.RatingResults
                                     join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                     where u.RatingResult__UserId == UserId && u.RatingResult_CreatedOn.Value.Month == DateTime.Now.Month && u.RatingResult_CreatedOn.Value.Year == DateTime.Now.Year
                                     group new { u, ug } by new { ug.Rating_Name }
                                         into grp
                                         select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            ViewBag.Month = DateTime.Now.Month.ToString();
            ViewBag.Year = DateTime.Now.Year.ToString();
            ViewBag.UserId = UserId.ToString();
            return View("MPersonalRating", CurrentRatingUser);
        }
        public ActionResult MPersonalRating(int Month, int Year, int UserId)
        {
            ViewBag.Month = Month.ToString();
            ViewBag.Year = Year.ToString();
            ViewBag.UserId = UserId.ToString();
            var CurrentRatingUser = (from u in db.RatingResults
                                     join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                     where u.RatingResult__UserId == UserId && (u.RatingResult_CreatedOn.Value.Month == Month || Month == 0) && (u.RatingResult_CreatedOn.Value.Year == Year || Year == 0)
                                     group new { u, ug } by new { ug.Rating_Name }
                                         into grp
                                         select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            return View("MPersonalRating", CurrentRatingUser);
        }
        public ActionResult MPersonalRatingPrint(int Month, int Year, int UserId)
        {
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
            return View("MPersonalRatingPrint", CurrentRatingUser);    
        }

        //-----------------------------------------------------------------------------
        public ActionResult YPersonalRatingdirect(int? Year, int UserId)
        {
            if (Year == null)
            {
                return RedirectToAction("YNowPersonalRating", new { UserId = UserId });
            }
            else
            {
                return RedirectToAction("YPersonalRating", new { Year = Year,UserId = UserId });
            }
        }
        public ActionResult YNowPersonalRating(int UserId)
        {
            ViewBag.Time = DateTime.Now.Year.ToString();
            ViewBag.UserId = UserId.ToString();
            return View("YPersonalRating", db.SOP_GetMCompareRating_ByYear(DateTime.Now.Year, UserId));
        }
        public ActionResult YPersonalRating(int Year, int UserId)
        {
            ViewBag.UserId = UserId.ToString();
            ViewBag.Time = Year.ToString();
            return View("YPersonalRating", db.SOP_GetMCompareRating_ByYear(Year, UserId));
        }
        public ActionResult YPersonalPrint(int Year, int UserId)
        {
            ViewBag.Time = Year.ToString();
            ViewBag.User = db.Users.Find(UserId);
            ViewBag.Organizations = db.Organizations.Find(ViewBag.User.User_CurrentOrganizationId).Organization_Name;
            return View(db.SOP_GetMCompareRating_ByYear(Year, UserId).ToList());

        }
        //--------------------------------------------------------------------------------------------------------------
        public ActionResult YComparePersonalRating(int UserId)
        {   ViewBag.UserId=UserId.ToString();
            return View(db.SOP_GetMCompareRating(UserId));
        }
        public ActionResult YComparePersonalPrint(int UserId)
        {
            ViewBag.User = db.Users.Find(UserId);
            ViewBag.Organizations = db.Organizations.Find(ViewBag.User.User_CurrentOrganizationId).Organization_Name;
            return View(db.SOP_GetMCompareRating(UserId).ToList());
        }

	}
}