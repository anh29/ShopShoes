namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addaccumulatedPointsforuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "accumulatedPoints", c => c.Single(nullable: false));
            DropColumn("dbo.AspNetUsers", "Phone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
            DropColumn("dbo.AspNetUsers", "accumulatedPoints");
        }
    }
}
