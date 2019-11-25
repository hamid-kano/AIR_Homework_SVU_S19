namespace AIR_SVU_S19.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Term_Document", "Freg_Term_in_docs", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Term_Document", "Freg_Term_in_docs");
        }
    }
}
