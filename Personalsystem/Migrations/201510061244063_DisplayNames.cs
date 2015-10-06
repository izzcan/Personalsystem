namespace Personalsystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisplayNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vacancies", "Expired", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vacancies", "Expired");
        }
    }
}
