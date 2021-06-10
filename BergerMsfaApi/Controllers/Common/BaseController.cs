using System;
using System.Linq;
using System.Net;
using Berger.Common.Constants;
using BergerMsfaApi.Core;
using BergerMsfaApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using X.PagedList;

namespace BergerMsfaApi.Controllers.Common
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult OkResult(IPagedList model)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = 200,
                Status = "Success",
                Msg = "Successful",
                Data = new
                {
                    model.PageCount,
                    model.PageNumber,
                    model.PageSize,
                    model.TotalItemCount,
                    model.IsLastPage,
                    model.HasNextPage,
                    model.LastItemOnPage,
                    model.FirstItemOnPage,
                    model
                }
            };
            return ObjectResult(apiResult);
        }

        protected IActionResult OkResult(object data)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Status = "Success",
                Msg = "Successful",
                Data = data
            };
            return ObjectResult(apiResult);
        }

        protected IActionResult OkResult(object data, string message)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Status = "Success",
                Msg = message,
                Data = data
            };
            return ObjectResult(apiResult);
        }

        protected IActionResult ValidationResult(ModelStateDictionary modelState)
        {
            var errors = modelState.GetErrors();
            var isAppPlatform = IsAppPlatform();
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = "ValidationError",
                Msg = isAppPlatform ? errors?.FirstOrDefault()?.ErrorList?.FirstOrDefault() ?? "Validation Fail" : "Validation Fail",
                Errors = errors,
                Data = isAppPlatform ? null : new object()
            };
            return ObjectResult(apiResult);
        }

        protected IActionResult ExceptionResult(Exception ex, string msg = null)
        {
            ex.ToWriteLog();
            
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Status = "Error",
                Msg = msg ?? ex.Message,
                Data = IsAppPlatform() ? null : new object()
            };
            return ObjectResult(apiResult);
        }

        protected IActionResult ObjectResult(ApiResponse model)
        {
            var result = new ObjectResult(model)
            {
                StatusCode = model.StatusCode
            };

            if (IsAppPlatform())
                result.StatusCode = (int)HttpStatusCode.OK;

            return result;
        }

        private bool IsAppPlatform()
        {
            var header = HttpContext.Request?.Headers[ConstantPlatformValue.PlatformHeaderName];
            return (header.HasValue && header.Equals(ConstantPlatformValue.AppPlatformHeader));
        }
    }
}