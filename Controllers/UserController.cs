using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP.Models;
using Ext.Net.MVC;
using Ext.Net;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.DirectoryServices.AccountManagement;
using System.Collections;
using System.DirectoryServices;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
namespace SOP.Controllers
{
    public class UserController : Controller
    {
        private SOPEntities db = new SOPEntities();

        // GET: /User/
        public ActionResult Index()
        {
            var lstUser = db.Users.Include(d => d.Department);
            lstUser.Include(o => o.Department.Organization);
            ViewBag.ListUser = lstUser.ToList();
            return View();
        }


        public ActionResult Synchronize()
        {
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "laocai.egov.vn", "pthoa-lcit", "123123");
                GroupPrincipal insGroupPrincipal = new GroupPrincipal(ctx);
                insGroupPrincipal.Name = "*";
                SearchGroups(insGroupPrincipal);
            }
            catch (Exception ex)
            {
                X.MessageBox.Alert("Thông Báo!", "Lỗi Kết Nối").Show();
            }
            return this.PartialExtView("Synchronize");
        }

        private void SearchGroups(GroupPrincipal parGroupPrincipal)
        {
            try
            {
                PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();
                insPrincipalSearcher.QueryFilter = parGroupPrincipal;
                PrincipalSearchResult<Principal> results = insPrincipalSearcher.FindAll();
                List<Ext.Net.ListItem> listUserId = new List<Ext.Net.ListItem>();
                foreach (Principal p in results)
                {
                    ListItem a = new Ext.Net.ListItem(p.Name, p.Name);
                    listUserId.Add(a);
                }
                ViewBag.listUser = listUserId;
            }
            catch (Exception)
            {
                X.MessageBox.Alert("Thông Báo!", "Lỗi Kết Nối").Show();
                throw;
            }
        }

        public ActionResult SearchUserAccountByOrganization(string Organization_OrganizationName)
        {
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "laocai.egov.vn", "pthoa-lcit", "123123");
                List<User> listUser1 = new List<User>();
                GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, Organization_OrganizationName);
                if (grp != null)
                {
                    foreach (Principal p in grp.GetMembers(false))
                    {
                        User user = new User();
                        UserPrincipal user1 = UserPrincipal.FindByIdentity(ctx, p.UserPrincipalName);
                        user.User_Email = p.SamAccountName;
                        user.User_FullName = p.DisplayName;
                        user.User_UserName = p.UserPrincipalName;
                        user.User_ReligionDetail = p.StructuralObjectClass;
                        //listUser1.Where(x => x.User_UserName == p.UserPrincipalName);
                        listUser1.Add(user);
                    }
                    grp.Dispose();
                    ctx.Dispose();
                }
                else
                {
                    X.MessageBox.Alert("Thông Báo!", " Không có dữ liệu ").Show();
                }
                return this.Store(listUser1.ToList());
            }
            catch (Exception)
            {
                X.MessageBox.Alert("Thông Báo!", "Lỗi Kết Nối").Show();
                throw;
            }
        }

        public ActionResult SynchronizeUserId(string User_Email)
        {
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "laocai.egov.vn", "pthoa-lcit", "123123");
                Principal item = Principal.FindByIdentity(ctx, User_Email);
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, User_Email);
                if (item.SamAccountName.Count() > 0)
                {
                    //user.SetPassword("123*");
                    SOP.Models.User us = new User();
                    us.User_UserName = item.SamAccountName;
                    us.User_Email = item.UserPrincipalName;
                    us.User_FullName = item.DisplayName;
                    us.User_Address = item.Description;
                    us.User_PhoneNumber = user.VoiceTelephoneNumber;
                    us.User_ReligionDetail = GetDepartmentForUser(User_Email);
                    us.User_Mobile = GetDepartmentForUser(User_Email);
                    db.Users.Add(us);
                    db.SaveChanges();
                    X.MessageBox.Alert("Thông Báo!", "Đồng Bộ User " + item.SamAccountName + " thành công").Show();
                    return this.Direct();
                }
                else
                {
                    X.MessageBox.Alert("Thông Báo!", "Tài khoản không thể tích hợp").Show();
                    return this.Direct();
                }
            }
            catch (Exception)
            {
                X.MessageBox.Alert("Thông Báo!", "Lỗi Kết Nối").Show();
                throw;
            }
        }
        public static string GetOUForUser(string samAccountName)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName))
                {
                    using (DirectoryEntry deUser = user.GetUnderlyingObject() as DirectoryEntry)
                    {
                        using (DirectoryEntry deUserContainer = deUser.Parent.Parent)
                        {
                            return deUserContainer.Properties["Name"].Value.ToString();
                        }
                    }
                }
            }
        }
        public string GetDepartmentForUser(string samAccountName)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName))
                {
                    using (DirectoryEntry deUser = user.GetUnderlyingObject() as DirectoryEntry)
                    {
                        using (DirectoryEntry deUserContainer = deUser.Parent)
                        {
                            return deUserContainer.Properties["Name"].Value.ToString();
                        }
                    }
                }
            }
        }

        //public PrincipalContext GetPrincipalContext()
        //{
        //    PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain);
        //    return oPrincipalContext;
        //}

        //public UserPrincipal GetUser(string sUserName)
        //{
        //    PrincipalContext oPrincipalContext = GetPrincipalContext();

        //    UserPrincipal oUserPrincipal = UserPrincipal.FindByIdentity(oPrincipalContext, sUserName);
        //    return oUserPrincipal;
        //}

        //public ICollection<Principal> GetUserGroups(object sUserName)
        //{
        //    GroupPrincipal insGroupPrincipal = (GroupPrincipal)sUserName;
        //    List<Principal> insListPrincipal = new List<Principal>();
        //    foreach (Principal p in insGroupPrincipal.Members)
        //    {
        //        insListPrincipal.Add(p);
        //    }
        //    return insListPrincipal;
        //List<Principal> myItems = new List<Principal>();
        //UserPrincipal oUserPrincipal = GetUser(sUserName);


        //if (oUserPrincipal != null)
        //{
        //    PrincipalSearchResult<Principal> oPrincipalSearchResult = oUserPrincipal.GetGroups();
        //    foreach (var oResult in oPrincipalSearchResult)
        //    {
        //        myItems.Add(oResult);
        //    }
        //}
        //else
        //{
        //    X.MessageBox.Alert("Thông Báo!", "khong co gia tri").Show();
        //}
        //return myItems;
        //}

        //private void SearchUsers(UserPrincipal parUserPrincipal)
        //{
        //    PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();
        //    insPrincipalSearcher.QueryFilter = parUserPrincipal;
        //    PrincipalSearchResult<Principal> results = insPrincipalSearcher.FindAll();
        //    List<Ext.Net.ListItem> insListPrincipal = new List<Ext.Net.ListItem>();
        //    foreach (Principal p in results)
        //    {
        //        ListItem a = new Ext.Net.ListItem(p.Name, p.Name);
        //        insListPrincipal.Add(a);
        //    }
        //    ViewBag.listUserSynchronize = insListPrincipal;
        //}

        // GET: /User/Create
        public ActionResult Create()
        {
            List<Ext.Net.ListItem> listOrganization = new List<Ext.Net.ListItem>();
            foreach (var i in db.Organizations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Organization_Name, i.Organization_Id);
                listOrganization.Add(a);
            }
            ViewBag.UserCurrentOrganizationId = listOrganization;
            List<Ext.Net.ListItem> listdepartment = new List<Ext.Net.ListItem>();
            foreach (var i in db.Departments.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Department_Name, i.Department_Id);
                listdepartment.Add(a);
            }
            ViewBag.UserCurrentDepartmentId = listdepartment;

            return this.PartialExtView("Create");
        }

        public ActionResult CloseClick()
        {

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SaveUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.User_CurrentOrganizationId == 0)
                    {
                        X.MessageBox.Alert("Thông Báo!", "Cơ quan không được để trống").Show();
                        return this.Direct();
                    }
                    else
                    {
                        string password = encrypt(user.User_PassWord.Trim());
                        user.User_PassWord = password;
                        db.Users.Add(user);
                        db.SaveChanges();
                        //return this.Store(db.Users.Include(d => d.Department.Organization).ToList());
                        return RedirectToAction("Index");
                    }
                }
                return View("Create");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: /User/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = await db.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return this.PartialExtView("Edit", user);
        //}

        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "User_Id,User_FullName,User_PassWord,User_Email,User_Birthday,User_Gender,User_IdentityNumber,User_IdentityCreatedOn,User_IdentityCreatedBy,User_Religion,User_ReligionDetail,User_Address,District_Id,Province_Id,User_CurrentResidence,Ethnic_Id,User_PhoneNumber,User_Mobile,User_Active")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //    }
        //    return this.PartialExtView("Edit", user);
        //}

        // Action Edit User
        public ActionResult Edit(int? User_Id)
        {
            List<Ext.Net.ListItem> listOrganization = new List<Ext.Net.ListItem>();
            foreach (var i in db.Organizations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Organization_Name, i.Organization_Id);
                listOrganization.Add(a);
            }
            ViewBag.UserCurrentOrganizationId = listOrganization;
            List<Ext.Net.ListItem> listdepartment = new List<Ext.Net.ListItem>();
            foreach (var i in db.Departments.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Department_Name, i.Department_Id);
                listdepartment.Add(a);
            }
            ViewBag.UserCurrentDepartmentId = listdepartment;

            List<Ext.Net.ListItem> listEthnic = new List<Ext.Net.ListItem>();
            foreach (var i in db.Ethnics.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.Ethnic_Name, i.Ethnic_Id);
                listEthnic.Add(a);
            }
            ViewBag.UserEthnic = listEthnic;

            List<Ext.Net.ListItem> listDistrict = new List<Ext.Net.ListItem>();
            foreach (var i in db.Districts.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.District_Name, i.District_Id);
                listDistrict.Add(a);
            }
            ViewBag.UserDistrict = listDistrict;

            if (User_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TextField User_PassWord = this.GetCmp<TextField>("User_PassWord");
            User user = db.Users.Find(User_Id);
            ViewBag.User_PassWord = user.User_PassWord.ToString();
            if (user == null)
            {
                return HttpNotFound();
            }
            return this.PartialExtView("Edit", user);
        }

        public ActionResult EditSave(User user)
        {
            if (ModelState.IsValid)
            {
                string password = encrypt(user.User_PassWord.Trim());
                user.User_PassWord = password;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }

        // POST: /User/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                User user = await db.Users.FindAsync(id);
                if (user.HistoricalWorks.Count() > 0)
                {
                    X.MessageBox.Alert("Thông Báo!", "Tài khoản không thể xóa được").Show();
                    return this.Direct();
                }
                else if (user.HistoricalWorks.Count() == 0)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    //return this.Store(db.Users.Include(d => d.Department).ToList());
                    return RedirectToAction("Index");
                }
                else
                {
                    X.MessageBox.Alert("Thông Báo!", "Tài khoản không thể xóa được").Show();
                    return this.Direct();
                }
            }
            catch (Exception)
            {
                X.MessageBox.Alert("Thông Báo!", "Tài khoản không thể xóa được").Show();
                return this.Direct();
            }
        }

        public async Task<ActionResult> Acception(int id)
        {
            try
            {
                User user = await db.Users.FindAsync(id);
                user.User_Active = true;
                await db.SaveChangesAsync();
                //return this.Store(db.Users.Include(d=>d.Department.Organization).ToList());
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult GetEthnic()
        {
            return this.Store(db.Ethnics.ToList());
        }

        public ActionResult GetDistrict(int provinceId)
        {
            try
            {
                var item = from x in db.Districts where x.District_ProvinceId == provinceId select x;
                return this.Store(item.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetOrganization()
        {
            var item = from x in db.Organizations
                       select new
                       {
                           x.Organization_Id,
                           x.Organization_Name

                       };
            return this.Store(item.ToList());
        }

        public ActionResult GetDepartment(int organizationId)
        {
            try
            {
                var item = from x in db.Departments
                           where x.Department_OrganizationId == organizationId
                           select new
                           {
                               x.Department_Id,
                               x.Department_Name
                           };
                return this.Store(item.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetListDistrict()
        {
            return this.Store(db.Districts.ToList());
        }

        public ActionResult GetProvince()
        {
            return this.Store(db.Provinces.ToList());
        }

        public JsonResult CheckEmail(string User_Email)
        {
            var items = from x in db.Users select x;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            foreach (var item in items)
            {
                if (User_Email == item.User_Email)
                {
                    return Json(new { valid = false, message = "Email đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                if (!re.IsMatch(User_Email))
                {
                    return Json(new { valid = false, message = "Email không đúng định dạng" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { valid = true }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private static string toMD5(string pass)
        //{
        //    MD5CryptoServiceProvider myMD5 = new MD5CryptoServiceProvider();
        //    byte[] myPass = System.Text.Encoding.UTF8.GetBytes(pass);
        //    myPass = myMD5.ComputeHash(myPass);
        //    StringBuilder s = new StringBuilder();
        //    foreach (byte p in myPass)
        //    {
        //        s.Append(p.ToString("x2").ToLower());
        //    }
        //    return s.ToString();
        //}

        public string encrypt(string pass)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pass.Trim(), "MD5");
        }

        public ActionResult View1()
        {
            return View();
        }

        public Paging<Ethnic> EthnicPaging(int start, int limit, string sort, string dir, string filter)
        {
            List<Ethnic> ethnic = db.Ethnics.ToList();
            foreach (var item in ethnic)
            {
                if (!string.IsNullOrEmpty(filter) && filter != "*")
                {
                    ethnic.RemoveAll(plant => !item.Ethnic_Name.ToLower().StartsWith(filter.ToLower()));
                }
            }
            if (!string.IsNullOrEmpty(sort))
            {
                ethnic.Sort(delegate(Ethnic x, Ethnic y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > ethnic.Count)
            {
                limit = ethnic.Count - start;
            }

            List<Ethnic> rangePlants = (start < 0 || limit < 0) ? ethnic : ethnic.GetRange(start, limit);

            return new Paging<Ethnic>(rangePlants, ethnic.Count);
        }

        public ActionResult GetDaTaEthnic(int start, int limit, int page, string query)
        {
            Paging<Ethnic> ethnic = EthnicPaging(start, limit, "", "", query);

            return this.Store(ethnic.Data, ethnic.TotalRecords);
        }

    }
}
