namespace AIR_SVU_S19.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "File_content", c => c.String());
            AlterColumn("dbo.Files", "File_Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "File_Name", c => c.String());
            DropColumn("dbo.Files", "File_content");
        }
    }
}
