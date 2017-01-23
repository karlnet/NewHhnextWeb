using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace hhnextWeb.Formats
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {

        private readonly string _issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];

            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            //var signingKey = new HmacSigningCredentials(keyByteArray);
            //const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            //var now = DateTime.UtcNow;
            //var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(sec));
            var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new  Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            var issued = data.Properties.IssuedUtc;

            var expires = data.Properties.ExpiresUtc;

            //var header = new JwtHeader(signingCredentials);
            //var payload = new JwtPayload
            //{
            //    {"iss", "a5fgde64-e84d-485a-be51-56e293d09a69"},
            //    {"scope", "https://example.com/ws"},
            //    {"aud", "https://example.com/oauth2/v1"},
            //    {"iat", now},
            //};
            //var secToken = new JwtSecurityToken(header, payload);

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}