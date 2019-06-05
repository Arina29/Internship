using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class addMessageToAppointment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "ProcedureId", "dbo.Procedures");
            DropIndex("dbo.Appointments", new[] { "ProcedureId" });
            AddColumn("dbo.Appointments", "Message", c => c.String());
            DropColumn("dbo.Appointments", "ProcedureId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "ProcedureId", c => c.Guid(nullable: false));
            DropColumn("dbo.Appointments", "Message");
            CreateIndex("dbo.Appointments", "ProcedureId");
            AddForeignKey("dbo.Appointments", "ProcedureId", "dbo.Procedures", "Id", cascadeDelete: true);
        }
    }
}
