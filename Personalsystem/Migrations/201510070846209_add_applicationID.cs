namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_applicationID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Interviews", "Application_Id", "dbo.Applications");
            DropIndex("dbo.Interviews", new[] { "Application_Id" });
            AlterColumn("dbo.Interviews", "Application_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Interviews", "Application_Id");
            AddForeignKey("dbo.Interviews", "Application_Id", "dbo.Applications", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interviews", "Application_Id", "dbo.Applications");
            DropIndex("dbo.Interviews", new[] { "Application_Id" });
            AlterColumn("dbo.Interviews", "Application_Id", c => c.Int());
            CreateIndex("dbo.Interviews", "Application_Id");
            AddForeignKey("dbo.Interviews", "Application_Id", "dbo.Applications", "Id");
        }
    }
}
