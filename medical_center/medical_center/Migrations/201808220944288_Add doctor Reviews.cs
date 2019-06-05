using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class AdddoctorReviews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorReviews",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PatientId = c.Guid(nullable: false),
                        DoctorId = c.Guid(nullable: false),
                        Review = c.String(),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            DropColumn("dbo.Doctors", "Review");
            DropColumn("dbo.Doctors", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Doctors", "Review", c => c.String());
            DropForeignKey("dbo.DoctorReviews", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.DoctorReviews", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.DoctorReviews", new[] { "DoctorId" });
            DropIndex("dbo.DoctorReviews", new[] { "PatientId" });
            DropTable("dbo.DoctorReviews");
        }
    }
}
