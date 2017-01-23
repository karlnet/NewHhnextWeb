using hhnextWeb.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using hhnextWeb.Models;
using hhnextWeb.Data;
using hhnextWeb.Data.Infrastructure;

namespace hhnextWeb.Controllers
{

    public class BaseApiController : ApiController
    {
        private IRepository repo;    //= new GHCBRepository();  //   
              
        private ModelFactory _modelFactory;

        private AppDbContext _AppDbContext = null;
        private AppUserManager _AppUserManager = null;
        private AppRoleManager _AppRoleManager = null;

        public BaseApiController()
        {
        }

        public BaseApiController(IRepository repo)
        {
            this.repo = repo;

        }

        protected AppDbContext AppDbContext
        {
            get
            {
                return _AppDbContext ?? Request.GetOwinContext().Get<AppDbContext>();
            }
        }
        protected AppRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }
        protected AppUserManager AppUserManager
        {
            get
            {
                if (_AppUserManager == null)
                {
                    var context = Request.GetOwinContext();
                    var res = context.GetUserManager<AppUserManager>();

                    _AppUserManager = res;
                }

                return _AppUserManager;

                //return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

 

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }
       

        protected IRepository TheRepository
        {
            get
            {
                return repo;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}