namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CvPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "CvPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "CvPath");
        }
    }
}
