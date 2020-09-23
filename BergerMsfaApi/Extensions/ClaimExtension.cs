using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using BergerMsfaApi.Core;

namespace BergerMsfaApi.Extensions
{
    public static class ClaimExtension
    {
        public static AppUserPrincipal EmptyAppUser = new AppUserPrincipal("")
        {
            UserId = 0,
            FullName = string.Empty,
            Email = string.Empty,
            UserAgentInfo = "CL:en-US|IP:127.0.0.1",
        };

        public static AppUserPrincipal ToAppUser(this IPrincipal user)
        {
            IEnumerable<Claim> userClaimes;
            ClaimsIdentity claimsIdentity;
            if (user != null)
            {
                claimsIdentity = (ClaimsIdentity)user;
            }
            else
            {
                if (user == null && user.Identity != null)
                {
                    claimsIdentity = ((ClaimsIdentity)user.Identity);
                }
                else
                {
                    return EmptyAppUser;
                }
            }
            if (claimsIdentity == null)
            {
                return EmptyAppUser;
            }
            else
            {
                userClaimes = claimsIdentity.Claims;
            }
            return userClaimes.ToAppUser();
        }
        public static AppUserPrincipal ToAppUser(this ClaimsPrincipal user)
        {
            IEnumerable<Claim> userClaimes;
            if (user != null)
            {
                userClaimes = user.Claims;
            }
            else
            {
                if (user == null && user.Identity != null)
                {
                    userClaimes = ((ClaimsIdentity)user.Identity).Claims;
                }
                else
                {
                    return EmptyAppUser;
                }
            }

            return userClaimes.ToAppUser();

        }

        public static AppUserPrincipal ToAppUser(this IIdentity user)
        {
            if (user == null)
            {
                return EmptyAppUser;
            }
            return ((ClaimsIdentity)user)
               .Claims.ToAppUser();
        }
        public static AppUserPrincipal ToAppUser(this IEnumerable<Claim> claims)
        {
            if (claims == null)
            {
                return EmptyAppUser;
            }
            var claimItems = claims
               .Where(s => !s.Type.Contains("schemas.microsoft.com") && !s.Type.Contains("schemas.xmlsoap.org"))
               .Select(x => new { Key = x.Type, x.Value })
               .ToDictionary(t => t.Key, t => t.Value);

            return AppUser(claimItems) ?? EmptyAppUser;
        }


        private static AppUserPrincipal AppUser(Dictionary<string, string> claims)
        {
            if (claims.Count == 0)
            {
                return EmptyAppUser;
            }
            return new AppUserPrincipal(claims["UserName"])
            {
                UserId = Convert.ToInt32(claims["UserId"]),
                FullName = claims["FullName"],
                Phone = claims["Phone"],
                Email = claims["Email"],
                EmployeeId = claims["EmployeeId"],
                Avatar = claims["Avatar"],
                ActiveRoleId = Convert.ToInt32(claims["ActiveRoleId"]),
                RoleIds = claims["RoleIds"],
                UserAgentInfo = claims["UserAgentInfo"],
                ActiveRoleName = claims["ActiveRoleName"],
            };
        }


    }
}
