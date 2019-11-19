namespace AIR_SVU_S19.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class table_Files : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        File_ID = c.Int(nullable: false, identity: true),
                        File_Name = c.String(),
                    })
                .PrimaryKey(t => t.File_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Files");
        }
    }
}
