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
    
    public partial class Companies
    {
        public Companies()
        {
            this.Departments = new HashSet<Departments>();
            this.NewsItems = new HashSet<NewsItems>();
            this.Vacancies = new HashSet<Vacancies>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Departments> Departments { get; set; }
        public virtual ICollection<NewsItems> NewsItems { get; set; }
        public virtual ICollection<Vacancies> Vacancies { get; set; }
    }
}
