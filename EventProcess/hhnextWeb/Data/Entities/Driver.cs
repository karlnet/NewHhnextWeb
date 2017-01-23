using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class Driver
    {
        public Driver()
        {
            //this.boardPorts = new HashSet<BoardPort>();
        }

        public int DriverId { get; set; }
        //public string DriverNo { get; set; }
        public string DriverName { get; set; }
        public string Location { get; set; }
        public string ClassType { get; set; }

        public string Comments { get; set; }
        //public string Item1 { get; set; }
        //public string Item2 { get; set; }
        //public string Item3 { get; set; }
        //public string Item4 { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<DevicePort> DevicePorts { get; set; }
    }
}