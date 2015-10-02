namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class leadership : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyApplicationUser1",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyApplicationUser1", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyApplicationUser1", "Company_Id", "dbo.Companies");
            DropIndex("dbo.CompanyApplicationUser1", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CompanyApplicationUser1", new[] { "Company_Id" });
            DropTable("dbo.CompanyApplicationUser1");
        }
    }
}
