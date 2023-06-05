namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_CartItem",
                c => new
                    {
                        CartItemId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Alias = c.String(),
                        CategoryName = c.String(),
                        ProductImg = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartItemId)
                .ForeignKey("dbo.tb_Cart", t => t.CartId, cascadeDelete: true)
                .Index(t => t.CartId);
            
            CreateTable(
                "dbo.tb_Cart",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.CartId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_CartItem", "CartId", "dbo.tb_Cart");
            DropIndex("dbo.tb_CartItem", new[] { "CartId" });
            DropTable("dbo.tb_Cart");
            DropTable("dbo.tb_CartItem");
        }
    }
}
