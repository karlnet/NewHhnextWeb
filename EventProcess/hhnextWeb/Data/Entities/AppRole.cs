using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace hhnextWeb.Data.Entities
{

    public class AppRole:IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name, string description) : base(name)
        {
            this.Description = description;
        }
        public  string Description { get; set; }

        public virtual ICollection<ProjectRole> ProjectRoles { get; set; }
        public virtual ICollection<ProjectUserRole> ProjectUserRoles { get; set; }
    }


}