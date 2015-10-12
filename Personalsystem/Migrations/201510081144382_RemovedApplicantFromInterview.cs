namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedApplicantFromInterview : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Interviews", "ApplicantId", "dbo.AspNetUsers");
            //DropIndex("dbo.Interviews", new[] { "ApplicantId" });
            //DropColumn("dbo.Interviews", "ApplicantId");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Interviews", "ApplicantId", c => c.String(maxLength: 128));
            //CreateIndex("dbo.Interviews", "ApplicantId");
            //AddForeignKey("dbo.Interviews", "ApplicantId", "dbo.AspNetUsers", "Id");
        }
    }
}
