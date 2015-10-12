namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Interview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interviews", "InterviewDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interviews", "InterviewDate");
        }
    }
}
