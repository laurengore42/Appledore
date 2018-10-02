namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mediumisanenumnow : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Adaptations", "Medium", "dbo.Media");
            DropIndex("dbo.Adaptations", new[] { "Medium" });
            DropTable("dbo.Media");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Adaptations", "Medium");
            AddForeignKey("dbo.Adaptations", "Medium", "dbo.Media", "ID", cascadeDelete: true);
        }
    }
}
