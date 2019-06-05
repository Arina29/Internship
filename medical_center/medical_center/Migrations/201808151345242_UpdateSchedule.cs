using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class UpdateSchedule : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "DocScheduleId", "dbo.Schedules");
            DropIndex("dbo.Schedules", new[] { "DocScheduleId" });
            AddColumn("dbo.Schedules", "DoctorScheduleId", c => c.Guid());
            CreateIndex("dbo.Schedules", "DoctorScheduleId");
            AddForeignKey("dbo.Schedules", "DoctorScheduleId", "dbo.DoctorSchedules", "Id");
            DropColumn("dbo.Schedules", "DocScheduleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "DocScheduleId", c => c.Guid());
            DropForeignKey("dbo.Schedules", "DoctorScheduleId", "dbo.DoctorSchedules");
            DropIndex("dbo.Schedules", new[] { "DoctorScheduleId" });
            DropColumn("dbo.Schedules", "DoctorScheduleId");
            CreateIndex("dbo.Schedules", "DocScheduleId");
            AddForeignKey("dbo.Schedules", "DocScheduleId", "dbo.Schedules", "Id");
        }
    }
}
