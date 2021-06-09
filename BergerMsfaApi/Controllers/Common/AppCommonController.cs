using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Common
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppCommonController : BaseController
    {
        private readonly ICommonService _commonSvc;

        public AppCommonController(
            ICommonService commonSvc)
        {
            _commonSvc = commonSvc;
        }

        [HttpGet("GetSaleOfficeList")]
        public async Task<IActionResult> GetSaleOfficeList()
        {
            try
            {
                var result = await _commonSvc.GetSaleOfficeList();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetSaleGroupList")]
        public async Task<IActionResult> GetSaleGroupList()
        {
            try
            {
                var result = await _commonSvc.GetSaleGroupList();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetTerritoryList")]
        public async Task<IActionResult> GetTerritoryList()
        {
            try
            {
                var result = await _commonSvc.GetTerritoryList();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetZoneList")]
        public async Task<IActionResult> GetZoneList()
        {
            try
            {
                var result = await _commonSvc.GetZoneList();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetDepotList")]
        public async Task<IActionResult> GetDepotList()
        {
            try
            {
                var result = await _commonSvc.GetDepotList();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetDivisionList")]
        public async Task<IActionResult> GetDivisionList()
        {
            try
            {
                var result = await _commonSvc.GetDivisionList();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetCreditControlAreaList")]
        public async Task<IActionResult> GetCreditControlAreaList()
        {
            try
            {
                var result = await _commonSvc.GetCreditControlAreaList();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetUserInfoListByCurrentUser")]
        public async Task<IActionResult> GetUserInfoListByCurrentUser()
        {
            try
            {
                var result = await _commonSvc.GetUserInfoListByCurrentUser();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }

        [HttpGet("GetUserInfoListByCurrentUserWithoutZoUser")]
        public async Task<IActionResult> GetUserInfoListByCurrentUserWithoutZoUser()
        {
            try
            {
                var result = await _commonSvc.GetUserInfoListByCurrentUserWithoutZoUser();
                return AppOkResult(result);
            }
            catch (Exception ex)
            {
                return AppExceptionResult(ex);
            }
        }
    }
}
