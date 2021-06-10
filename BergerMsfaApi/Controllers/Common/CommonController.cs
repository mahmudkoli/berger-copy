using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Services.Brand.Interfaces;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Common
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonSvc;
        private readonly IBrandService _brandService;

        public CommonController(
            ICommonService commonSvc, 
            IBrandService brandService)
        {
            _commonSvc = commonSvc;
            _brandService = brandService;
        }

        [HttpGet("GetDealList")]
        public async Task<IActionResult> GetDealerList()
        {
            try
            {
                var result = await _commonSvc.GetDealerInfoList();
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

        [HttpGet("GetRoleList")]
        public async Task<IActionResult> GetRoleList()
        {
            try
            {
                var result = await _commonSvc.GetRoleList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetUserInfoList")]
        public async Task<IActionResult> GetUserInfoList()
        {
            try
            {
                var result = await _commonSvc.GetUserInfoList();
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

        [HttpGet("GetPainterList")]
        public async Task<IActionResult> GetPainterList()
        {
            try
            {
                var result = await _commonSvc.GetPainterList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetMonthList")]
        public async Task<IActionResult> GetMonthList()
        {
            try
            {
                var result = _commonSvc.GetMonthList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetYearList")]
        public async Task<IActionResult> GetYearList()
        {
            try
            {
                var result = _commonSvc.GetYearList();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetBrandDropDown")]
        public async Task<IActionResult> GetBrandDropDown()
        {
            try
            {
                var result = await _brandService.GetBrandFamilyDropDownAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetMaterialGroupOrBrand")]
        public async Task<IActionResult> GetMaterialGroupOrBrand()
        {
            try
            {
                var result = await _brandService.GetBrandDropDownAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetActivitySummaryDropDown")]
        public IActionResult GetActivitySummaryDropDown()
        {
            try
            {
                var list = new List<KeyValuePairObjectModel>
                {
                    new KeyValuePairObjectModel()
                    {
                        Text = "JOURNEY PLAN",
                        Value = "JOURNEY PLAN"
                    }, new KeyValuePairObjectModel()
                    {
                        Text = "SALES CALL- SUB DEALER",
                        Value = "SALES CALL- SUB DEALER"
                    }, new KeyValuePairObjectModel()
                    {
                        Text = "SALES CALL- DIRECT DEALER",
                        Value = "SALES CALL- DIRECT DEALER"
                    }, new KeyValuePairObjectModel()
                    {
                        Text = "PAINTER CALL",
                        Value = "PAINTER CALL"
                    },
                    new KeyValuePairObjectModel()
                    {
                        Text = "PAINTER REGISTRATION",
                        Value = "PAINTER REGISTRATION"
                    },   new KeyValuePairObjectModel()
                    {
                        Text = "AD HOC VISIT IN DEALERS POINT",
                        Value = "AD HOC VISIT IN DEALERS POINT"
                    },   new KeyValuePairObjectModel()
                    {
                        Text = "LEAD GENERATION",
                        Value = "LEAD GENERATION"
                    },   new KeyValuePairObjectModel()
                    {
                        Text = "LEAD FOLLOWUP",
                        Value = "LEAD FOLLOWUP"
                    }, new KeyValuePairObjectModel()
                    {
                        Text = "TOTAL COLLECTION VALUE",
                        Value = "TOTAL COLLECTION VALUE"
                    },
                }.OrderBy(x => x.Text).ToList();
                return OkResult(list);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
