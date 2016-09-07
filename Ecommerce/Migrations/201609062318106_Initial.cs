namespace Ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departaments",
                c => new
                    {
                        DepartamentID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DepartamentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Departaments");
        }
    }
}
