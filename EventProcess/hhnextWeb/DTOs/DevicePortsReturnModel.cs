using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Models
{
    public class DevicePortsReturnModel
    {
        public int DeviceId { get; set; }

        public int PortId { get; set; }
        public string PortNo { get; set; }
        public string PortName { get; set; }
        public string Alias { get; set; }

        public string PortType { set; get; }
        public string Enable { get; set; }

        public string DataType { get; set; }
        public decimal? Uplimit { get; set; }
        public decimal? Lowlimit { get; set; }
        public decimal? UpOffset { get; set; }
        public decimal? LowOffset { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public decimal DefaultValue { get; set; }

        public string Permission { get; set; }
        public string IP { get; set; }
        public string Config { get; set; }
        public string Address { get; set; }

        public string Comments { get; set; }
    }
}