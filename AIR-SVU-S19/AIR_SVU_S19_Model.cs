namespace AIR_SVU_S19
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AIR_SVU_S19_Model : DbContext
    {
        // Your context has been configured to use a 'AIR_SVU_S19' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'AIR_SVU_S19.AIR_SVU_S19' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AIR_SVU_S19' 
        // connection string in the application configuration file.
        public AIR_SVU_S19_Model()
            : base("name=AIR_SVU_S19_Model")
        {
        }

        public System.Data.Entity.DbSet<AIR_SVU_S19.Models.Files> Files { get; set; }

        public System.Data.Entity.DbSet<AIR_SVU_S19.Models.Term_Document> Term_Document { get; set; }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}