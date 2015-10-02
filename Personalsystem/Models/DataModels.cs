using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Personalsystem.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<NewsItem> NewsItems { get; set; }

        //public virtual ICollection<CompanyUserRole> CompanyUserRoles { get; set; }

        public virtual ICollection<ApplicationUser> Admins { get; set; }

        [NotMapped]
        public virtual ICollection<ApplicationUser> Bosses { 
        get { 
            var bosses = new List<ApplicationUser>();
            foreach (var department in Departments)
            {
                bosses.Concat(department.Bosses);
            }
            return bosses;
        }}
    }
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DepartmentGroup> Groups { get; set; }
        public virtual ICollection<ApplicationUser> Bosses { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
    public class DepartmentGroup
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<ApplicationUser> Employees { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime{ get; set; }
        //public virtual ICollection<ApplicationUser> Users { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("Group")]
        public int? GroupId { get; set; }
        public virtual DepartmentGroup Group { get; set; }

        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }

    public class ScheduleItem
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
    }

    public class NewsItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("Creator")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
    }
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("Creator")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<Application> Applications { get; set; } 
    }
    public class Application
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        [ForeignKey("Applicant")]
        public string ApplicantId { get; set; }
        public virtual ApplicationUser Applicant { get; set; }
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
    }
    public class Interview
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        [ForeignKey("Applicant")]
        public string ApplicantId { get; set; }
        public virtual ApplicationUser Applicant { get; set; }
        [ForeignKey("Interviewer")]
        public string InterviewerId { get; set; }
        public virtual ApplicationUser Interviewer { get; set; }
    }

    //public class CompanyUserRole
    //{
    //    [Key]
    //    [Column(Order = 1)]
    //    [ForeignKey("Company")]
    //    public int CompanyId { get; set; }
    //    public virtual Company Company { get; set; }
    //    [Key]
    //    [Column(Order = 2)]
    //    [ForeignKey("User")]
    //    public string UserId { get; set; }
    //    public virtual ApplicationUser User { get; set; }
    //    [Key]
    //    [Column(Order = 3)]
    //    [ForeignKey("Role")]
    //    public string RoleId { get; set; }
    //    public virtual IdentityRole Role { get; set; }
    //}
}