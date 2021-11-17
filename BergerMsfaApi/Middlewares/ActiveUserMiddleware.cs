using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Berger.Common.Constants;
using BergerMsfaApi.Core;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Helpers;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BergerMsfaApi.Middlewares
{
    public class ActiveUserMiddleware
    {

        private readonly RequestDelegate _next;

        public ActiveUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserInfoService userService)
        {
            string appUserName = AppIdentity.AppUser.UserName;

            if (context.User.Identity.IsAuthenticated)
            {
                var isActive = await userService.IsGlobalActiveUserAsync(appUserName);

                if (!isActive)
                    await HandleActiveUserAsync(context);
            }

            await _next(context);
        }

        private Task HandleActiveUserAsync(HttpContext context)
        {
            var httpStatusCode = (int)HttpStatusCode.Forbidden;

            var apiResponse = new ApiResponse
            {
                StatusCode = httpStatusCode,
                Status = "AuthorizationError",
                Msg = "User is not active. Please contact with Admin.",
                Data = null
            };

            context.Response.StatusCode = httpStatusCode;

            var header = context.Request?.Headers[ConstantPlatformValue.PlatformHeaderName];
            if (header.HasValue && header.Equals(ConstantPlatformValue.AppPlatformHeader))
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }

            context.Response.ContentType = "application/json";

            //context.Response.Headers.Clear();

            return context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
        }
    }
}
