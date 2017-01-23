using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities.Mapping
{
    public class ProjectUserRoleMap : EntityTypeConfiguration<ProjectUserRole>
    {
        public ProjectUserRoleMap()
        {


            // Primary Key
            this.HasKey(p => new { p.ProjectId, p.AppRoleId ,p.AppUserId});

            // Properties
            //this.Property(p => p.ProjectNo).IsRequired();

            // Table & Column Mappings

        }
    }
}