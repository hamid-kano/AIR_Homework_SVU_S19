namespace AIR_SVU_S19.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boolean_doc_term : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderTerms_DocsBoolean",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Term = c.String(maxLength: 100),
                        Docs = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderTerms_DocsBoolean");
        }
    }
}
