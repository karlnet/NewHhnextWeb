using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class ProjectUserMap : EntityTypeConfiguration<ProjectUser>
    {
        public ProjectUserMap()
        {


            // Primary Key
            this.HasKey(p => new { p.ProjectId, p.AppUserId });

            // Properties
            //this.Property(p => p.ProjectNo).IsRequired();

            // Table & Column Mappings

        }
    }
}