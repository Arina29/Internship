using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class updatedBookModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "ProcedureId", "dbo.Procedures");
            DropIndex("dbo.Books", new[] { "ProcedureId" });
            AddColumn("dbo.Books", "DoctorId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Books", "DoctorId");
            AddForeignKey("dbo.Books", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
            DropColumn("dbo.Books", "ProcedureId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "ProcedureId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Books", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Books", new[] { "DoctorId" });
            DropColumn("dbo.Books", "DoctorId");
            CreateIndex("dbo.Books", "ProcedureId");
            AddForeignKey("dbo.Books", "ProcedureId", "dbo.Procedures", "Id", cascadeDelete: true);
        }
    }
}
