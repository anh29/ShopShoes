namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Editcart1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Cart",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tb_CartItem", "CartId", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_CartItem", "ProductId");
            CreateIndex("dbo.tb_CartItem", "CartId");
            AddForeignKey("dbo.tb_CartItem", "CartId", "dbo.tb_Cart", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tb_CartItem", "ProductId", "dbo.tb_Product", "Id", cascadeDelete: true);
            DropColumn("dbo.tb_CartItem", "UserId");
            DropColumn("dbo.tb_CartItem", "ProductName");
            DropColumn("dbo.tb_CartItem", "Alias");
            DropColumn("dbo.tb_CartItem", "CategoryName");
            DropColumn("dbo.tb_CartItem", "ProductImg");
            DropColumn("dbo.tb_CartItem", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_CartItem", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tb_CartItem", "ProductImg", c => c.String());
            AddColumn("dbo.tb_CartItem", "CategoryName", c => c.String());
            AddColumn("dbo.tb_CartItem", "Alias", c => c.String());
            AddColumn("dbo.tb_CartItem", "ProductName", c => c.String());
            AddColumn("dbo.tb_CartItem", "UserId", c => c.String());
            DropForeignKey("dbo.tb_CartItem", "ProductId", "dbo.tb_Product");
            DropForeignKey("dbo.tb_CartItem", "CartId", "dbo.tb_Cart");
            DropIndex("dbo.tb_CartItem", new[] { "CartId" });
            DropIndex("dbo.tb_CartItem", new[] { "ProductId" });
            DropColumn("dbo.tb_CartItem", "CartId");
            DropTable("dbo.tb_Cart");
        }
    }
}
