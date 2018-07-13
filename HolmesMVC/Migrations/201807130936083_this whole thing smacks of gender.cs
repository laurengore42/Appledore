namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thiswholethingsmacksofgender : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Actors", "Gender");
            DropColumn("dbo.Characters", "Gender");
            DropTable("dbo.Genders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Characters", "Gender", c => c.Int());
            AddColumn("dbo.Actors", "Gender", c => c.Int());
        }
    }
}
