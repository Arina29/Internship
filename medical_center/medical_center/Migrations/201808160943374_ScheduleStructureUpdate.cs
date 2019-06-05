using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class ScheduleStructureUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "WorkStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "WorkEnd", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schedules", "WorkEnd");
            DropColumn("dbo.Schedules", "WorkStart");
        }
    }
}
