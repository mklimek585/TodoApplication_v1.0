namespace TodoApplication_v1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Priority = c.Int(nullable: false),
                        DayID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Days", t => t.DayID, cascadeDelete: true)
                .Index(t => t.DayID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "DayID", "dbo.Days");
            DropIndex("dbo.Tasks", new[] { "DayID" });
            DropTable("dbo.Tasks");
            DropTable("dbo.Days");
        }
    }
}
