//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Personalsystem.Diagrams
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            this.Applications = new HashSet<Applications>();
            this.AspNetUserClaims = new HashSet<AspNetUserClaims>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogins>();
            this.Interviews = new HashSet<Interviews>();
            this.Interviews1 = new HashSet<Interviews>();
            this.NewsItems = new HashSet<NewsItems>();
            this.Vacancies = new HashSet<Vacancies>();
            this.AspNetRoles = new HashSet<AspNetRoles>();
        }
    
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public Nullable<int> Schedule_Id { get; set; }
        public Nullable<int> DepartmentGroup_Id { get; set; }
        public Nullable<int> Department_Id { get; set; }
    
        public virtual ICollection<Applications> Applications { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DepartmentGroups DepartmentGroups { get; set; }
        public virtual Departments Departments { get; set; }
        public virtual Schedules Schedules { get; set; }
        public virtual ICollection<Interviews> Interviews { get; set; }
        public virtual ICollection<Interviews> Interviews1 { get; set; }
        public virtual ICollection<NewsItems> NewsItems { get; set; }
        public virtual ICollection<Vacancies> Vacancies { get; set; }
        public virtual ICollection<AspNetRoles> AspNetRoles { get; set; }
    }
}
