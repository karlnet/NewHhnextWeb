using hhnextWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace hhnextWeb.Controllers
{
    public class DriversController : BaseApiController
    {
        public DriversController(IRepository repo)
            : base(repo)
        {
        }
       /* public class PortDescriptionModel
        {
            public PortDescriptionModel(PortDescription pd)
            {
                PortType = new List<string>();
                PortTypeId = new List<string>();
                PortModel = pd.DeviceModel;
                PortDesc = pd.Description;
                PortClassInfo = pd.ClassType;
                PortType.Add(pd.DeviceType);
                PortTypeId.Add(pd.Id.ToString());
            }
            public List<String> PortTypeId { set; get; }
            public List<String> PortType { set; get; }
            public String PortDesc { set; get; }
            public String PortModel { set; get; }
            public String PortClassInfo { get; set; }
        }
        // GET api/<controller>
        public IEnumerable<PortDescriptionModel> Get()
        {
            IEnumerable<PortDescription> pdList = TheRepository.GetAllPortDescriptions().ToList();

            ConcurrentDictionary<string, PortDescriptionModel> pdDict = new ConcurrentDictionary<string, PortDescriptionModel>();

            var itor = pdList.GetEnumerator();
            while (itor.MoveNext())
            {
                PortDescription pd = itor.Current;
                pdDict.AddOrUpdate(pd.DeviceModel, new PortDescriptionModel(pd),
                        (x, y) =>
                        {
                            y.PortType.Add(pd.DeviceType);
                            y.PortTypeId.Add(pd.Id.ToString());
                            return y;
                        }
                        );
            }

            return pdDict.Values;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }*/
    }
}