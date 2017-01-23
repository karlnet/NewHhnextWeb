using hhnextWeb.Data.Entities;
using hhnextWeb.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hhnextWeb.Validators
{
    public class CustomUserValidator : UserValidator<AppUser>
    {

        List<string> _allowedEmailDomains = new List<string> { "126.com", "163.com", "qq.com", "vip.163.com" };

        public CustomUserValidator(AppUserManager appUserManager)
            : base(appUserManager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            //var emailDomain = user.Email.Split('@')[1];

            //if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
            //{
            //    var errors = result.Errors.ToList();

            //    errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

            //    result = new IdentityResult(errors);
            //}

            return result;
        }
    }
}