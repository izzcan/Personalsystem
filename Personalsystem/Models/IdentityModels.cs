using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Personalsystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("Admins")]
        public virtual ICollection<Company> AdminForCompanies { get; set; }
        [InverseProperty("Leadership")]
        public virtual ICollection<Company> LeaderForCompanies { get; set; }
        public virtual ICollection<Department> BossForDepartments { get; set; }
        public virtual ICollection<DepartmentGroup> EmployeeForGroups { get; set; }
        public virtual ICollection<Application> Applications { get; set; }

        [NotMapped]
        public virtual ICollection<Company> EmployeeForCompanies
        {
            get
            {
                var inGroups = EmployeeForGroups.Select(q => q.Department.Company);
                var bossFor = BossForDepartments.Select(q => q.Company);
                return AdminForCompanies.Union(LeaderForCompanies).Union(inGroups).Union(bossFor).ToList();
            }
        }
        [NotMapped]
        public virtual ICollection<Vacancy> ResponsibleBossForVacancies
        {
            get
            {
                return BossForDepartments.SelectMany(q => q.Vacancies).ToList();
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}