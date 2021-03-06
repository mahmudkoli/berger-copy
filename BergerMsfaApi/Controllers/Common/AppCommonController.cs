using System;
using System.Threading.Tasks;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Common
{
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
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetSaleGroupList")]
        public async Task<IActionResult> GetSaleGroupList()
        {
            try
            {
                var result = await _commonSvc.GetSaleGroupList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetTerritoryList")]
        public async Task<IActionResult> GetTerritoryList()
        {
            try
            {
                var result = await _commonSvc.GetTerritoryList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetZoneList")]
        public async Task<IActionResult> GetZoneList()
        {
            try
            {
                var result = await _commonSvc.GetZoneList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDepotList")]
        public async Task<IActionResult> GetDepotList()
        {
            try
            {
                var result = await _commonSvc.GetDepotList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDivisionList")]
        public async Task<IActionResult> GetDivisionList()
        {
            try
            {
                var result = await _commonSvc.GetDivisionList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetCreditControlAreaList")]
        public async Task<IActionResult> GetCreditControlAreaList()
        {
            try
            {
                var result = await _commonSvc.GetCreditControlAreaList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetUserInfoListByCurrentUser")]
        public async Task<IActionResult> GetUserInfoListByCurrentUser()
        {
            try
            {
                var result = await _commonSvc.GetUserInfoListByCurrentUser();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetUserInfoListByCurrentUserWithoutZoUser")]
        public async Task<IActionResult> GetUserInfoListByCurrentUserWithoutZoUser()
        {
            try
            {
                var result = await _commonSvc.GetUserInfoListByCurrentUserWithoutZoUser();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
