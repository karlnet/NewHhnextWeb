using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class Project
    {
        public Project()
        {
            //this.boardPorts = new HashSet<BoardPort>();
        }

        public int ProjectId { get; set; }

        //public string ProjectNo { get; set; }
        public string ProjectName { get; set; }

        public string AppId { get; set; }
        public string AppSecretKey { get; set; }

        public string Comments { get; set; }
        //public string Item1 { get; set; }
        //public string Item2 { get; set; }
        //public string Item3 { get; set; }
        //public string Item4 { get; set; }

        public virtual ICollection<DevicePort> DevicePorts { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<ProjectRole> ProjectRoles { get; set; }
        public virtual ICollection<RoleDeviceGroup> RoleDeviceGroups { get; set; }
        public virtual ICollection<RoleDevicePortGroup> RoleDevicePortGroups { get; set; }
        


    }
}