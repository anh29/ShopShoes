namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editcartitem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_CartItem", "CartId", "dbo.tb_Cart");
            DropForeignKey("dbo.tb_CartItem", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_CartItem", new[] { "ProductId" });
            DropIndex("dbo.tb_CartItem", new[] { "CartId" });
            CreateTable(
                "dbo.tb_ShoppingCart",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ProductId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Alias = c.String(),
                        CategoryName = c.String(),
                        ProductImg = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropTable("dbo.tb_CartItem");
            DropTable("dbo.tb_Cart");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tb_Cart",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.CartId);
            
            CreateTable(
                "dbo.tb_CartItem",
                c => new
                    {
                        CartItemId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductId = c.Int(nullable: false),
                        CartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartItemId);
            
            DropTable("dbo.tb_ShoppingCart");
            CreateIndex("dbo.tb_CartItem", "CartId");
            CreateIndex("dbo.tb_CartItem", "ProductId");
            AddForeignKey("dbo.tb_CartItem", "ProductId", "dbo.tb_Product", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tb_CartItem", "CartId", "dbo.tb_Cart", "CartId", cascadeDelete: true);
        }
    }
}
