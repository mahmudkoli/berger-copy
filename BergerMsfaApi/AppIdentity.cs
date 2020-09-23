using BergerMsfaApi.Core;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Helpers;

namespace BergerMsfaApi
{
    public static class AppIdentity
    {

        public static AppUserPrincipal AppUser
        {
            get
            {
                if (HttpHelper.HttpContext == null)
                {
                    return ClaimExtension.EmptyAppUser;
                }
                var user = HttpHelper.HttpContext.Items.ContainsKey("AppUser")
                    ? HttpHelper.HttpContext.Items["AppUser"] as AppUserPrincipal
                    : HttpHelper.HttpContext.User.ToAppUser();
                //var user = Thread.CurrentPrincipal as AppUserPrincipal;
                return user ?? ClaimExtension.EmptyAppUser;
            }
            set { ClaimExtension.EmptyAppUser = value; }

        }
        public static string UserAgentInfo
        {
            get
            {
                if (HttpHelper.HttpContext == null)
                {
                    return "CL:en-US|IP:127.0.0.1";
                }
                var user = HttpHelper.HttpContext.Items.ContainsKey("UserAgentInfo")
                    ? HttpHelper.HttpContext.Items["UserAgentInfo"] as string
                    : "CL:en-US|IP:127.0.0.1";
                //var user = Thread.CurrentPrincipal as AppUserPrincipal;
                return user ?? "CL:en-US|IP:127.0.0.1";
            }

        }
        public static string UserIP
        {
            get
            {
                if (HttpHelper.HttpContext == null)
                {
                    return "127.0.0.1";
                }
                var user = HttpHelper.HttpContext.Items.ContainsKey("UserIP")
                    ? HttpHelper.HttpContext.Items["UserIP"] as string
                    : "127.0.0.1";
                //var user = Thread.CurrentPrincipal as AppUserPrincipal;
                return user ?? "127.0.0.1";
            }

        }

        public static bool IsMigrationEnable = false;
    }
}
