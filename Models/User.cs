//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SOP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.AccessHistories = new HashSet<AccessHistory>();
            this.HistoricalWorks = new HashSet<HistoricalWork>();
            this.Opinions = new HashSet<Opinion>();
            this.Opinions1 = new HashSet<Opinion>();
            this.RatingResults = new HashSet<RatingResult>();
            this.UserFieldOperations = new HashSet<UserFieldOperation>();
            this.UserGroups = new HashSet<UserGroup>();
        }
    
        public int User_Id { get; set; }
        public string User_FullName { get; set; }
        public string User_PassWord { get; set; }
        public string User_Email { get; set; }
        public Nullable<System.DateTime> User_Birthday { get; set; }
        public Nullable<int> User_Gender { get; set; }
        public string User_IdentityNumber { get; set; }
        public Nullable<System.DateTime> User_IdentityCreatedOn { get; set; }
        public string User_IdentityCreatedBy { get; set; }
        public Nullable<bool> User_Religion { get; set; }
        public string User_ReligionDetail { get; set; }
        public string User_Address { get; set; }
        public Nullable<int> User_DistrictId { get; set; }
        public Nullable<int> User_ProvinceId { get; set; }
        public string User_CurrentResidence { get; set; }
        public Nullable<int> User_EthnicId { get; set; }
        public string User_PhoneNumber { get; set; }
        public string User_Mobile { get; set; }
        public Nullable<int> User_CurrentOrganizationId { get; set; }
        public Nullable<bool> User_Active { get; set; }
        public Nullable<int> User_CurrentDepartmentId { get; set; }
        public string User_UserName { get; set; }
    
        public virtual ICollection<AccessHistory> AccessHistories { get; set; }
        public virtual Department Department { get; set; }
        public virtual Ethnic Ethnic { get; set; }
        public virtual ICollection<HistoricalWork> HistoricalWorks { get; set; }
        public virtual ICollection<Opinion> Opinions { get; set; }
        public virtual ICollection<Opinion> Opinions1 { get; set; }
        public virtual ICollection<RatingResult> RatingResults { get; set; }
        public virtual ICollection<UserFieldOperation> UserFieldOperations { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
