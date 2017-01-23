using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class ProjectUserRole
    {
        public ProjectUserRole()
        {
                
        }
        public int ProjectId { get; set; }
        public string AppRoleId { get; set; }
        public string AppUserId { get; set; }

        //[ForeignKey("AppRoleId")]
        public virtual AppRole AppRole { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Project Project { get; set; }
       

    }
}