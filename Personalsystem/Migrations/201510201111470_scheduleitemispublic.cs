namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scheduleitemispublic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsItems", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewsItems", "IsPublic");
        }
    }
}
