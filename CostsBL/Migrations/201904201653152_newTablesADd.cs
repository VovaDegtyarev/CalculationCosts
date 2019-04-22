namespace CostsBL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTablesADd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterilaCost = c.Double(),
                        SemiFinishedProducts = c.Double(),
                        FuelAndEnergyCosts = c.Double(),
                        Depreciation = c.Double(),
                        Insurance = c.Double(),
                        Transport = c.Double(),
                        Sales = c.Double(),
                        Other = c.Double(),
                        idLink = c.Int(),
                        Products_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Products_id)
                .Index(t => t.Products_id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.EmployeeCosts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Salary = c.Double(),
                        Training = c.Double(),
                        idLink = c.Int(),
                        Products_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Products", t => t.Products_id)
                .Index(t => t.Products_id);
            
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeCosts", "Products_id", "dbo.Products");
            DropForeignKey("dbo.CostPrices", "Products_id", "dbo.Products");
            DropIndex("dbo.EmployeeCosts", new[] { "Products_id" });
            DropIndex("dbo.CostPrices", new[] { "Products_id" });
            DropTable("dbo.Registrations");
            DropTable("dbo.EmployeeCosts");
            DropTable("dbo.Products");
            DropTable("dbo.CostPrices");
        }
    }
}
