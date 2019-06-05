using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class ChangedDoctorModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "Experience", c => c.Int(nullable: false));
            AddColumn("dbo.Doctors", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "Rating");
            DropColumn("dbo.Doctors", "Experience");
        }
    }
}
