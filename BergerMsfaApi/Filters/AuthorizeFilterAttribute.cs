using Berger.Common.Constants;
using BergerMsfaApi.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
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
                UnAuthObjectResult(context);
            }
            else
            {
                var appType = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantsApplicationCategory.ApplicationCategory);
                
                //var actionName = string.Empty;
                //var controllerName = string.Empty;

                //var descriptor = context?.ActionDescriptor as ControllerActionDescriptor;
                //if (descriptor != null)
                //{
                //    actionName = descriptor.ActionName;
                //    controllerName = descriptor.ControllerName;
                //}

                if (appType == null)
                    return;
                else if (appType.Value == ConstantsApplicationCategory.MSFAApp)
                {
                    UnAuthObjectResultForMSFA(context);
                }
                else if (appType.Value == ConstantsApplicationCategory.SomporkoApp)
                {
                    UnAuthObjectResultForSomporko(context);
                }
            }
        }

        private bool SkipAuthorization(AuthorizationFilterContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }

        private void UnAuthObjectResult(AuthorizationFilterContext context)
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
            if (header.HasValue && header.Equals(ConstantPlatformValue.AppPlatformHeader))
            {
                result.StatusCode = (int)HttpStatusCode.OK;
            }

            context.Result = result;
        }

        private void UnAuthObjectResultForMSFA(AuthorizationFilterContext context)
        {
            // TODO: Add another new controller attribute if another specific api come and need to unauthorized for msfa app
            var isUnAuthForSpecificApi = context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(SomporkoAttribute));
            if (!isUnAuthForSpecificApi) return;

            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Status = "AuthorizationError",
                Msg = "Unauthorized For MSFA",
                Data = null
            };

            var result = new ObjectResult(apiResult)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };

            var header = context.HttpContext.Request?.Headers[ConstantPlatformValue.PlatformHeaderName];
            if (header.HasValue && header.Equals(ConstantPlatformValue.AppPlatformHeader))
            {
                result.StatusCode = (int)HttpStatusCode.OK;
            }

            context.Result = result;
        }

        private void UnAuthObjectResultForSomporko(AuthorizationFilterContext context)
        {
            var isUnAuthForSpecificApi = context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(SomporkoAttribute));
            if (isUnAuthForSpecificApi) return;

            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Status = "AuthorizationError",
                Msg = "Unauthorized For Somporko",
                Data = null
            };

            var result = new ObjectResult(apiResult)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };

            var header = context.HttpContext.Request?.Headers[ConstantPlatformValue.PlatformHeaderName];
            if (header.HasValue && header.Equals(ConstantPlatformValue.AppPlatformHeader))
            {
                result.StatusCode = (int)HttpStatusCode.OK;
            }

            context.Result = result;
        }
    }
}
