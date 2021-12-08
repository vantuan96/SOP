using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP.Models;
using Ext.Net.MVC;
using Ext.Net;

namespace SOP.Controllers
{
    public class UserGroupController : Controller
    {
        private SOPEntities db = new SOPEntities();
        //
        // GET: /UserGroup/
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }
        public ActionResult UserGroupList(int? Group_Id)
        {
            ViewBag.Group_Id = Group_Id.ToString();
            ViewBag.GetListUserGroup = (from u in db.Users
                      join ug in db.UserGroups on u.User_Id equals ug.UserGroup_UserId
                      where ug.UserGroup_GroupId == Group_Id &&  ug.UserGroup_Active==true
                      select u).ToList();

            List<Ext.Net.ListItem> UserList = new List<Ext.Net.ListItem>();
            foreach (var i in db.Users.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.User_Email, i.User_Id);
                UserList.Add(a);
            }
            ViewBag.UserList = UserList;
            return View();
        }
        public ActionResult InsertUserGroup(int UserGroup_UserId, int UserGroup_GroupId)
        {
            if (ModelState.IsValid)
            {   try
                {
                UserGroup userGroup = (from u in db.UserGroups where u.UserGroup_GroupId == UserGroup_GroupId && u.UserGroup_UserId == UserGroup_UserId select u).First();
                userGroup.UserGroup_Active = true;
                db.SaveChanges();
                }
                catch
                    {
                    UserGroup userGroup = new UserGroup();
                    userGroup.UserGroup_UserId = UserGroup_UserId;
                    userGroup.UserGroup_GroupId = UserGroup_GroupId;
                    userGroup.UserGroup_Active = true;
                    db.UserGroups.Add(userGroup);
                    db.SaveChanges();
                    }
            }
            var listgroup = (from u in db.Users
                                        join ug in db.UserGroups on u.User_Id equals ug.UserGroup_UserId
                             where ug.UserGroup_GroupId == UserGroup_GroupId && ug.UserGroup_Active == true
                                        select u).ToList();
            return this.Store(listgroup);
        }
        public ActionResult DeleteUserGroup(int User_Id, int UserGroup_GroupId)
        {
            try
            {
                UserGroup userGroup = (from u in db.UserGroups where u.UserGroup_GroupId == UserGroup_GroupId && u.UserGroup_UserId == User_Id select u).First();
                userGroup.UserGroup_Active = false;
                db.SaveChanges();
                var listgroup = (from u in db.Users
                                 join ug in db.UserGroups on u.User_Id equals ug.UserGroup_UserId
                                 where ug.UserGroup_GroupId == UserGroup_GroupId && ug.UserGroup_Active==true
                                 select u).ToList();
                return this.Store(listgroup);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //------------------------------------------------------------------------------------
        public ActionResult UserFieldOperationList(int? User_Id)
        {
            ViewBag.User_Id = User_Id.ToString();
            ViewBag.UserFieldOperationList = (from u in db.UserFieldOperations
                                        join fo in db.FieldOperations on u.UserFieldOperation_FieldOperationId equals fo.FieldOperation_Id
                                        where u.UserFieldOperation_UserId==User_Id && u.UserFieldOperation_Active==true
                                        select fo).ToList();

            List<Ext.Net.ListItem> FieldOperationList = new List<Ext.Net.ListItem>();
            foreach (var i in db.FieldOperations.ToList())
            {
                ListItem a = new Ext.Net.ListItem(i.FieldOperation_Name,i.FieldOperation_Id.ToString());
                FieldOperationList.Add(a);
            }
            ViewBag.FieldOperationList = FieldOperationList;
            return this.PartialExtView();
        }
        public ActionResult InsertUserFieldOperation(int User_Id, int FieldOperation_Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserFieldOperation userFieldOperation = (from u in db.UserFieldOperations where u.UserFieldOperation_UserId == User_Id && u.UserFieldOperation_FieldOperationId == FieldOperation_Id select u).First();
                    userFieldOperation.UserFieldOperation_Active = true;
                    db.SaveChanges();
                }
                catch
                {
                    UserFieldOperation userFieldOperation = new UserFieldOperation();
                    userFieldOperation.UserFieldOperation_UserId = User_Id;
                    userFieldOperation.UserFieldOperation_FieldOperationId = FieldOperation_Id;
                    userFieldOperation.UserFieldOperation_Active = true;
                    db.UserFieldOperations.Add(userFieldOperation);
                    db.SaveChanges();
                }
            }
            var UserFieldOperation = (from u in db.UserFieldOperations
                             join fo in db.FieldOperations on u.UserFieldOperation_FieldOperationId equals fo.FieldOperation_Id
                             where u.UserFieldOperation_UserId == User_Id && u.UserFieldOperation_Active==true
                             select fo).ToList();
            return this.Store(UserFieldOperation);
        }
        public ActionResult DeleteUserFieldOperation(int User_Id, int FieldOperation_Id)
        {
            try
            {
                UserFieldOperation userFieldOperation = (from u in db.UserFieldOperations where u.UserFieldOperation_UserId == User_Id && u.UserFieldOperation_FieldOperationId == FieldOperation_Id select u).First();

                userFieldOperation.UserFieldOperation_Active = false;
                db.SaveChanges();
                var UserFieldOperation = (from u in db.UserFieldOperations
                                          join fo in db.FieldOperations on u.UserFieldOperation_FieldOperationId equals fo.FieldOperation_Id
                                          where u.UserFieldOperation_UserId == User_Id && u.UserFieldOperation_Active == true
                                          select fo).ToList();
                return this.Store(UserFieldOperation);
            }
            catch (Exception)
            {

                throw;
            }
        }
	}
}