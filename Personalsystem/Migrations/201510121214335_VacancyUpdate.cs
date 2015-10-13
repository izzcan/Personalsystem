namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VacancyUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vacancies", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Vacancies", new[] { "CompanyId" });
            AddColumn("dbo.Vacancies", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vacancies", "DepartmentId");
            AddForeignKey("dbo.Vacancies", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
            DropColumn("dbo.Vacancies", "CompanyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacancies", "CompanyId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Vacancies", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Vacancies", new[] { "DepartmentId" });
            DropColumn("dbo.Vacancies", "DepartmentId");
            CreateIndex("dbo.Vacancies", "CompanyId");
            AddForeignKey("dbo.Vacancies", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
