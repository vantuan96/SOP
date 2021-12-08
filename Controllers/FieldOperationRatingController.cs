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
using System.Web.Mvc;

namespace SOP.Controllers
{
    public class FieldOperationRatingController : Controller
    {
        private SOPEntities db = new SOPEntities();
        // GET: /FieldOperationRating/
        public ActionResult Index()
        {
            return View(db.FieldOperations.Where(u => u.FieldOperation_Active == true).ToList());
        }
        public ActionResult MFieldOperationRatingdirect(int? Month, int? Year, int FieldOperationId)
        {
            if (Month == null)
            {
                return RedirectToAction("MNowFieldOperationRating", new { FieldOperationId = FieldOperationId });
            }
            else
            {
                return RedirectToAction("MFieldOperationRating", new { Month = Month, Year = Year, FieldOperationId = FieldOperationId });
            }
        }
        public ActionResult MNowFieldOperationRating(int FieldOperationId)
        {
            var CurrentRatingFieldOperation = (from u in db.RatingResults
                                             join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                             join o in db.Users on u.RatingResult__UserId equals o.User_Id
                                             join d in db.UserFieldOperations on o.User_Id equals d.UserFieldOperation_UserId
                                             join b in db.FieldOperations on d.UserFieldOperation_FieldOperationId equals b.FieldOperation_Id
                                             where b.FieldOperation_Id == FieldOperationId && u.RatingResult_CreatedOn.Value.Month == DateTime.Now.Month && u.RatingResult_CreatedOn.Value.Year == DateTime.Now.Year
                                             group new { u, ug } by new { ug.Rating_Name }
                                             into grp
                                             select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            ViewBag.Month = DateTime.Now.Month.ToString();
            ViewBag.Year = DateTime.Now.Year.ToString();
            ViewBag.FieldOperationId = FieldOperationId.ToString();
            return View("MFieldOperationRating", CurrentRatingFieldOperation);
        }
        public ActionResult MFieldOperationRating(int Month, int Year, int FieldOperationId)
        {
            ViewBag.Month = Month.ToString();
            ViewBag.Year = Year.ToString();
            ViewBag.FieldOperationId = FieldOperationId.ToString();
            var CurrentRatingFieldOperation = (from u in db.RatingResults
                                             join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                             join o in db.Users on u.RatingResult__UserId equals o.User_Id
                                             join d in db.UserFieldOperations on o.User_Id equals d.UserFieldOperation_UserId
                                             join b in db.FieldOperations on d.UserFieldOperation_FieldOperationId equals b.FieldOperation_Id
                                             where b.FieldOperation_Id == FieldOperationId && (u.RatingResult_CreatedOn.Value.Month == Month || Month == 0) && (u.RatingResult_CreatedOn.Value.Year == Year || Year == 0)
                                             group new { u, ug } by new { ug.Rating_Name }
                                             into grp
                                             select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            return View("MFieldOperationRating", CurrentRatingFieldOperation);
        }
        public ActionResult MFieldOperationRatingPrint(int Month, int Year, int FieldOperationId)
        {
            var CurrentRatingFieldOperation = (from u in db.RatingResults
                                               join ug in db.Ratings on u.RatingResult_RatingId equals ug.Rating_Id
                                               join o in db.Users on u.RatingResult__UserId equals o.User_Id
                                               join d in db.UserFieldOperations on o.User_Id equals d.UserFieldOperation_UserId
                                               join b in db.FieldOperations on d.UserFieldOperation_FieldOperationId equals b.FieldOperation_Id
                                               where b.FieldOperation_Id == FieldOperationId && (u.RatingResult_CreatedOn.Value.Month == Month || Month == 0) && (u.RatingResult_CreatedOn.Value.Year == Year || Year == 0)
                                               group new { u, ug } by new { ug.Rating_Name }
                                               into grp
                                               select new { Rating_Name = grp.Key.Rating_Name, Rating_Total = grp.Count() }).ToList();
            ViewBag.FieldOperation = db.FieldOperations.Find(FieldOperationId).FieldOperation_Name;
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
            return View("MFieldOperationRatingPrint", CurrentRatingFieldOperation);
        }
        //-----------------------------------------------------------------------------
        public ActionResult YFieldOperationRatingdirect(int? Year, int FieldOperationId)
        {
            if (Year == null)
            {
                return RedirectToAction("YNowFieldOperationRating", new { FieldOperationId = FieldOperationId });
            }
            else
            {
                return RedirectToAction("YFieldOperationRating", new { Year = Year, FieldOperationId = FieldOperationId });
            }
        }
        public ActionResult YNowFieldOperationRating(int FieldOperationId)
        {
            ViewBag.Time = DateTime.Now.Year.ToString();
            ViewBag.FieldOperationId = FieldOperationId.ToString();
            return View("YFieldOperationRating", db.SOP_GetYFieldOperationRating_ByYear(DateTime.Now.Year, FieldOperationId));
        }
        public ActionResult YFieldOperationRating(int Year, int FieldOperationId)
        {
            ViewBag.FieldOperationId = FieldOperationId.ToString();
            ViewBag.Time = Year.ToString();
            return View("YFieldOperationRating", db.SOP_GetYFieldOperationRating_ByYear(Year, FieldOperationId));
        }
        public ActionResult YFieldOperationPrint(int Year, int FieldOperationId)
        {
            ViewBag.Time = Year.ToString();
            ViewBag.FieldOperation = db.FieldOperations.Find(FieldOperationId).FieldOperation_Name;
            return View(db.SOP_GetYFieldOperationRating_ByYear(Year, FieldOperationId).ToList());

        }
        //--------------------------------------------------------------------------------------------------------------
        public ActionResult YCompareFieldOperationRating(int FieldOperationId)
        {
            ViewBag.FieldOperationId = FieldOperationId.ToString();
            return View(db.SOP_GetYCompareFieldOperationRating(FieldOperationId));
        }
        public ActionResult YCompareFieldOperationPrint(int FieldOperationId)
        {
            ViewBag.FieldOperation = db.FieldOperations.Find(FieldOperationId).FieldOperation_Name;
            return View(db.SOP_GetYCompareFieldOperationRating(FieldOperationId).ToList());
        }
        //--------------------------------------------------------------------------------------------------------------
        public ActionResult DetailFieldOperationRating(DateTime? FromTime, DateTime? ToTime, int FieldOperationId)
        {
            ViewBag.FieldOperationId = FieldOperationId.ToString();
            if (FromTime == null)
            {

                FromTime = new DateTime(DateTime.Now.Year, 1, 1);
                ViewBag.FromTime = FromTime;
                ToTime = DateTime.Now;
                ViewBag.ToTime = ToTime;
                return View(db.SOP_GetDetailFieldOperationRating(FromTime, ToTime, FieldOperationId).ToList());
            }
            else
            {
                return RedirectToAction("DetailFieldOperationRatingRedirect", new { FieldOperationId = FieldOperationId, FromTime = FromTime, ToTime = ToTime });
            }

        }
        public ActionResult DetailFieldOperationRatingRedirect(int FieldOperationId, DateTime FromTime, DateTime ToTime)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.FieldOperationId = FieldOperationId.ToString();
            ViewBag.FromTime = FromTime;
            ViewBag.ToTime = ToTime;
            return View("DetailFieldOperationRating", db.SOP_GetDetailFieldOperationRating(FromTime, ToTime, FieldOperationId).ToList());

        }
        public ActionResult DetailFieldOperationRatingPrint(int FieldOperationId, DateTime FromTime, DateTime ToTime)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.FieldOperation = db.FieldOperations.Find(FieldOperationId).FieldOperation_Name;
            ViewBag.FromTime = FromTime.ToString("dd/M/yyyy");
            ViewBag.ToTime = ToTime.ToString("dd/M/yyyy");
            return View(db.SOP_GetDetailFieldOperationRating(FromTime, ToTime, FieldOperationId).ToList());

        }
        //---------------------------------------------------------------
        public ActionResult ListFieldOperationRating(DateTime? FromTime, DateTime? ToTime)
        {
            if (FromTime == null)
            {
                FromTime = new DateTime(DateTime.Now.Year, 1, 1);
                ViewBag.FromTime = FromTime;
                ToTime = DateTime.Now;
                ViewBag.ToTime = ToTime;
                return View(db.SOP_GetListFieldOperationRating(FromTime, ToTime).ToList());
            }
            else
            {

                return RedirectToAction("ListFieldOperationRatingDirect", new { FromTime = FromTime, ToTime = ToTime });
            }
        }
        public ActionResult ListFieldOperationRatingDirect(DateTime FromTime, DateTime ToTime)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.FromTime = FromTime;
            ViewBag.ToTime = ToTime;
            return View("ListFieldOperationRating", db.SOP_GetListFieldOperationRating(FromTime, ToTime).ToList());
        }
        public ActionResult ListFieldOperationRatingPrint(DateTime FromTime, DateTime ToTime)
        {
            ToTime = ToTime.AddHours(23);
            ViewBag.FromTime = FromTime.ToString("dd/M/yyyy");
            ViewBag.ToTime = ToTime.ToString("dd/M/yyyy");
            return View(db.SOP_GetListFieldOperationRating(FromTime, ToTime).ToList());
        }
    }
}