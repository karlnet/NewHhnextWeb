namespace hhnextWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceGroups",
                c => new
                    {
                        DeviceGroupId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        DeviceGroupName = c.String(),
                    })
                .PrimaryKey(t => t.DeviceGroupId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        GatewayId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        DeviceNo = c.String(nullable: false, maxLength: 8, fixedLength: true, unicode: false),
                        DeviceName = c.String(nullable: false),
                        DeviceGroupId = c.Int(nullable: false),
                        DeviceType = c.String(),
                        MAC = c.String(),
                        ROMVersion = c.String(),
                        PrivateIP = c.String(),
                        PublicIP = c.String(),
                        SSID = c.String(),
                        BSSID = c.String(),
                        Token = c.String(),
                        Config = c.String(),
                        Address = c.String(),
                        Manufacturer = c.String(),
                        Brand = c.String(),
                        Model = c.String(),
                        Status = c.String(),
                        Comments = c.String(),
                        Offtime = c.DateTime(),
                        Onlinetime = c.DateTime(),
                        Createtime = c.DateTime(),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.DeviceGroups", t => t.DeviceGroupId)
                .ForeignKey("dbo.Devices", t => t.GatewayId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.GatewayId)
                .Index(t => t.DriverId)
                .Index(t => t.ProjectId)
                .Index(t => t.DeviceNo, unique: true, name: "IX_Devices_DeviceNo")
                .Index(t => t.DeviceGroupId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverId = c.Int(nullable: false, identity: true),
                        DriverName = c.String(nullable: false),
                        Location = c.String(),
                        ClassType = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.DriverId);
            
            CreateTable(
                "dbo.DevicePorts",
                c => new
                    {
                        PortId = c.Int(nullable: false, identity: true),
                        DevicePortGroupId = c.Int(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        GatewayId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        PortNo = c.String(nullable: false),
                        PortName = c.String(nullable: false),
                        Alias = c.String(nullable: false),
                        PortType = c.String(),
                        Enable = c.Boolean(nullable: false),
                        DataType = c.String(nullable: false),
                        Uplimit = c.Decimal(precision: 18, scale: 2),
                        Lowlimit = c.Decimal(precision: 18, scale: 2),
                        UpOffset = c.Decimal(precision: 18, scale: 2),
                        LowOffset = c.Decimal(precision: 18, scale: 2),
                        Max = c.Decimal(precision: 18, scale: 2),
                        Min = c.Decimal(precision: 18, scale: 2),
                        DefaultValue = c.String(nullable: false),
                        IP = c.String(),
                        Address = c.String(),
                        Config = c.String(),
                        NetConnectType = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.PortId)
                .ForeignKey("dbo.Devices", t => t.DeviceId)
                .ForeignKey("dbo.DevicePortGroups", t => t.DevicePortGroupId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Devices", t => t.GatewayId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.DevicePortGroupId)
                .Index(t => t.DeviceId)
                .Index(t => t.GatewayId)
                .Index(t => t.DriverId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.DevicePortGroups",
                c => new
                    {
                        DevicePortGroupId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        DevicePortGroupName = c.String(),
                    })
                .PrimaryKey(t => t.DevicePortGroupId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false),
                        AppId = c.String(),
                        AppSecretKey = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectRoles",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        AppRoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.AppRoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.AppRoleId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.AppRoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RoleDeviceGroups",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        AppRoleId = c.String(nullable: false, maxLength: 128),
                        DeviceGroupId = c.Int(nullable: false),
                        read = c.Boolean(nullable: false),
                        write = c.Boolean(nullable: false),
                        exec = c.Boolean(nullable: false),
                        permission = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.AppRoleId, t.DeviceGroupId })
                .ForeignKey("dbo.AspNetRoles", t => t.AppRoleId)
                .ForeignKey("dbo.DeviceGroups", t => t.DeviceGroupId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.ProjectRoles", t => new { t.ProjectId, t.AppRoleId })
                .Index(t => t.ProjectId)
                .Index(t => new { t.ProjectId, t.AppRoleId })
                .Index(t => t.AppRoleId)
                .Index(t => t.DeviceGroupId);
            
            CreateTable(
                "dbo.RoleDevicePortGroups",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        AppRoleId = c.String(nullable: false, maxLength: 128),
                        DevicePortGroupId = c.Int(nullable: false),
                        read = c.Boolean(nullable: false),
                        write = c.Boolean(nullable: false),
                        exec = c.Boolean(nullable: false),
                        permission = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.AppRoleId, t.DevicePortGroupId })
                .ForeignKey("dbo.AspNetRoles", t => t.AppRoleId)
                .ForeignKey("dbo.DevicePortGroups", t => t.DevicePortGroupId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.ProjectRoles", t => new { t.ProjectId, t.AppRoleId })
                .Index(t => t.ProjectId)
                .Index(t => new { t.ProjectId, t.AppRoleId })
                .Index(t => t.AppRoleId)
                .Index(t => t.DevicePortGroupId);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        AppUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.AppUserId })
                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NickName = c.String(),
                        UserType = c.String(),
                        UserToken = c.String(),
                        UserKey = c.String(),
                        Createtime = c.DateTime(nullable: false),
                        Comments = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.DeviceGroups", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Devices", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DevicePorts", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DevicePorts", "GatewayId", "dbo.Devices");
            DropForeignKey("dbo.DevicePortGroups", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectUsers", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUsers", "AppUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RoleDevicePortGroups", new[] { "ProjectId", "AppRoleId" }, "dbo.ProjectRoles");
            DropForeignKey("dbo.RoleDevicePortGroups", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.RoleDevicePortGroups", "DevicePortGroupId", "dbo.DevicePortGroups");
            DropForeignKey("dbo.RoleDevicePortGroups", "AppRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RoleDeviceGroups", new[] { "ProjectId", "AppRoleId" }, "dbo.ProjectRoles");
            DropForeignKey("dbo.RoleDeviceGroups", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.RoleDeviceGroups", "DeviceGroupId", "dbo.DeviceGroups");
            DropForeignKey("dbo.RoleDeviceGroups", "AppRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProjectRoles", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectRoles", "AppRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Devices", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.DevicePorts", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.DevicePorts", "DevicePortGroupId", "dbo.DevicePortGroups");
            DropForeignKey("dbo.DevicePorts", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "GatewayId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "DeviceGroupId", "dbo.DeviceGroups");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ProjectUsers", new[] { "AppUserId" });
            DropIndex("dbo.ProjectUsers", new[] { "ProjectId" });
            DropIndex("dbo.RoleDevicePortGroups", new[] { "DevicePortGroupId" });
            DropIndex("dbo.RoleDevicePortGroups", new[] { "AppRoleId" });
            DropIndex("dbo.RoleDevicePortGroups", new[] { "ProjectId", "AppRoleId" });
            DropIndex("dbo.RoleDevicePortGroups", new[] { "ProjectId" });
            DropIndex("dbo.RoleDeviceGroups", new[] { "DeviceGroupId" });
            DropIndex("dbo.RoleDeviceGroups", new[] { "AppRoleId" });
            DropIndex("dbo.RoleDeviceGroups", new[] { "ProjectId", "AppRoleId" });
            DropIndex("dbo.RoleDeviceGroups", new[] { "ProjectId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProjectRoles", new[] { "AppRoleId" });
            DropIndex("dbo.ProjectRoles", new[] { "ProjectId" });
            DropIndex("dbo.DevicePortGroups", new[] { "ProjectId" });
            DropIndex("dbo.DevicePorts", new[] { "ProjectId" });
            DropIndex("dbo.DevicePorts", new[] { "DriverId" });
            DropIndex("dbo.DevicePorts", new[] { "GatewayId" });
            DropIndex("dbo.DevicePorts", new[] { "DeviceId" });
            DropIndex("dbo.DevicePorts", new[] { "DevicePortGroupId" });
            DropIndex("dbo.Devices", new[] { "DeviceGroupId" });
            DropIndex("dbo.Devices", "IX_Devices_DeviceNo");
            DropIndex("dbo.Devices", new[] { "ProjectId" });
            DropIndex("dbo.Devices", new[] { "DriverId" });
            DropIndex("dbo.Devices", new[] { "GatewayId" });
            DropIndex("dbo.DeviceGroups", new[] { "ProjectId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.RoleDevicePortGroups");
            DropTable("dbo.RoleDeviceGroups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProjectRoles");
            DropTable("dbo.Projects");
            DropTable("dbo.DevicePortGroups");
            DropTable("dbo.DevicePorts");
            DropTable("dbo.Drivers");
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceGroups");
        }
    }
}
