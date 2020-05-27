namespace AIR_SVU_S19.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lengthDocs : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderTerms_DocsBoolean", "Docs", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderTerms_DocsBoolean", "Docs", c => c.String(maxLength: 200));
        }
    }
}
