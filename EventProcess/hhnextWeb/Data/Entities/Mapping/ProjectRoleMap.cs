using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class ProjectRoleMap : EntityTypeConfiguration<ProjectRole>
    {
        public ProjectRoleMap()
        {


            // Primary Key
            this.HasKey(p => new { p.ProjectId, p.AppRoleId });

            // Properties
            //this.Property(p => p.ProjectNo).IsRequired();

            // Table & Column Mappings

        }
    }
}