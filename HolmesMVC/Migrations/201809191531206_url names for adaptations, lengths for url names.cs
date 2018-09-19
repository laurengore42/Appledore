namespace HolmesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class urlnamesforadaptationslengthsforurlnames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Adaptations", "UrlName", c => c.String(maxLength: 150));
            AlterColumn("dbo.Actors", "UrlName", c => c.String(maxLength: 150));
            AlterColumn("dbo.Characters", "UrlName", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Characters", "UrlName", c => c.String());
            AlterColumn("dbo.Actors", "UrlName", c => c.String());
            DropColumn("dbo.Adaptations", "UrlName");
        }
    }
}
