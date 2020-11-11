using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerSalesCall
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerSalesCallController : BaseController
    {
        private readonly IEnumService _enumService;
        private readonly IDealerSalesCallService _dealerSalesCallService;

        public AppDealerSalesCallController(
                IEnumService enumService,
                IDealerSalesCallService dealerSalesCallService
            )
        {
            this._enumService = enumService;
            this._dealerSalesCallService = dealerSalesCallService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _dealerSalesCallService.GetAllAsync(1, int.MaxValue);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _dealerSalesCallService.GetByIdAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SaveDealerSalesCallModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationResult(ModelState);
            }

            try
            {
                var result = await _dealerSalesCallService.AddAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        #region Helper Get data
        [HttpGet("GetRatingsSelect")]
        public async Task<IActionResult> GetRatingsSelect()
        {
            try
            {
                var result = _enumService.GetRatingsSelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetPrioritySelect")]
        public async Task<IActionResult> GetPrioritySelect()
        {
            try
            {
                var result = _enumService.GetPrioritySelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetSatisfactionSelect")]
        public async Task<IActionResult> GetSatisfactionSelect()
        {
            try
            {
                var result = _enumService.GetSatisfactionSelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetProductLiftingSelect")]
        public async Task<IActionResult> GetProductLiftingSelect()
        {
            try
            {
                var result = _enumService.GetProductLiftingSelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerSalesIssueSelect")]
        public async Task<IActionResult> GetDealerSalesIssueSelect()
        {
            try
            {
                var result = _enumService.GetDealerSalesIssueSelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetPainterInfluenceSelect")]
        public async Task<IActionResult> GetPainterInfluenceSelect()
        {
            try
            {
                var result = _enumService.GetPainterInfluenceSelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetSubDealerInfluenceSelect")]
        public async Task<IActionResult> GetSubDealerInfluenceSelect()
        {
            try
            {
                var result = _enumService.GetSubDealerInfluenceSelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetCompetitionPresenceSelect")]
        public async Task<IActionResult> GetCompetitionPresenceSelect()
        {
            try
            {
                var result = _enumService.GetCompetitionPresenceSelect();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        #endregion
    }
}
