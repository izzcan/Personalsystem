﻿using Microsoft.AspNet.Identity.EntityFramework;
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
        [Display(Name = "Company")]
        public string Name { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<NewsItem> NewsItems { get; set; }

        [InverseProperty("AdminForCompanies")]
        public virtual ICollection<ApplicationUser> Admins { get; set; }
        [InverseProperty("LeaderForCompanies")]
        public virtual ICollection<ApplicationUser> Leadership { get; set; }

        [NotMapped]
        public virtual ICollection<ApplicationUser> Bosses
        {
            get
            {
                return Departments.SelectMany(q => q.Bosses).ToList();
            }
        }
        [NotMapped]
        public virtual ICollection<ApplicationUser> Employees
        {
            get
            {
                return Departments.SelectMany(q => q.Groups).SelectMany(q => q.Employees).Union(Bosses).Union(Admins).Union(Leadership).ToList();
            }
        }
    }

    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Department")]
        public string Name { get; set; }
        public virtual ICollection<DepartmentGroup> Groups { get; set; }
        public virtual ICollection<ApplicationUser> Bosses { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [NotMapped]
        public virtual ICollection<ApplicationUser> Employees //Does not include the bosses for the department
        {
            get {
                return Groups.SelectMany(q => q.Employees).ToList();
            }
        }
    }

    public class DepartmentGroup
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Group")]
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
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }

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
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }


        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        [InverseProperty("ScheduleItems")]
        public virtual ICollection<ScheduleDayOfWeek> WeekDays { get; set; }

        public ScheduleItem() { }

        public ScheduleItem(ScheduleItemEditViewmodel that)
        {
            this.Id = that.Id;
            this.StartTime = that.StartTime;
            this.EndTime = that.EndTime;
            this.Description = that.Description;
            this.ScheduleId = that.ScheduleId;
            this.WeekDays = new List<ScheduleDayOfWeek>();
        }
    }

    //Seeded Monday-Sunday
    public class ScheduleDayOfWeek
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        [InverseProperty("WeekDays")]
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }

    public class NewsItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public bool IsPublic { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("Creator")]
        [Display(Name = "Contact")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
    }

    public class Vacancy
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Position")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Content { get; set; }
        [Display(Name = "Published")]
        public DateTime Created { get; set; }
        [Display(Name = "Last Application Date")]
        [DataType(DataType.Date)]
        public DateTime Expired { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        [ForeignKey("Creator")]
        [Display(Name = "Contact")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }

    public class Application
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Personal Letter")]
        public string Content { get; set; }
        [ForeignKey("Applicant")]
        [Display(Name = "Applicant")]
        public string ApplicantId { get; set; }
        public virtual ApplicationUser Applicant { get; set; }
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
        public string CvPath { get; set; }
    }

    public class Interview
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Interview Date")]        
        public DateTime InterviewDate { get; set; } //Sätts
        [Display(Name = "Notes")]
        public string Description { get; set; } //Sätts
        [ForeignKey("Interviewer")]
        public string InterviewerId { get; set; } //Sätts automatiskt, inloggningsid
        public virtual ApplicationUser Interviewer { get; set; }
        public int Application_Id { get; set; }
        [ForeignKey("Application_Id")]
        public virtual Application Applications { get; set; }
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