using hhnextWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hhnextWeb.Data
{
    public interface IRepository
    {
        IList<Device> GetAllDevice(string userId);
        IList<DevicePort> GetAllDevicePort(string userId);
        IList<DevicePort> GetAllDevicePortByDeviceNo(string deviceId);
    }
}
