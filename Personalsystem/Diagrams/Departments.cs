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
    
    public partial class Departments
    {
        public Departments()
        {
            this.AspNetUsers = new HashSet<AspNetUsers>();
            this.DepartmentGroups = new HashSet<DepartmentGroups>();
            this.Schedules = new HashSet<Schedules>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
    
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual Companies Companies { get; set; }
        public virtual ICollection<DepartmentGroups> DepartmentGroups { get; set; }
        public virtual ICollection<Schedules> Schedules { get; set; }
    }
}
