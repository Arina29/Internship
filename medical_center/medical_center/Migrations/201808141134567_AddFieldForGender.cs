using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class AddFieldForGender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "Gender", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "Gender");
        }
    }
}
