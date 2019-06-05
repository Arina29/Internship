using System.Data.Entity.Migrations;

namespace MED.Presentation.Migrations
{
    public partial class AddReviewCreateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DoctorReviews", "CreateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DoctorReviews", "CreateTime");
        }
    }
}
