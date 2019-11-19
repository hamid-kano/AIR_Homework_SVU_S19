namespace AIR_SVU_S19.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class term_doc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Term_Document",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Terms = c.String(),
                        Docs = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Term_Document");
        }
    }
}
