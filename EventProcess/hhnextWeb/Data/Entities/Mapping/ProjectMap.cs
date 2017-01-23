using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {


            // Primary Key
            this.HasKey(p => p.ProjectId);

            // Properties
            //this.Property(p => p.ProjectNo).IsRequired();
            this.Property(p => p.ProjectName).IsRequired();

            // Table & Column Mappings
            HasMany(p => p.Devices).WithRequired(pp => pp.Project).HasForeignKey(pp => pp.ProjectId);

        }
    }
}