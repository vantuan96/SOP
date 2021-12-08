using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOP.Models;
using SOP.FunctionLibrary;
using Ext.Net;
using Ext.Net.MVC;
namespace SOP.Controllers
{
    public class LoginController : Controller
    {
        private SOPEntities db = new SOPEntities();
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckLogin(User user)
        {   string pass=FunctionLibrary.Function.encryption(user.User_PassWord);
            if (ModelState.IsValid)
            {
                var obj = (from u in db.Users
                           join ug in db.UserGroups on u.User_Id equals ug.UserGroup_UserId
                           where u.User_UserName == user.User_UserName
                           select u).ToList();
                if (obj.Count>=1)
                {
                    var a = obj.First().User_PassWord;
                    if (obj.First().User_PassWord.ToLower() == pass.ToLower())
                    {
                        Session["UserId"] = obj.First().User_Id;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (FunctionLibrary.Function.Authenticate(user.User_UserName, user.User_PassWord))
                        { 
                             Session["UserId"] = obj.First().User_Id;
                             obj.First().User_PassWord = pass;
                             db.SaveChanges();
                             return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            X.Msg.Alert("Thông báo", "Mật khẩu sai").Show();
                        }
                    }
                }
                else
                {   
                    X.Msg.Alert("Thông báo", "Không tồn tại tài khoản này hoặc bạn không có quyền truy cập hệ thống này").Show();
                }
            }
            return this.Direct();
        }
        public ActionResult Logout()
        {
            Session["UserId"] = null;
            return RedirectToAction("Index", "Login");
        }
        public ActionResult ChangePassWord()
        {

            return this.PartialExtView("ChangePassWord");
        }
        public ActionResult SavePassWord(FormCollection values)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            User Obj = db.Users.Find(userId);
            string OldPass = FunctionLibrary.Function.encryption(values["Old_Password"].Trim());
            string New1Pass = values["New1_Password"].Trim();
            string New2Pass = values["New2_Password"].Trim();
            if (New1Pass == New2Pass && OldPass == Obj.User_PassWord)
            {
                Obj.User_PassWord = FunctionLibrary.Function.encryption(New1Pass);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            else
            {
                X.Msg.Alert("Thông báo", "Mật khẩu xác thực hoặc mật khẩu cũ không đúng").Show();
                return this.Direct();
            }
        }
        
	}
}