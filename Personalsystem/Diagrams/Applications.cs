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
    
    public partial class Applications
    {
        public Applications()
        {
            this.Interviews = new HashSet<Interviews>();
        }
    
        public int Id { get; set; }
        public string Content { get; set; }
        public string ApplicantId { get; set; }
        public int VacancyId { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Vacancies Vacancies { get; set; }
        public virtual ICollection<Interviews> Interviews { get; set; }
    }
}
