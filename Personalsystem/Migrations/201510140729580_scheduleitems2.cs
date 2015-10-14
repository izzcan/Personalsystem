namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scheduleitems2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ScheduleDayOfWeekScheduleItems", newName: "ScheduleItemScheduleDayOfWeeks");
            DropPrimaryKey("dbo.ScheduleItemScheduleDayOfWeeks");
            AddPrimaryKey("dbo.ScheduleItemScheduleDayOfWeeks", new[] { "ScheduleItem_Id", "ScheduleDayOfWeek_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ScheduleItemScheduleDayOfWeeks");
            AddPrimaryKey("dbo.ScheduleItemScheduleDayOfWeeks", new[] { "ScheduleDayOfWeek_Id", "ScheduleItem_Id" });
            RenameTable(name: "dbo.ScheduleItemScheduleDayOfWeeks", newName: "ScheduleDayOfWeekScheduleItems");
        }
    }
}
