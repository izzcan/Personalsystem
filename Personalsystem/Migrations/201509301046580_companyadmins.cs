namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyadmins : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyUserRoles", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CompanyUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DepartmentGroup_Id", "dbo.DepartmentGroups");
            DropForeignKey("dbo.AspNetUsers", "Department_Id", "dbo.Departments");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentGroup_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Department_Id" });
            DropIndex("dbo.CompanyUserRoles", new[] { "CompanyId" });
            DropIndex("dbo.CompanyUserRoles", new[] { "UserId" });
            DropIndex("dbo.CompanyUserRoles", new[] { "RoleId" });
            CreateTable(
                "dbo.CompanyApplicationUsers",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Companies", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.DepartmentGroupApplicationUsers",
                c => new
                    {
                        DepartmentGroup_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DepartmentGroup_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.DepartmentGroups", t => t.DepartmentGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.DepartmentGroup_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.DepartmentApplicationUsers",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.ApplicationUser_Id);
            
            DropColumn("dbo.AspNetUsers", "DepartmentGroup_Id");
            DropColumn("dbo.AspNetUsers", "Department_Id");
            DropTable("dbo.CompanyUserRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyUserRoles",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.UserId, t.RoleId });
            
            AddColumn("dbo.AspNetUsers", "Department_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DepartmentGroup_Id", c => c.Int());
            DropForeignKey("dbo.DepartmentApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DepartmentApplicationUsers", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.DepartmentGroupApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DepartmentGroupApplicationUsers", "DepartmentGroup_Id", "dbo.DepartmentGroups");
            DropForeignKey("dbo.CompanyApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyApplicationUsers", "Company_Id", "dbo.Companies");
            DropIndex("dbo.DepartmentApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.DepartmentApplicationUsers", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentGroupApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.DepartmentGroupApplicationUsers", new[] { "DepartmentGroup_Id" });
            DropIndex("dbo.CompanyApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CompanyApplicationUsers", new[] { "Company_Id" });
            DropTable("dbo.DepartmentApplicationUsers");
            DropTable("dbo.DepartmentGroupApplicationUsers");
            DropTable("dbo.CompanyApplicationUsers");
            CreateIndex("dbo.CompanyUserRoles", "RoleId");
            CreateIndex("dbo.CompanyUserRoles", "UserId");
            CreateIndex("dbo.CompanyUserRoles", "CompanyId");
            CreateIndex("dbo.AspNetUsers", "Department_Id");
            CreateIndex("dbo.AspNetUsers", "DepartmentGroup_Id");
            AddForeignKey("dbo.AspNetUsers", "Department_Id", "dbo.Departments", "Id");
            AddForeignKey("dbo.AspNetUsers", "DepartmentGroup_Id", "dbo.DepartmentGroups", "Id");
            AddForeignKey("dbo.CompanyUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyUserRoles", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
