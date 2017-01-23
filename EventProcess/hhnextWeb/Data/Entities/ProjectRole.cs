using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class ProjectRole
    {
        public ProjectRole()
        {
                
        }
        public int ProjectId { get; set; }
        public string AppRoleId { get; set; }

        //public string UserType { get; set; }

        //[ForeignKey("AppRoleId")]
        public virtual AppRole AppRole { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<RoleDevicePortGroup> RoleDevicePortGroups { get; set; }
        public virtual ICollection<RoleDeviceGroup> RoleDeviceGroups { get; set; }
        public virtual ICollection<ProjectUserRole> ProjectUserRoles { get; set; }

    }
}