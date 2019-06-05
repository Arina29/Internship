using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class ChangedSomeModels : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appointments", "CreateTime");
            DropColumn("dbo.Appointments", "DeleteTime");
            DropColumn("dbo.Appointments", "IsDeleted");
            DropColumn("dbo.Images", "CreateTime");
            DropColumn("dbo.Images", "DeleteTime");
            DropColumn("dbo.Images", "IsDeleted");
            DropColumn("dbo.Schedules", "CreateTime");
            DropColumn("dbo.Schedules", "DeleteTime");
            DropColumn("dbo.Schedules", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Schedules", "DeleteTime", c => c.DateTime());
            AddColumn("dbo.Schedules", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Images", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Images", "DeleteTime", c => c.DateTime());
            AddColumn("dbo.Images", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Appointments", "DeleteTime", c => c.DateTime());
            AddColumn("dbo.Appointments", "CreateTime", c => c.DateTime(nullable: false));
        }
    }
}