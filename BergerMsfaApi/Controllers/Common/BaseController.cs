using System;
using System.Linq;
using System.Net;
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

        protected IActionResult AppOkResult(object data)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Status = "Success",
                Msg = "Successful",
                Data = data
            };
            return AppObjectResult(apiResult);
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

        protected IActionResult AppOkResult(object data, string message)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Status = "Success",
                Msg = message,
                Data = data
            };
            return AppObjectResult(apiResult);
        }

        protected IActionResult ValidationResult(ModelStateDictionary modelState)
        {
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = "ValidationError",
                Msg = "Validation Fail",
                Errors = modelState.GetErrors()
            };
            return ObjectResult(apiResult);
        }

        protected IActionResult AppValidationResult(ModelStateDictionary modelState)
        {
            var errors = modelState.GetErrors();
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = "ValidationError",
                Msg = errors?.FirstOrDefault()?.ErrorList?.FirstOrDefault()??"Validation Fail",
                Errors = errors,
                Data = null
            };
            return AppObjectResult(apiResult);
        }

        protected IActionResult ExceptionResult(Exception ex, string msg = null)
        {
            ex.ToWriteLog();
            
            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Status = "Error",
                Msg = msg ?? ex.Message,
                Data = new object()
            };
            return ObjectResult(apiResult);
        }

        protected IActionResult AppExceptionResult(Exception ex, string msg = null)
        {
            ex.ToWriteLog();

            var apiResult = new ApiResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Status = "Error",
                Msg = msg ?? ex.Message,
                Data = null
            };
            return AppObjectResult(apiResult);
        }

        protected IActionResult ObjectResult(ApiResponse model)
        {
            var result = new ObjectResult(model)
            {
                StatusCode = model.StatusCode
            };
            return result;
        }

        protected IActionResult AppObjectResult(ApiResponse model)
        {
            var result = new ObjectResult(model)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
            return result;
        }
    }
}