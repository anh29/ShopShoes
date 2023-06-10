namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteunuseproperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tb_Product", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Product", "Image", c => c.String(maxLength: 250));
        }
    }
}
