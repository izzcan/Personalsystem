namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScheduleDayOfWeeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduleDayOfWeekScheduleItems",
                c => new
                    {
                        ScheduleDayOfWeek_Id = c.Int(nullable: false),
                        ScheduleItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ScheduleDayOfWeek_Id, t.ScheduleItem_Id })
                .ForeignKey("dbo.ScheduleDayOfWeeks", t => t.ScheduleDayOfWeek_Id, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleItems", t => t.ScheduleItem_Id, cascadeDelete: true)
                .Index(t => t.ScheduleDayOfWeek_Id)
                .Index(t => t.ScheduleItem_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleDayOfWeekScheduleItems", "ScheduleItem_Id", "dbo.ScheduleItems");
            DropForeignKey("dbo.ScheduleDayOfWeekScheduleItems", "ScheduleDayOfWeek_Id", "dbo.ScheduleDayOfWeeks");
            DropIndex("dbo.ScheduleDayOfWeekScheduleItems", new[] { "ScheduleItem_Id" });
            DropIndex("dbo.ScheduleDayOfWeekScheduleItems", new[] { "ScheduleDayOfWeek_Id" });
            DropTable("dbo.ScheduleDayOfWeekScheduleItems");
            DropTable("dbo.ScheduleDayOfWeeks");
        }
    }
}
