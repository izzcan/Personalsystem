using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personalsystem.Models
{
    public class HomeIndexViewmodel
    {
        public ICollection<HomeIndexCompany> Companies {get;set;}
        public ICollection<HomeIndexApplication> Applications {get;set;}
        public ICollection<HomeIndexVacancy> Vacancies { get; set; }

        public HomeIndexViewmodel() { }
        public HomeIndexViewmodel(ApplicationUser currentUser) 
        {
            Companies = currentUser.EmployeeForCompanies.Select(q => new HomeIndexCompany(q, currentUser)).ToList();
            Applications = currentUser.Applications.Select(q => new HomeIndexApplication(q)).ToList();
            Vacancies = currentUser.ResponsibleBossForVacancies.Select(q => new HomeIndexVacancy(q)).ToList();
        }
    }

    public class HomeIndexCompany
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public string Roles {get;set;} //"Admin", "Boss for HR", "Worker in HR :: HR1", "Leader" etc
        public ICollection<NewsListitemViewmodel> NewsItems { get; set; } //3 latest newsitems per company

        public HomeIndexCompany(){}
        public HomeIndexCompany(Company that, ApplicationUser currentUser)
        {
            this.Id = that.Id;
            this.Name = that.Name;

            var roles = new List<string>();
            if (that.Leadership.Contains(currentUser))
                roles.Add("Leader");
            if (that.Admins.Contains(currentUser))
                roles.Add("Admin");
            roles.AddRange(currentUser.BossForDepartments.Where(q => q.Company == that).Select(q => "Boss for " + q.Name).ToList());
            roles.AddRange(currentUser.EmployeeForGroups.Where(q => q.Department.Company == that).Select(q => "Worker in " + q.Department.Name + ":" + q.Name).ToList());
            Roles += string.Join(", ", roles);

            NewsItems = that.NewsItems.OrderByDescending(q => q.Created).Take(3).Select(q => new NewsListitemViewmodel() { Id = q.Id, Title = q.Title, Content = q.Content, Created = q.Created, CreatorName = q.Creator.UserName }).ToList();
        }
    }

    //For unemployed
    public class HomeIndexApplication
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public int NrOfInterviews { get; set; }

        public HomeIndexApplication() { }
        public HomeIndexApplication(Application that)
        {
            this.Id = that.Id;
            this.CompanyName = that.Vacancy.Department.Company.Name;
            this.DepartmentName = that.Vacancy.Department.Name;
            this.PositionName = that.Vacancy.Title;
            this.NrOfInterviews = that.Interviews.Count();
        }
    }

    //For boss
    public class HomeIndexVacancy
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public int NrOfApplications { get; set; }

        public HomeIndexVacancy() { }
        public HomeIndexVacancy(Vacancy that) 
        {
            this.Id = that.Id;
            this.CompanyName = that.Department.Company.Name;
            this.DepartmentName = that.Department.Name;
            this.PositionName = that.Title;
            this.NrOfApplications = that.Applications.Count();
        }
    }

    ////3 latest newsitems per company
    //public class HomeIndexNewsItem
    //{
    
    //}
}