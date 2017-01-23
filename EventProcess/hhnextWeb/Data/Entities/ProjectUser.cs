using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class ProjectUser
    {
        public ProjectUser()
        {

            //this.board = new Board();
            //this.applicationUser = new ApplicationUser();
        }

        //public int ProjectUserId { get; set; }
        public int ProjectId { get; set; }
        public string AppUserId { get; set; }

        //public string UserType { get; set; }


        public virtual AppUser AppUser { get; set; }
        public virtual Project Project { get; set; }
    }
}