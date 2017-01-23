using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class DriverMap : EntityTypeConfiguration<Driver>
    {
        public DriverMap()
        {


            // Primary Key
            this.HasKey(p => p.DriverId);

            // Properties
            //this.Property(p => p.DriverNo).IsRequired();
            this.Property(p => p.DriverName).IsRequired();

            // Table & Column Mappings
            HasMany(p => p.Devices).WithRequired(pp => pp.Driver).HasForeignKey(pp => pp.DriverId);
            HasMany(p => p.DevicePorts).WithRequired(pp => pp.Driver).HasForeignKey(pp => pp.DriverId);

        }
    }
}