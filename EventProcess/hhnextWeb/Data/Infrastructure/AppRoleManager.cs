using hhnextWeb.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhnextWeb.Data.Infrastructure
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(IRoleStore<AppRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new AppRoleManager(new RoleStore<AppRole>(context.Get<AppDbContext>()));

            return appRoleManager;
        }
    }
}