namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scheduleitems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Schedules", "GroupId", "dbo.DepartmentGroups");
            DropIndex("dbo.AspNetUsers", new[] { "Schedule_Id" });
            DropIndex("dbo.Schedules", new[] { "GroupId" });
            CreateTable(
                "dbo.ScheduleItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ScheduleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId, cascadeDelete: true)
                .Index(t => t.ScheduleId);
            
            AlterColumn("dbo.Schedules", "EndTime", c => c.DateTime());
            AlterColumn("dbo.Schedules", "GroupId", c => c.Int());
            CreateIndex("dbo.Schedules", "GroupId");
            AddForeignKey("dbo.Schedules", "GroupId", "dbo.DepartmentGroups", "Id");
            DropColumn("dbo.AspNetUsers", "Schedule_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Schedule_Id", c => c.Int());
            DropForeignKey("dbo.Schedules", "GroupId", "dbo.DepartmentGroups");
            DropForeignKey("dbo.ScheduleItems", "ScheduleId", "dbo.Schedules");
            DropIndex("dbo.ScheduleItems", new[] { "ScheduleId" });
            DropIndex("dbo.Schedules", new[] { "GroupId" });
            AlterColumn("dbo.Schedules", "GroupId", c => c.Int(nullable: false));
            AlterColumn("dbo.Schedules", "EndTime", c => c.DateTime(nullable: false));
            DropTable("dbo.ScheduleItems");
            CreateIndex("dbo.Schedules", "GroupId");
            CreateIndex("dbo.AspNetUsers", "Schedule_Id");
            AddForeignKey("dbo.Schedules", "GroupId", "dbo.DepartmentGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "Schedule_Id", "dbo.Schedules", "Id");
        }
    }
}
