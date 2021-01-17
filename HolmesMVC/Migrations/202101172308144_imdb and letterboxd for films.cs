namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imdbandletterboxdforfilms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Adaptations", "Imdb", c => c.String());
            AddColumn("dbo.Adaptations", "Letterboxd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Adaptations", "Letterboxd");
            DropColumn("dbo.Adaptations", "Imdb");
        }
    }
}
