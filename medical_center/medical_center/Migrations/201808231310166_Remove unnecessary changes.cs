using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class Removeunnecessarychanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DoctorProcedures", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorProcedures", "ProcedureId", "dbo.Procedures");
            DropIndex("dbo.DoctorProcedures", new[] { "DoctorId" });
            DropIndex("dbo.DoctorProcedures", new[] { "ProcedureId" });
            AddColumn("dbo.Doctors", "ConsultationPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.DoctorProcedures");
            DropTable("dbo.Procedures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Procedures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreateTime = c.DateTime(nullable: false),
                        DeleteTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoctorProcedures",
                c => new
                    {
                        DoctorId = c.Guid(nullable: false),
                        ProcedureId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DoctorId, t.ProcedureId });
            
            DropColumn("dbo.Doctors", "ConsultationPrice");
            CreateIndex("dbo.DoctorProcedures", "ProcedureId");
            CreateIndex("dbo.DoctorProcedures", "DoctorId");
            AddForeignKey("dbo.DoctorProcedures", "ProcedureId", "dbo.Procedures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DoctorProcedures", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
        }
    }
}
