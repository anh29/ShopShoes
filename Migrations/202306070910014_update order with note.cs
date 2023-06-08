namespace ShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateorderwithnote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "State", c => c.Int(nullable: false));
            AddColumn("dbo.tb_Order", "Note", c => c.String());
            DropColumn("dbo.tb_Order", "TypePayment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "TypePayment", c => c.Int(nullable: false));
            DropColumn("dbo.tb_Order", "Note");
            DropColumn("dbo.tb_Order", "State");
        }
    }
}
