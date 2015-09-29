namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyUserRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyUserRoles",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.UserId, t.RoleId })
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CompanyUserRoles", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyUserRoles", new[] { "RoleId" });
            DropIndex("dbo.CompanyUserRoles", new[] { "UserId" });
            DropIndex("dbo.CompanyUserRoles", new[] { "CompanyId" });
            DropTable("dbo.CompanyUserRoles");
        }
    }
}
