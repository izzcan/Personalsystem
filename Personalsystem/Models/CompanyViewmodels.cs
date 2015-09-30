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
}