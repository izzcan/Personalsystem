using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personalsystem.Models
{
    public class CompanyProfileViewmodel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<DepartmentListitemViewmodel> Departments { get; set; }
        public ICollection<NewsListitemViewmodel> NewsItems { get; set; }

        public CompanyProfileViewmodel()
        {
            Departments = new List<DepartmentListitemViewmodel>();
            NewsItems = new List<NewsListitemViewmodel>();
        }

        public CompanyProfileViewmodel(Company that)
        {
            this.Id = that.Id;
            this.Name = that.Name;
            this.Departments = that.Departments.Select(q => new DepartmentListitemViewmodel() { Id = q.Id, Name = q.Name }).ToList();
            this.NewsItems = that.NewsItems.Select(q => new NewsListitemViewmodel() { Id = q.Id, Title = q.Title, Content = q.Content, Created = q.Created }).ToList();
        }
    }

    public class DepartmentListitemViewmodel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class NewsListitemViewmodel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }


    public class CompanyUsersViewmodel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserListitemViewmodel> Admins { get; set; }
        public ICollection<CompanyDepartmentListitemViewmodel> Departments { get; set; }

        public CompanyUsersViewmodel()
        {
            Admins = new List<UserListitemViewmodel>();
            Departments = new List<CompanyDepartmentListitemViewmodel>();
        }

        public CompanyUsersViewmodel(Company that)
        {
            this.Id = that.Id;
            this.Name = that.Name;
            this.Admins = that.Admins.Select(q => new UserListitemViewmodel() { Id = q.Id, Name = q.UserName }).ToList();

            this.Departments = that.Departments.Select(q => new CompanyDepartmentListitemViewmodel(q)).ToList();
        }
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