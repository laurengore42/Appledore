namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolmesLinkandHolmesLinkAppearancetableadd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HolmesLinkAppearances",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Actor = c.Int(nullable: false),
                        HolmesLink = c.Int(nullable: false),
                        Actor1_ID = c.Int(),
                        HolmesLink1_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Actors", t => t.Actor1_ID)
                .ForeignKey("dbo.HolmesLinks", t => t.HolmesLink1_ID)
                .Index(t => t.Actor1_ID)
                .Index(t => t.HolmesLink1_ID);
            
            CreateTable(
                "dbo.HolmesLinks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Airdate = c.DateTime(nullable: false),
                        Title = c.String(),
                        AirdatePrecision = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.HolmesLinkAppearances", new[] { "HolmesLink1_ID" });
            DropIndex("dbo.HolmesLinkAppearances", new[] { "Actor1_ID" });
            DropForeignKey("dbo.HolmesLinkAppearances", "HolmesLink1_ID", "dbo.HolmesLinks");
            DropForeignKey("dbo.HolmesLinkAppearances", "Actor1_ID", "dbo.Actors");
            DropTable("dbo.HolmesLinks");
            DropTable("dbo.HolmesLinkAppearances");
        }
    }
}
