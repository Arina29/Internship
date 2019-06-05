using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class AddSchedule : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            DropIndex("dbo.Schedules", new[] { "AppointmentId" });
            CreateTable(
                "dbo.DoctorSchedules",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DoctorId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            AddColumn("dbo.Schedules", "DocScheduleId", c => c.Guid());
            AddColumn("dbo.Schedules", "Day", c => c.String());
            AddColumn("dbo.Schedules", "LunchStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "LunchEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "IsWorking", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Schedules", "DocScheduleId");
            AddForeignKey("dbo.Schedules", "DocScheduleId", "dbo.Schedules", "Id");
            DropColumn("dbo.Schedules", "DoctorId");
            DropColumn("dbo.Schedules", "Date");
            DropColumn("dbo.Schedules", "StartLunch");
            DropColumn("dbo.Schedules", "EndLunch");
            DropColumn("dbo.Schedules", "AppointmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "AppointmentId", c => c.Guid(nullable: false));
            AddColumn("dbo.Schedules", "EndLunch", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "StartLunch", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "DoctorId", c => c.Guid());
            DropForeignKey("dbo.Schedules", "DocScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.DoctorSchedules", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Schedules", new[] { "DocScheduleId" });
            DropIndex("dbo.DoctorSchedules", new[] { "DoctorId" });
            DropColumn("dbo.Schedules", "IsWorking");
            DropColumn("dbo.Schedules", "LunchEnd");
            DropColumn("dbo.Schedules", "LunchStart");
            DropColumn("dbo.Schedules", "Day");
            DropColumn("dbo.Schedules", "DocScheduleId");
            DropTable("dbo.DoctorSchedules");
            CreateIndex("dbo.Schedules", "AppointmentId");
            CreateIndex("dbo.Schedules", "DoctorId");
            AddForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors", "Id");
            AddForeignKey("dbo.Schedules", "AppointmentId", "dbo.Appointments", "Id", cascadeDelete: true);
        }
    }
}
