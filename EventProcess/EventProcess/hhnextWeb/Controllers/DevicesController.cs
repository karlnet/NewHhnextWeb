using hhnextWeb.Data;
using hhnextWeb.Data.Entities;
using hhnextWeb.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace hhnextWeb.Controllers
{
    [RoutePrefix("api/device")]
    [BasicAuthorize]
    public class DevicesController : BaseApiController
    {

        public DevicesController(IRepository repo)
            : base(repo)
        {
        }
        /*        public class UsersFromFog
                {
                    public string user_id;
                    public string username;
                    public string role;

                }
                public class ActiveTokenModel
                {
                    [Required]
                    public string mac { set; get; }
                }
                private async Task<List<UsersFromFog>> FogDeviceUsersQuery(string deviceId)
                {
                    using (var client = new HttpClient())
                    {
                        setRequestUserToken(client);

                        //var jsonObject = new
                        //{
                        //    device_id = deviceId,      
                        //};

                        HttpResponseMessage response = await client.GetAsync(MyAPPs.FogUserQueryURL + "?device_id=" + deviceId);

                        if (response.IsSuccessStatusCode)
                        {
                            List<UsersFromFog> users = await response.Content.ReadAsAsync<List<UsersFromFog>>();
                            return users;
                        }

                        return new List<UsersFromFog>();
                    }

                }
                private async Task<bool> FogDeviceUsersAdd(string deviceId, string userName, string role)
                {
                    using (var client = new HttpClient())
                    {
                        setRequestUserToken(client);

                        var jsonObject = new
                        {
                            device_id = deviceId,
                            username = userName,
                            owner_type = role
                        };

                        HttpResponseMessage response = await client.PostAsJsonAsync(MyAPPs.FogUserAddAuthURL, jsonObject);

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }

                        return false;
                    }

                }

                public class BoardFromFog
                {
                    public string id { get; set; }
                    public string serial { get; set; }
                    public string MAC { get; set; }
                    public string created { get; set; }
                    public string secret_key { get; set; }
                    public string alias { get; set; }
                    public string online { get; set; }
                    public string ip { get; set; }
                    public string ssid { get; set; }
                    public string wx_device_id { get; set; }
                }

                private async Task<BoardFromFog> FogGetDevice(string mac)
                {
                    using (var client = new HttpClient())
                    {
                        setRequestUserToken(client);
                        string url = MyAPPs.FogGetBoardURL + "?MAC=" + mac;
                        HttpResponseMessage response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            return (await response.Content.ReadAsAsync<List<BoardFromFog>>()).First();

                        }

                        return null;
                    }

                }
                private async Task<bool> FogDeviceUsersDel(string deviceId, string userId)
                {
                    using (var client = new HttpClient())
                    {
                        setRequestUserToken(client);

                        string url = userId + "?device_id=" + deviceId;

                        HttpResponseMessage response = await client.DeleteAsync(MyAPPs.FogUserDelAuthURL + url);

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }

                        return false;
                    }

                }
                private async Task<bool> FogActive(string mac)
                {
                    using (var client = new HttpClient())
                    {
                        var jsonObject = new
                        {
                            product_id = MyAPPs.ProductID,
                            MAC = mac,
                            secret_key = MyUtils.getActiveToken(mac)
                        };

                        HttpResponseMessage response = await client.PostAsJsonAsync(MyAPPs.FogActiveURL, jsonObject);

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }

                        return false;
                    }

                }
                private void setRequestUserToken(HttpClient client)
                {
                    string userName = User.Identity.Name;
                    string userToken = "token " + MyAPPs.currentUsers[userName].UserToken;

                    MyAPPs.setRequest(client);
                    client.DefaultRequestHeaders.Add("Authorization", userToken);

                }
                private async Task<bool> FogBinding(string mac)
                {
                    using (var client = new HttpClient())
                    {

                        setRequestUserToken(client);

                        string userName = User.Identity.Name;
                        var jsonObject = new
                        {
                            product_id = MyAPPs.ProductID,
                            user_id = MyAPPs.currentUsers[userName].UserId,
                            MAC = mac,
                            secret_key = MyUtils.getActiveToken(mac)
                        };

                        HttpResponseMessage response = await client.PostAsJsonAsync(MyAPPs.FogAuthorizeURL, jsonObject);

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }

                        return false;
                    }

                }
                // GET: api/boards
                [Route("authorize")]
                [HttpPost]
                public async Task<IHttpActionResult> Authorize(ActiveTokenModel model)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    string mac = model.mac;
                    bool result;
                    Board b = null;
                    result = await FogActive(mac);

                    if (!result)
                        return BadRequest(ModelState);

                    //if (TheRepository.IsExistsBoard(mac))
                    //TheRepository.Delete(mac);

                    result = await FogBinding(mac);

                    if (!result)
                        return BadRequest(ModelState);

                    BoardFromFog board = await FogGetDevice(mac);

                    if (board != null)
                    {
                        b = new Board
                        {
                            MAC = board.MAC,
                            Deviceid = board.id,
                            SerialNo = board.serial == null ? "" : board.serial,
                            Createtime = DateTime.Parse(board.created),
                            Token = board.secret_key,
                            Name = board.alias,
                            Status = board.online,
                            Publicip = board.ip,
                            SSID = board.ssid == null ? "" : board.ssid,
                            Description = board.wx_device_id
                        };

                        TheRepository.InsertOrUpdate(b, mac);

                    }
                    else
                        return BadRequest();

                    string userName = User.Identity.Name;
                    string userId = MyAPPs.currentUsers[userName].Id;
                    string userType = "share";

                    if (!TheRepository.IsExistsBoardAnyUsers(b.Id))
                        userType = "owner";

                    var applicationUserBoard = new ApplicationUserBoard()
                    {
                        BoardId = b.Id,
                        UserType = userType,
                        ApplicationUserName = userName,
                        ApplicationUserId = userId
                    };

                    if (!TheRepository.InsertOrUpdate(applicationUserBoard))
                        return BadRequest("Bingding failed.");

                    return Ok();
                }
*/
        // GET: api/devices
        [Route("")]
        public IEnumerable<Device> Get()
        {
            string userId = Request.Properties["UserId"] as string;
            string userName = Request.Properties["UserName"] as string;
            return TheRepository.GetAllDevice(userId);

        }
        /*                public class DeviceUsers
                        {
                            [Required]
                            public string id { set; get; }
                            [Required]
                            public string login_id { set; get; }
                            [Required]
                            public string action { set; get; }

                            public string owner_type { set; get; }

                        }

                        [ResponseType(typeof(List<UsersFromFog>))]
                        [Route("users")]
                        [HttpGet]
                        public async Task<IHttpActionResult> GetDeviceUsers([FromUri] string device_id)
                        {
                            if (!ModelState.IsValid)
                                return BadRequest(ModelState);

                            string id = device_id;

                            try
                            {
                                List<UsersFromFog> users = await FogDeviceUsersQuery(id);
                                if (users.Count == 0)
                                    return NotFound();
                                else
                                    return Ok(users);
                            }
                            catch
                            {
                                return BadRequest();
                            }
                        }


                        [Route("users")]
                        [HttpPost]
                        public async Task<IHttpActionResult> AddOrDelDeviceUsers(DeviceUsers deviceUsers)
                        {
                            if (!ModelState.IsValid)
                                return BadRequest(ModelState);

                            string id = deviceUsers.id;
                            string username = deviceUsers.login_id;
                            string action = deviceUsers.action;
                            string role = deviceUsers.owner_type;

                            var user = await AppUserManager.FindByNameAsync(username);
                            string userId = user.UserId;
                            string aspnetUserId = user.Id;


                            Board b = TheRepository.GetBoardByDeviceId(id);

                            bool result = false;

                            try
                            {
                                if (action.Equals("add"))
                                {
                                    result = await FogDeviceUsersAdd(id, username, role);
                                    if (result)
                                    {
                                        var boardUser = new ApplicationUserBoard()
                                        {
                                            BoardId = b.Id,
                                            ApplicationUserName = username,
                                            UserType = role,
                                            ApplicationUserId = aspnetUserId
                                        };

                                        if (TheRepository.InsertOrUpdate(boardUser))
                                        {
                                            return Ok();
                                        }
                                        else
                                        {
                                            return BadRequest();
                                        }
                                    }
                                    else
                                        return BadRequest();
                                }
                                else
                                {
                                    result = await FogDeviceUsersDel(id, userId);
                                    if (result)
                                    {
                                        TheRepository.DeleteBoardUser(b.Id, username);

                                        return Ok();
                                    }
                                    else
                                        return BadRequest();
                                }
                            }
                            catch
                            {
                                return BadRequest();
                            }
                        }
                        // GET api/<controller>
                        public IEnumerable<string> Get()
                        {
                            return new string[] { "value1", "value2" };
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