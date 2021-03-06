using BergerMsfaApi.Models.Report;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Berger.Odata.Services;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Services.KPI.interfaces;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Berger.Odata.Model;

namespace BergerMsfaApi.Controllers.Odata
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppKpiReportController : BaseController
    {
        private readonly IKpiDataService _kpiDataService;
        private readonly IKPIReportService _kpiReportService;
        private readonly IUniverseReachAnalysisService _universeReachAnalysisService;
        private readonly INewDealerDevelopmentService _newDealerDevelopmentService;
        
        public AppKpiReportController(IKpiDataService kpiDataService, IKPIReportService kpiReportService, 
          IUniverseReachAnalysisService universeReachAnalysisService, INewDealerDevelopmentService newDealerDevelopmentService)
        {
            _kpiDataService = kpiDataService;
            _kpiReportService = kpiReportService;
            _universeReachAnalysisService = universeReachAnalysisService;
            _newDealerDevelopmentService = newDealerDevelopmentService;
        }

        [HttpGet("GetSalesTargetAchievement")]
        public async Task<IActionResult> GetTargetAchievement([FromQuery] SalesTargetAchievementSearchModel model)
        {
            try
            {
                model.ForApp = true;
                var data = await _kpiDataService.GetAppSalesTargetAchievement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerWiseTargetAchievement")]
        public async Task<IActionResult> GetDealerWiseTargetAchievement([FromQuery] DealerWiseTargetAchievementSearchModel model)
        {
            try
            {
                model.ForApp = true;
                var data = await _kpiDataService.GetAppDealerWiseTargetAchievement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetProductWiseTargetAchievement")]
        public async Task<IActionResult> GetProductWiseTargetAchivement([FromQuery] ProductWiseTargetAchievementSearchModel model)
        {
            try
            {
                model.ForApp = true;
                var data = await _kpiDataService.GetAppProductWiseTargetAchievement(model);
                return OkResult(data);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetBusinessCallAnalysis")]
        [ProducesResponseType(typeof(IList<BusinessCallAPPKPIReportResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBusinessCallKpiReport([FromQuery] BusinessCallKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetBusinessCallKPIReportAsync(model,EnumReportFor.App);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetPremiumBrandBillingStrikeRate")]
        [ProducesResponseType(typeof(IList<StrikeRateKPIReportResultModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPremiumBrandBillingStrikeRateKPIReport([FromQuery] StrikeRateKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.PremiumBrandBillingStrikeRateKPIReportAsync(model,EnumReportFor.App);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetBillingAnalysis")]
        public async Task<IActionResult> GetBillingAnalysisKPIReport([FromQuery] BillingAnalysisKPIReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetBillingAnalysisKPIReportAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetUniverseReachAnalysis")]
        public async Task<IActionResult> GetUniverseReachAnalysisReportReport([FromQuery] UniverseReachAnalysisReportSearchModel model)
        {
            try
            {
                model.ForApp = true;
                var result = await _universeReachAnalysisService.GetUniverseReachAnalysisReportAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetColorBankInstallationPlanVsActual")]
        [ProducesResponseType(typeof(IList<ColorBankInstallationPlanVsActualKPIReportResultModel>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetColorBankInstallationPlanVsActual([FromQuery] ColorBankInstallationPlanVsActualKpiReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetColorBankInstallationPlanVsActual(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetColorBankProductivity")]
        [ProducesResponseType(typeof(IList<ColorBankProductivityBase>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetColorBankProductivity([FromQuery] ColorBankProductivityKpiReportSearchModel model)
        {
            try
            {
                var result = await _kpiReportService.GetColorBankProductivity(model,EnumReportFor.App);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpGet("GetFinancialCollectionPlan")]
        public async Task<IActionResult> GetFinancialCollectionPlanKPIReport([FromQuery] CollectionPlanKPIReportSearchModelForApp model)
        {
            try
            {
                var result = await _kpiReportService.GetFinancialCollectionPlanKPIReportForAppAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        // for app get to update data
        [HttpGet("GetDealerConversionData")]
        public async Task<IActionResult> GetDealerConversionData([FromQuery] SearchNewDealerDevelopment model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetDealerConversionByYearAsync(model);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        // for app save data
        [HttpPost("SaveDealerConversion")]
        public async Task<IActionResult> SaveDealerConversion(IList<NewDealerDevelopmentSaveModel> model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.AddDealerConversionAsync(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDealerOpeningStatusReport")]
        public async Task<IActionResult> GetDealerOpeningStatusReport([FromQuery] SearchNewDealerDevelopment model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetNewDealerDevelopmentReport(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("GetDealerConversionReport")]
        public async Task<IActionResult> GetDealerConversionReport([FromQuery] SearchNewDealerDevelopment model)
        {
            try
            {
                var result = await _newDealerDevelopmentService.GetDealerConversionReport(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
