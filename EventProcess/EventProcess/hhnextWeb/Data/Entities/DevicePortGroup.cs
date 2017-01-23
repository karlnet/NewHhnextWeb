using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class DevicePortGroup
    {
        public int DevicePortGroupId { get; set; }
        public int ProjectId { get; set; }
        //public string DevicePortGroupNo { get; set; }
        public string DevicePortGroupName { get; set; }

        public virtual ICollection<DevicePort> DevicePorts { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<RoleDevicePortGroup> RoleDevicePortGroups { get; set; }

        public DevicePortGroup()
        {

            //this.board = new Board();
            //this.portDescription = new PortDescription();
        }
    }
}