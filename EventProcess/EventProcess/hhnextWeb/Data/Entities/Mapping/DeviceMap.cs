using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class DeviceMap : EntityTypeConfiguration<Device>
    {
        public DeviceMap()
        {
           
            
             // Primary Key
            this.HasKey(p => p.DeviceId);
            this.HasIndex("IX_Devices_DeviceNo", IndexOptions.Unique, e => e.Property(x => x.DeviceNo));

            // Properties
            this.Property(p => p.DeviceNo).IsRequired().HasColumnType("CHAR").HasMaxLength(8);
            this.Property(p => p.DeviceName).IsRequired();

            // Table & Column Mappings
            this.ToTable("Devices");

            

        }
    }
}