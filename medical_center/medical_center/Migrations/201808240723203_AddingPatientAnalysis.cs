using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class AddingPatientAnalysis : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientAnalysis",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        File = c.Binary(),
                        PatientId = c.Guid(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        DeleteTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientAnalysis", "PatientId", "dbo.Patients");
            DropIndex("dbo.PatientAnalysis", new[] { "PatientId" });
            DropTable("dbo.PatientAnalysis");
        }
    }
}
