namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class optiontotranslateepisodesandadaptations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "Translation", c => c.String());
            AddColumn("dbo.Adaptations", "Translation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Adaptations", "Translation");
            DropColumn("dbo.Episodes", "Translation");
        }
    }
}
