using hhnextWeb.Data.Entities;
using hhnextWeb.Data.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace hhnextWeb.Filters
{

    public class BasicAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private static readonly Task<object> nullTask= Task.FromResult<object>(null);

        //[Dependency]
        //public GHCBRepository TheRepository { get; set; }
            
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            //Case that user is authenticated using forms authentication 
            //so no need to check header for basic authentication.
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return nullTask;
            }

            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                    !String.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var credArray = GetCredentials(authHeader);
                    var userName = credArray[0];
                    var password = credArray[1];

                    //if (IsResourceOwner(userName, actionContext))
                    //{
                    //You can use Websecurity or asp.net memebrship provider to login, for
                    //for he sake of keeping example simple, we used out own login functionality
                    //if (TheRepository.LoginUser(userName, password))

                    var user = actionContext.Request.GetOwinContext().GetUserManager<AppUserManager>().FindAsync(userName, password).Result;

                    if (user != null)
                    {
                        var currentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                        actionContext.RequestContext.Principal = currentPrincipal;
                        //Thread.CurrentPrincipal = currentPrincipal;
                        //MyAPPs.currentUsers.TryAdd(userName, user.Result);
                        actionContext.Request.Properties.Add("UserName", userName);
                        actionContext.Request.Properties.Add("UserId", user.Id);
                        return nullTask;
                    }
                    //}
                }
            }

            HandleUnauthorizedRequest(actionContext);
            return nullTask;
        }

        private string[] GetCredentials(AuthenticationHeaderValue authHeader)
        {

            //Base 64 encoded string
            var rawCred = authHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var cred = encoding.GetString(Convert.FromBase64String(rawCred));

            var credArray = cred.Split(':');

            return credArray;
        }

        private bool IsResourceOwner(string userName, HttpActionContext actionContext)
        {
            var routeData = actionContext.Request.GetRouteData();
            var resourceUserName = routeData.Values["userName"] as string;

            if (resourceUserName == userName)
            {
                return true;
            }
            return false;
        }

        private void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            actionContext.Response.Headers.Add("WWW-Authenticate",
                                               "Basic location='http://hhnext.com/accounts/login'");

        }
    }
}