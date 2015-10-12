namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleItem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScheduleItems", "StartTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.ScheduleItems", "EndTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ScheduleItems", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ScheduleItems", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
