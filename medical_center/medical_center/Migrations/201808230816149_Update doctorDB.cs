using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class UpdatedoctorDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "Studies", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "Studies");
        }
    }
}
