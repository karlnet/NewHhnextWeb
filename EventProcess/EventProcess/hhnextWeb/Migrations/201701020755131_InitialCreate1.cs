namespace hhnextWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectUserRoles",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        AppRoleId = c.String(nullable: false, maxLength: 128),
                        AppUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.AppRoleId, t.AppUserId })
                .ForeignKey("dbo.AspNetRoles", t => t.AppRoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.ProjectRoles", t => new { t.ProjectId, t.AppRoleId })
                .Index(t => t.ProjectId)
                .Index(t => new { t.ProjectId, t.AppRoleId })
                .Index(t => t.AppRoleId)
                .Index(t => t.AppUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectUserRoles", new[] { "ProjectId", "AppRoleId" }, "dbo.ProjectRoles");
            DropForeignKey("dbo.ProjectUserRoles", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectUserRoles", "AppUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUserRoles", "AppRoleId", "dbo.AspNetRoles");
            DropIndex("dbo.ProjectUserRoles", new[] { "AppUserId" });
            DropIndex("dbo.ProjectUserRoles", new[] { "AppRoleId" });
            DropIndex("dbo.ProjectUserRoles", new[] { "ProjectId", "AppRoleId" });
            DropIndex("dbo.ProjectUserRoles", new[] { "ProjectId" });
            DropTable("dbo.ProjectUserRoles");
        }
    }
}
