using hhnextWeb.Data.Entities;
using hhnextWeb.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Web;

namespace hhnextWeb.Data.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
       
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<ProjectUserRole> ProjectUserRoles { get; set; }        
        public DbSet<Device> Devices { get; set; }
        public DbSet<DevicePort> DevicePorts { get; set; }
        public DbSet<DeviceGroup> DeviceGroups { get; set; }
        public DbSet<DevicePortGroup> DevicePortGroups { get; set; }
        public DbSet<RoleDeviceGroup> RoleDeviceGroups { get; set; }
        public DbSet<RoleDevicePortGroup> RoleDevicePortGroups { get; set; }

        public AppDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>("DefaultConnection"));

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 禁用一对多级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // 禁用多对多级联删除
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<AppRole>().ToTable("AppRoles");
            //modelBuilder.Entity<AppUserRole>().ToTable("AppUserRoles");
            //modelBuilder.Entity<AppUserLogin>().ToTable("AppUserLogins");
            //modelBuilder.Entity<AppUserClaim>().ToTable("AppUserClaims");

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }


    }
}