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
    
    public partial class Organization
    {
        public Organization()
        {
            this.Departments = new HashSet<Department>();
        }
    
        public int Organization_Id { get; set; }
        public string Organization_Name { get; set; }
        public Nullable<int> Organization_Superior { get; set; }
        public string Organization_Information { get; set; }
        public string Organization_Address { get; set; }
        public Nullable<int> Organization_PhoneNumber { get; set; }
        public Nullable<int> Organization_Fax { get; set; }
        public string Organization_Email { get; set; }
        public string Organization_Website { get; set; }
        public Nullable<bool> Organization_Active { get; set; }
        public Nullable<System.DateTime> Organization_CreatedOn { get; set; }
        public Nullable<int> Organization_OrganizationTypeId { get; set; }
        public string Organization_FieldOperation { get; set; }
        public Nullable<int> Organization_DirectlyUnder { get; set; }
        public Nullable<int> Organization_Specialized { get; set; }
        public Nullable<int> Organization_Level { get; set; }
    
        public virtual ICollection<Department> Departments { get; set; }
        public virtual OrganizationType OrganizationType { get; set; }
    }
}
