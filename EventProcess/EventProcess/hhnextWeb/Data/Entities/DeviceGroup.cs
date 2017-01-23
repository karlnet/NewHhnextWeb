using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class DeviceGroup
    {

        public int DeviceGroupId { get; set; }
        public int ProjectId { get; set; }
        //public string DeviceGroupNo { get; set; }
        public string DeviceGroupName { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<RoleDeviceGroup> RoleDeviceGroups { get; set; }
        

        public DeviceGroup()
        {

            //this.board = new Board();
            //this.portDescription = new PortDescription();
        }
    }
}