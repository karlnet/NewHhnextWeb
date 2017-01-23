using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class DevicePortMap : EntityTypeConfiguration<DevicePort>
    {
        public DevicePortMap()
        {

            // Table & Column Mappings
            this.ToTable("DevicePorts");

            // Primary Key
            this.HasKey(p => p.PortId);

            // Properties
            this.Property(p => p.PortNo).IsRequired();
            this.Property(p => p.PortName).IsRequired();
            this.Property(p => p.Alias).IsRequired();
            this.Property(p => p.DataType).IsRequired();
            this.Property(p => p.DefaultValue).IsRequired().HasColumnName("DefaultValue");

            //HasRequired(p => p.Device).WithMany(pp => pp.DevicePorts).HasForeignKey(pp => pp.DeviceId).WillCascadeOnDelete(false);
            //HasRequired(p => p.Device).WithMany(pp => pp.DevicePorts).HasForeignKey(pp => pp.GatewayId).WillCascadeOnDelete();
        }
    }
        
}