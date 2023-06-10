namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editcart : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tb_CartItem", "ProductId");
            AddForeignKey("dbo.tb_CartItem", "ProductId", "dbo.tb_Product", "Id", cascadeDelete: true);
            DropColumn("dbo.tb_CartItem", "ProductName");
            DropColumn("dbo.tb_CartItem", "Alias");
            DropColumn("dbo.tb_CartItem", "CategoryName");
            DropColumn("dbo.tb_CartItem", "ProductImg");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_CartItem", "ProductImg", c => c.String());
            AddColumn("dbo.tb_CartItem", "CategoryName", c => c.String());
            AddColumn("dbo.tb_CartItem", "Alias", c => c.String());
            AddColumn("dbo.tb_CartItem", "ProductName", c => c.String());
            DropForeignKey("dbo.tb_CartItem", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_CartItem", new[] { "ProductId" });
        }
    }
}
