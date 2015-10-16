using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personalsystem.Models
{
    public class CompanyDetailsViewmodel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserListitemViewmodel> Admins { get; set; }
        public ICollection<UserListitemViewmodel> Leadership { get; set; }
        public ICollection<CompanyDepartmentListitemViewmodel> Departments { get; set; }
        public ICollection<NewsListitemViewmodel> NewsItems { get; set; }

        public CompanyDetailsViewmodel() { }

        public CompanyDetailsViewmodel(Company that)
        {
            this.Id = that.Id;
            this.Name = that.Name;
            this.Admins = that.Admins.Select(q => new UserListitemViewmodel() { Id = q.Id, Name = q.UserName }).ToList();
            this.Leadership = that.Leadership.Select(q => new UserListitemViewmodel() { Id = q.Id, Name = q.UserName }).ToList();

            this.Departments = that.Departments.Select(q => new CompanyDepartmentListitemViewmodel(q)).ToList();
            this.NewsItems = that.NewsItems.OrderByDescending(q => q.Created).Take(3).Select(q => new NewsListitemViewmodel() { Id = q.Id, Title = q.Title, Content = q.Content, Created = q.Created, CreatorName = q.Creator.UserName }).ToList();
        }
    }

    public class NewsListitemViewmodel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string CreatorName { get; set; }
    }

    public class CompanyDepartmentListitemViewmodel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserListitemViewmodel> Bosses { get; set; }
        public ICollection<CompanyDepartmentGroupListitemViewmodel> Groups { get; set; }

        public CompanyDepartmentListitemViewmodel(){}

        public CompanyDepartmentListitemViewmodel(Department that)
        {
            this.Id = that.Id;
            this.Name = that.Name;
            this.Bosses = that.Bosses.Select(q => new UserListitemViewmodel() { Id = q.Id, Name = q.UserName }).ToList();

            this.Groups = that.Groups.Select(q => new CompanyDepartmentGroupListitemViewmodel(q)).ToList();
        }
    }
    public class CompanyDepartmentGroupListitemViewmodel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserListitemViewmodel> Employees { get; set; }

        public CompanyDepartmentGroupListitemViewmodel(){}

        public CompanyDepartmentGroupListitemViewmodel(DepartmentGroup that)
        {
            this.Id = that.Id;
            this.Name = that.Name;
            this.Employees = that.Employees.Select(q => new UserListitemViewmodel() { Id = q.Id, Name = q.UserName }).ToList();

        }
    }

    public class UserListitemViewmodel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }



}