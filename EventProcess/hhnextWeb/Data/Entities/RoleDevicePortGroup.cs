using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class RoleDevicePortGroup
    {
        public RoleDevicePortGroup()
        {

            //this.board = new Board();
            //this.applicationUser = new ApplicationUser();
        }

        //public int RoleDevicePortGroupId { get; set; }
        public int DevicePortGroupId { get; set; }
        public string AppRoleId { get; set; }
        public int ProjectId { get; set; }

        public bool read { get; set; }
        public bool write { get; set; }
        public bool exec { get; set; }
        public int permission { get; set; }

        public virtual Project Project { get; set; }
        public virtual AppRole AppRole { get; set; }
        public virtual DevicePortGroup DevicePortGroup { get; set; }
    }
}