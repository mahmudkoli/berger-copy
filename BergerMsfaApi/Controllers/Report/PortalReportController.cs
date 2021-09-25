using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Services.Excel.Interface;

namespace BergerMsfaApi.Controllers.Report
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class PortalReportController : BaseController
    {
        private readonly IPortalReportService _portalReportService;
        private readonly ICommonService _commonService;
        private readonly IExcelReaderService _excelReaderService;

        public PortalReportController(
                IPortalReportService portalReportService,
                ICommonService commonService,
                IExcelReaderService excelReaderService)
        {
            _portalReportService = portalReportService;
            _commonService = commonService;
            _excelReaderService = excelReaderService;
        }

        [HttpGet("GetLeadSummary")]
        public async Task<IActionResult> GetLeadSummary([FromQuery] LeadSummaryReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadSummaryReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadSummary")]
        public async Task<IActionResult> DownloadLeadSummary([FromQuery] LeadSummaryReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadSummaryReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetLeadGenerationDetails")]
        public async Task<IActionResult> GetLeadGenerationDetails([FromQuery] LeadGenerationDetailsReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadGenerationDetailsReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadGenerationDetails")]
        public async Task<IActionResult> DownloadLeadGenerationDetails([FromQuery] LeadGenerationDetailsReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadGenerationDetailsReportAsync(query);
                IList<LeadGenerationDetailsReportResultModel> leadGenerationDetailsReportResultModels = result.Items;

                var imageUrl = new Dictionary<string, string>() { { nameof(LeadGenerationDetailsReportResultModel.ImageUrl), "Image Url" } };
                var columns = new Dictionary<string, string>()
                {
                    {nameof(LeadGenerationDetailsReportResultModel.UserId), "User Id"},
                    {nameof(LeadGenerationDetailsReportResultModel.ProjectCode), "Project Code"},
                    {nameof(LeadGenerationDetailsReportResultModel.ProjectName), "Project Name"},
                    {nameof(LeadGenerationDetailsReportResultModel.Depot), "Depot"},
                    {nameof(LeadGenerationDetailsReportResultModel.DepotName), "Depot Name"},
                    {nameof(LeadGenerationDetailsReportResultModel.Territory), "Territory"},
                    {nameof(LeadGenerationDetailsReportResultModel.Zone), "Zone"},
                    {nameof(LeadGenerationDetailsReportResultModel.LeadCreatedDate), "Lead Created Date"},
                    {nameof(LeadGenerationDetailsReportResultModel.TypeOfClient), "Type Of Client"},
                    {nameof(LeadGenerationDetailsReportResultModel.ProjectAddress), "Project Address"},
                    {nameof(LeadGenerationDetailsReportResultModel.KeyContactPersonName), "Key Contact Person Name"},
                    {nameof(LeadGenerationDetailsReportResultModel.KeyContactPersonMobile), "Key Contact Person Mobile"},
                    {nameof(LeadGenerationDetailsReportResultModel.PaintContractorName), "Paint Contractor Name"},
                    {nameof(LeadGenerationDetailsReportResultModel.PaintContractorMobile), "Paint Contractor Mobile"},
                    {nameof(LeadGenerationDetailsReportResultModel.PaintingStage), "Painting Stage"},
                    {nameof(LeadGenerationDetailsReportResultModel.ExpectedDateOfPainting), "Expected Date Of Painting"},
                    {nameof(LeadGenerationDetailsReportResultModel.NumberOfStoriedBuilding), "Number Of Storied Building"},
                    {nameof(LeadGenerationDetailsReportResultModel.TotalPaintingAreaSqftInterior), "Total Painting Area Sqft Interior"},
                    {nameof(LeadGenerationDetailsReportResultModel.ExpectedValue), "Expected Value"},
                    {nameof(LeadGenerationDetailsReportResultModel.ExpectedMonthlyBusinessValue), "Expected Monthly Business Value"},
                    {nameof(LeadGenerationDetailsReportResultModel.RequirementOfColorScheme), "Requirement Of ColorScheme"},
                    {nameof(LeadGenerationDetailsReportResultModel.ProductSamplingRequired), "Product Sampling Required"},
                    {nameof(LeadGenerationDetailsReportResultModel.NextFollowUpDate), "Next Follow Up Date"},
                    {nameof(LeadGenerationDetailsReportResultModel.Remarks), "Remarks"},
                    {nameof(LeadGenerationDetailsReportResultModel.ImageUrl), "Image Url"},
                    {nameof(LeadGenerationDetailsReportResultModel.OtherClientName), "Other Client Name"},
                };


                var data = await _excelReaderService.GetExcelWithImage("LeadGenerationDetailsReport.xlsx", "data", leadGenerationDetailsReportResultModels, columns, imageUrl);
                return data;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetLeadFollowUpDetails")]
        public async Task<IActionResult> GetLeadFollowUpDetails([FromQuery] LeadFollowUpDetailsReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadFollowUpDetailsReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadFollowUpDetails")]
        public async Task<IActionResult> DownloadLeadFollowUpDetails([FromQuery] LeadFollowUpDetailsReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadFollowUpDetailsReportAsync(query);


                var imageUrl = new Dictionary<string, string>() { { nameof(LeadFollowUpDetailsReportResultModel.ImageUrl), "Image Url" } };
                var columns = new Dictionary<string, string>()
                {
                    {nameof(LeadFollowUpDetailsReportResultModel.UserId), "User Id"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ProjectCode), "Project Code"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ProjectName), "Project Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.Depot), "Depot"},
                    {nameof(LeadFollowUpDetailsReportResultModel.DepotName), "Depot Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.Territory), "Territory"},
                    {nameof(LeadFollowUpDetailsReportResultModel.Zone), "Zone"},
                    {nameof(LeadFollowUpDetailsReportResultModel.PlanVisitDatePlan), "Plan Visit Date"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualVisitDate), "Actual Visit Date"},
                    {nameof(LeadFollowUpDetailsReportResultModel.TypeOfClient), "Type Of Client"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ProjectAddress), "Project Address"},
                    {nameof(LeadFollowUpDetailsReportResultModel.KeyContactPersonName), "Key Contact Person Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.KeyContactPersonMobile), "Key Contact Person Mobile"},
                    {nameof(LeadFollowUpDetailsReportResultModel.PaintContractorName), "Paint Contractor Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.PaintContractorMobile), "Paint Contractor Mobile"},
                    {nameof(LeadFollowUpDetailsReportResultModel.NumberOfStoriedBuilding), "Number Of Storied Building"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ExpectedValue), "Expected Value"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ExpectedMonthlyBusinessValue), "Expected Monthly Business Value"},
                    {nameof(LeadFollowUpDetailsReportResultModel.SwappingCompetition), "Swapping Competition"},
                    {nameof(LeadFollowUpDetailsReportResultModel.SwappingCompetitionAnotherCompetitorName), "Swapping Competition Another Competitor Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.UpTradingFromBrandName), "Up Trading From Brand Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.BrandUsedInteriorBrandName), "Brand Used Interior Brand Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.BrandUsedExteriorBrandName), "Brand Used Exterior Brand Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.BrandUsedUnderCoatBrandName), "Brand Used Under Coat Brand Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.BrandUsedTopCoatBrandName), "Brand Used Top Coat Brand Name"},
                    {nameof(LeadFollowUpDetailsReportResultModel.TotalPaintingAreaSqftInterior), "Total Painting Area Sqft Interior"},
                    {nameof(LeadFollowUpDetailsReportResultModel.TotalPaintingAreaSqftExterior), "Total Painting Area Sqft Exterior"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualPaintJobCompletedInterior), "Actual Paint Job Completed Interior"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualPaintJobCompletedExterior), "Actual Paint Job Completed Exterior"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualVolumeSoldInteriorLitre), "Actual Volume Sold Interior Liter"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualVolumeSoldInteriorKg), "Actual Volume Sold Interior Kg"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualVolumeSoldExteriorLitre), "Actual Volume Sold Exterior Liter"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualVolumeSoldExteriorKg), "Actual Volume Sold Exterior Kg"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualVolumeSoldUnderCoatGallon), "Actual Volume Sold Under Coat Gallon"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ActualVolumeSoldTopCoatGallon), "Actual Volume Sold Top Coat Gallon"},
                    {nameof(LeadFollowUpDetailsReportResultModel.BergerValueSales), "Berger Value Sales"},
                    {nameof(LeadFollowUpDetailsReportResultModel.BergerPremiumBrandSalesValue), "Berger Premium Brand Sales Value"},
                    {nameof(LeadFollowUpDetailsReportResultModel.CompetitionValueSales), "Competition Value Sales"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ProductSourcing), "Product Sourcing"},
                    {nameof(LeadFollowUpDetailsReportResultModel.IsColorSchemeGiven), "Is Color Scheme Given"},
                    {nameof(LeadFollowUpDetailsReportResultModel.IsProductSampling), "Is Product Sampling"},
                    {nameof(LeadFollowUpDetailsReportResultModel.NextVisitDate), "Next Visit Date"},
                    {nameof(LeadFollowUpDetailsReportResultModel.Comments), "Comments"},
                    {nameof(LeadFollowUpDetailsReportResultModel.ImageUrl), "Image Url"},
                };


                var data = await _excelReaderService.GetExcelWithImage("LeadFollowUpDetailsReport.xlsx", "data", result.Items, columns, imageUrl);
                return data;

               // return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetLeadBusinessUpdate")]
        public async Task<IActionResult> GetLeadBusinessUpdate([FromQuery] LeadBusinessReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLeadBusinessUpdateReportAsync(query);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLeadBusinessUpdate")]
        public async Task<IActionResult> DownloadLeadBusinessUpdate([FromQuery] LeadBusinessReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLeadBusinessUpdateReportAsync(query);

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region Nasir
        [HttpGet("GetPainterRegistration")]
        public async Task<IActionResult> GetPainterRegistration([FromQuery] PainterRegistrationReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetPainterRegistrationReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadPainterRegistration")]
        public async Task<IActionResult> DownloadPainterRegistration([FromQuery] PainterRegistrationReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetPainterRegistrationReportAsync(query);
                
                var imageUrl = new Dictionary<string, string>() { { nameof(PainterRegistrationReportResultModel.PainterImageUrl), "Painter Image" } };
                var columns = new Dictionary<string, string>()
                {
                    {nameof(PainterRegistrationReportResultModel.UserId), nameof(PainterRegistrationReportResultModel.UserId).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.PainterId), nameof(PainterRegistrationReportResultModel.PainterId).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.PainerRegistrationDate), nameof(PainterRegistrationReportResultModel.PainerRegistrationDate).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.TypeOfPainer), nameof(PainterRegistrationReportResultModel.TypeOfPainer).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.DepotName), nameof(PainterRegistrationReportResultModel.DepotName).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.SalesGroup), nameof(PainterRegistrationReportResultModel.SalesGroup).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.Territory), nameof(PainterRegistrationReportResultModel.Territory).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.Zone), nameof(PainterRegistrationReportResultModel.Zone).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.PainterName), nameof(PainterRegistrationReportResultModel.PainterName).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.PainterAddress), nameof(PainterRegistrationReportResultModel.PainterAddress).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.MobileNumber), nameof(PainterRegistrationReportResultModel.MobileNumber).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.NoOfPaintingAttached), nameof(PainterRegistrationReportResultModel.NoOfPaintingAttached).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.DBBLRocketAccountStatus), nameof(PainterRegistrationReportResultModel.DBBLRocketAccountStatus).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.AccountNumber), nameof(PainterRegistrationReportResultModel.AccountNumber).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.AccountHolderName), nameof(PainterRegistrationReportResultModel.AccountHolderName).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.IdentificationNo), "NID/Passport/Birth Certificate No"},
                    {nameof(PainterRegistrationReportResultModel.AttachedTaggedDealerId), nameof(PainterRegistrationReportResultModel.AttachedTaggedDealerId).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.AttachedTaggedDealerName), nameof(PainterRegistrationReportResultModel.AttachedTaggedDealerName).AddSpacesToSentence(true)},
                    {nameof(PainterRegistrationReportResultModel.APPInstalledStatus), "Shamparka APP Installed Status"},
                    {nameof(PainterRegistrationReportResultModel.APPNotInstalledReason), "Shamparka App Not Installed \"Reason\""},
                    {nameof(PainterRegistrationReportResultModel.AverageMonthlyUse), "Average Monthly Use (Value)"},
                    {nameof(PainterRegistrationReportResultModel.BergerLoyalty), "Berger Loyalty %"},
                    {nameof(PainterRegistrationReportResultModel.PainterImageUrl), "Painter Image"},
                };


                var data = await _excelReaderService.GetExcelWithImage("PainterRegistration.xlsx", "data", result.Items, columns, imageUrl);
                return data;
                //return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerOpening")]
        public async Task<IActionResult> GetDealerOpening([FromQuery] DealerOpeningReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerOpeningReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //[HttpGet("DownloadDealerOpening")]
        //public async Task<IActionResult> DownloadDealerOpening([FromQuery] DealerOpeningReportSearchModel query)
        //{
        //    try
        //    {
        //        query.Page = 1;
        //        query.PageSize = int.MaxValue;
        //        var result = await _portalReportService.GetDealerOpeningReportAsync(query);

        //        _commonService.SetEmptyString(result.Items.ToList(), 
        //            nameof(DealerOpeningReportResultModel.DealershipOpeningApplicationForm),
        //            nameof(DealerOpeningReportResultModel.NomineePhotograph),
        //            nameof(DealerOpeningReportResultModel.PhotographOfproprietor),
        //            nameof(DealerOpeningReportResultModel.Cheque),
        //            nameof(DealerOpeningReportResultModel.TradeLicensee),
        //            nameof(DealerOpeningReportResultModel.IdentificationNo),
        //            nameof(DealerOpeningReportResultModel.NomineeIdentificationNo));

        //        return Ok(result.Items);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        [HttpGet("DownloadDealerOpening")]
        public async Task<IActionResult> DownloadDealerOpening([FromQuery] DealerOpeningReportSearchModel query)
        {
            string sFileName = @"DealerOpeningReport.xlsx";

            try
            {
                if (string.IsNullOrEmpty(query.Depot))
                {
                    throw new Exception("Please select mandatory field.");
                }

                query.Page = 1;
                query.PageSize = int.MaxValue;
                var dataResult = await _portalReportService.GetDealerOpeningReportAsync(query);

                var data = await _excelReaderService.DealerOpeningWriteToFileWithImage(dataResult.Items);

                var result = File(data,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                sFileName);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerCollection")]
        public async Task<IActionResult> GetDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerCollection")]
        public async Task<IActionResult> DownloadDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSubDealerCollection")]
        public async Task<IActionResult> GetSubDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetSubDealerCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSubDealerCollection")]
        public async Task<IActionResult> DownloadSubDealerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetSubDealerCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetCustomerCollection")]
        public async Task<IActionResult> GetCustomerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetCustomerCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadCustomerCollection")]
        public async Task<IActionResult> DownloadCustomerCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetCustomerCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetProjectCollection")]
        public async Task<IActionResult> GetProjectCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDirectProjectCollectionReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadProjectCollection")]
        public async Task<IActionResult> DownloadProjectCollection([FromQuery] CollectionReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDirectProjectCollectionReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetPaintersCall")]
        public IActionResult GetPaintersCall([FromQuery] PainterCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetPainterCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadPaintersCall")]
        public IActionResult DownloadPaintersCall([FromQuery] PainterCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetPainterCallReportBySp(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerVisit")]
        public async Task<IActionResult> GetDealerVisit([FromQuery] DealerVisitReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerVisitReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerVisit")]
        public async Task<IActionResult> DownloadDealerVisit([FromQuery] DealerVisitReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerVisitReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerSalesCall")]
        public IActionResult GetDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetDealerSalesCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerSalesCall")]
        public async Task<IActionResult> DownloadDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerSalesCallReportBySp(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSubDealerSalesCall")]
        public IActionResult GetSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetSubDealerSalesCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSubDealerSalesCall")]
        public IActionResult DownloadSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetSubDealerSalesCallReportBySp(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAddhocDealerSalesCall")]
        public IActionResult GetAddhocDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetAddhocDealerSalesCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadAddhocDealerSalesCall")]
        public IActionResult DownloadAddhocDealerSalesCall([FromQuery] DealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetAddhocDealerSalesCallReportBySp(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAddhocSubDealerSalesCall")]
        public IActionResult GetAddhocSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetAddhocSubDealerSalesCallReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadAddhocSubDealerSalesCall")]
        public IActionResult DownloadAddhocSubDealerSalesCall([FromQuery] SubDealerSalesCallReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = _portalReportService.GetAddhocSubDealerSalesCallReportBySp(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDealerIssue")]
        public async Task<IActionResult> GetDealerIssue([FromQuery] DealerIssueReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetDealerIssueReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadDealerIssue")]
        public async Task<IActionResult> DownloadDealerIssue([FromQuery] DealerIssueReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetDealerIssueReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSubDealerIssue")]
        public async Task<IActionResult> GetSubDealerIssue([FromQuery] SubDealerIssueReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetSubDealerIssueReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadSubDealerIssue")]
        public async Task<IActionResult> DownloadSubDealerIssue([FromQuery] SubDealerIssueReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetSubDealerIssueReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetTintingMachine")]
        public async Task<IActionResult> GetTintingMachine([FromQuery] TintingMachineReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetTintingMachineReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadTintingMachine")]
        public async Task<IActionResult> DownloadTintingMachine([FromQuery] TintingMachineReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetTintingMachineReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetActiveSummery")]
        public async Task<IActionResult> GetActiveSummery([FromQuery] ActiveSummeryReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetActiveSummeryReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadActiveSummery")]
        public async Task<IActionResult> DownloadActiveSummery([FromQuery] ActiveSummeryReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetActiveSummeryReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetLogInReport")]
        public async Task<IActionResult> GetLogInReport([FromQuery] LogInReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetLogInReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadLogInReport")]
        public async Task<IActionResult> DownloadLogInReport([FromQuery] LogInReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetLogInReportAsync(query);

                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetSnapShotReport")]
        public IActionResult GetSnapShotReport([FromQuery] MerchendizingSnapShotReportSearchModel query)
        {
            try
            {
                var result = _portalReportService.GetSnapShotReportBySp(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //[HttpGet("DownloadSnapShotReport")]
        //public IActionResult DownloadSnapShotReport([FromQuery] MerchendizingSnapShotReportSearchModel query)
        //{
        //    try
        //    {
        //        query.Page = 1;
        //        query.PageSize = int.MaxValue;
        //        var result = _portalReportService.GetSnapShotReportBySp(query);

        //        _commonService.SetEmptyString(result.Items.ToList(),
        //            nameof(MerchendizingSnapShotReportResultModel.CompetitionDisplay),
        //            nameof(MerchendizingSnapShotReportResultModel.GlowSignBoard),
        //            nameof(MerchendizingSnapShotReportResultModel.ProductDisplay),
        //            nameof(MerchendizingSnapShotReportResultModel.Scheme),
        //            nameof(MerchendizingSnapShotReportResultModel.Brochure),
        //            nameof(MerchendizingSnapShotReportResultModel.Others));

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        [HttpGet("DownloadSnapShotReport")]
        public async Task<IActionResult> DownloadSnapShotReport([FromQuery] MerchendizingSnapShotReportSearchModel query)
        {
            string sFileName = @"SnapShotResult.xlsx";

            try
            {
                if (string.IsNullOrEmpty(query.Depot))
                {
                    throw new Exception("Please select mandatory field.");

                }

                dynamic datatabledata = _portalReportService.GetSnapShotReportBySp(query);

                var data = await _excelReaderService.WriteToFileWithImage(datatabledata);

                var result = File(
                data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                sFileName);

                return result;

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpGet("GetInactivePainters")]
        public async Task<IActionResult> GetInactivePainters([FromQuery] InactivePainterReportSearchModel query)
        {
            try
            {
                var result = await _portalReportService.GetInactivePainterReportAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("DownloadInactivePainters")]
        public async Task<IActionResult> DownloadInactivePainters([FromQuery] InactivePainterReportSearchModel query)
        {
            try
            {
                query.Page = 1;
                query.PageSize = int.MaxValue;
                var result = await _portalReportService.GetInactivePainterReportAsync(query);
                return Ok(result.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
