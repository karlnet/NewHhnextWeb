using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class DevicePort
    {
        
        public int          PortId { get; set; }
        public int          DevicePortGroupId { get; set; }
        public int          DeviceId { get; set; }
        public int          GatewayId { get; set; }
        public int          DriverId { get; set; }
        public int          ProjectId { get; set; }
        public string       PortNo { get; set; }
        public string       PortName { get; set; }
        public string       Alias { get; set; }
        
        public string       PortType { get; set; }
        public bool         Enable { get; set; }

        public string       DataType { get; set; }
        public string       Uplimit { get; set; }
        public string       Lowlimit { get; set; }
        public string       UpOffset { get; set; }
        public string       LowOffset { get; set; }
        public string       Max { get; set; }
        public string       Min { get; set; }
        public string       DefaultValue { get; set; }

        //public string       Permission { get; set; }
        public string       IP { get; set; }      
        public string       Address { get; set; }
        public string       Config { get; set; }
        public string       NetConnectType { set; get; }


        public string       Comments { get; set; }
        //public string       Item1 { get; set; }
        //public string       Item2 { get; set; }
        //public string       Item3 { get; set; }
        //public string       Item4 { get; set; }

        [ForeignKey("DeviceId")]
        [JsonIgnore]
        public virtual Device Device { get; set; }
        [ForeignKey("GatewayId")]
        [JsonIgnore]
        public virtual Device Gateway { get; set; }
        [JsonIgnore]
        public virtual DevicePortGroup DevicePortGroup { get; set; }
        [JsonIgnore]
        public virtual Driver Driver { get; set; }
        [JsonIgnore]
        public virtual Project Project { get; set; }

        public DevicePort()
        {

            //this.board = new Board();
            //this.portDescription = new PortDescription();
        }

        public int getIntDefaultValue()
        {
            return int.Parse(this.DefaultValue);
        }

        public decimal getDecimalDefaultValue()
        {
            return decimal.Parse(DefaultValue);
        }


    }
}