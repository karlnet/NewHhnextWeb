using hhnextWeb.Data;
using hhnextWeb.Data.Entities;
using hhnextWeb.Filters;
using hhnextWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace hhnextWeb.Controllers
{
    [RoutePrefix("api/port")]
    [BasicAuthorizeAttribute]
    public class DevicePortsController : BaseApiController
    {
        public DevicePortsController(IRepository repo)
            : base(repo)
        {
        }



        //GET: api/port/5
        [Route("{deviceNo:string}", Name = "GetAllPortsByDeviceNo")]
        [HttpPost]
        public IEnumerable<DevicePort> GetAllPortsByDeviceNo(string  deviceNo)
        {
            var devicePort = TheRepository.GetAllDevicePortByDeviceNo(deviceNo);
            return devicePort;
        }

        //GET: api/port
        [Route("")]
        public IEnumerable<DevicePort> Get()
        {
            string userId = Request.Properties["UserId"] as string;
            string userName = Request.Properties["UserName"] as string;
            return TheRepository.GetAllDevicePort(userId);
        }

        /*        [Route("create")]
                [HttpPost]
                public IHttpActionResult Add(DevicePortBindingModel devicePortsModels)
                {

                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    Device device = TheRepository.GetDeviceByMac(devicePortsModels.mac);
                    if (null == device)
                        return BadRequest(ModelState);

                    string userName = User.Identity.Name;
                    if (!TheRepository.CheckDeviceUser(device.Id, userName))
                        return BadRequest();


                    while (TheRepository.IsExistsDevicePort(device.Id, devicePortsModels.PortNo))
                        TheRepository.DeleteDevicePort(device.Id, devicePortsModels.PortNo);

                    DevicePort bp = new DevicePort()
                    {
                        DeviceId = device.Id,
                        Port = devicePortsModels.PortNo,
                        PortDescriptionId = devicePortsModels.PortTypeId,
                        Alias = devicePortsModels.PortName
                    };

                    if (!TheRepository.Insert(bp))
                        return BadRequest(ModelState);

                    return Ok();

                }

                [Route("remove")]
                [HttpPost]
                public IHttpActionResult Remove(DevicePortBindingModel devicePortsModels)
                {
                    //if (!ModelState.IsValid)
                    //{
                    //    return BadRequest(ModelState);
                    //}

                    Device device = TheRepository.GetDeviceByMac(devicePortsModels.mac);
                    if (null == device)
                        return BadRequest(ModelState);

                    string userName = User.Identity.Name;
                    if (!TheRepository.CheckDeviceUser(device.Id, userName))
                        return BadRequest();

                    TheRepository.DeleteDevicePort(device.Id, devicePortsModels.PortNo);

                    return Ok();

                }

                // PUT: api/DevicePorts/5
                //public void Put(int id, [FromBody]string value)
                //{
                //}

                // DELETE: api/DevicePorts/5
                //public void Delete(int id)
                //{
                //}
                */
    }
}