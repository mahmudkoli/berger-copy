using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.Report.Interfaces;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Services.Report.Implementation
{
    public class PortalReportService : IPortalReportService
    {
        private readonly IRepository<LeadGeneration> _leadGenerationRepository;
        private readonly IRepository<LeadFollowUp> _leadFollowUpRepository;
        private readonly IRepository<DSC.DealerSalesCall> _dealerSalesCallRepository;
        private readonly IDropdownService _dropdownService;
        private readonly IMapper _mapper;

        public PortalReportService(
                IRepository<LeadGeneration> leadGenerationRepository,
                IRepository<LeadFollowUp> leadFollowUpRepository,
                IRepository<DSC.DealerSalesCall> dealerSalesCallRepository,
                IDropdownService dropdownService,
                IMapper mapper
            )
        {
            this._leadGenerationRepository = leadGenerationRepository;
            this._leadFollowUpRepository = leadFollowUpRepository;
            this._dealerSalesCallRepository = dealerSalesCallRepository;
            this._dropdownService = dropdownService;
            this._mapper = mapper;
        }

        public async Task<QueryResultModel<LeadSummaryReportResultModel>> GetLeadSummaryReportAsync(LeadSummaryReportSearchModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            {
                ["createdTime"] = v => v.CreatedTime,
            };

            var reportResult = new List<LeadSummaryReportResultModel>();

            var leads = await _leadGenerationRepository.GetAllIncludeAsync(x => x,
                            x => (!query.UserId.HasValue || x.UserId == query.UserId.Value)
                                && (!query.EmployeeRole.HasValue || x.User.EmployeeRole == query.EmployeeRole.Value)
                                && (string.IsNullOrWhiteSpace(query.Depot) || x.Depot == query.Depot)
                                && (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(x.Zone))
                                && (!query.FromDate.HasValue || x.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || x.CreatedTime.Date <= query.ToDate.Value.Date),
                            x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                            x => x.Include(i => i.User)
                                .Include(i => i.LeadFollowUps).ThenInclude(i => i.ProjectStatus)
                                .Include(i => i.LeadFollowUps).ThenInclude(i => i.BusinessAchievement),
                            //query.Page,
                            //query.PageSize,
                            true);

            var groupOfLeads = leads.GroupBy(x => new { x.UserId, x.Territory, x.Zone });

            reportResult = groupOfLeads.Select(x =>
            {
                var reportModel = new LeadSummaryReportResultModel();
                var leadFollowUps = x.SelectMany(x => x.LeadFollowUps);
                reportModel.UserId = x.FirstOrDefault()?.User?.Email ?? string.Empty;
                reportModel.Territory = x.Key.Territory;
                reportModel.Zone = x.Key.Zone;
                reportModel.NoOfLeadGenerate = x.Count();
                reportModel.NoOfLeadFollowUp = leadFollowUps.Count();
                reportModel.TotalCall = reportModel.NoOfLeadGenerate + reportModel.NoOfLeadFollowUp;
                reportModel.NoOfUnderConstructionLead = leadFollowUps.Count(p => (p.ProjectStatus?.DropdownName ?? string.Empty) == ConstantsLeadValue.ProjectStatusUnderConstruction);
                reportModel.NoOfGoingPaintLead = leadFollowUps.Count(p => (p.ProjectStatus?.DropdownName ?? string.Empty) == ConstantsLeadValue.ProjectStatusPaintingOngoing);
                reportModel.NoOfTotalWinLead = leadFollowUps.Count(p => (p.ProjectStatus?.DropdownName ?? string.Empty) == ConstantsLeadValue.ProjectStatusLeadCompletedTotalWin);
                reportModel.NoOfTotalLossLead = leadFollowUps.Count(p => (p.ProjectStatus?.DropdownName ?? string.Empty) == ConstantsLeadValue.ProjectStatusLeadCompletedTotalLoss);
                reportModel.NoOfPartialBusinessLead = leadFollowUps.Count(p => (p.ProjectStatus?.DropdownName ?? string.Empty) == ConstantsLeadValue.ProjectStatusLeadCompletedPartialBusiness);
                reportModel.NoOfCompetitionSnatchLead = leadFollowUps.Count(p => p.SwappingCompetitionId > 0); //TODO: need to change to actual logic
                reportModel.BergerValueSales = leadFollowUps.Sum(p => p.BusinessAchievement?.BergerValueSales ?? (decimal)0);
                reportModel.BergerPremiumBrandValueSales = leadFollowUps.Sum(p => p.BusinessAchievement?.BergerPremiumBrandSalesValue ?? (decimal)0);
                reportModel.CompetitionValueSales = leadFollowUps.Sum(p => p.BusinessAchievement?.CompetitionValueSales ?? (decimal)0);
                reportModel.NoOfColorSchemeGiven = leadFollowUps.Count(p => p.BusinessAchievement?.IsColorSchemeGiven ?? false);
                reportModel.NoOfProductSampling = leadFollowUps.Count(p => p.BusinessAchievement?.IsProductSampling ?? false);
                return reportModel;
            }).ToList();

            var queryResult = new QueryResultModel<LeadSummaryReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = reportResult.Count();
            queryResult.Total = reportResult.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<LeadGenerationDetailsReportResultModel>> GetLeadGenerationDetailsReportAsync(LeadGenerationDetailsReportSearchModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            {
                ["createdTime"] = v => v.CreatedTime,
            };

            var reportResult = new List<LeadGenerationDetailsReportResultModel>();

            var leads = await _leadGenerationRepository.GetAllIncludeAsync(x => x,
                            x => (!query.UserId.HasValue || x.UserId == query.UserId.Value)
                                && (!query.EmployeeRole.HasValue || x.User.EmployeeRole == query.EmployeeRole.Value)
                                && (string.IsNullOrWhiteSpace(query.Depot) || x.Depot == query.Depot)
                                && (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(x.Zone))
                                && (!query.FromDate.HasValue || x.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || x.CreatedTime.Date <= query.ToDate.Value.Date)
                                && (string.IsNullOrWhiteSpace(query.ProjectName) || x.ProjectName.Contains(query.ProjectName))
                                && (!query.PaintingStageId.HasValue || x.PaintingStageId == query.PaintingStageId.Value),
                            x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                            x => x.Include(i => i.User).Include(i => i.TypeOfClient).Include(i => i.PaintingStage),
                            query.Page,
                            query.PageSize,
                            true);

            reportResult = leads.Items.Select(x =>
            {
                var reportModel = new LeadGenerationDetailsReportResultModel();
                reportModel.UserId = x.User?.Email ?? string.Empty;
                reportModel.ProjectCode = x.Code;
                reportModel.ProjectName = x.ProjectName;
                reportModel.Depot = x.Depot;
                //reportModel.DepotName = x.DepotName;
                reportModel.Territory = x.Territory;
                reportModel.Zone = x.Zone;
                reportModel.LeadCreatedDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime);
                reportModel.TypeOfClient = x.TypeOfClient?.DropdownName ?? string.Empty;
                reportModel.ProjectAddress = x.ProjectAddress;
                reportModel.KeyContactPersonName = x.KeyContactPersonName;
                reportModel.KeyContactPersonMobile = x.KeyContactPersonMobile;
                reportModel.PaintContractorName = x.PaintContractorName;
                reportModel.PaintContractorMobile = x.PaintContractorMobile;
                reportModel.PaintContractorMobile = x.PaintContractorMobile;
                reportModel.PaintingStage = x.PaintingStage?.DropdownName ?? string.Empty;
                reportModel.ExpectedDateOfPainting = CustomConvertExtension.ObjectToDateString(x.ExpectedDateOfPainting);
                reportModel.NumberOfStoriedBuilding = x.NumberOfStoriedBuilding;
                reportModel.TotalPaintingAreaSqftInterior = x.TotalPaintingAreaSqftInterior;
                reportModel.TotalPaintingAreaSqftExterior = x.TotalPaintingAreaSqftExterior;
                reportModel.ExpectedValue = x.ExpectedValue;
                reportModel.ExpectedMonthlyBusinessValue = x.ExpectedMonthlyBusinessValue;
                reportModel.RequirementOfColorScheme = x.RequirementOfColorScheme ? "YES" : "NO";
                reportModel.ProductSamplingRequired = x.ProductSamplingRequired ? "YES" : "NO";
                reportModel.NextFollowUpDate = CustomConvertExtension.ObjectToDateString(x.NextFollowUpDate);
                reportModel.Remarks = x.Remarks;
                reportModel.ImageUrl = x.PhotoCaptureUrl;
                return reportModel;
            }).ToList();

            var queryResult = new QueryResultModel<LeadGenerationDetailsReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = reportResult.Count();
            queryResult.Total = reportResult.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<LeadFollowUpDetailsReportResultModel>> GetLeadFollowUpDetailsReportAsync(LeadFollowUpDetailsReportSearchModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<LeadFollowUp, object>>>()
            {
                ["createdTime"] = v => v.CreatedTime,
            };

            var reportResult = new List<LeadFollowUpDetailsReportResultModel>();

            var leads = await _leadFollowUpRepository.GetAllIncludeAsync(x => x,
                            x => (!query.UserId.HasValue || x.LeadGeneration.UserId == query.UserId.Value)
                                && (!query.EmployeeRole.HasValue || x.LeadGeneration.User.EmployeeRole == query.EmployeeRole.Value)
                                && (string.IsNullOrWhiteSpace(query.Depot) || x.LeadGeneration.Depot == query.Depot)
                                && (!query.Territories.Any() || query.Territories.Contains(x.LeadGeneration.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(x.LeadGeneration.Zone))
                                && (!query.FromDate.HasValue || x.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || x.CreatedTime.Date <= query.ToDate.Value.Date)
                                && (string.IsNullOrWhiteSpace(query.ProjectName) || x.LeadGeneration.ProjectName.Contains(query.ProjectName))
                                && (string.IsNullOrWhiteSpace(query.ProjectCode) || x.LeadGeneration.Code.Contains(query.ProjectCode))
                                && (!query.PaintingStageId.HasValue || x.LeadGeneration.PaintingStageId == query.PaintingStageId.Value),
                            x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                            x => x.Include(i => i.TypeOfClient).Include(i => i.SwappingCompetition).Include(i => i.ProjectStatus)
                                .Include(i => i.LeadGeneration).ThenInclude(i => i.User),
                            query.Page,
                            query.PageSize,
                            true);

            reportResult = leads.Items.Select(x =>
            {
                var reportModel = new LeadFollowUpDetailsReportResultModel();
                reportModel.ProjectCode = x.LeadGeneration.Code;
                reportModel.ProjectName = x.LeadGeneration.ProjectName;
                reportModel.Depot = x.LeadGeneration.Depot;
                //reportModel.DepotName = x.LeadGeneration.DepotName;
                reportModel.UserId = x.LeadGeneration.User?.Email ?? string.Empty;
                reportModel.Territory = x.LeadGeneration.Territory;
                reportModel.Zone = x.LeadGeneration.Zone;
                reportModel.PlanVisitDatePlan = CustomConvertExtension.ObjectToDateString(x.NextVisitDatePlan);
                reportModel.ActualVisitDate = CustomConvertExtension.ObjectToDateString(x.ActualVisitDate);
                reportModel.TypeOfClient = x.TypeOfClient?.DropdownName ?? string.Empty;
                reportModel.ProjectAddress = x.LeadGeneration.ProjectAddress;
                reportModel.KeyContactPersonName = x.KeyContactPersonName;
                reportModel.KeyContactPersonMobile = x.KeyContactPersonMobile;
                reportModel.PaintContractorName = x.PaintContractorName;
                reportModel.PaintContractorMobile = x.PaintContractorMobile;
                reportModel.PaintContractorMobile = x.PaintContractorMobile;
                reportModel.NumberOfStoriedBuilding = x.NumberOfStoriedBuilding;
                reportModel.ExpectedValue = x.ExpectedValue;
                reportModel.ExpectedMonthlyBusinessValue = x.ExpectedMonthlyBusinessValue;
                reportModel.SwappingCompetition = x.SwappingCompetition?.DropdownName ?? string.Empty;
                reportModel.SwappingCompetitionAnotherCompetitorName = x.SwappingCompetitionAnotherCompetitorName;
                reportModel.UpTradingFromBrandName = x.UpTradingFromBrandName;
                reportModel.UpTradingToBrandName = x.UpTradingToBrandName;
                reportModel.BrandUsedInteriorBrandName = x.BrandUsedInteriorBrandName;
                reportModel.BrandUsedExteriorBrandName = x.BrandUsedExteriorBrandName;
                reportModel.BrandUsedUnderCoatBrandName = x.BrandUsedUnderCoatBrandName;
                reportModel.BrandUsedTopCoatBrandName = x.BrandUsedTopCoatBrandName;
                reportModel.TotalPaintingAreaSqftInterior = x.TotalPaintingAreaSqftInterior;
                reportModel.TotalPaintingAreaSqftExterior = x.TotalPaintingAreaSqftExterior;
                reportModel.ActualPaintJobCompletedInterior = x.ActualPaintJobCompletedInteriorPercentage;
                reportModel.ActualPaintJobCompletedExterior = x.ActualPaintJobCompletedExteriorPercentage;
                reportModel.ActualVolumeSoldInteriorGallon = x.ActualVolumeSoldInteriorGallon;
                reportModel.ActualVolumeSoldInteriorKg = x.ActualVolumeSoldInteriorKg;
                reportModel.ActualVolumeSoldExteriorGallon = x.ActualVolumeSoldExteriorGallon;
                reportModel.ActualVolumeSoldExteriorKg = x.ActualVolumeSoldExteriorKg;
                reportModel.ActualVolumeSoldUnderCoatGallon = x.ActualVolumeSoldUnderCoatGallon;
                reportModel.ActualVolumeSoldTopCoatGallon = x.ActualVolumeSoldTopCoatGallon;
                reportModel.BergerValueSales = x.BusinessAchievement?.BergerValueSales ?? (decimal)0;
                reportModel.BergerPremiumBrandSalesValue = x.BusinessAchievement?.BergerValueSales ?? (decimal)0;
                reportModel.CompetitionValueSales = x.BusinessAchievement?.BergerValueSales ?? (decimal)0;
                reportModel.ProductSourcing = x.BusinessAchievement?.ProductSourcing ?? string.Empty;
                reportModel.ProjectStatus = x.ProjectStatus?.DropdownName ?? string.Empty;
                reportModel.IsColorSchemeGiven = x.BusinessAchievement?.IsColorSchemeGiven ?? false ? "YES" : "NO";
                reportModel.IsProductSampling = x.BusinessAchievement?.IsProductSampling ?? false ? "YES" : "NO";
                reportModel.Comments = x.BusinessAchievement?.RemarksOrOutcome ?? string.Empty;
                reportModel.NextVisitDate = CustomConvertExtension.ObjectToDateString(x.BusinessAchievement?.NextVisitDate);
                reportModel.ImageUrl = x.BusinessAchievement?.PhotoCaptureUrl ?? string.Empty;
                return reportModel;
            }).ToList();

            var queryResult = new QueryResultModel<LeadFollowUpDetailsReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = reportResult.Count();
            queryResult.Total = reportResult.Count();

            return queryResult;
        }
    }
}
