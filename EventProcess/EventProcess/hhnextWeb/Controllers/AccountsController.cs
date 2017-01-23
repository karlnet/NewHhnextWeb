using hhnextWeb.Data;
using hhnextWeb.Data.Entities;
using hhnextWeb.Data.Infrastructure;
using hhnextWeb.Filters;
using hhnextWeb.Models;
using hhnextWeb.Providers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace hhnextWeb.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        public AccountsController(IRepository repo)
            : base(repo)
        {
        }

        public class EZSystem
        {
            public string key { get; set; }
            public string sign { get; set; }
            public string time { get; set; }
            public string ver { get; set; }
        }

        public class EZRequest
        {
            public string id { get; set; }
            [JsonProperty("system")]
            public EZSystem _system { get; set; }
            public string method { get; set; }
            [JsonProperty("params")]
            public SortedDictionary<string, string> _params = new SortedDictionary<string, string>();

        }


        public class EZResponse<T> where T : class
        {
            public string id { get; set; }
            public EZResult<T> result { get; set; }
        }

        public class EZResult<T> where T : class
        {
            public T data { get; set; }
            public string code { get; set; }
            public string msg { get; set; }


        }

        public class EZAccessToken
        {
            public string accessToken { get; set; }
            public string userId { get; set; }
        }

        public class EZServerTime
        {
            public string serverTime { get; set; }
        }

        private async Task<EZResponse<EZServerTime>> EZGetServerTime()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Helper.EZBASEURL);
                HttpResponseMessage response = await client.PostAsync(Helper.EZGETSERVERTIMEMETHOD + "?id=123456&appKey=" + Helper.EZAPPKEY, null);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<EZResponse<EZServerTime>>();
                }
                return null;
            }
        }
        private async Task<EZResponse<EZAccessToken>> EZCheck(string phoneNumber)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(MyAPPs.EZBaseURL);

                EZRequest request = new EZRequest
                {
                    id = "123456",
                    _system = new EZSystem
                    {
                        ver = Helper.EZCLOUDVERSION,
                        key = Helper.EZAPPKEY,
                        time = Helper.GetUnixTimeStamp()
                    },
                    method = Helper.EZGETTOKENMETHOD

                };

                request._params.Add("phone", phoneNumber);
                Helper.EZSetRequest(request);

                string ss = JsonConvert.SerializeObject(request);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync(Helper.EZBASEURL, request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<EZResponse<EZAccessToken>>();
                }

                return null;
            }


        }

        //private async Task<FogToken> FogCheck(CreateUserBindingModel model)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(Helper.FOGBASEURL);
        //        Helper.setRequest(client);

        //        HttpResponseMessage response = await client.PostAsJsonAsync(Helper.FOGLOGINURL, new { username = model.Username, password = model.Password });

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return await response.Content.ReadAsAsync<FogToken>();
        //        }

        //        return null;
        //    }

        //}

        //private async Task<FogToken> FogAddUser(CreateUserBindingModel model, string url)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(Helper.FOGBASEURL);
        //        Helper.setRequest(client);

        //        HttpResponseMessage response = await client.PostAsJsonAsync(url,
        //            new { username = model.Username, password = model.Password, verification_code = model.verification_code });

        //        if (response.IsSuccessStatusCode)
        //        {

        //            return await response.Content.ReadAsAsync<FogToken>();
        //        }

        //        return null;
        //    }

        //}

        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }
        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }
        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }
        [BasicAuthorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = Request.Properties["UserId"].ToString();
            string userName = Request.Properties["UserName"].ToString();

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            UserToken u = CreateToken(userName, model.NewPassword);
            return Ok(u);
        }
        ////[GHCBAuthorizeAttribute]
        //[Route("Reset")]
        //[HttpPost]
        //public async Task<IHttpActionResult> ResetPassword(CreateUserBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //FogToken t = await FogAddUser(model, Helper.FOGRESETPASSWORDURL);

        //    //if (null == t)
        //    //    return BadRequest(ModelState);

        //    var user = await AppUserManager.FindByNameAsync(model.Username);
        //    if (null == user)
        //        return BadRequest();

        //    string resetToken = await AppUserManager.GeneratePasswordResetTokenAsync(user.Id);
        //    IdentityResult result = await AppUserManager.ResetPasswordAsync(user.Id, resetToken, model.Password);
        //    if (null == result)
        //        return BadRequest();

        //    t = await FogCheck(model);

        //    if (null == t)
        //        return BadRequest(ModelState);

        //    if (!user.UserToken.Equals(t.user_token) || !user.UserKey.Equals(t.user_id))
        //    {
        //        user.UserToken = t.user_token;
        //        user.UserKey = t.user_id;
        //        IdentityResult x = await AppUserManager.UpdateAsync(user);

        //        if (null == x)
        //            return BadRequest();
        //    }

        //    UserToken u = CreateToken(model.Username, model.Password);
        //    //u.fog_user_id = t.user_id;
        //    //u.fog_user_token = t.user_token;

        //    return Ok(u);

        //}


        [Route("EZGetToken")]
        [HttpPost]
        public async Task<IHttpActionResult> EZGetToken()
        {

            EZResponse<EZAccessToken> token = await EZCheck("13701308059");

            if (token == null)
                return BadRequest();
            else
                return Ok(token.result.data);
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(UserLoginBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //FogToken t = await FogCheck(model);

            //if (null == t)
            //    return BadRequest(ModelState);

            var user = await AppUserManager.FindByNameAsync(model.Username);

            if (null == user)
                return BadRequest();

            //if (!user.UserToken.Equals(t.user_token) || !user.UserKey.Equals(t.user_id))
            //{
            //    user.UserToken = t.user_token;
            //    user.UserKey = t.user_id;
            //    IdentityResult x = await AppUserManager.UpdateAsync(user);

            //    if (null == x)
            //        return BadRequest();
            //}

            UserToken u = CreateToken(model.Username, model.Password);
            //u.fog_user_id = t.user_id;
            //u.fog_user_token = t.user_token;

            //EZResponse<EZAccessToken> token = await EZCheck("13701308059");

            //if (token != null)
            //{
            //    u.ez_accessToken = token.result.data.accessToken;
            //    u.ez_userId = token.result.data.userId;
            //}

            return Ok(u);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.AppUserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }
        /*
              [Route("create")]
              public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
              {
                  if (!ModelState.IsValid)
                  {
                      return BadRequest(ModelState);
                  }
                  var user = new AppUser()
                  {
                      UserName = createUserModel.Username,
                      Email = createUserModel.Email,
                      //FirstName = createUserModel.FirstName,
                      //LastName = createUserModel.LastName,
                      //Level = 3,
                      //JoinDate = DateTime.Now.Date,
                  };
                  IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);
                  if (!addUserResult.Succeeded)
                  {
                      return GetErrorResult(addUserResult);
                  }
                  Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                  return Created(locationHeader, TheModelFactory.Create(user));
              }

              [AllowAnonymous]
              [Route("create")]
              public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel model)
              {
                  if (!ModelState.IsValid)
                  {
                      return BadRequest(ModelState);
                  }

                  FogToken t = await FogAddUser(model, Helper.FOGREGISTERURL);

                  if (null == t)
                      return BadRequest(ModelState);

                  t = await FogCheck(model);

                  if (null == t)
                      return BadRequest(ModelState);

                  var user = new AppUser()
                  {
                      UserName = model.Username,
                      Email = "1@2.3",
                      //FirstName = createUserModel.FirstName,
                      //LastName = createUserModel.LastName,
                      //Level = 3,
                      //JoinDate = DateTime.Now.Date,
                      UserToken = t.user_token,
                      UserKey = t.user_id  //  id or username?

                  };

                  IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, model.Password);

                  if (!addUserResult.Succeeded)
                  {
                      return GetErrorResult(addUserResult);
                  }



                  //string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                  //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

                  //await this.AppUserManager.SendEmailAsync(user.Id,
                  //                                        "Confirm your account",
                  //                                        "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                  //Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

                  UserToken u = CreateToken(model.Username, model.Password);
                  u.fog_user_id = t.user_id;
                  u.fog_user_token = t.user_token;

                  return Ok(u);
              }*/
        [Route("add")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> AddUser(CreateUserBindingModel model)
        {
            //return Ok("hello world.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //FogToken t = await FogAddUser(model, Helper.FogRegisterURL);

            //if (null == t)
            //    return BadRequest(ModelState);

            //FogToken t = await FogCheck(model);

            //if (null == t)
            //    return BadRequest(ModelState);

            var user = new AppUser()
            {
                UserName = model.Username,
                //Email = model.Email==null?"1@2.3",
                //FirstName = createUserModel.FirstName,
                //LastName = createUserModel.LastName,
                //Level = 3,
                //JoinDate = DateTime.Now.Date,
                //UserToken = t.user_token,
                //UserKey = t.user_id  //////id is right?
                UserType = "AppUser"

            };


            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, model.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }



            //string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

            //await this.AppUserManager.SendEmailAsync(user.Id,
            //                                        "Confirm your account",
            //                                        "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            UserToken u = CreateToken(model.Username, model.Password);
            //u.fog_user_id = t.user_id;
            //u.fog_user_token = t.user_token;

            return Ok(u);
        }
        private UserToken CreateToken(string u, string p)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            return new UserToken()
            {
                userName = u,
                userToken = Convert.ToBase64String(encoding.GetBytes(u + ":" + p))
            };
        }
        private class UserToken
        {
            public string userName { set; get; }
            public string userToken { set; get; }
            //public string fog_user_token { set; get; }
            //public string fog_user_id { set; get; }
            //public string ez_accessToken { get; set; }
            //public string ez_userId { get; set; }
        }

        //private class FogToken
        //{
        //    public string user_token { set; get; }
        //    public string user_id { set; get; }
        //    public string token { set; get; }
        //    public string result { set; get; }
        //}
    }
}
