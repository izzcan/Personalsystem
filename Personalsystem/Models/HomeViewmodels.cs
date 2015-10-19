using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personalsystem.Models
{
    public class HomeIndexViewmodel
    {
        public ICollection<HomeIndexCompany> Companies {get;set;}
        public ICollection<HomeIndexApplication> Applications {get;set;} //Responses / Interviews
    }

    public class HomeIndexCompany
    {
        public string Name {get;set;}
        public string Roles {get;set;} //"Admin", "Boss for HR", "Worker in HR :: HR1", "Leader" etc
        public ICollection<HomeIndexNewsItem> NewsItems {get;set;} //3 latest newsitems per company

        public HomeIndexCompany(){}
        public HomeIndexCompany(Company that, ApplicationUser currentUser)
        {
            this.Name = that.Name;

            var roles = new List<string>();
            if (that.Leadership.Contains(currentUser))
                roles.Add("Leader");
            if (that.Admins.Contains(currentUser))
                roles.Add("Admin");
            roles.AddRange(currentUser.BossForDepartments.Where(q => q.Company == that).Select(q => "Boss for " + q.Name).ToList());
            roles.AddRange(currentUser.EmployeeForGroups.Where(q => q.Department.Company == that).Select(q => "Worker in " + q.Department.Name + ":" + q.Name).ToList());
            Roles += string.Join(", ", roles);
           
        }
    }

    //For unemployed
    public class HomeIndexApplication
    {
    
    }

    //For boss
    public class HomeIndexVacancy
    {
    
    }

    //3 latest newsitems per company
    public class HomeIndexNewsItem
    {
    
    }
}