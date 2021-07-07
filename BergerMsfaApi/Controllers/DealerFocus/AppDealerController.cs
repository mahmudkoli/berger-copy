using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerController : BaseController
    {
        private readonly ICommonService _commonSvc;

        public AppDealerController(
            ICommonService commonSvc)
        {
            _commonSvc = commonSvc;
        }

        [HttpGet("GetDealList/{territory}")]
        public async Task<IActionResult> GetDealerList(string territory)
        {
            try
            { 
                if (string.IsNullOrEmpty(territory))
                {
                    ModelState.AddModelError(nameof(territory), "territory can not be null");
                    return ValidationResult(ModelState);
                }
                var result = await _commonSvc.AppGetDealerInfoList(territory.Trim());
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetFocusDealerList/{EmployeeId}")]
        public async Task<IActionResult> GetFocusDealerList(string EmployeeId)
        {
            try
            {
                var result = await _commonSvc.AppGetFocusDealerInfoList(EmployeeId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerList")]
        public async Task<IActionResult> GetDealerList()
        {
            try
            {
                var userId = AppIdentity.AppUser.UserId;
                var result = await _commonSvc.AppGetDealerInfoListByCurrentUser(userId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerListByCategory")]
        public async Task<IActionResult> GetDealerListByCategory([FromQuery] AppDealerSearchModel model)
        {
            try
            {
                var result = await _commonSvc.AppGetDealerInfoListByCurrentUser(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
