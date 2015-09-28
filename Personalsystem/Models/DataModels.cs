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
    }
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DepartmentGroup> Groups { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
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
        public virtual ICollection<ApplicationUser> Users { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime{ get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public virtual DepartmentGroup Group { get; set; }
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
        public Guid CreatorId { get; set; }
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
        public Guid CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<Application> Applications { get; set; } 
    }
    public class Application
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        [ForeignKey("Applicant")]
        public Guid ApplicantId { get; set; }
        public virtual ApplicationUser Applicant { get; set; }
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        public virtual Vacancy Vacancies { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
    }
    public class Interview
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        [ForeignKey("Applicant")]
        public Guid ApplicantId { get; set; }
        public virtual ApplicationUser Applicant { get; set; }
        [ForeignKey("Interviewer")]
        public Guid InterviewerId { get; set; }
        public virtual ApplicationUser Interviewer { get; set; }
    }
}