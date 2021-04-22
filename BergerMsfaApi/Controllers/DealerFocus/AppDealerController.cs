using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerController : BaseController
    {
        private readonly ICommonService _commonSvc;

        public AppDealerController(ICommonService commonSvc)
        {
            _commonSvc = commonSvc;
        }

        //this method expose dealer list by territory for App
        [HttpGet("GetDealList/{territory}")]
        public async Task<IActionResult> GetDealerList(string territory)
        {
            try
            { 
                if(string.IsNullOrEmpty(territory))
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

        [Authorize]
        [HttpGet("GetDealerList")]
        public async Task<IActionResult> GetDealerList([FromQuery] string userCategory, [FromQuery] List<string> userCategoryIds)
        {
            try
            {
                //if (string.IsNullOrEmpty(userCategory))
                //{
                //    ModelState.AddModelError(nameof(userCategory), "User Category can not be null");
                //    return ValidationResult(ModelState);
                //}

                //var result = await _commonSvc.AppGetDealerInfoListByUserCategory(userCategory.Trim(), userCategoryIds);

                var userId = AppIdentity.AppUser.UserId;
                var result = await _commonSvc.AppGetDealerInfoListByCurrentUser(userId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }

        [Authorize]
        [HttpGet("GetDealerListByCategory")]
        public async Task<IActionResult> GetDealerListByCategory([FromQuery] AppDealerSearchModel model)
        {
            try
            {
                //var result = await _commonSvc.AppGetDealerInfoListByDealerCategory(model);
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
