namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class translationsforseasonnames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Seasons", "Translation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Seasons", "Translation");
        }
    }
}
