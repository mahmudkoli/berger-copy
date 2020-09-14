using System.Threading.Tasks;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Helpers;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Middlewares
{
    public class ItemInHttpContextMiddleware
    {
        private readonly RequestDelegate _next;

        public ItemInHttpContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, IUserRequest userRequest)
        {

            var useragentinfo = userRequest.GetUserAgentInfo;
            if (context.Items.ContainsKey("UserAgentInfo"))
            {
                context.Items.Remove("UserAgentInfo");
            }
            var userIp = userRequest.GetUserIp;
            if (context.Items.ContainsKey("UserIP"))
            {
                context.Items.Remove("UserIP");
            }
            context.Items.Add("UserIP", userIp);
            if (context.User != null )
            {
                var appUser = context.User.ToAppUser();
                appUser.UserAgentInfo = useragentinfo;
                if (context.Items.ContainsKey("AppUser"))
                {
                    context.Items.Remove("AppUser");
                }
                context.Items.Add("AppUser", appUser);

            }
            return this._next(context);
        }
    }
}
