namespace hhnextWeb.Migrations
{
    using Data.Entities;
    using Data.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<AppUser>(new UserStore<AppUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //var user = new AppUser()
            //{
            //    UserName = "hhnext",
            //    Email = "hhnext@163.com",
            //    EmailConfirmed = true,
            //    NickName="hhnext",
            //    Createtime = DateTime.Now,
            //    PhoneNumber="13701308059",
            //    UserType="AppOwner"
            //};

            //manager.Create(user, "123456");

            //if (roleManager.Roles.Count() == 0)
            //{
            //    //roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            var adminUser = manager.FindByName("hhnext");
            var adminRole = roleManager.FindByName("Admin");


            ////manager.AddToRoles(adminUser.Id, new string[] {"SuperAdmin", "Admin" });


            //context.Projects.AddOrUpdate(
            //  p => p.ProjectId,
            //  new Project { ProjectName = "EnvironmentSupervisory " }
            //);

            //context.ProjectRoles.AddOrUpdate(new ProjectRole { ProjectId = 1, AppRoleId = adminRole.Id });
            //context.ProjectUsers.AddOrUpdate(new ProjectUser { ProjectId = 1, AppUserId = adminUser.Id });
            context.ProjectUserRoles.AddOrUpdate(new ProjectUserRole { ProjectId = 1, AppUserId = adminUser.Id , AppRoleId = adminRole.Id });

            //context.DeviceGroups.AddOrUpdate(new DeviceGroup { ProjectId = 1, DeviceGroupName="Default" });
            //context.DevicePortGroups.AddOrUpdate(new DevicePortGroup { ProjectId = 1, DevicePortGroupName = "Default" });

            //context.RoleDeviceGroups.AddOrUpdate(new RoleDeviceGroup { ProjectId = 1, DeviceGroupId = 1, AppRoleId = adminRole.Id, read = true, write = true, exec = true, permission = 7 });
            //context.RoleDevicePortGroups.AddOrUpdate(new RoleDevicePortGroup { ProjectId = 1, DevicePortGroupId = 1, AppRoleId = adminRole.Id, read = true, write = true, exec = true, permission = 7 });

            //context.Drivers.AddOrUpdate(p => p.DriverId, new Driver { DriverName = "PASS" });
            //context.Devices.AddOrUpdate(p => p.DeviceId,
            //    new Device
            //    {
            //        ProjectId = 1,
            //        DeviceGroupId = 1,
            //        DeviceName = "NC5000",
            //        DeviceNo = "GNC00001",
            //        DriverId = 1,
            //        GatewayId = 1,
            //        DeviceType = "Gateway",
            //        MAC = "123456789012",
            //        ROMVersion = "1.2.0",
            //        PublicIP = "10.0.0.2",
            //        PrivateIP = "192.168.0.22",
            //        Manufacturer = "NEXTCOM",
            //        Status = "RUN",
            //        Createtime = DateTime.Now
            //    });
            //context.Devices.AddOrUpdate(p => p.DeviceId,
            //   new Device
            //   {
            //       ProjectId = 1,
            //       DeviceGroupId = 1,
            //       DeviceName = "YFSModel",
            //       DeviceNo = "DYFS0001",
            //       DriverId = 1,
            //       GatewayId = 1,
            //       DeviceType = "Sensor",
            //       MAC = "74E50BE31D92",
            //       ROMVersion = "1.1.2",                
            //       Manufacturer = "YFTech",
            //       Status = "RUN",
            //       Createtime = DateTime.Now
            //   });

            //context.DevicePorts.AddOrUpdate(p => p.PortId,
            //   new DevicePort
            //   {
            //       ProjectId = 1,
            //       DevicePortGroupId = 1,                
            //       DriverId = 1,
            //       GatewayId = 1,
            //       DeviceId=3,
            //       PortNo="V1",
            //       PortName="HUMI",
            //       Alias= "HUMI",
            //       PortType="DI",
            //       Enable=true,
            //       DataType="FLOAT",
            //       Max="100.0",
            //       Min="0",
            //       DefaultValue="30",
            //       NetConnectType="RS485",
            //       Address="1",
            //       Config="9600,8N1"

            //   });
            //context.DevicePorts.AddOrUpdate(p => p.PortId,
            // new DevicePort
            // {
            //     ProjectId = 1,
            //     DevicePortGroupId = 1,                
            //     DriverId = 1,
            //     GatewayId = 1,
            //     DeviceId=3,
            //     PortNo="V3",
            //     PortName="PM25",
            //     Alias= "PM25",
            //     PortType="DI",
            //     Enable=true,
            //     DataType="FLOAT",
            //     Max="1000.0",
            //     Min="0",
            //     DefaultValue="100",
            //     NetConnectType="RS485",
            //     Address="1",
            //     Config="9600,8N1"

            // });

        }
    }
}
