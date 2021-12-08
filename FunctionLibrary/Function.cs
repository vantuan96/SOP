using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net.MVC;
using Ext.Net;
using SOP.Models;
using System.Security.Cryptography;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
namespace SOP.FunctionLibrary
{
    public class Function
    {

        public static List<Ext.Net.ListItem> OrganizationrList()
        {   SOPEntities db = new SOPEntities();
            List<Ext.Net.ListItem> OrganizationrList = new List<Ext.Net.ListItem>();
                foreach (var i in db.Organizations.ToList())
                {
                    ListItem a = new Ext.Net.ListItem(i.Organization_Name, i.Organization_Id);
                    OrganizationrList.Add(a);
                }
                return OrganizationrList;
        }
        public static string encryption(string pass)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pass.Trim(), "MD5");
        }
        public static bool Authenticate(string uSerName, string passWord)
        {
            bool authentic = false;
            DirectoryEntry entry;
            try
            {
                entry = new DirectoryEntry("LDAP://laocai.egov.vn", uSerName, passWord);

                object nativeObject = entry.NativeObject;

                authentic = true;

            }
            catch (DirectoryServicesCOMException) { }

            return authentic;
        }
    }
}