using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Entities
{
    public class RoleDeviceGroup
    {

        public RoleDeviceGroup()
        {

            //this.board = new Board();
            //this.applicationUser = new ApplicationUser();
        }

        //public int RoleDeviceGroupId { get; set; }
        public int DeviceGroupId { get; set; }
        public string AppRoleId { get; set; }
        public int ProjectId { get; set; }

        public bool read { get; set; }
        public bool write { get; set; }
        public bool exec { get; set; }
        public int  permission { get; set; }

        public virtual Project Project { get; set; }
        //[ForeignKey("AppRoleId")]
        public virtual  AppRole AppRole { get; set; }
        //[ForeignKey("DeviceGroupId")]
        public virtual DeviceGroup DeviceGroup { get; set; }
    }
}