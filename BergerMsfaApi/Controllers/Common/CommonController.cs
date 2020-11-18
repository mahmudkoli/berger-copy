using System;
using System.Threading.Tasks;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Common
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonSvc;
        public CommonController(
            ICommonService commonSvc)
        {
            _commonSvc = commonSvc;
        }
        [HttpGet("GetDealList/{territory}")]
        public async Task<IActionResult> GetDealerList(string territory)
        {
            try
            {
                var result = await _commonSvc.GetDealerInfoList(territory);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpGet("GetSaleOfficeList")]
        public async Task<IActionResult> GetSaleOfficeList()
        {
            try
            {
                var result =await _commonSvc.GetSaleOfficeList();
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
        [HttpGet("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList()
        {
            try
            {
                var result = await _commonSvc.GetEmployeeList();
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
                var result = await _commonSvc.GetDealerList();
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpGet("GetDealerListByCode/{code}")]

        public async Task<IActionResult> GetDealerListByCode(string code)
        {
            try
            {  
                var result = await  _commonSvc.GetDealerListByCode(code);
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        
    }
}
