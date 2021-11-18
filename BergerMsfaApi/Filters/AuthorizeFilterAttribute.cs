using Berger.Common.Constants;
using BergerMsfaApi.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Filters
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeFilterAttribute : Attribute, IAuthorizationFilter
    {
        public AuthorizeFilterAttribute()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (SkipAuthorization(context)) return;

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                var apiResult = new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Status = "AuthorizationError",
                    Msg = "Unauthorized",
                    Data = null
                };

                var result = new ObjectResult(apiResult)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };

                var header = context.HttpContext.Request?.Headers[ConstantPlatformValue.PlatformHeaderName];
                if (header.HasValue && header.Equals(ConstantPlatformValue.AppPlatformHeader) && result.StatusCode != (int)HttpStatusCode.Unauthorized)
                {
                    result.StatusCode = (int)HttpStatusCode.OK;
                }

                context.Result = result;
            }
        }

        private bool SkipAuthorization(AuthorizationFilterContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}
