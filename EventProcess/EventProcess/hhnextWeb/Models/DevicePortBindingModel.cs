using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hhnextWeb.Models
{
    public class DevicePortBindingModel
    {
        [Required]
        public int  DeviceId { set; get; }
        [Required]
        public string PortNo { get; set; }
        [Required]
        public string PortName { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        public int DataType { get; set; }
        [Required]
        public int DefaultValue { get; set; }
    }
}