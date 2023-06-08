namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeaccumulatedPointsforuser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "accumulatedPoints", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "accumulatedPoints", c => c.Single(nullable: false));
        }
    }
}
