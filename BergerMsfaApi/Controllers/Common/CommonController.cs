﻿using System;
using System.Collections.Generic;
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
       
    }
}
