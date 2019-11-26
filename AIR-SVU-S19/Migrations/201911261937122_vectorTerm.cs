namespace AIR_SVU_S19.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vectorTerm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderTerms_DocsBoolean", "VectorTerm", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderTerms_DocsBoolean", "VectorTerm");
        }
    }
}
