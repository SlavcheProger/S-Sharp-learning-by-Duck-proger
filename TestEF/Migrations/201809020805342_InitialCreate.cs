namespace TestEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Model = c.String(),
                        Color = c.String(),
                        Speed = c.Int(nullable: false),
                        FuelConsum = c.Double(nullable: false),
                        CostOfMaintain = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Planes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AviaComp = c.String(),
                        AmountOfTurb = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                        FuelConsum = c.Double(nullable: false),
                        CostOfMaintain = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Planes");
            DropTable("dbo.Cars");
        }
    }
}
