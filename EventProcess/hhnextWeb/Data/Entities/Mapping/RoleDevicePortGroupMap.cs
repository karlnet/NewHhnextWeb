using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class RoleDevicePortGroupMap : EntityTypeConfiguration<RoleDevicePortGroup>
    {
        public RoleDevicePortGroupMap()
        {


            // Primary Key
            this.HasKey(p => new { p.ProjectId, p.AppRoleId ,p.DevicePortGroupId});

            // Properties
            //this.Property(p => p.ProjectNo).IsRequired();

            // Table & Column Mappings

        }
    }
}