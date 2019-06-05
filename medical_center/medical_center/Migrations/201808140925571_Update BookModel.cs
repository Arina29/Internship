using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class UpdateBookModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "DiseaseType", c => c.Byte(nullable: false));
            AddColumn("dbo.Books", "Symptoms", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Symptoms");
            DropColumn("dbo.Books", "DiseaseType");
        }
    }
}
