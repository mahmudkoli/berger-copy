using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.CollectionEntry;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Tinting;
using Berger.Data.MsfaEntity.Users;
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
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Services.Implementation;
using BergerMsfaApi.Services.Interfaces;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;
using Microsoft.Data.SqlClient;

namespace BergerMsfaApi.Services.Report.Implementation
{
    public class PortalReportService : IPortalReportService
    {
        private readonly IRepository<LeadGeneration> _leadGenerationRepository;
        private readonly IRepository<LeadFollowUp> _leadFollowUpRepository;
        private readonly IRepository<DSC.DealerSalesCall> _dealerSalesCallRepository;
        private readonly IRepository<Painter> _painterRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<DropdownDetail> _dorpDownDetailsRepository;
        private readonly IRepository<AttachedDealerPainter> _attachmentDealerRepository;
        private readonly IRepository<DealerInfo> _dealerInfoRepository;
        private readonly IRepository<Zone> _zoneSvc;
        private readonly IRepository<Territory> _territorySvc;
        private readonly IRepository<SaleGroup> _saleGroupSvc;
        private readonly IRepository<SaleOffice> _saleOfficeSvc;
        private readonly IRepository<Depot> _depotSvc;
        private readonly IRepository<DealerOpening> _dealerOpening;
        private readonly IRepository<DealerOpeningAttachment> _dealerOpeningAttachmentSvc;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<PainterCall> _painterCallRepository;
        private readonly IRepository<PainterCompanyMTDValue> _painterCompanyMtdRepository;
        private readonly IRepository<CreditControlArea> _creditControlAreaRepository;
        private readonly IRepository<JourneyPlanMaster> _journeyPlanMasterRepository;
        private readonly IRepository<JourneyPlanDetail> _journeyPlanDetailRepository;
        private readonly IRepository<TintingMachine> _tintingMachine;
        private readonly IRepository<DSC.DealerCompetitionSales> _dealerCompetitionSaleRepository;
        private readonly IRepository<DSC.DealerSalesIssue> _dealerSaleIssueRepository;
        private readonly IDropdownService _dropdownService;
        private readonly IMapper _mapper;
        private readonly IFinancialDataService _financialDataService;
        private readonly ICollectionDataService _collectionDataService;


        private readonly ApplicationDbContext _context;
        private readonly IAuthService _service;

        public PortalReportService(
                IRepository<LeadGeneration> leadGenerationRepository,
                IRepository<LeadFollowUp> leadFollowUpRepository,
                IRepository<DSC.DealerSalesCall> dealerSalesCallRepository,
                IRepository<Painter> painterRepository,
                IRepository<UserInfo> userInfoRepository,
                IRepository<DropdownDetail> dropDownDetailsRepository,
                IRepository<AttachedDealerPainter> attachmentDealerRepository,
                IRepository<DealerInfo> dealerInfoRepository,
                IRepository<Zone> zoneSvc,
                IRepository<Territory> territorySvc,
                IRepository<SaleGroup> saleGroupSvc,
                IRepository<SaleOffice> saleOfficeSvc,
                IRepository<Depot> depotSvc,
                IRepository<DealerOpening> dealerOpening,
                IRepository<DealerOpeningAttachment> dealerOpeningAttachmentSvc,
                IRepository<Payment> paymentRepository,
                IRepository<PainterCall> painterCallRepository,
                IRepository<PainterCompanyMTDValue> painterCompanyMtdRepository,
                IRepository<CreditControlArea> creditControlAreaRepository,
                IRepository<JourneyPlanMaster> journeyPlanMasterRepository,
                IRepository<JourneyPlanDetail> journeyPlanDetailRepository,
                IRepository<TintingMachine> tintingMachine,
                IRepository<DSC.DealerCompetitionSales> dealerCompetitionSaleRepository,
                IRepository<DSC.DealerSalesIssue> dealerSaleIssueRepository,
                IDropdownService dropdownService,
                IMapper mapper,
                IFinancialDataService financialDataService,
                ApplicationDbContext context,
                IAuthService service,
                ICollectionDataService collectionDataService
            )
        {
            this._leadGenerationRepository = leadGenerationRepository;
            this._leadFollowUpRepository = leadFollowUpRepository;
            this._dealerSalesCallRepository = dealerSalesCallRepository;
            this._painterRepository = painterRepository;
            this._userInfoRepository = userInfoRepository;
            this._dorpDownDetailsRepository = dropDownDetailsRepository;
            this._attachmentDealerRepository = attachmentDealerRepository;
            this._dealerInfoRepository = dealerInfoRepository;
            this._zoneSvc = zoneSvc;
            this._territorySvc = territorySvc;
            this._saleGroupSvc = saleGroupSvc;
            this._saleOfficeSvc = saleOfficeSvc;
            this._depotSvc = depotSvc;
            this._dealerOpening = dealerOpening;
            this._dealerOpeningAttachmentSvc = dealerOpeningAttachmentSvc;
            this._dropdownService = dropdownService;
            this._paymentRepository = paymentRepository;
            this._painterCallRepository = painterCallRepository;
            this._painterCompanyMtdRepository = painterCompanyMtdRepository;
            this._creditControlAreaRepository = creditControlAreaRepository;
            this._journeyPlanMasterRepository = journeyPlanMasterRepository;
            this._journeyPlanDetailRepository = journeyPlanDetailRepository;
            this._dealerCompetitionSaleRepository = dealerCompetitionSaleRepository;
            this._dealerSaleIssueRepository = dealerSaleIssueRepository;
            this._tintingMachine = tintingMachine;
            this._mapper = mapper;
            _financialDataService = financialDataService;
            _financialDataService = financialDataService;
            _financialDataService = financialDataService;

            this._context = context;
            _service = service;
            _collectionDataService = collectionDataService;
        }

        private int SkipCount(QueryObjectModel query) => (query.Page - 1) * query.PageSize;

        public async Task<QueryResultModel<LeadSummaryReportResultModel>> GetLeadSummaryReportAsync(LeadSummaryReportSearchModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            {
                ["createdTime"] = v => v.CreatedTime,
            };

            var reportResult = new List<LeadSummaryReportResultModel>();

            var leads = await _leadGenerationRepository.GetAllIncludeAsync(x => x,
                            x => (!query.UserId.HasValue || x.UserId == query.UserId.Value)
                                //&& (!query.EmployeeRole.HasValue || x.User.EmployeeRole == query.EmployeeRole.Value)
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
                reportModel.NoOfCompetitionSnatchLead = leadFollowUps.Count(p => p.HasSwappingCompetition);
                reportModel.BergerValueSales = leadFollowUps.Sum(p => p.BusinessAchievement?.BergerValueSales ?? (decimal)0);
                reportModel.BergerPremiumBrandValueSales = leadFollowUps.Sum(p => p.BusinessAchievement?.BergerPremiumBrandSalesValue ?? (decimal)0);
                reportModel.CompetitionValueSales = leadFollowUps.Sum(p => p.BusinessAchievement?.CompetitionValueSales ?? (decimal)0);
                reportModel.NoOfColorSchemeGiven = leadFollowUps.Count(p => p.BusinessAchievement?.IsColorSchemeGiven ?? false);
                reportModel.NoOfProductSampling = leadFollowUps.Count(p => p.BusinessAchievement?.IsProductSampling ?? false);
                return reportModel;
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<LeadSummaryReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = groupOfLeads.Count();
            queryResult.Total = groupOfLeads.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<LeadGenerationDetailsReportResultModel>> GetLeadGenerationDetailsReportAsync(LeadGenerationDetailsReportSearchModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            {
                ["createdTime"] = v => v.CreatedTime,
            };

            var reportResult = new List<LeadGenerationDetailsReportResultModel>();

            var leads = await (from lg in _context.LeadGenerations
                                join u in _context.UserInfos on lg.UserId equals u.Id into uleftjoin
                                from uinfo in uleftjoin.DefaultIfEmpty()
                                join tc in _context.DropdownDetails on lg.TypeOfClientId equals tc.Id into tcleftjoin
                                from tcinfo in tcleftjoin.DefaultIfEmpty()
                                join ps in _context.DropdownDetails on lg.PaintingStageId equals ps.Id into psleftjoin
                                from psinfo in psleftjoin.DefaultIfEmpty()
                                join d in _context.Depots on lg.Depot equals d.Werks into dleftjoin
                                from dinfo in dleftjoin.DefaultIfEmpty()
                                where (
                                 (!query.UserId.HasValue || lg.UserId == query.UserId.Value)
                                 && (string.IsNullOrWhiteSpace(query.Depot) || lg.Depot == query.Depot)
                                 && (!query.Territories.Any() || query.Territories.Contains(lg.Territory))
                                 && (!query.Zones.Any() || query.Zones.Contains(lg.Zone))
                                 && (!query.FromDate.HasValue || lg.CreatedTime.Date >= query.FromDate.Value.Date)
                                 && (!query.ToDate.HasValue || lg.CreatedTime.Date <= query.ToDate.Value.Date)
                                 && (string.IsNullOrWhiteSpace(query.ProjectName) || lg.ProjectName.Contains(query.ProjectName))
                                 && (!query.PaintingStageId.HasValue || lg.PaintingStageId == query.PaintingStageId.Value)
                                )
                                select new
                                {
                                    uinfo.Email,
                                    lg.Code,
                                    lg.ProjectName,
                                    lg.Depot,
                                    depotName = dinfo.Name1,
                                    lg.Territory,
                                    lg.Zone,
                                    lg.CreatedTime,
                                    typeOfClient = tcinfo.DropdownName,
                                    lg.ProjectAddress,
                                    lg.KeyContactPersonName,
                                    lg.KeyContactPersonMobile,
                                    lg.PaintContractorName,
                                    lg.PaintContractorMobile,
                                    paintingStage = psinfo.DropdownName,
                                    lg.ExpectedDateOfPainting,
                                    lg.NumberOfStoriedBuilding,
                                    lg.TotalPaintingAreaSqftInterior,
                                    lg.TotalPaintingAreaSqftExterior,
                                    lg.ExpectedValue,
                                    lg.ExpectedMonthlyBusinessValue,
                                    lg.RequirementOfColorScheme,
                                    lg.ProductSamplingRequired,
                                    lg.NextFollowUpDate,
                                    lg.Remarks,
                                    lg.PhotoCaptureUrl,
                                    lg.OtherClientName
                                }).ToListAsync();

            //var leads = await _leadGenerationRepository.GetAllIncludeAsync(x => x,
            //                x => (!query.UserId.HasValue || x.UserId == query.UserId.Value)
            //                    //&& (!query.EmployeeRole.HasValue || x.User.EmployeeRole == query.EmployeeRole.Value)
            //                    && (string.IsNullOrWhiteSpace(query.Depot) || x.Depot == query.Depot)
            //                    && (!query.Territories.Any() || query.Territories.Contains(x.Territory))
            //                    && (!query.Zones.Any() || query.Zones.Contains(x.Zone))
            //                    && (!query.FromDate.HasValue || x.CreatedTime.Date >= query.FromDate.Value.Date)
            //                    && (!query.ToDate.HasValue || x.CreatedTime.Date <= query.ToDate.Value.Date)
            //                    && (string.IsNullOrWhiteSpace(query.ProjectName) || x.ProjectName.Contains(query.ProjectName))
            //                    && (!query.PaintingStageId.HasValue || x.PaintingStageId == query.PaintingStageId.Value),
            //                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
            //                x => x.Include(i => i.User).Include(i => i.TypeOfClient).Include(i => i.PaintingStage),
            //                query.Page,
            //                query.PageSize,
            //                true);

            reportResult = leads.Select(x => new LeadGenerationDetailsReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                ProjectCode = x.Code,
                ProjectName = x.ProjectName,
                Depot = x.Depot,
                DepotName = x.depotName,
                Territory = x.Territory,
                Zone = x.Zone,
                LeadCreatedDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime),
                TypeOfClient = x.typeOfClient ?? string.Empty,
                ProjectAddress = x.ProjectAddress,
                KeyContactPersonName = x.KeyContactPersonName,
                KeyContactPersonMobile = x.KeyContactPersonMobile,
                PaintContractorName = x.PaintContractorName,
                PaintContractorMobile = x.PaintContractorMobile,
                PaintingStage = x.paintingStage ?? string.Empty,
                ExpectedDateOfPainting = CustomConvertExtension.ObjectToDateString(x.ExpectedDateOfPainting),
                NumberOfStoriedBuilding = x.NumberOfStoriedBuilding,
                TotalPaintingAreaSqftInterior = x.TotalPaintingAreaSqftInterior,
                TotalPaintingAreaSqftExterior = x.TotalPaintingAreaSqftExterior,
                ExpectedValue = x.ExpectedValue,
                ExpectedMonthlyBusinessValue = x.ExpectedMonthlyBusinessValue,
                RequirementOfColorScheme = x.RequirementOfColorScheme ? "YES" : "NO",
                ProductSamplingRequired = x.ProductSamplingRequired ? "YES" : "NO",
                NextFollowUpDate = CustomConvertExtension.ObjectToDateString(x.NextFollowUpDate),
                Remarks = x.Remarks,
                ImageUrl = x.PhotoCaptureUrl,
                OtherClientName = x.OtherClientName
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<LeadGenerationDetailsReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = leads.Count();
            queryResult.Total = leads.Count();

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
                                //&& (!query.EmployeeRole.HasValue || x.LeadGeneration.User.EmployeeRole == query.EmployeeRole.Value)
                                && (string.IsNullOrWhiteSpace(query.Depot) || x.LeadGeneration.Depot == query.Depot)
                                && (!query.Territories.Any() || query.Territories.Contains(x.LeadGeneration.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(x.LeadGeneration.Zone))
                                && (!query.FromDate.HasValue || x.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || x.CreatedTime.Date <= query.ToDate.Value.Date)
                                && (string.IsNullOrWhiteSpace(query.ProjectName) || x.LeadGeneration.ProjectName.Contains(query.ProjectName))
                                && (string.IsNullOrWhiteSpace(query.ProjectCode) || x.LeadGeneration.Code.Contains(query.ProjectCode))
                                && (!query.ProjectStatusId.HasValue || x.ProjectStatusId == query.ProjectStatusId.Value),
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
            queryResult.TotalFilter = leads.TotalFilter;
            queryResult.Total = leads.Total;

            return queryResult;
        }

        public async Task<QueryResultModel<PainterRegistrationReportResultModel>> GetPainterRegistrationReportAsync(PainterRegistrationReportSearchModel query)
        {
            var reportResult = new List<PainterRegistrationReportResultModel>();

            var painters = await (from p in _context.Painters
                                  join u in _context.UserInfos on p.EmployeeId equals u.EmployeeId into uleftjoin
                                  from userInfo in uleftjoin.DefaultIfEmpty()
                                  join d in _context.DropdownDetails on p.PainterCatId equals d.Id into dleftjoin
                                  from dropDownInfo in dleftjoin.DefaultIfEmpty()
                                  join adp in _context.AttachedDealerPainters on p.AttachedDealerCd equals adp.Id.ToString() into adpleftjoin
                                  from adpInfo in adpleftjoin.DefaultIfEmpty()
                                  join di in _context.DealerInfos on adpInfo.DealerId equals di.Id into dileftjoin
                                  from diInfo in dileftjoin.DefaultIfEmpty()
                                  join dep in _context.Depots on p.Depot equals dep.Werks into depleftjoin
                                  from depinfo in depleftjoin.DefaultIfEmpty()
                                  join sg in _context.SaleGroup on p.SaleGroup equals sg.Code into sgleftjoin
                                  from sginfo in sgleftjoin.DefaultIfEmpty()
                                  join t in _context.Territory on p.Territory equals t.Code into tleftjoin
                                  from tinfo in tleftjoin.DefaultIfEmpty()
                                  join z in _context.Zone on p.Zone equals z.Code into zleftjoin
                                  from zinfo in zleftjoin.DefaultIfEmpty()
                                  where (
                                     (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                     && (string.IsNullOrWhiteSpace(query.Depot) || p.Depot == query.Depot)
                                     && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                     && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                     && (!query.FromDate.HasValue || p.CreatedTime.Date >= query.FromDate.Value.Date)
                                     && (!query.ToDate.HasValue || p.CreatedTime.Date <= query.ToDate.Value.Date)
                                     && (!query.PainterId.HasValue || p.Id == query.PainterId.Value)
                                     && (!query.PainterType.HasValue || p.PainterCatId == query.PainterType.Value)
                                     && (string.IsNullOrWhiteSpace(query.PainterMobileNo) || p.Phone == query.PainterMobileNo)
                                  )
                                  select new
                                  {
                                      userInfo.Email,
                                      territoryName = tinfo.Name,
                                      zoneName = zinfo.Name,
                                      painterId = p.Id.ToString(),
                                      p.CreatedTime,
                                      typeOfPainter = dropDownInfo.DropdownName,
                                      depotName = depinfo.Name1,
                                      salesGroupName = sginfo.Name,
                                      p.PainterName,
                                      p.Address,
                                      p.Phone,
                                      p.NoOfPainterAttached,
                                      rocketAccountStatus = p.HasDbbl ? "Created" : "Not Created",
                                      p.AccDbblNumber,
                                      p.AccDbblHolderName,
                                      identification = !string.IsNullOrEmpty(p.NationalIdNo) ? p.NationalIdNo
                                                       : (!string.IsNullOrEmpty(p.PassportNo) ? p.PassportNo
                                                       : (!string.IsNullOrEmpty(p.BrithCertificateNo)) ? p.BrithCertificateNo : string.Empty),
                                      p.AttachedDealerCd,
                                      diInfo.CustomerName,
                                      appInstalledStatus = p.IsAppInstalled ? "Installed" : "Not Installed",
                                      p.Remark,
                                      avgMonthlyUse = p.AvgMonthlyVal.ToString(),
                                      bergerLoyalty = p.Loyality.ToString(),
                                      p.PainterImageUrl
                                  }).ToListAsync();

            reportResult = painters.Select(x => new PainterRegistrationReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                Territory = x.territoryName,
                Zone = x.zoneName,
                PainterId = x.painterId,
                PainerRegistrationDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime),
                TypeOfPainer = x.typeOfPainter,
                DepotName = x.depotName,
                SalesGroup = x.salesGroupName,
                PainterName = x.PainterName,
                PainterAddress = x.Address,
                MobileNumber = x.Phone,
                NoOfPaintingAttached = x.NoOfPainterAttached,
                DBBLRocketAccountStatus = x.rocketAccountStatus,
                AccountNumber = x.AccDbblNumber,
                AccountHolderName = x.AccDbblHolderName,
                IdentificationNo = x.identification,
                AttachedTaggedDealerId = x.AttachedDealerCd,
                AttachedTaggedDealerName = x.CustomerName,
                APPInstalledStatus = x.appInstalledStatus,
                APPNotInstalledReason = x.Remark,
                AverageMonthlyUse = x.avgMonthlyUse,
                BergerLoyalty = x.bergerLoyalty,
                PainterImageUrl = x.PainterImageUrl,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<PainterRegistrationReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = painters.Count();
            queryResult.Total = painters.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerOpeningReportResultModel>> GetDealerOpeningReportAsync(DealerOpeningReportSearchModel query)
        {
            var reportResult = new List<DealerOpeningReportResultModel>();

            var dealers = await (from d in _context.DealerOpenings
                                 join u in _context.UserInfos on d.EmployeeId equals u.EmployeeId into uleftjoin
                                 from uinfo in uleftjoin.DefaultIfEmpty()
                                 join dep in _context.Depots on d.BusinessArea equals dep.Werks into depleftjoin
                                 from depinfo in depleftjoin.DefaultIfEmpty()
                                 join so in _context.SaleOffice on d.SaleOffice equals so.Code into soleftjoin
                                 from soinfo in soleftjoin.DefaultIfEmpty()
                                 join sg in _context.SaleGroup on d.SaleGroup equals sg.Code into sgleftjoin
                                 from sginfo in sgleftjoin.DefaultIfEmpty()
                                 join t in _context.Territory on d.Territory equals t.Code into tleftjoin
                                 from tinfo in tleftjoin.DefaultIfEmpty()
                                 join z in _context.Zone on d.Zone equals z.Code into zleftjoin
                                 from zinfo in zleftjoin.DefaultIfEmpty()
                                 where (
                                   (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                   && (string.IsNullOrWhiteSpace(query.Depot) || d.BusinessArea == query.Depot)
                                   && (!query.Territories.Any() || query.Territories.Contains(d.Territory))
                                   && (!query.Zones.Any() || query.Zones.Contains(d.Zone))
                                   && (!query.FromDate.HasValue || d.CreatedTime.Date >= query.FromDate.Value.Date)
                                   && (!query.ToDate.HasValue || d.CreatedTime.Date <= query.ToDate.Value.Date)
                                 )
                                 select new
                                 {
                                     uinfo.Email,
                                     dealerId = d.Id.ToString(),
                                     code = d.Code,
                                     d.BusinessArea,
                                     businessAreaName = depinfo.Name1,
                                     salesOffice = sginfo.Name,
                                     saleGroupName = sginfo.Name,
                                     territoryName = tinfo.Name,
                                     zoneName = zinfo.Name,
                                     d.EmployeeId
                                 }).ToListAsync();

            var dealerAttachments = await (from doa in _context.DealerOpeningAttachments
                                           join di in _context.DealerInfos on doa.DealerOpeningId equals di.Id into dileftjoin
                                           from diinfo in dileftjoin.DefaultIfEmpty()
                                           select new
                                           {
                                               attachmentName = doa.Name,
                                               dealerOpeningId = doa.DealerOpeningId.ToString(),
                                               doa.Path
                                           }).ToListAsync();

            reportResult = dealers.Select(x => new DealerOpeningReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                DealrerOpeningCode = x.code,
                BusinessArea = x.BusinessArea,
                BusinessAreaName = x.businessAreaName,
                SalesOffice = x.salesOffice,
                SalesGroup = x.saleGroupName,
                Territory = x.territoryName,
                Zone = x.zoneName,
                EmployeeId = x.EmployeeId,
                DealershipOpeningApplicationForm = dealerAttachments.FirstOrDefault(y => y.attachmentName == "Application Form" && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                TradeLicensee = dealerAttachments.FirstOrDefault(y => y.attachmentName == "Trade Licensee" && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                IdentificationNo = dealerAttachments.FirstOrDefault(y => y.attachmentName == "NID/Passport/Birth" && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                PhotographOfproprietor = dealerAttachments.FirstOrDefault(y => y.attachmentName == "Photograph of proprietor" && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                NomineeIdentificationNo = dealerAttachments.FirstOrDefault(y => y.attachmentName == "Nominee NID/PASSPORT/BIRTH" && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                NomineePhotograph = dealerAttachments.FirstOrDefault(y => y.attachmentName == "Nominee/Photograph" && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                Cheque = dealerAttachments.FirstOrDefault(y => y.attachmentName == "Cheque" && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                CurrentStatusOfThisApplication = "",
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerOpeningReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealers.Count();
            queryResult.Total = dealers.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerCollectionReportResultModel>> GetDealerCollectionReportAsync(CollectionReportSearchModel query)
        {
            var reportResult = new List<DealerCollectionReportResultModel>();

            var dealers = await (from p in _context.Payments
                                 join u in _context.UserInfos on p.EmployeeId equals u.EmployeeId into uleftjoin
                                 from uinfo in uleftjoin.DefaultIfEmpty()
                                 join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                 from dctinfo in dctleftjoin.DefaultIfEmpty()
                                 join dpm in _context.DropdownDetails on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                                 from dpminfo in dpmleftjoin.DefaultIfEmpty()
                                 join ca in _context.CreditControlAreas on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                                 from cainfo in caleftjoin.DefaultIfEmpty()
                                 join d in _context.DealerInfos on p.DealerId equals d.Id.ToString() into dleftjoin
                                 from dinfo in dleftjoin.DefaultIfEmpty()
                                 join t in _context.Territory on dinfo.Territory equals t.Code into tleftjoin
                                 from tinfo in tleftjoin.DefaultIfEmpty()
                                 join z in _context.Zone on dinfo.CustZone equals z.Code into zleftjoin
                                 from zinfo in zleftjoin.DefaultIfEmpty()
                                 join dep in _context.Depots on dinfo.BusinessArea equals dep.Werks into depleftjoin
                                 from depinfo in depleftjoin.DefaultIfEmpty()
                                 where (
                                   dctinfo.DropdownName == ConstantsCustomerTypeValue.Dealer
                                   && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                   && (!query.Territories.Any() || query.Territories.Contains(dinfo.Territory))
                                   && (!query.Zones.Any() || query.Zones.Contains(dinfo.CustZone))
                                   && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                   && (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                   && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                   && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                 )
                                 select new
                                 {
                                     uinfo.Email,
                                     p.CollectionDate,
                                     customerType = dctinfo.DropdownName,
                                     p.SapId,
                                     projectName = p.Name,
                                     p.Address,
                                     paymentMethod = dpminfo.DropdownName,
                                     creditControlArea = cainfo.Description,
                                     p.BankName,
                                     p.Number,
                                     p.Amount,
                                     p.ManualNumber,
                                     p.Remarks,
                                     p.Name,
                                     p.MobileNumber,
                                     depotId = depinfo.Werks,
                                     depotName = depinfo.Name1,
                                     territoryName = tinfo.Name,
                                     zoneName = zinfo.Name,
                                     p.DealerId,
                                     dinfo.CustomerNo
                                 }).ToListAsync();

            reportResult = dealers.Select(x => new DealerCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                DepotId = x.depotId,
                DepotName = x.depotName,
                Territory = x.territoryName,
                Zone = x.zoneName,
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.CollectionDate),
                TypeOfCustomer = x.customerType,
                DealerId = x?.CustomerNo.ToString(),
                DealerName = x.Name,
                PaymentMethod = x.paymentMethod,
                CreditControlArea = x.creditControlArea,
                BankName = x.BankName,
                ChequeNumber = x.Number,
                CashAmount = x.Amount,
                ManualMrNumber = x.ManualNumber,
                Remarks = x.Remarks
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerCollectionReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealers.Count();
            queryResult.Total = dealers.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<SubDealerCollectionReportResultModel>> GetSubDealerCollectionReportAsync(CollectionReportSearchModel query)
        {
            var reportResult = new List<SubDealerCollectionReportResultModel>();

            var subDealers = await (from p in _context.Payments
                                    join u in _context.UserInfos on p.EmployeeId equals u.EmployeeId into uleftjoin
                                    from uinfo in uleftjoin.DefaultIfEmpty()
                                    join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                    from dctinfo in dctleftjoin.DefaultIfEmpty()
                                    join dpm in _context.DropdownDetails on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                                    from dpminfo in dpmleftjoin.DefaultIfEmpty()
                                    join ca in _context.CreditControlAreas on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                                    from cainfo in caleftjoin.DefaultIfEmpty()
                                    join d in _context.DealerInfos on p.DealerId equals d.Id.ToString() into dleftjoin
                                    from dinfo in dleftjoin.DefaultIfEmpty()
                                    join t in _context.Territory on dinfo.Territory equals t.Code into tleftjoin
                                    from tinfo in tleftjoin.DefaultIfEmpty()
                                    join z in _context.Zone on dinfo.CustZone equals z.Code into zleftjoin
                                    from zinfo in zleftjoin.DefaultIfEmpty()
                                    join dep in _context.Depots on dinfo.BusinessArea equals dep.Werks into depleftjoin
                                    from depinfo in depleftjoin.DefaultIfEmpty()
                                    where (
                                      dctinfo.DropdownName == ConstantsCustomerTypeValue.SubDealer
                                      && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                      && (!query.Territories.Any() || query.Territories.Contains(dinfo.Territory))
                                      && (!query.Zones.Any() || query.Zones.Contains(dinfo.CustZone))
                                      && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                      && (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                      && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                      && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                    )
                                    select new
                                    {
                                        uinfo.Email,
                                        p.CollectionDate,
                                        customerType = dctinfo.DropdownName,
                                        p.SapId,
                                        projectName = p.Name,
                                        p.Address,
                                        paymentMethod = dpminfo.DropdownName,
                                        creditControlArea = cainfo.Description,
                                        p.BankName,
                                        p.Number,
                                        p.Amount,
                                        p.ManualNumber,
                                        p.Remarks,
                                        p.Name,
                                        p.MobileNumber,
                                        depotId = depinfo.Werks,
                                        depotName = depinfo.Name1,
                                        territoryName = tinfo.Name,
                                        zoneName = zinfo.Name,
                                        p.DealerId,
                                        dinfo.CustomerNo
                                    }).ToListAsync();

            reportResult = subDealers.Select(x => new SubDealerCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                DepotId = x.depotId,
                DepotName = x.depotName,
                Territory = x.territoryName,
                Zone = x.zoneName,
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.CollectionDate),
                TypeOfCustomer = x.customerType,
                SubDealerCode = x?.CustomerNo.ToString(),
                SubDealerName = x.Name,
                SubDealerMobileNumber = x.MobileNumber,
                SubDealerAddress = x.Address,
                PaymentMethod = x.paymentMethod,
                CreditControlArea = x.creditControlArea,
                BankName = x.BankName,
                ChequeNumber = x.Number,
                CashAmount = x.Amount,
                ManualMrNumber = x.ManualNumber,
                Remarks = x.Remarks
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<SubDealerCollectionReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = subDealers.Count();
            queryResult.Total = subDealers.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<CustomerCollectionReportResultModel>> GetCustomerCollectionReportAsync(CollectionReportSearchModel query)
        {
            var reportResult = new List<CustomerCollectionReportResultModel>();

            var customers = await (from p in _context.Payments
                                   join u in _context.UserInfos on p.EmployeeId equals u.EmployeeId into uleftjoin
                                   from uinfo in uleftjoin.DefaultIfEmpty()
                                   join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                   from dctinfo in dctleftjoin.DefaultIfEmpty()
                                   join dpm in _context.DropdownDetails on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                                   from dpminfo in dpmleftjoin.DefaultIfEmpty()
                                   join ca in _context.CreditControlAreas on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                                   from cainfo in caleftjoin.DefaultIfEmpty()
                                   where (
                                     dctinfo.DropdownName == ConstantsCustomerTypeValue.Customer
                                     && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                     && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                     && (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                     && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                     && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                   )
                                   select new
                                   {
                                       uinfo.Email,
                                       p.CollectionDate,
                                       customerType = dctinfo.DropdownName,
                                       p.SapId,
                                       projectName = p.Name,
                                       p.Address,
                                       paymentMethod = dpminfo.DropdownName,
                                       creditControlArea = cainfo.Description,
                                       p.BankName,
                                       p.Number,
                                       p.Amount,
                                       p.ManualNumber,
                                       p.Remarks,
                                       p.Name,
                                       p.MobileNumber
                                   }).ToListAsync();

            reportResult = customers.Select(x => new CustomerCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                //DepotId = "",
                //DepotName = "",
                //Territory = "",
                //Zone = "",
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.CollectionDate),
                TypeOfCustomer = x.customerType,
                CustomerName = x.Name,
                CustomerMobileNumber = x.MobileNumber,
                CustomerAddress = x.Address,
                PaymentMethod = x.paymentMethod,
                CreditControlArea = x.creditControlArea,
                BankName = x.BankName,
                ChequeNumber = x.Number,
                CashAmount = x.Amount,
                ManualMrNumber = x.ManualNumber,
                Remarks = x.Remarks
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<CustomerCollectionReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = customers.Count();
            queryResult.Total = customers.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DirectProjectCollectionReportResultModel>> GetDirectProjectCollectionReportAsync(CollectionReportSearchModel query)
        {
            var reportResult = new List<DirectProjectCollectionReportResultModel>();

            var directProjects = await (from p in _context.Payments
                                        join u in _context.UserInfos on p.EmployeeId equals u.EmployeeId into uleftjoin
                                        from uinfo in uleftjoin.DefaultIfEmpty()
                                        join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                        from dctinfo in dctleftjoin.DefaultIfEmpty()
                                        join dpm in _context.DropdownDetails on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                                        from dpminfo in dpmleftjoin.DefaultIfEmpty()
                                        join ca in _context.CreditControlAreas on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                                        from cainfo in caleftjoin.DefaultIfEmpty()
                                        where (
                                          dctinfo.DropdownName == ConstantsCustomerTypeValue.DirectProject
                                          && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                          && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                          && (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                          && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                          && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                        )
                                        select new
                                        {
                                            uinfo.Email,
                                            p.CollectionDate,
                                            customerType = dctinfo.DropdownName,
                                            p.SapId,
                                            projectName = p.Name,
                                            p.Address,
                                            paymentMethod = dpminfo.DropdownName,
                                            creditControlArea = cainfo.Description,
                                            p.BankName,
                                            p.Number,
                                            p.Amount,
                                            p.ManualNumber,
                                            p.Remarks
                                        }).ToListAsync();

            reportResult = directProjects.Select(x => new DirectProjectCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                //DepotId = "",
                //DepotName = "",
                //Territory = "",
                //Zone = "",
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.CollectionDate),
                TypeOfCustomer = x.customerType,
                ProjectSapId = x.SapId,
                ProjectName = x.projectName,
                ProjectAddress = x.Address,
                PaymentMethod = x.paymentMethod,
                CreditControlArea = x.creditControlArea,
                BankName = x.BankName,
                ChequeNumber = x.Number,
                CashAmount = x.Amount,
                ManualMrNumber = x.ManualNumber,
                Remarks = x.Remarks
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DirectProjectCollectionReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = directProjects.Count();
            queryResult.Total = directProjects.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<PainterCallReportResultModel>> GetPainterCallReportAsync(PainterCallReportSearchModel query)
        {
            var reportResult = new List<PainterCallReportResultModel>();

            var paintersCalls = await (from pcm in _context.PainterCompanyMTDValues
                                       join pc in _context.PainterCalls on pcm.PainterCallId equals pc.Id into pcleftjoin
                                       from pcinfo in pcleftjoin.DefaultIfEmpty()
                                       join p in _context.Painters on pcinfo.PainterId equals p.Id into pleftjoin
                                       from pinfo in pleftjoin.DefaultIfEmpty()
                                       join u in _context.UserInfos on pinfo.EmployeeId equals u.EmployeeId into uleftjoin
                                       from userInfo in uleftjoin.DefaultIfEmpty()
                                       join ddc in _context.DropdownDetails on pinfo.PainterCatId equals ddc.Id into ddcleftjoin
                                       from ddcinfo in ddcleftjoin.DefaultIfEmpty()
                                       join dep in _context.Depots on pinfo.Depot equals dep.Werks into depleftjoin
                                       from depinfo in depleftjoin.DefaultIfEmpty()
                                       join sg in _context.SaleGroup on pinfo.SaleGroup equals sg.Code into sgleftjoin
                                       from sginfo in sgleftjoin.DefaultIfEmpty()
                                       join t in _context.Territory on pinfo.Territory equals t.Code into tleftjoin
                                       from tinfo in tleftjoin.DefaultIfEmpty()
                                       join z in _context.Zone on pinfo.Zone equals z.Code into zleftjoin
                                       from zinfo in zleftjoin.DefaultIfEmpty()
                                       join adp in _context.AttachedDealerPainters on pinfo.AttachedDealerCd equals adp.Id.ToString() into adpleftjoin
                                       from adpInfo in adpleftjoin.DefaultIfEmpty()
                                       join di in _context.DealerInfos on adpInfo.DealerId equals di.Id into dileftjoin
                                       from diInfo in dileftjoin.DefaultIfEmpty()
                                       where (
                                       (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                       && (string.IsNullOrWhiteSpace(query.Depot) || pinfo.Depot == query.Depot)
                                       && (!query.Territories.Any() || query.Territories.Contains(pinfo.Territory))
                                       && (!query.Zones.Any() || query.Zones.Contains(pinfo.Zone))
                                       && (!query.FromDate.HasValue || pcinfo.CreatedTime.Date >= query.FromDate.Value.Date)
                                       && (!query.ToDate.HasValue || pcinfo.CreatedTime.Date <= query.ToDate.Value.Date)
                                       && (!query.PainterId.HasValue || pinfo.Id == query.PainterId.Value)
                                       && (!query.PainterType.HasValue || pinfo.PainterCatId == query.PainterType.Value)
                                   )
                                       select new
                                       {
                                           userInfo.Email,
                                           painterId = pcinfo.PainterId.ToString(),
                                           pcinfo.CreatedTime,
                                           painterType = ddcinfo.DropdownName,
                                           depot = depinfo.Name1,
                                           salesGroupName = sginfo.Name,
                                           territoryName = tinfo.Name,
                                           zoneName = zinfo.Name,
                                           pinfo.PainterName,
                                           pinfo.Address,
                                           pinfo.Phone,
                                           noOfAttachment = pinfo.NoOfPainterAttached.ToString(),
                                           rocketAccountStatus = pinfo.HasDbbl ? "Created" : "Not Created",
                                           pinfo.AccDbblNumber,
                                           identification = !string.IsNullOrEmpty(pinfo.NationalIdNo) ? pinfo.NationalIdNo
                                                              : (!string.IsNullOrEmpty(pinfo.PassportNo) ? pinfo.PassportNo
                                                              : (!string.IsNullOrEmpty(pinfo.BrithCertificateNo)) ? pinfo.BrithCertificateNo : string.Empty),
                                           pinfo.AttachedDealerCd,
                                           diInfo.CustomerName,
                                           shamparkaAppStatus = pinfo.IsAppInstalled ? "Installed" : "Not Installed",
                                           loyality = pinfo.Loyality.ToString(),
                                           painterSchemeCommunication = pcinfo.HasSchemeComnunaction ? "Yes" : "No",
                                           premiumProductBriefing = pcinfo.HasPremiumProtBriefing ? "Yes" : "No",
                                           newProductBriefing = pcinfo.HasNewProBriefing ? "Yes" : "No",
                                           epToolsUsage = pcinfo.HasUsageEftTools ? "Yes" : "No",
                                           painterAppUsage = pcinfo.HasAppUsage ? "Yes" : "No",
                                           workInHandNo = pcinfo.WorkInHandNumber.ToString(),
                                           issueWithDbblAccount = pcinfo.HasDbblIssue ? "Yes" : "No",
                                           pcinfo.Comment
                                       }).Distinct().ToListAsync();

            var paintersCallMtd = await (from pmtd in _context.PainterCompanyMTDValues
                                         join dd in _context.DropdownDetails on pmtd.CompanyId equals dd.Id into ddleftjoin
                                         from ddinfo in ddleftjoin.DefaultIfEmpty()
                                         where (
                                         (!query.FromDate.HasValue || pmtd.CreatedTime.Date >= query.FromDate.Value.Date)
                                         && (!query.ToDate.HasValue || pmtd.CreatedTime.Date <= query.ToDate.Value.Date)
                                         )
                                         select new
                                         {
                                             companyName = ddinfo.DropdownName,
                                             pmtd.Value,
                                             pmtd.CountInPercent
                                         }).ToListAsync();

            reportResult = paintersCalls.Select(x => new PainterCallReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                PainterId = x?.painterId ?? string.Empty,
                PainterVisitDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime),
                TypeOfPainter = x.painterType,
                DepotName = x.depot,
                SalesGroup = x.salesGroupName,
                Territory = x.territoryName,
                Zone = x.zoneName,
                PainterName = x.PainterName,
                PainterAddress = x.Address,
                MobileNumber = x.Phone,
                NoOfPainterAttached = x.noOfAttachment,
                DbblRocketAccountStatus = x.rocketAccountStatus,
                AccountNumber = x.AccDbblNumber,
                AcccountHolderName = x.AccDbblNumber,
                IdentificationNo = x.identification,
                AttachedTaggedDealerId = x.AttachedDealerCd,
                AttachedTaggedDealerName = x.CustomerName,
                ShamparkaAppInstallStatus = x.shamparkaAppStatus,
                BergerLoyalty = x.loyality,
                PainterSchemeCommunication = x.painterSchemeCommunication,
                PremiumProductBriefing = x.premiumProductBriefing,
                NewProductBriefing = x.newProductBriefing,
                EpToolsUsage = x.epToolsUsage,
                PainterAppUsage = x.painterAppUsage,
                WorkInHandNo = x.workInHandNo,

                BpblMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.BPBL).Sum(x => x.Value).ToString(),
                BpblCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.BPBL).Sum(x => x.CountInPercent).ToString(),
                ApMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.AP).Sum(x => x.Value).ToString(),
                ApCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.AP).Sum(x => x.CountInPercent).ToString(),
                NerolacMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Nerolac).Sum(x => x.Value).ToString(),
                NerolacCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Nerolac).Sum(x => x.CountInPercent).ToString(),
                EliteMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Elite).Sum(x => x.Value).ToString(),
                EliteCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Elite).Sum(x => x.CountInPercent).ToString(),
                NipponMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Nippon).Sum(x => x.Value).ToString(),
                NipponCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Nippon).Sum(x => x.CountInPercent).ToString(),
                //DuluxMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Dulux).Sum(x => x.Value).ToString(),
                //DuluxCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Dulux).Sum(x => x.CountInPercent).ToString(),
                //MoonstarMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Moonstar).Sum(x => x.Value).ToString(),
                //MoonstarCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Moonstar).Sum(x => x.CountInPercent).ToString(),
                OthersMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Others).Sum(x => x.Value).ToString(),
                OthersCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.Others).Sum(x => x.CountInPercent).ToString(),

                TotalMtdValue = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.AP
                                                    || x.companyName == ConstantPaintUsageMTDValue.Nerolac
                                                    || x.companyName == ConstantPaintUsageMTDValue.Elite
                                                    || x.companyName == ConstantPaintUsageMTDValue.Nippon
                                                    //|| x.companyName == ConstantPaintUsageMTDValue.Dulux
                                                    //|| x.companyName == ConstantPaintUsageMTDValue.Moonstar
                                                    || x.companyName == ConstantPaintUsageMTDValue.BPBL
                                                    || x.companyName == ConstantPaintUsageMTDValue.Others).Sum(x => x.Value).ToString(),
                TotalCount = paintersCallMtd.Where(x => x.companyName == ConstantPaintUsageMTDValue.AP
                                                    || x.companyName == ConstantPaintUsageMTDValue.Nerolac
                                                    || x.companyName == ConstantPaintUsageMTDValue.Elite
                                                    || x.companyName == ConstantPaintUsageMTDValue.Nippon
                                                    //|| x.companyName == ConstantPaintUsageMTDValue.Dulux
                                                    //|| x.companyName == ConstantPaintUsageMTDValue.Moonstar
                                                    || x.companyName == ConstantPaintUsageMTDValue.BPBL
                                                    || x.companyName == ConstantPaintUsageMTDValue.Others).Sum(x => x.CountInPercent).ToString(),

                IssueWithDbblAccount = x.issueWithDbblAccount,
                RemarkIssueWithDbblAccount = "",
                Comments = x.Comment,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<PainterCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = paintersCalls.Count();
            queryResult.Total = paintersCalls.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerVisitReportResultModel>> GetDealerVisitReportAsync(DealerVisitReportSearchModel query)
        {
            var reportResult = new List<DealerVisitReportResultModel>();
            int? month = (!query.Month.HasValue) ? DateTime.Now.Month : query.Month;
            int? year = (!query.Year.HasValue) ? DateTime.Now.Year : query.Year;
            int tvist = 0;
            int avisit = 0;

            var dealerVisit = await (from jpd in _context.JourneyPlanDetails
                                     join jpm in _context.JourneyPlanMasters on jpd.PlanId equals jpm.Id into jpmleftjoin
                                     from jpminfo in jpmleftjoin.DefaultIfEmpty()
                                     join dsc in _context.DealerSalesCalls.Select(x => new { x.JourneyPlanId }).Distinct() on jpd.PlanId equals dsc.JourneyPlanId into dscleftjoin
                                     from dscinfo in dscleftjoin.DefaultIfEmpty()
                                     join u in _context.UserInfos on jpminfo.EmployeeId equals u.EmployeeId into uleftjoin
                                     from userInfo in uleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on jpd.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                     from depinfo in depleftjoin.DefaultIfEmpty()
                                     join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                     from tinfo in tleftjoin.DefaultIfEmpty()
                                     join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                     from zinfo in zleftjoin.DefaultIfEmpty()
                                     where (
                                       (jpminfo.PlanDate.Month == month && jpminfo.PlanDate.Year == year)
                                       && (jpminfo.PlanStatus == PlanStatus.Approved)
                                       && (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                       && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                       && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                       && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                       && (!query.DealerId.HasValue || jpd.DealerId == query.DealerId.Value)
                                     )
                                     select new
                                     {
                                         jpminfo.EmployeeId,
                                         jpd.DealerId,
                                         userInfo.Email,
                                         diInfo.BusinessArea,
                                         depot = depinfo.Name1,
                                         territoryName = tinfo.Name,
                                         zoneName = zinfo.Name,
                                         diInfo.CustomerNo,
                                         diInfo.CustomerName,
                                         jpminfo.PlanDate,
                                         JourneyPlanId = dscinfo.JourneyPlanId
                                     }).Distinct().ToListAsync();

            var dealerVisitGroup = dealerVisit.GroupBy(x => new { x.EmployeeId, x.DealerId }).Select(x => new
            {
                userId = x.FirstOrDefault()?.Email,
                depotId = x.FirstOrDefault()?.BusinessArea,
                depotName = x.FirstOrDefault()?.depot ?? string.Empty,
                territory = x.FirstOrDefault()?.territoryName,
                zone = x.FirstOrDefault()?.zoneName,
                dealerId = x.FirstOrDefault()?.CustomerNo.ToString() ?? string.Empty,
                dealerName = x.FirstOrDefault()?.CustomerName,

                d1 = x.Count(c => c?.PlanDate.Day == 1) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 1) > 0 ? "Visited" : "Not Visited" : "",
                d2 = x.Count(c => c?.PlanDate.Day == 2) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 2) > 0 ? "Visited" : "Not Visited" : "",
                d3 = x.Count(c => c?.PlanDate.Day == 3) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 3) > 0 ? "Visited" : "Not Visited" : "",
                d4 = x.Count(c => c?.PlanDate.Day == 4) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 4) > 0 ? "Visited" : "Not Visited" : "",
                d5 = x.Count(c => c?.PlanDate.Day == 5) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 5) > 0 ? "Visited" : "Not Visited" : "",
                d6 = x.Count(c => c?.PlanDate.Day == 6) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 6) > 0 ? "Visited" : "Not Visited" : "",
                d7 = x.Count(c => c?.PlanDate.Day == 7) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 7) > 0 ? "Visited" : "Not Visited" : "",
                d8 = x.Count(c => c?.PlanDate.Day == 8) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 8) > 0 ? "Visited" : "Not Visited" : "",
                d9 = x.Count(c => c?.PlanDate.Day == 9) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 9) > 0 ? "Visited" : "Not Visited" : "",
                d10 = x.Count(c => c?.PlanDate.Day == 10) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 10) > 0 ? "Visited" : "Not Visited" : "",
                d11 = x.Count(c => c?.PlanDate.Day == 11) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 11) > 0 ? "Visited" : "Not Visited" : "",
                d12 = x.Count(c => c?.PlanDate.Day == 12) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 12) > 0 ? "Visited" : "Not Visited" : "",
                d13 = x.Count(c => c?.PlanDate.Day == 13) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 13) > 0 ? "Visited" : "Not Visited" : "",
                d14 = x.Count(c => c?.PlanDate.Day == 14) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 14) > 0 ? "Visited" : "Not Visited" : "",
                d15 = x.Count(c => c?.PlanDate.Day == 15) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 15) > 0 ? "Visited" : "Not Visited" : "",
                d16 = x.Count(c => c?.PlanDate.Day == 16) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 16) > 0 ? "Visited" : "Not Visited" : "",
                d17 = x.Count(c => c?.PlanDate.Day == 17) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 17) > 0 ? "Visited" : "Not Visited" : "",
                d18 = x.Count(c => c?.PlanDate.Day == 18) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 18) > 0 ? "Visited" : "Not Visited" : "",
                d19 = x.Count(c => c?.PlanDate.Day == 19) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 19) > 0 ? "Visited" : "Not Visited" : "",
                d20 = x.Count(c => c?.PlanDate.Day == 20) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 20) > 0 ? "Visited" : "Not Visited" : "",
                d21 = x.Count(c => c?.PlanDate.Day == 21) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 21) > 0 ? "Visited" : "Not Visited" : "",
                d22 = x.Count(c => c?.PlanDate.Day == 22) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 22) > 0 ? "Visited" : "Not Visited" : "",
                d23 = x.Count(c => c?.PlanDate.Day == 23) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 23) > 0 ? "Visited" : "Not Visited" : "",
                d24 = x.Count(c => c?.PlanDate.Day == 24) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 24) > 0 ? "Visited" : "Not Visited" : "",
                d25 = x.Count(c => c?.PlanDate.Day == 25) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 25) > 0 ? "Visited" : "Not Visited" : "",
                d26 = x.Count(c => c?.PlanDate.Day == 26) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 26) > 0 ? "Visited" : "Not Visited" : "",
                d27 = x.Count(c => c?.PlanDate.Day == 27) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 27) > 0 ? "Visited" : "Not Visited" : "",
                d28 = x.Count(c => c?.PlanDate.Day == 28) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 28) > 0 ? "Visited" : "Not Visited" : "",
                d29 = x.Count(c => c?.PlanDate.Day == 29) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 29) > 0 ? "Visited" : "Not Visited" : "",
                d30 = x.Count(c => c?.PlanDate.Day == 30) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 30) > 0 ? "Visited" : "Not Visited" : "",
                d31 = x.Count(c => c?.PlanDate.Day == 31) > 0 ?
                                        x.Count(c => c?.JourneyPlanId != null && c?.PlanDate.Day == 31) > 0 ? "Visited" : "Not Visited" : "",
                targetVisits = tvist = x.Count(c => c?.PlanDate.Month == month && c?.PlanDate.Year == year),
                actualVisits = avisit = x.Count(c => c?.JourneyPlanId != null && (c?.PlanDate.Month == month && c?.PlanDate.Year == year)),
                notVisits = (tvist - avisit)
            }).ToList();


            reportResult = dealerVisitGroup.Select(x => new DealerVisitReportResultModel
            {
                UserId = x.userId,
                DepotId = x.depotId,
                DepotName = x.depotName,
                Territory = x.territory,
                Zone = x?.zone,
                DealerId = x.dealerId,
                DealerName = x.dealerName,
                D1 = x.d1,
                D2 = x.d2,
                D3 = x.d3,
                D4 = x.d4,
                D5 = x.d5,
                D6 = x.d6,
                D7 = x.d7,
                D8 = x.d8,
                D9 = x.d9,
                D10 = x.d10,
                D11 = x.d11,
                D12 = x.d12,
                D13 = x.d13,
                D14 = x.d14,
                D15 = x.d15,
                D16 = x.d16,
                D17 = x.d17,
                D18 = x.d18,
                D19 = x.d19,
                D20 = x.d20,
                D21 = x.d21,
                D22 = x.d22,
                D23 = x.d23,
                D24 = x.d24,
                D25 = x.d25,
                D26 = x.d26,
                D27 = x.d27,
                D28 = x.d28,
                D29 = x.d29,
                D30 = x.d30,
                D31 = x.d31,
                TargetVisits = x.targetVisits,
                ActualVisits = x.actualVisits,
                NotVisits = x.notVisits,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerVisitReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealerVisitGroup.Count();
            queryResult.Total = dealerVisitGroup.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerSalesCallReportResultModel>> GetDealerSalesCallReportAsync(DealerSalesCallReportSearchModel query)
        {
            var reportResult = new List<DealerSalesCallReportResultModel>();

            var dealerCalls = await (from dsc in _context.DealerSalesCalls
                                     join jpm in _context.JourneyPlanMasters on dsc.JourneyPlanId equals jpm.Id
                                     join ssdd in _context.DropdownDetails on dsc.SecondarySalesRatingsId equals ssdd.Id into ssddleftjoin
                                     from ssddinfo in ssddleftjoin.DefaultIfEmpty()
                                     join ppldd in _context.DropdownDetails on dsc.PremiumProductLiftingId equals ppldd.Id into pplddleftjoin
                                     from pplddinfo in pplddleftjoin.DefaultIfEmpty()
                                     join mdd in _context.DropdownDetails on dsc.MerchendisingId equals mdd.Id into mddleftjoin
                                     from mddinfo in mddleftjoin.DefaultIfEmpty()
                                     join sdidd in _context.DropdownDetails on dsc.SubDealerInfluenceId equals sdidd.Id into sdiddleftjoin
                                     from sdiddinfo in sdiddleftjoin.DefaultIfEmpty()
                                     join pidd in _context.DropdownDetails on dsc.PainterInfluenceId equals pidd.Id into piddleftjoin
                                     from piddinfo in piddleftjoin.DefaultIfEmpty()
                                     join dsdd in _context.DropdownDetails on dsc.DealerSatisfactionId equals dsdd.Id into dsddleftjoin
                                     from dsddinfo in dsddleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                     from depinfo in depleftjoin.DefaultIfEmpty()
                                     join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                     from tinfo in tleftjoin.DefaultIfEmpty()
                                     join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                     from zinfo in zleftjoin.DefaultIfEmpty()
                                     join u in _context.UserInfos on dsc.UserId equals u.Id into uleftjoin
                                     from userInfo in uleftjoin.DefaultIfEmpty()
                                     where (
                                        (dsc.IsSubDealerCall == false)
                                        && (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                        && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                        && (!query.FromDate.HasValue || jpm.PlanDate.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || jpm.PlanDate.Date <= query.ToDate.Value.Date)
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.DealerId.HasValue || dsc.DealerId == query.DealerId.Value)
                                     )
                                     select new
                                     {
                                         dsc.Id,
                                         userInfo.Email,
                                         diInfo.BusinessArea,
                                         depot = depinfo.Name1,
                                         territory = tinfo.Name,
                                         zone = zinfo.Name,
                                         dsc.DealerId,
                                         diInfo.CustomerNo,
                                         diInfo.CustomerName,
                                         dsc.CreatedTime,
                                         dsc.IsTargetPromotionCommunicated,
                                         ssStatus = ssddinfo.DropdownName,
                                         dsc.SecondarySalesReasonRemarks,
                                         dsc.HasOS,
                                         dsc.IsSlippageCommunicated,
                                         dsc.IsPremiumProductCommunicated,
                                         ProductLiftingStatus = pplddinfo.DropdownName,
                                         dsc.PremiumProductLiftingOthers,
                                         Merchendising = mddinfo.DropdownName,
                                         dsc.HasPainterInfluence,
                                         PainterInfluecePercent = piddinfo.DropdownName,
                                         dsc.IsShopManProductKnowledgeDiscussed,
                                         dsc.IsShopManSalesTechniquesDiscussed,
                                         dsc.IsShopManMerchendizingImprovementDiscussed,
                                         dsc.BPBLAverageMonthlySales,
                                         dsc.BPBLActualMTDSales,
                                         dsc.HasCompetitionPresence,
                                         dsc.IsCompetitionServiceBetterThanBPBL,
                                         dsc.CompetitionServiceBetterThanBPBLRemarks,
                                         dsc.IsCompetitionProductDisplayBetterThanBPBL,
                                         dsc.CompetitionProductDisplayBetterThanBPBLRemarks,
                                         dsc.CompetitionProductDisplayImageUrl,
                                         dsc.CompetitionSchemeModalityComments,
                                         dsc.CompetitionSchemeModalityImageUrl,
                                         dsc.CompetitionShopBoysComments,
                                         dsc.HasDealerSalesIssue,
                                         DealerSatisfactionStatus = dsddinfo.DropdownName,
                                         dsc.DealerSatisfactionReason,
                                         dsc.IsTargetCommunicated,
                                         dsc.IsOSCommunicated,
                                         dsc.IsCBInstalled,
                                         dsc.IsCBProductivityCommunicated,
                                         dsc.HasSubDealerInfluence,
                                         sdInfluecePercent = sdiddinfo.DropdownName,
                                     }).ToListAsync();

            var dealerCompititions = (from dcs in _context.DealerCompetitionSales
                                      join dd in _context.DropdownDetails on dcs.CompanyId equals dd.Id into ddleft
                                      from ddinfo in ddleft.DefaultIfEmpty()
                                      select new
                                      {
                                          dcs.DealerSalesCallId,
                                          companyName = ddinfo.DropdownName,
                                          dcs.AverageMonthlySales,
                                          dcs.ActualMTDSales
                                      }).ToList();

            reportResult = dealerCalls.Select(x => new DealerSalesCallReportResultModel
            {
                UserId = x.Email ?? string.Empty,
                DepotId = x.BusinessArea ?? string.Empty,
                DepotName = x.depot,
                Territory = x.territory,
                Zone = x.zone,
                DealerId = x.CustomerNo.ToString(),
                DealerName = x.CustomerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime),
                TradePromotion = x.IsTargetPromotionCommunicated ? "Yes" : "No",
                Target = x.IsTargetCommunicated ? "Yes" : "No",
                SsStatus = x.ssStatus,
                SsReasonForPourOrAverage = x.SecondarySalesReasonRemarks,
                OsCommunication = x.IsOSCommunicated ? "Yes" : "No",
                SlippageCommunication = x.IsSlippageCommunicated ? "Yes" : "No",
                UspCommunication = x.IsPremiumProductCommunicated ? "Yes" : "No",
                ProductLiftingStatus = x.ProductLiftingStatus,
                ReasonForNotLifting = x.PremiumProductLiftingOthers,
                CbMachineStatus = x.IsCBInstalled ? "Yes" : "No",
                CbProductivity = x.IsCBProductivityCommunicated ? "Yes" : "No",
                Merchendising = x.Merchendising,
                SubDealerInfluence = x.HasSubDealerInfluence ? "Yes" : "No",
                SdInfluecePercent = x.sdInfluecePercent,
                PainterInfluence = x.HasPainterInfluence ? "Yes" : "No",
                PainterInfluecePercent = x.PainterInfluecePercent,
                ProductKnoledge = x.IsShopManProductKnowledgeDiscussed ? "Yes" : "No",
                SalesTechniques = x.IsShopManSalesTechniquesDiscussed ? "Yes" : "No",
                MerchendisingImprovement = x.IsShopManMerchendizingImprovementDiscussed ? "Yes" : "No",
                CompetitionPresence = x.HasCompetitionPresence ? "Yes" : "No",
                CompetitionService = x.IsCompetitionServiceBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                CsRemarks = x.CompetitionServiceBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingStatus = x.IsCompetitionProductDisplayBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                PdmRemarks = x.CompetitionProductDisplayBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingImage = x.CompetitionProductDisplayImageUrl,
                SchemeModality = x.CompetitionSchemeModalityComments,
                SchemeModalityImage = x.CompetitionSchemeModalityImageUrl,
                ShopBoy = x.CompetitionShopBoysComments,

                ApAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.AverageMonthlySales.ToString() ?? string.Empty,
                ApActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.ActualMTDSales.ToString() ?? string.Empty,
                NerolacAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.AverageMonthlySales.ToString() ?? string.Empty,
                NerolacActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.ActualMTDSales.ToString() ?? string.Empty,
                NipponAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.AverageMonthlySales.ToString() ?? string.Empty,
                NipponActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.ActualMTDSales.ToString() ?? string.Empty,
                DuluxAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.AverageMonthlySales.ToString() ?? string.Empty,
                DuluxActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.ActualMTDSales.ToString() ?? string.Empty,
                JotunAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.AverageMonthlySales.ToString() ?? string.Empty,
                JotunActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.ActualMTDSales.ToString() ?? string.Empty,
                MoonstarAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.AverageMonthlySales.ToString() ?? string.Empty,
                MoonstarActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.ActualMTDSales.ToString() ?? string.Empty,
                EliteAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.AverageMonthlySales.ToString() ?? string.Empty,
                EliteActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.ActualMTDSales.ToString() ?? string.Empty,
                AlkarimAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.AverageMonthlySales.ToString() ?? string.Empty,
                AlkarimActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.ActualMTDSales.ToString() ?? string.Empty,
                OthersAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.AverageMonthlySales.ToString() ?? string.Empty,
                OthersActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.ActualMTDSales.ToString() ?? string.Empty,
                TotalAvrgMonthlySales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.AverageMonthlySales).ToString(),
                TotalActualMtdSales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.ActualMTDSales).ToString(),

                DealerIssueStatus = x.HasDealerSalesIssue ? "Yes" : "No",
                DealerSatisfactionStatus = x.DealerSatisfactionStatus,
                DealerDissatisfactionReason = x.DealerSatisfactionReason,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerSalesCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealerCalls.Count();
            queryResult.Total = dealerCalls.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<SubDealerSalesCallReportResultModel>> GetSubDealerSalesCallReportAsync(SubDealerSalesCallReportSearchModel query)
        {
            var reportResult = new List<SubDealerSalesCallReportResultModel>();

            var dealerCalls = await (from dsc in _context.DealerSalesCalls
                                     join jpm in _context.JourneyPlanMasters on dsc.JourneyPlanId equals jpm.Id
                                     join ssdd in _context.DropdownDetails on dsc.SecondarySalesRatingsId equals ssdd.Id into ssddleftjoin
                                     from ssddinfo in ssddleftjoin.DefaultIfEmpty()
                                     join ppldd in _context.DropdownDetails on dsc.PremiumProductLiftingId equals ppldd.Id into pplddleftjoin
                                     from pplddinfo in pplddleftjoin.DefaultIfEmpty()
                                     join mdd in _context.DropdownDetails on dsc.MerchendisingId equals mdd.Id into mddleftjoin
                                     from mddinfo in mddleftjoin.DefaultIfEmpty()
                                     join sdidd in _context.DropdownDetails on dsc.SubDealerInfluenceId equals sdidd.Id into sdiddleftjoin
                                     from sdiddinfo in sdiddleftjoin.DefaultIfEmpty()
                                     join pidd in _context.DropdownDetails on dsc.PainterInfluenceId equals pidd.Id into piddleftjoin
                                     from piddinfo in piddleftjoin.DefaultIfEmpty()
                                     join dsdd in _context.DropdownDetails on dsc.DealerSatisfactionId equals dsdd.Id into dsddleftjoin
                                     from dsddinfo in dsddleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                     from depinfo in depleftjoin.DefaultIfEmpty()
                                     join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                     from tinfo in tleftjoin.DefaultIfEmpty()
                                     join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                     from zinfo in zleftjoin.DefaultIfEmpty()
                                     join u in _context.UserInfos on dsc.UserId equals u.Id into uleftjoin
                                     from userInfo in uleftjoin.DefaultIfEmpty()
                                     where (
                                        (dsc.IsSubDealerCall == true)
                                        && (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                        && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                        && (!query.FromDate.HasValue || jpm.PlanDate.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || jpm.PlanDate.Date <= query.ToDate.Value.Date)
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.SubDealerId.HasValue || dsc.DealerId == query.SubDealerId.Value)
                                     )
                                     select new
                                     {
                                         dsc.Id,
                                         userInfo.Email,
                                         diInfo.BusinessArea,
                                         depot = depinfo.Name1,
                                         territory = tinfo.Name,
                                         zone = zinfo.Name,
                                         dsc.DealerId,
                                         diInfo.CustomerNo,
                                         diInfo.CustomerName,
                                         dsc.CreatedTime,
                                         dsc.IsTargetPromotionCommunicated,
                                         ssStatus = ssddinfo.DropdownName,
                                         dsc.SecondarySalesReasonRemarks,
                                         dsc.HasOS,
                                         dsc.IsSlippageCommunicated,
                                         dsc.IsPremiumProductCommunicated,
                                         ProductLiftingStatus = pplddinfo.DropdownName,
                                         dsc.PremiumProductLiftingOthers,
                                         Merchendising = mddinfo.DropdownName,
                                         dsc.HasPainterInfluence,
                                         PainterInfluecePercent = piddinfo.DropdownName,
                                         dsc.IsShopManProductKnowledgeDiscussed,
                                         dsc.IsShopManSalesTechniquesDiscussed,
                                         dsc.IsShopManMerchendizingImprovementDiscussed,
                                         dsc.BPBLAverageMonthlySales,
                                         dsc.BPBLActualMTDSales,
                                         dsc.HasCompetitionPresence,
                                         dsc.IsCompetitionServiceBetterThanBPBL,
                                         dsc.CompetitionServiceBetterThanBPBLRemarks,
                                         dsc.IsCompetitionProductDisplayBetterThanBPBL,
                                         dsc.CompetitionProductDisplayBetterThanBPBLRemarks,
                                         dsc.CompetitionProductDisplayImageUrl,
                                         dsc.CompetitionSchemeModalityComments,
                                         dsc.CompetitionSchemeModalityImageUrl,
                                         dsc.CompetitionShopBoysComments,
                                         dsc.HasDealerSalesIssue,
                                         DealerSatisfactionStatus = dsddinfo.DropdownName,
                                         dsc.DealerSatisfactionReason
                                     }).ToListAsync();

            var dealerCompititions = (from dcs in _context.DealerCompetitionSales
                                      join dd in _context.DropdownDetails on dcs.CompanyId equals dd.Id into ddleft
                                      from ddinfo in ddleft.DefaultIfEmpty()
                                      select new
                                      {
                                          dcs.DealerSalesCallId,
                                          companyName = ddinfo.DropdownName,
                                          dcs.AverageMonthlySales,
                                          dcs.ActualMTDSales
                                      }).ToList();


            reportResult = dealerCalls.Select(x => new SubDealerSalesCallReportResultModel
            {
                UserId = x.Email ?? string.Empty,
                DepotId = x.BusinessArea ?? string.Empty,
                DepotName = x.depot,
                Territory = x.territory,
                Zone = x.zone,
                SubDealerId = x.CustomerNo.ToString(),
                SubDealerName = x.CustomerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime),
                TradePromotion = x.IsTargetPromotionCommunicated ? "Yes" : "No",
                SsStatus = x.ssStatus,
                SsReasonForPourOrAverage = x.SecondarySalesReasonRemarks,
                OsStatus = x.HasOS ? "Yes" : "No",
                OsActivity = x.IsSlippageCommunicated ? "Yes" : "No",
                UspCommunication = x.IsPremiumProductCommunicated ? "Yes" : "No",
                ProductLiftingStatus = x.ProductLiftingStatus,
                ReasonForNotLifting = x.PremiumProductLiftingOthers,
                Merchendising = x.Merchendising,
                PainterInfluence = x.HasPainterInfluence ? "Yes" : "No",
                PainterInfluecePercent = x.PainterInfluecePercent,
                ProductKnoledge = x.IsShopManProductKnowledgeDiscussed ? "Yes" : "No",
                SalesTechniques = x.IsShopManSalesTechniquesDiscussed ? "Yes" : "No",
                MerchendisingImprovement = x.IsShopManMerchendizingImprovementDiscussed ? "Yes" : "No",
                BergerAvrgMonthlySales = x.BPBLAverageMonthlySales.ToString(),
                BergerActualMtdSales = x.BPBLActualMTDSales.ToString(),
                CompetitionPresence = x.HasCompetitionPresence ? "Yes" : "No",
                CompetitionService = x.IsCompetitionServiceBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                CsRemarks = x.CompetitionServiceBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingStatus = x.IsCompetitionProductDisplayBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                PdmRemarks = x.CompetitionProductDisplayBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingImage = x.CompetitionProductDisplayImageUrl,
                SchemeModality = x.CompetitionSchemeModalityComments,
                SchemeModalityImage = x.CompetitionSchemeModalityImageUrl,
                ShopBoy = x.CompetitionShopBoysComments,

                ApAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.AverageMonthlySales.ToString() ?? string.Empty,
                ApActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.ActualMTDSales.ToString() ?? string.Empty,
                NerolacAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.AverageMonthlySales.ToString() ?? string.Empty,
                NerolacActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.ActualMTDSales.ToString() ?? string.Empty,
                NipponAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.AverageMonthlySales.ToString() ?? string.Empty,
                NipponActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.ActualMTDSales.ToString() ?? string.Empty,
                DuluxAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.AverageMonthlySales.ToString() ?? string.Empty,
                DuluxActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.ActualMTDSales.ToString() ?? string.Empty,
                JotunAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.AverageMonthlySales.ToString() ?? string.Empty,
                JotunActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.ActualMTDSales.ToString() ?? string.Empty,
                MoonstarAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.AverageMonthlySales.ToString() ?? string.Empty,
                MoonstarActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.ActualMTDSales.ToString() ?? string.Empty,
                EliteAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.AverageMonthlySales.ToString() ?? string.Empty,
                EliteActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.ActualMTDSales.ToString() ?? string.Empty,
                AlkarimAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.AverageMonthlySales.ToString() ?? string.Empty,
                AlkarimActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.ActualMTDSales.ToString() ?? string.Empty,
                OthersAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.AverageMonthlySales.ToString() ?? string.Empty,
                OthersActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.ActualMTDSales.ToString() ?? string.Empty,
                TotalAvrgMonthlySales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.AverageMonthlySales).ToString(),
                TotalActualMtdSales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.ActualMTDSales).ToString(),

                SubDealerIssueStatus = x.HasDealerSalesIssue ? "Yes" : "No",
                DealerSatisfactionStatus = x.DealerSatisfactionStatus,
                DealerDissatisfactionReason = x.DealerSatisfactionReason,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<SubDealerSalesCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealerCalls.Count();
            queryResult.Total = dealerCalls.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerSalesCallReportResultModel>> GetAddhocDealerSalesCallReportAsync(DealerSalesCallReportSearchModel query)
        {
            var reportResult = new List<DealerSalesCallReportResultModel>();

            var dealerCalls = await (from dsc in _context.DealerSalesCalls
                                     join ssdd in _context.DropdownDetails on dsc.SecondarySalesRatingsId equals ssdd.Id into ssddleftjoin
                                     from ssddinfo in ssddleftjoin.DefaultIfEmpty()
                                     join ppldd in _context.DropdownDetails on dsc.PremiumProductLiftingId equals ppldd.Id into pplddleftjoin
                                     from pplddinfo in pplddleftjoin.DefaultIfEmpty()
                                     join mdd in _context.DropdownDetails on dsc.MerchendisingId equals mdd.Id into mddleftjoin
                                     from mddinfo in mddleftjoin.DefaultIfEmpty()
                                     join sdidd in _context.DropdownDetails on dsc.SubDealerInfluenceId equals sdidd.Id into sdiddleftjoin
                                     from sdiddinfo in sdiddleftjoin.DefaultIfEmpty()
                                     join pidd in _context.DropdownDetails on dsc.PainterInfluenceId equals pidd.Id into piddleftjoin
                                     from piddinfo in piddleftjoin.DefaultIfEmpty()
                                     join dsdd in _context.DropdownDetails on dsc.DealerSatisfactionId equals dsdd.Id into dsddleftjoin
                                     from dsddinfo in dsddleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                     from depinfo in depleftjoin.DefaultIfEmpty()
                                     join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                     from tinfo in tleftjoin.DefaultIfEmpty()
                                     join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                     from zinfo in zleftjoin.DefaultIfEmpty()
                                     join u in _context.UserInfos on dsc.UserId equals u.Id into uleftjoin
                                     from userInfo in uleftjoin.DefaultIfEmpty()
                                     where (
                                        (dsc.IsSubDealerCall == false)
                                        && (dsc.JourneyPlanId == null)
                                        && (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                        && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                        && (!query.FromDate.HasValue || dsc.CreatedTime.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || dsc.CreatedTime.Date <= query.ToDate.Value.Date)
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.DealerId.HasValue || dsc.DealerId == query.DealerId.Value)
                                     )
                                     select new
                                     {
                                         dsc.Id,
                                         userInfo.Email,
                                         diInfo.BusinessArea,
                                         depot = depinfo.Name1,
                                         territory = tinfo.Name,
                                         zone = zinfo.Name,
                                         dsc.DealerId,
                                         diInfo.CustomerNo,
                                         diInfo.CustomerName,
                                         dsc.CreatedTime,
                                         dsc.IsTargetPromotionCommunicated,
                                         ssStatus = ssddinfo.DropdownName,
                                         dsc.SecondarySalesReasonRemarks,
                                         dsc.HasOS,
                                         dsc.IsSlippageCommunicated,
                                         dsc.IsPremiumProductCommunicated,
                                         ProductLiftingStatus = pplddinfo.DropdownName,
                                         dsc.PremiumProductLiftingOthers,
                                         Merchendising = mddinfo.DropdownName,
                                         dsc.HasPainterInfluence,
                                         PainterInfluecePercent = piddinfo.DropdownName,
                                         dsc.IsShopManProductKnowledgeDiscussed,
                                         dsc.IsShopManSalesTechniquesDiscussed,
                                         dsc.IsShopManMerchendizingImprovementDiscussed,
                                         dsc.BPBLAverageMonthlySales,
                                         dsc.BPBLActualMTDSales,
                                         dsc.HasCompetitionPresence,
                                         dsc.IsCompetitionServiceBetterThanBPBL,
                                         dsc.CompetitionServiceBetterThanBPBLRemarks,
                                         dsc.IsCompetitionProductDisplayBetterThanBPBL,
                                         dsc.CompetitionProductDisplayBetterThanBPBLRemarks,
                                         dsc.CompetitionProductDisplayImageUrl,
                                         dsc.CompetitionSchemeModalityComments,
                                         dsc.CompetitionSchemeModalityImageUrl,
                                         dsc.CompetitionShopBoysComments,
                                         dsc.HasDealerSalesIssue,
                                         DealerSatisfactionStatus = dsddinfo.DropdownName,
                                         dsc.DealerSatisfactionReason,
                                         dsc.IsTargetCommunicated,
                                         dsc.IsOSCommunicated,
                                         dsc.IsCBInstalled,
                                         dsc.IsCBProductivityCommunicated,
                                         dsc.HasSubDealerInfluence,
                                         sdInfluecePercent = sdiddinfo.DropdownName,
                                     }).ToListAsync();

            var dealerCompititions = (from dcs in _context.DealerCompetitionSales
                                      join dd in _context.DropdownDetails on dcs.CompanyId equals dd.Id into ddleft
                                      from ddinfo in ddleft.DefaultIfEmpty()
                                      select new
                                      {
                                          dcs.DealerSalesCallId,
                                          companyName = ddinfo.DropdownName,
                                          dcs.AverageMonthlySales,
                                          dcs.ActualMTDSales
                                      }).ToList();

            reportResult = dealerCalls.Select(x => new DealerSalesCallReportResultModel
            {
                UserId = x.Email ?? string.Empty,
                DepotId = x.BusinessArea ?? string.Empty,
                DepotName = x.depot,
                Territory = x.territory,
                Zone = x.zone,
                DealerId = x.CustomerNo.ToString(),
                DealerName = x.CustomerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime),
                TradePromotion = x.IsTargetPromotionCommunicated ? "Yes" : "No",
                Target = x.IsTargetCommunicated ? "Yes" : "No",
                SsStatus = x.ssStatus,
                SsReasonForPourOrAverage = x.SecondarySalesReasonRemarks,
                OsCommunication = x.IsOSCommunicated ? "Yes" : "No",
                SlippageCommunication = x.IsSlippageCommunicated ? "Yes" : "No",
                UspCommunication = x.IsPremiumProductCommunicated ? "Yes" : "No",
                ProductLiftingStatus = x.ProductLiftingStatus,
                ReasonForNotLifting = x.PremiumProductLiftingOthers,
                CbMachineStatus = x.IsCBInstalled ? "Yes" : "No",
                CbProductivity = x.IsCBProductivityCommunicated ? "Yes" : "No",
                Merchendising = x.Merchendising,
                SubDealerInfluence = x.HasSubDealerInfluence ? "Yes" : "No",
                SdInfluecePercent = x.sdInfluecePercent,
                PainterInfluence = x.HasPainterInfluence ? "Yes" : "No",
                PainterInfluecePercent = x.PainterInfluecePercent,
                ProductKnoledge = x.IsShopManProductKnowledgeDiscussed ? "Yes" : "No",
                SalesTechniques = x.IsShopManSalesTechniquesDiscussed ? "Yes" : "No",
                MerchendisingImprovement = x.IsShopManMerchendizingImprovementDiscussed ? "Yes" : "No",
                CompetitionPresence = x.HasCompetitionPresence ? "Yes" : "No",
                CompetitionService = x.IsCompetitionServiceBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                CsRemarks = x.CompetitionServiceBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingStatus = x.IsCompetitionProductDisplayBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                PdmRemarks = x.CompetitionProductDisplayBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingImage = x.CompetitionProductDisplayImageUrl,
                SchemeModality = x.CompetitionSchemeModalityComments,
                SchemeModalityImage = x.CompetitionSchemeModalityImageUrl,
                ShopBoy = x.CompetitionShopBoysComments,

                ApAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.AverageMonthlySales.ToString() ?? string.Empty,
                ApActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.ActualMTDSales.ToString() ?? string.Empty,
                NerolacAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.AverageMonthlySales.ToString() ?? string.Empty,
                NerolacActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.ActualMTDSales.ToString() ?? string.Empty,
                NipponAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.AverageMonthlySales.ToString() ?? string.Empty,
                NipponActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.ActualMTDSales.ToString() ?? string.Empty,
                DuluxAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.AverageMonthlySales.ToString() ?? string.Empty,
                DuluxActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.ActualMTDSales.ToString() ?? string.Empty,
                JotunAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.AverageMonthlySales.ToString() ?? string.Empty,
                JotunActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.ActualMTDSales.ToString() ?? string.Empty,
                MoonstarAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.AverageMonthlySales.ToString() ?? string.Empty,
                MoonstarActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.ActualMTDSales.ToString() ?? string.Empty,
                EliteAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.AverageMonthlySales.ToString() ?? string.Empty,
                EliteActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.ActualMTDSales.ToString() ?? string.Empty,
                AlkarimAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.AverageMonthlySales.ToString() ?? string.Empty,
                AlkarimActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.ActualMTDSales.ToString() ?? string.Empty,
                OthersAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.AverageMonthlySales.ToString() ?? string.Empty,
                OthersActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.ActualMTDSales.ToString() ?? string.Empty,
                TotalAvrgMonthlySales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.AverageMonthlySales).ToString(),
                TotalActualMtdSales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.ActualMTDSales).ToString(),

                DealerIssueStatus = x.HasDealerSalesIssue ? "Yes" : "No",
                DealerSatisfactionStatus = x.DealerSatisfactionStatus,
                DealerDissatisfactionReason = x.DealerSatisfactionReason,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerSalesCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealerCalls.Count();
            queryResult.Total = dealerCalls.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<SubDealerSalesCallReportResultModel>> GetAddhocSubDealerSalesCallReportAsync(SubDealerSalesCallReportSearchModel query)
        {
            var reportResult = new List<SubDealerSalesCallReportResultModel>();

            var dealerCalls = await (from dsc in _context.DealerSalesCalls
                                     join ssdd in _context.DropdownDetails on dsc.SecondarySalesRatingsId equals ssdd.Id into ssddleftjoin
                                     from ssddinfo in ssddleftjoin.DefaultIfEmpty()
                                     join ppldd in _context.DropdownDetails on dsc.PremiumProductLiftingId equals ppldd.Id into pplddleftjoin
                                     from pplddinfo in pplddleftjoin.DefaultIfEmpty()
                                     join mdd in _context.DropdownDetails on dsc.MerchendisingId equals mdd.Id into mddleftjoin
                                     from mddinfo in mddleftjoin.DefaultIfEmpty()
                                     join sdidd in _context.DropdownDetails on dsc.SubDealerInfluenceId equals sdidd.Id into sdiddleftjoin
                                     from sdiddinfo in sdiddleftjoin.DefaultIfEmpty()
                                     join pidd in _context.DropdownDetails on dsc.PainterInfluenceId equals pidd.Id into piddleftjoin
                                     from piddinfo in piddleftjoin.DefaultIfEmpty()
                                     join dsdd in _context.DropdownDetails on dsc.DealerSatisfactionId equals dsdd.Id into dsddleftjoin
                                     from dsddinfo in dsddleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                     from depinfo in depleftjoin.DefaultIfEmpty()
                                     join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                     from tinfo in tleftjoin.DefaultIfEmpty()
                                     join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                     from zinfo in zleftjoin.DefaultIfEmpty()
                                     join u in _context.UserInfos on dsc.UserId equals u.Id into uleftjoin
                                     from userInfo in uleftjoin.DefaultIfEmpty()
                                     where (
                                        (dsc.IsSubDealerCall == true) 
                                        && (dsc.JourneyPlanId == null)
                                        && (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                        && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                        && (!query.FromDate.HasValue || dsc.CreatedTime.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || dsc.CreatedTime.Date <= query.ToDate.Value.Date)
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.SubDealerId.HasValue || dsc.DealerId == query.SubDealerId.Value)
                                     )
                                     select new
                                     {
                                         dsc.Id,
                                         userInfo.Email,
                                         diInfo.BusinessArea,
                                         depot = depinfo.Name1,
                                         territory = tinfo.Name,
                                         zone = zinfo.Name,
                                         dsc.DealerId,
                                         diInfo.CustomerNo,
                                         diInfo.CustomerName,
                                         dsc.CreatedTime,
                                         dsc.IsTargetPromotionCommunicated,
                                         ssStatus = ssddinfo.DropdownName,
                                         dsc.SecondarySalesReasonRemarks,
                                         dsc.HasOS,
                                         dsc.IsSlippageCommunicated,
                                         dsc.IsPremiumProductCommunicated,
                                         ProductLiftingStatus = pplddinfo.DropdownName,
                                         dsc.PremiumProductLiftingOthers,
                                         Merchendising = mddinfo.DropdownName,
                                         dsc.HasPainterInfluence,
                                         PainterInfluecePercent = piddinfo.DropdownName,
                                         dsc.IsShopManProductKnowledgeDiscussed,
                                         dsc.IsShopManSalesTechniquesDiscussed,
                                         dsc.IsShopManMerchendizingImprovementDiscussed,
                                         dsc.BPBLAverageMonthlySales,
                                         dsc.BPBLActualMTDSales,
                                         dsc.HasCompetitionPresence,
                                         dsc.IsCompetitionServiceBetterThanBPBL,
                                         dsc.CompetitionServiceBetterThanBPBLRemarks,
                                         dsc.IsCompetitionProductDisplayBetterThanBPBL,
                                         dsc.CompetitionProductDisplayBetterThanBPBLRemarks,
                                         dsc.CompetitionProductDisplayImageUrl,
                                         dsc.CompetitionSchemeModalityComments,
                                         dsc.CompetitionSchemeModalityImageUrl,
                                         dsc.CompetitionShopBoysComments,
                                         dsc.HasDealerSalesIssue,
                                         DealerSatisfactionStatus = dsddinfo.DropdownName,
                                         dsc.DealerSatisfactionReason
                                     }).ToListAsync();

            var dealerCompititions = (from dcs in _context.DealerCompetitionSales
                                      join dd in _context.DropdownDetails on dcs.CompanyId equals dd.Id into ddleft
                                      from ddinfo in ddleft.DefaultIfEmpty()
                                      select new
                                      {
                                          dcs.DealerSalesCallId,
                                          companyName = ddinfo.DropdownName,
                                          dcs.AverageMonthlySales,
                                          dcs.ActualMTDSales
                                      }).ToList();


            reportResult = dealerCalls.Select(x => new SubDealerSalesCallReportResultModel
            {
                UserId = x.Email ?? string.Empty,
                DepotId = x.BusinessArea ?? string.Empty,
                DepotName = x.depot,
                Territory = x.territory,
                Zone = x.zone,
                SubDealerId = x.CustomerNo.ToString(),
                SubDealerName = x.CustomerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.CreatedTime),
                TradePromotion = x.IsTargetPromotionCommunicated ? "Yes" : "No",
                SsStatus = x.ssStatus,
                SsReasonForPourOrAverage = x.SecondarySalesReasonRemarks,
                OsStatus = x.HasOS ? "Yes" : "No",
                OsActivity = x.IsSlippageCommunicated ? "Yes" : "No",
                UspCommunication = x.IsPremiumProductCommunicated ? "Yes" : "No",
                ProductLiftingStatus = x.ProductLiftingStatus,
                ReasonForNotLifting = x.PremiumProductLiftingOthers,
                Merchendising = x.Merchendising,
                PainterInfluence = x.HasPainterInfluence ? "Yes" : "No",
                PainterInfluecePercent = x.PainterInfluecePercent,
                ProductKnoledge = x.IsShopManProductKnowledgeDiscussed ? "Yes" : "No",
                SalesTechniques = x.IsShopManSalesTechniquesDiscussed ? "Yes" : "No",
                MerchendisingImprovement = x.IsShopManMerchendizingImprovementDiscussed ? "Yes" : "No",
                BergerAvrgMonthlySales = x.BPBLAverageMonthlySales.ToString(),
                BergerActualMtdSales = x.BPBLActualMTDSales.ToString(),
                CompetitionPresence = x.HasCompetitionPresence ? "Yes" : "No",
                CompetitionService = x.IsCompetitionServiceBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                CsRemarks = x.CompetitionServiceBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingStatus = x.IsCompetitionProductDisplayBetterThanBPBL ? "Better than BPBL" : "Less than BPBL",
                PdmRemarks = x.CompetitionProductDisplayBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingImage = x.CompetitionProductDisplayImageUrl,
                SchemeModality = x.CompetitionSchemeModalityComments,
                SchemeModalityImage = x.CompetitionSchemeModalityImageUrl,
                ShopBoy = x.CompetitionShopBoysComments,

                ApAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.AverageMonthlySales.ToString() ?? string.Empty,
                ApActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AP)?.ActualMTDSales.ToString() ?? string.Empty,
                NerolacAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.AverageMonthlySales.ToString() ?? string.Empty,
                NerolacActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nerolac)?.ActualMTDSales.ToString() ?? string.Empty,
                NipponAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.AverageMonthlySales.ToString() ?? string.Empty,
                NipponActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Nippon)?.ActualMTDSales.ToString() ?? string.Empty,
                DuluxAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.AverageMonthlySales.ToString() ?? string.Empty,
                DuluxActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Dulux)?.ActualMTDSales.ToString() ?? string.Empty,
                JotunAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.AverageMonthlySales.ToString() ?? string.Empty,
                JotunActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Jotun)?.ActualMTDSales.ToString() ?? string.Empty,
                MoonstarAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.AverageMonthlySales.ToString() ?? string.Empty,
                MoonstarActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Moonstar)?.ActualMTDSales.ToString() ?? string.Empty,
                EliteAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.AverageMonthlySales.ToString() ?? string.Empty,
                EliteActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Elite)?.ActualMTDSales.ToString() ?? string.Empty,
                AlkarimAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.AverageMonthlySales.ToString() ?? string.Empty,
                AlkarimActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.AlKarim)?.ActualMTDSales.ToString() ?? string.Empty,
                OthersAvrgMonthlySales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.AverageMonthlySales.ToString() ?? string.Empty,
                OthersActualMtdSales = dealerCompititions.FirstOrDefault(y => y.DealerSalesCallId == x?.Id && y.companyName == ConstantSwappingCompetitionValue.Others)?.ActualMTDSales.ToString() ?? string.Empty,
                TotalAvrgMonthlySales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.AverageMonthlySales).ToString(),
                TotalActualMtdSales = dealerCompititions.Where(y => y.DealerSalesCallId == x?.Id).Sum(y => y.ActualMTDSales).ToString(),

                SubDealerIssueStatus = x.HasDealerSalesIssue ? "Yes" : "No",
                DealerSatisfactionStatus = x.DealerSatisfactionStatus,
                DealerDissatisfactionReason = x.DealerSatisfactionReason,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<SubDealerSalesCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealerCalls.Count();
            queryResult.Total = dealerCalls.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerIssueReportResultModel>> GetDealerIssueReportAsync(DealerIssueReportSearchModel query)
        {
            var reportResult = new List<DealerIssueReportResultModel>();

            var dealerIssue = await (from dsi in _context.DealerSalesIssues
                                     join dsc in _context.DealerSalesCalls on dsi.DealerSalesCallId equals dsc.Id into dscleft
                                     from dscInfo in dscleft.DefaultIfEmpty()
                                     join di in _context.DealerInfos on dscInfo.DealerId equals di.Id into dileft
                                     from diInfo in dileft.DefaultIfEmpty()
                                     join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                     from depInfo in depleftjoin.DefaultIfEmpty()
                                     join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                     from tInfo in tleftjoin.DefaultIfEmpty()
                                     join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                     from zInfo in zleftjoin.DefaultIfEmpty()
                                     join u in _context.UserInfos on dscInfo.UserId equals u.Id into uleftjoin
                                     from userInfo in uleftjoin.DefaultIfEmpty()
                                     join dscdd in _context.DropdownDetails on dsi.DealerSalesIssueCategoryId equals dscdd.Id into dscddleft
                                     from dscddInfo in dscddleft.DefaultIfEmpty()
                                     join pdd in _context.DropdownDetails on dsi.PriorityId equals pdd.Id into pddleft
                                     from pddInfo in pddleft.DefaultIfEmpty()
                                     join cbmdd in _context.DropdownDetails on dsi.CBMachineMantainanceId equals cbmdd.Id into cbmddleft
                                     from cbmddInfo in cbmddleft.DefaultIfEmpty()
                                     where (
                                        (dscInfo.IsSubDealerCall == false)
                                        && (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                        && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                        && (!query.FromDate.HasValue || dscInfo.CreatedTime.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || dscInfo.CreatedTime.Date <= query.ToDate.Value.Date)
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.DealerId.HasValue || dscInfo.DealerId == query.DealerId.Value)
                                     )
                                     select new
                                     {
                                         userInfo.Email,
                                         diInfo.BusinessArea,
                                         depot = depInfo.Name1,
                                         territory = tInfo.Name,
                                         zone = zInfo.Name,
                                         dscInfo.DealerId,
                                         diInfo.CustomerNo,
                                         diInfo.CustomerName,
                                         dscInfo.CreatedTime,
                                         dsi.MaterialName,
                                         dsi.MaterialGroup,
                                         dsi.BatchNumber,
                                         dsi.Comments,
                                         priority = pddInfo.DropdownName,
                                         dsi.Quantity,
                                         dsi.HasCBMachineMantainance,
                                         maintinaceFrequency = cbmddInfo.DropdownName,
                                         dsi.CBMachineMantainanceRegularReason,
                                         issueCategory = dscddInfo.DropdownName,
                                         dsi.DealerSalesCallId
                                     }).ToListAsync();

            var groupdealerIssue = dealerIssue.GroupBy(x => x.DealerSalesCallId).Select(x => new
            {
                dealerSalesCallId = x.Key,
                userId = x.FirstOrDefault()?.Email,
                depotId = x.FirstOrDefault()?.DealerId,
                depotName = x.FirstOrDefault()?.depot,
                territoryName = x.FirstOrDefault()?.territory,
                zoneName = x.FirstOrDefault()?.zone,
                dealerId = x.FirstOrDefault()?.CustomerNo,
                dealerName = x.FirstOrDefault()?.CustomerName,
                visitDate = x.FirstOrDefault()?.CreatedTime,
                pcMaterial = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaint)?.MaterialName,
                pcMaterialGroup = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaint)?.MaterialGroup,
                pcQuantity = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaint)?.Quantity,
                pcBatchNumber = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaint)?.BatchNumber,
                pcComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaint)?.Comments,
                pcPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaint)?.priority,
                posComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShort)?.Comments,
                posPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShort)?.priority,
                shadeComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCard)?.Comments,
                shadePriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCard)?.priority,
                shopsignComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplain)?.Comments,
                shopsignPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplain)?.priority,
                deliveryComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssue)?.Comments,
                deliveryPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssue)?.priority,
                damageMaterial = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProduct)?.MaterialName,
                damageMaterialGroup = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProduct)?.MaterialGroup,
                damageQuantity = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProduct)?.Quantity,
                damageComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProduct)?.Comments,
                damagePriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProduct)?.priority,
                cbmStatus = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.HasCBMachineMantainance ?? false,
                cbmMaintatinanceFrequency = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.maintinaceFrequency,
                cbmRemarks = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.CBMachineMantainanceRegularReason,
                cbmPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.priority,
                othersComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.Others)?.Comments,
                othersPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.Others)?.priority
            }).ToList();

            reportResult = groupdealerIssue.Select(x => new DealerIssueReportResultModel
            {
                UserId = x.userId,
                DepotId = x.depotId.ToString(),
                DepotName = x.depotName,
                Territory = x.territoryName,
                Zone = x.zoneName,
                DealerId = x.dealerId.ToString(),
                DealerName = x.dealerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.visitDate),
                PcMaterial = x.pcMaterial,
                PcMaterialGroup = x.pcMaterialGroup,
                PcQuantity = x.pcQuantity.ToString(),
                PcBatchNumber = x.pcBatchNumber,
                PcComments = x.pcComments,
                PcPriority = x.pcPriority,
                PosComments = x.posComments,
                PosPriority = x.posPriority,
                ShadeComments = x.shadeComments,
                ShadePriority = x.shadePriority,
                ShopSignComments = x.shopsignComments,
                ShopSignPriority = x.shopsignPriority,
                DeliveryComments = x.deliveryComments,
                DeliveryPriority = x.deliveryPriority,
                DamageMaterial = x.damageMaterial,
                DamageMaterialGroup = x.damageMaterialGroup,
                DamageQuantity = x.damageQuantity.ToString(),
                DamageComments = x.damageComments,
                DamagecPriority = x.damagePriority,
                CBMStatus = x.cbmStatus ? "Yes" : "No",
                CBMMaintatinanceFrequency = x.cbmMaintatinanceFrequency,
                CBMRemarks = x.cbmRemarks,
                CBMPriority = x.cbmPriority,
                OthersComment = x.othersComments,
                Othersriority = x.othersPriority
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerIssueReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = groupdealerIssue.Count();
            queryResult.Total = groupdealerIssue.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<SubDealerIssueReportResultModel>> GetSubDealerIssueReportAsync(SubDealerIssueReportSearchModel query)
        {
            var reportResult = new List<SubDealerIssueReportResultModel>();

            var subDealerIssue = await (from dsi in _context.DealerSalesIssues
                                        join dsc in _context.DealerSalesCalls on dsi.DealerSalesCallId equals dsc.Id into dscleft
                                        from dscInfo in dscleft.DefaultIfEmpty()
                                        join di in _context.DealerInfos on dscInfo.DealerId equals di.Id into dileft
                                        from diInfo in dileft.DefaultIfEmpty()
                                        join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                        from depInfo in depleftjoin.DefaultIfEmpty()
                                        join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                        from tInfo in tleftjoin.DefaultIfEmpty()
                                        join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                        from zInfo in zleftjoin.DefaultIfEmpty()
                                        join u in _context.UserInfos on dscInfo.UserId equals u.Id into uleftjoin
                                        from userInfo in uleftjoin.DefaultIfEmpty()
                                        join dscdd in _context.DropdownDetails on dsi.DealerSalesIssueCategoryId equals dscdd.Id into dscddleft
                                        from dscddInfo in dscddleft.DefaultIfEmpty()
                                        join pdd in _context.DropdownDetails on dsi.PriorityId equals pdd.Id into pddleft
                                        from pddInfo in pddleft.DefaultIfEmpty()
                                        join cbmdd in _context.DropdownDetails on dsi.CBMachineMantainanceId equals cbmdd.Id into cbmddleft
                                        from cbmddInfo in cbmddleft.DefaultIfEmpty()
                                        where (
                                           (dscInfo.IsSubDealerCall == true)
                                           && (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                           && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                           && (!query.FromDate.HasValue || dscInfo.CreatedTime.Date >= query.FromDate.Value.Date)
                                           && (!query.ToDate.HasValue || dscInfo.CreatedTime.Date <= query.ToDate.Value.Date)
                                           && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                           && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                           && (!query.SubDealerId.HasValue || dscInfo.DealerId == query.SubDealerId.Value)
                                        )
                                        select new
                                        {
                                            userInfo.Email,
                                            diInfo.BusinessArea,
                                            depot = depInfo.Name1,
                                            territory = tInfo.Name,
                                            zone = zInfo.Name,
                                            dscInfo.DealerId,
                                            diInfo.CustomerNo,
                                            diInfo.CustomerName,
                                            dscInfo.CreatedTime,
                                            dsi.MaterialName,
                                            dsi.MaterialGroup,
                                            dsi.BatchNumber,
                                            dsi.Comments,
                                            priority = pddInfo.DropdownName,
                                            dsi.Quantity,
                                            dsi.HasCBMachineMantainance,
                                            maintinaceFrequency = cbmddInfo.DropdownName,
                                            dsi.CBMachineMantainanceRegularReason,
                                            issueCategory = dscddInfo.DropdownName,
                                            dsi.DealerSalesCallId
                                        }).ToListAsync();

            var groupSubDealerIssue = subDealerIssue.GroupBy(x => x.DealerSalesCallId).Select(x => new
            {
                dealerSalesCallId = x.Key,
                userId = x.FirstOrDefault()?.Email,
                depotId = x.FirstOrDefault()?.DealerId,
                depotName = x.FirstOrDefault()?.depot,
                territoryName = x.FirstOrDefault()?.territory,
                zoneName = x.FirstOrDefault()?.zone,
                dealerId = x.FirstOrDefault()?.CustomerNo,
                dealerName = x.FirstOrDefault()?.CustomerName,
                visitDate = x.FirstOrDefault()?.CreatedTime,
                posComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShort)?.Comments,
                posPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShort)?.priority,
                shadeComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCard)?.Comments,
                shadePriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCard)?.priority,
                shopsignComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplain)?.Comments,
                shopsignPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplain)?.priority,
                deliveryComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssue)?.Comments,
                deliveryPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssue)?.priority,
                cbmStatus = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.HasCBMachineMantainance ?? false,
                cbmMaintatinanceFrequency = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.maintinaceFrequency,
                cbmRemarks = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.CBMachineMantainanceRegularReason,
                cbmPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainance)?.priority,
                othersComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.Others)?.Comments,
                othersPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.Others)?.priority
            }).ToList();

            reportResult = groupSubDealerIssue.Select(x => new SubDealerIssueReportResultModel
            {
                UserId = x.userId,
                DepotId = x.depotId.ToString(),
                DepotName = x.depotName,
                Territory = x.territoryName,
                Zone = x.zoneName,
                SubDealerId = x.dealerId.ToString(),
                SubDealerName = x.dealerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.visitDate),
                PosComments = x.posComments,
                PosPriority = x.posPriority,
                ShadeComments = x.shadeComments,
                ShadePriority = x.shadePriority,
                ShopSignComments = x.shopsignComments,
                ShopSignPriority = x.shopsignPriority,
                DeliveryComments = x.deliveryComments,
                DeliveryPriority = x.deliveryPriority,
                CBMStatus = x.cbmStatus ? "Yes" : "No",
                CBMMaintatinanceFrequency = x.cbmMaintatinanceFrequency,
                CBMRemarks = x.cbmRemarks,
                CBMPriority = x.cbmPriority,
                OthersComment = x.othersComments,
                Othersriority = x.othersPriority
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<SubDealerIssueReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = groupSubDealerIssue.Count();
            queryResult.Total = groupSubDealerIssue.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<OsOver90daysTrendReportResultModel>> GetOsOver90daysTrendReport(OsOver90daysTrendReportSearchModel query)
        {
            //var userDealerIds = await _service.GetDealerByUserId(AppIdentity.AppUser.UserId);
            var userDealerIds = new List<string>();

            if (query.UserId.HasValue)
                userDealerIds = (await _service.GetDealerByUserId(query.UserId.Value)).ToList();

            var dbResult = await _dealerInfoRepository.FindByCondition(x =>
                (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                && (!query.SalesGroups.Any() || query.SalesGroups.Contains(x.SalesGroup))
                && (!query.Zones.Any() || query.Zones.Contains(x.CustZone))
                && (string.IsNullOrWhiteSpace(query.Depot) || query.Depot == x.BusinessArea)
                //&& (string.IsNullOrWhiteSpace(query.AccountGroup) || query.AccountGroup == x.AccountGroup)
                //&& (string.IsNullOrWhiteSpace(query.SalesOffice) || query.SalesOffice == x.SalesOffice)
                //&& (string.IsNullOrWhiteSpace(query.CreditControlArea) || query.CreditControlArea == x.CreditControlArea)
                && (!query.DealerId.HasValue || query.DealerId == x.Id)
                && (!query.UserId.HasValue || userDealerIds.Contains(x.CustomerNo))
            ).Select(x => new
            {
                x.Territory,
                x.CustomerNo,
                x.CustZone,
                //x.CreditControlArea,
                x.CustomerName,
            }).ToListAsync();

            var dealerIds = dbResult.Select(x => x.CustomerNo).Distinct().ToList();
            //dealerIds = new List<int> { 24 };

            query.FromDate = query.FromDate ?? new DateTime(query.FromYear, query.FromMonth, 1);
            query.ToDate = query.ToDate ?? new DateTime(query.ToYear, query.ToMonth, 1);

            var monthList = Enumerable.Range(0, Int32.MaxValue)
                .Select(e => query.FromDate.Value.AddMonths(e))
                .TakeWhile(e => e <= query.ToDate.Value)
                .Select(e => new
                {
                    e.Month,
                    e.Year,
                    MonthName = e.ToString("MMMM")
                }).ToList();

            IList<FinancialDataModel> firstMonthData = new List<FinancialDataModel>();
            IList<FinancialDataModel> secondMonthData = new List<FinancialDataModel>();
            IList<FinancialDataModel> thirdMonthData = new List<FinancialDataModel>();

            #region data call by single customer
            var dataAll = new List<FinancialDataModel>();

            foreach (var dealerId in dealerIds)
            {
                var fDate = monthList[0];
                var lDate = monthList[2];
                var startDate = new DateTime(fDate.Year, fDate.Month, 1);
                var endDate = new DateTime(lDate.Year, lDate.Month, DateTime.DaysInMonth(lDate.Year, lDate.Month));
                var dataSingle = (await _financialDataService.GetOsOver90DaysTrend(dealerId, startDate, endDate, query.CreditControlArea)).ToList();
                if (dataSingle.Any())
                {
                    dataAll.AddRange(dataSingle);
                }
            }
            #endregion

            for (int i = 0; i < monthList.Take(3).Count(); i++)
            {
                var item = monthList[i];
                var startDate = new DateTime(item.Year, item.Month, 1);
                var endDate = new DateTime(item.Year, item.Month, DateTime.DaysInMonth(item.Year, item.Month));
                //IList<FinancialDataModel> data = await _financialDataService.GetOsOver90DaysTrend(dealerIds, startDate, endDate);
                //IList<FinancialDataModel> data = dataAll.Where(x => CustomConvertExtension.ObjectToDateTime(x.PostingDate).Date >= startDate.Date
                //                                    && CustomConvertExtension.ObjectToDateTime(x.PostingDate).Date <= endDate.Date).ToList();
                IList<FinancialDataModel> data = dataAll.Where(x => CustomConvertExtension.ObjectToDateTime(x.Date).Date >= startDate.Date
                                                    && CustomConvertExtension.ObjectToDateTime(x.Date).Date <= endDate.Date).ToList();

                switch (i)
                {
                    case 0:
                        firstMonthData = data;
                        break;
                    case 1:
                        secondMonthData = data;
                        break;
                    case 2:
                        thirdMonthData = data;
                        break;
                }
            }

            Func<FinancialDataModel, string> selectFunc = x => x.CustomerNo;

            Func<FinancialDataModel, string, bool> predicateFunc = (x, val) => x.CustomerNo == val;

            Func<FinancialDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.Amount);

            var contactResult = firstMonthData.Select(x => selectFunc(x))
                .Concat(secondMonthData.Select(x => selectFunc(x)))
                .Concat(thirdMonthData.Select(x => selectFunc(x)))
                .Distinct()
                .ToList();

            var result = new List<OsOver90daysTrendReportResultModel>();

            foreach (var item in contactResult)
            {
                var res = new OsOver90daysTrendReportResultModel();
                if (firstMonthData.Any(x => predicateFunc(x, item)))
                {
                    res.Month1Value = firstMonthData.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.Month1Name = monthList[0].MonthName;
                    res.CreditControlArea = string.IsNullOrEmpty(res.CreditControlArea) ?
                        firstMonthData.Where(x => predicateFunc(x, item)).Select(x => x.CreditControlArea).FirstOrDefault() : string.Empty;
                }

                if (secondMonthData.Any(x => predicateFunc(x, item)))
                {
                    res.Month2Value = secondMonthData.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.Month2Name = monthList[1].MonthName;
                    res.CreditControlArea = string.IsNullOrEmpty(res.CreditControlArea) ?
                        secondMonthData.Where(x => predicateFunc(x, item)).Select(x => x.CreditControlArea).FirstOrDefault() : string.Empty;
                }

                if (thirdMonthData.Any(x => predicateFunc(x, item)))
                {
                    res.Month3Value = thirdMonthData.Where(x => predicateFunc(x, item)).Sum(calcFunc);
                    res.Month3Name = monthList[2].MonthName;
                    res.CreditControlArea = string.IsNullOrEmpty(res.CreditControlArea) ?
                        thirdMonthData.Where(x => predicateFunc(x, item)).Select(x => x.CreditControlArea).FirstOrDefault() : string.Empty;
                }

                if (dbResult.Any(x => x.CustomerNo.ToString() == item))
                {
                    var dbItem = dbResult.First(x => x.CustomerNo.ToString() == item);
                    res.Territory = dbItem.Territory;
                    res.Zone = dbItem.CustZone;
                    res.DealerName = dbItem.CustomerName;
                    res.DealerId = dbItem.CustomerNo.ToString();
                    //res.CreditControlArea = dbItem.CreditControlArea;
                }

                res.Change1 = (res.Month2Value - res.Month1Value);
                res.Change2 = (res.Month3Value - res.Month2Value);

                result.Add(res);
            }

            if (monthList.Any())
            {
                result.ForEach(x =>
                {
                    x.Month1Name = monthList[0].MonthName;
                    x.Month2Name = monthList[1].MonthName;
                    x.Month3Name = monthList[2].MonthName;
                });
            }

            var returnResult = new QueryResultModel<OsOver90daysTrendReportResultModel>
            {
                Items = result.OrderBy(x => x.DealerName).Skip(SkipCount(query)).Take(query.PageSize).ToList(),
                Total = result.Count,
                TotalFilter = result.Count
            };

            #region Credit control area name
            var creditArea = returnResult.Items.Select(x => x.CreditControlArea).Distinct().ToList();
            var creditControlArea = await _context.CreditControlAreas.Where(x => creditArea.Contains(x.CreditControlAreaId.ToString())).ToListAsync();

            foreach (var item in returnResult.Items)
            {
                item.CreditControlArea = creditControlArea.FirstOrDefault(x => x.CreditControlAreaId.ToString() == item.CreditControlArea)?.Description ?? string.Empty;
            }
            #endregion

            return returnResult;
        }

        public async Task<QueryResultModel<TintingMachineReportResultModel>> GetTintingMachineReportAsync(TintingMachineReportSearchModel query)
        {
            //var queryResult = new QueryResultModel<TintingMachineReportResultModel>();

            var reportResult = new List<TintingMachineReportResultModel>();

            reportResult = await (_tintingMachine
                .GetAllInclude(x => x.Company).Where(p =>
                        (string.IsNullOrEmpty(query.Depot) || query.Depot == p.Depot)
                        && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                        && (!query.UserId.HasValue || query.UserId.Value == p.UserInfoId))
                    )
                    .Select(c => new TintingMachineReportResultModel()
                    {
                        ActiveMachineNO = c.NoOfActiveMachine,
                        Company = c.Company.DropdownName,
                        Contribution = c.Contribution,
                        InactiveMachineNO = c.NoOfInactiveMachine,
                        Territory = c.Territory,
                        TotalCBMachineNO = c.No

                    }).Skip(this.SkipCount(query)).Take(query.PageSize).ToListAsync();

            var queryResult = new QueryResultModel<TintingMachineReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = reportResult.Count();
            queryResult.Total = reportResult.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<ActiveSummaryReportResultModel>> GetActiveSummeryReportAsync(ActiveSummeryReportSearchModel query)
        {
            UserInfo userinfo = new UserInfo();

            string territory = string.Join(",", query.Territories);//    query.Territories.Count > 0 ? query.Territories[0] : string.Empty;
            string zone = string.Join(",", query.Zones);//    query.Territories.Count > 0 ? query.Territories[0] : string.Empty;
            //string zone = query.Zones.Count > 0 ? query.Zones[0] : string.Empty;
            query.ActivitySummary = string.IsNullOrWhiteSpace(query.ActivitySummary) ? "" : query.ActivitySummary;
            //IList<int> userDealerIds = await _service.GetDealerByUserId(AppIdentity.AppUser.UserId);
            var userDealerIds = new List<string>();

            if (query.UserId.HasValue)
                userDealerIds = (await _service.GetDealerByUserId(query.UserId.Value)).ToList();

            var dbResult = await _dealerInfoRepository.FindByCondition(x =>
                (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                && (!query.SalesGroups.Any() || query.SalesGroups.Contains(x.SalesGroup))
                && (!query.Zones.Any() || query.Zones.Contains(x.CustZone))
                && (string.IsNullOrWhiteSpace(query.Depot) || query.Depot == x.BusinessArea)
                && (!query.UserId.HasValue || userDealerIds.Contains(x.CustomerNo))
            ).Select(x => new
            {
                //x.Territory,
                x.CustomerNo,
                //x.CustZone,
                //x.CreditControlArea,
                x.CustomerName,
            }).ToListAsync();

            var dealerIds = dbResult.Select(x => x.CustomerNo).Distinct().ToList();

            if (query.UserId.HasValue)
            {
                userinfo = _context.UserInfos.FirstOrDefault(p => p.Id == query.UserId);

            }



            var data = _context.JourneyPlanMasters.Join(_context.JourneyPlanDetails, jpm => jpm.Id, jpd => jpd.PlanId, (JourneyPlanMaster, JourneyPlanDetail) => new { JourneyPlanMaster, JourneyPlanDetail })
                        .Join(_context.DealerSalesCalls.Include(p => p.Dealer), jpm => jpm.JourneyPlanMaster.Id, dsc => dsc.JourneyPlanId, (JourneyPlanMaster, DealerSalesCall) => new { JourneyPlanMaster, DealerSalesCall })
                        .Where(p =>
                            p.JourneyPlanMaster.JourneyPlanMaster.PlanStatus == PlanStatus.Approved &&
                           (!query.UserId.HasValue ? true : userinfo.EmployeeId == p.JourneyPlanMaster.JourneyPlanMaster.EmployeeId)

                           && (!query.FromDate.HasValue ? true : p.JourneyPlanMaster.JourneyPlanMaster.CreatedTime >= query.FromDate)
                           && (!query.ToDate.HasValue ? true : p.JourneyPlanMaster.JourneyPlanMaster.CreatedTime <= query.ToDate)

                           && (query.Zones.Count == 0 ? true : query.Zones.Contains(p.DealerSalesCall.Dealer.CustZone))
                           && (query.Territories.Count == 0 ? true : query.Territories.Contains(p.DealerSalesCall.Dealer.Territory))
                )
                        .Select(p => new
                        {
                            JourneyPlanMaster = p.JourneyPlanMaster.JourneyPlanMaster,
                            JourneyPlanDetail = p.JourneyPlanMaster.JourneyPlanMaster.JourneyPlanDetail,
                            DealerSalesCall = p.DealerSalesCall
                        }).ToList();
            var dealerSalesCall = _context.DealerSalesCalls.Where(p => p.JourneyPlanId == null

               && (!query.FromDate.HasValue ? true : p.CreatedTime.Date >= query.FromDate.Value.Date)
                              && (!query.ToDate.HasValue ? true : p.CreatedTime.Date <= query.ToDate.Value.Date)
            ).ToList();

            var painter = _context.Painters.Join(_context.PainterCalls, p => p.Id, pc => pc.PainterId, (Painter, PainterCall) => new { Painter, PainterCall })
                                .Where(p =>
                              (!query.FromDate.HasValue ? true : p.PainterCall.CreatedTime.Date >= query.FromDate.Value.Date)
                            && (!query.ToDate.HasValue ? true : p.PainterCall.CreatedTime.Date <= query.ToDate.Value.Date)
                            && (!query.FromDate.HasValue ? true : p.Painter.CreatedTime.Date >= query.FromDate.Value.Date)
                            && (!query.ToDate.HasValue ? true : p.Painter.CreatedTime.Date <= query.ToDate.Value.Date)

                            && (query.Zones.Count == 0 ? true : query.Zones.Contains(p.Painter.Zone))
                           && (query.Territories.Count == 0 ? true : query.Territories.Contains(p.Painter.Territory))
                                )
                                .Select(p => new
                                {
                                    PainterRegistration = p.Painter,
                                    PainterCalls = p.PainterCall
                                }).ToList();


            var lead = _context.LeadGenerations.Join(_context.LeadFollowUps, lg => lg.Id, lf => lf.LeadGenerationId, (LeadGeneration, LeadFollowUp) => new { LeadGeneration, LeadFollowUp })
                .Where(p =>
               (!query.FromDate.HasValue ? true : p.LeadGeneration.CreatedTime.Date >= query.FromDate.Value.Date)
                           && (!query.ToDate.HasValue ? true : p.LeadGeneration.CreatedTime.Date <= query.ToDate.Value.Date)
                           && (!query.FromDate.HasValue ? true : p.LeadFollowUp.CreatedTime.Date >= query.FromDate.Value.Date)
                           && (!query.ToDate.HasValue ? true : p.LeadFollowUp.CreatedTime.Date <= query.ToDate.Value.Date)

                           && (query.Zones.Count == 0 ? true : query.Zones.Contains(p.LeadGeneration.Zone))
                           && (query.Territories.Count == 0 ? true : query.Territories.Contains(p.LeadGeneration.Territory))
                )

                .Select(p => new
                {
                    LeadGeneration = p.LeadGeneration,
                    LeadFollowUp = p.LeadFollowUp
                }).ToList();


            var reportResult = new List<ActiveSummaryReportResultModel>()
            {
                new ActiveSummaryReportResultModel
                {
                    Activity="JOURNEY PLAN",
                    Target=data.Select(p=>p.JourneyPlanDetail.Where(q=>q.PlanId==p.JourneyPlanMaster.Id).Select(p=>p.DealerId)).Count().ToString(),
                    Actual = data.Select(p=>p.DealerSalesCall.Id).Distinct().Count().ToString(),
                    Variance=(data.Select(p=>p.JourneyPlanDetail.Where(q=>q.PlanId==p.JourneyPlanMaster.Id).Select(p=>p.DealerId)).Count()-data.Select(p=>p.DealerSalesCall.Id).Distinct().Count()).ToString(),
                    BusinessGeneration="N/A",
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },
                new ActiveSummaryReportResultModel
                {

                    Activity="SALES CALL- SUB DEALER",
                    Target="0",
                    Actual = data.Where(p=>p.DealerSalesCall.IsSubDealerCall).Select(x => x.DealerSalesCall.Id).Count().ToString(),
                    Variance="0",
                    BusinessGeneration="N/A",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },

                new ActiveSummaryReportResultModel
                {

                    Activity="SALES CALL- DIRECT DEALER",
                    Target=data.Select(x => x.DealerSalesCall.DealerId).Distinct().Count(x=>x>0).ToString(),
                    Actual = data.Where(x=>!x.DealerSalesCall.IsSubDealerCall).Select(x => x.DealerSalesCall.DealerId).Distinct().Count().ToString(),
                    Variance=(data.Select(x => x.DealerSalesCall.DealerId).Distinct().Count(x=>x>0)-data.Where(x=>!x.DealerSalesCall.IsSubDealerCall).Select(x => x.DealerSalesCall.DealerId).Distinct().Count()).ToString(),
                    BusinessGeneration="N/A",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },
                new ActiveSummaryReportResultModel
                {
                    Activity="PAINTER CALL",
                    Target="N/A",
                    Actual = painter.Select(x => x.PainterCalls.Id).Distinct().Count(x=>x>0).ToString(),
                    Variance="N/A",
                    BusinessGeneration="N/A",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },
                new ActiveSummaryReportResultModel
                {
                    Activity="PAINTER REGISTRATION",
                    Target="N/A",
                    Actual = painter.Select(x => x.PainterRegistration.Id).Distinct().Count(x=>x>0).ToString(),
                    Variance="0",
                    BusinessGeneration="N/A",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },
                new ActiveSummaryReportResultModel
                {

                    Activity="AD HOC VISIT IN DEALERS POINT",
                    Target="N/A",
                    Actual = dealerSalesCall.Count(x=>!x.IsSubDealerCall).ToString(),
                    Variance="N/A",
                    BusinessGeneration="N/A",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone


                },
                new ActiveSummaryReportResultModel
                {

                    Activity="AD HOC VISIT IN SUB-DEALERS POINT",
                    Target="N/A",
                    Actual = dealerSalesCall.Count(x=>x.IsSubDealerCall).ToString(),
                    Variance="N/A",
                    BusinessGeneration="N/A",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone


                },
                new ActiveSummaryReportResultModel
                {

                    Activity="LEAD GENERATION",
                    Target="N/A",
                    Actual =lead.Select(p=>p.LeadGeneration.Id).Distinct().Count(x=>x>0).ToString(),
                    Variance="N/A",
                    BusinessGeneration="0",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },
                new ActiveSummaryReportResultModel
                {
                    Activity="LEAD FOLLOWUP",
                    Target="N/A",
                    Actual =lead.Select(p=>p.LeadFollowUp.Id).Distinct().Count(x=>x>0).ToString(),
                    Variance="N/A",
                    BusinessGeneration="0",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },
                new ActiveSummaryReportResultModel
                {
                    //TODO: need to update collection value
                    Activity="TOTAL COLLECTION VALUE",
                    Target="N/A",
                    Actual =  (string.IsNullOrWhiteSpace(query.ActivitySummary) || query.ActivitySummary.ToLower()=="TOTAL COLLECTION VALUE".ToLower())?
                        (await _collectionDataService.GetTotalCollectionValue(dealerIds, query.FromDate, query.ToDate)).ToString():"0",
                    //Actual ="0",
                    Variance="N/A",
                    BusinessGeneration="0",
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                }
            };


            reportResult = reportResult
                .Where(x => x.Activity.ToLower().Contains(query.ActivitySummary.ToLower()))
         .Skip(this.SkipCount(query)).Take(query.PageSize).ToList();



            var queryResult = new QueryResultModel<ActiveSummaryReportResultModel>
            {
                Items = reportResult,
                TotalFilter = reportResult.Count(),
                Total = reportResult.Count()
            };


            return queryResult;
        }

        private void activesummery()
        {
            //var journeyPlanMasters = _journeyPlanMasterRepository.GetAllInclude(p => p.JourneyPlanDetail).ToList();

            //var user = _context.UserInfos.Where(p => journeyPlanMasters.Select(p => p.EmployeeId).ToArray().Contains(p.EmployeeId)).ToList();

            //var dealerSalesCall = _dealerSalesCallRepository.GetAllInclude(p => p.JourneyPlan).ToList();


            //var lead = _leadGenerationRepository.GetAllInclude(p => p.LeadFollowUps).ToList();





            //var data = _context.JourneyPlanMasters.Join(_context.JourneyPlanDetails, jpm => jpm.Id, jpd => jpd.PlanId, (JourneyPlanMaster, JourneyPlanDetail) => new { JourneyPlanMaster, JourneyPlanDetail })
            //            .Join(_context.DealerSalesCalls, jpm => jpm.JourneyPlanMaster.Id, dsc => dsc.JourneyPlanId, (JourneyPlanMaster, DealerSalesCall) => new { JourneyPlanMaster, DealerSalesCall })

            //            .Select(p=> new {
            //                JourneyPlanMaster=p.JourneyPlanMaster.JourneyPlanMaster,
            //                JourneyPlanDetail=p.JourneyPlanMaster.JourneyPlanMaster.JourneyPlanDetail,
            //                DealerSalesCall=p.DealerSalesCall
            //            }).ToList();


            //var painter = _context.JourneyPlanMasters.Join(_context.Painters, jpm => jpm.EmployeeId, p => p.EmployeeId, (JourneyPlanMaster, Painter) => new { JourneyPlanMaster, Painter })

            //                    .Join(_context.PainterCalls, p => p.Painter.Id, pc => pc.PainterId, (JourneyPlanMaster, PainterCall) => new { JourneyPlanMaster, PainterCall })
            //                    .Select(p => new {
            //                        PainterRegistration=p.JourneyPlanMaster.Painter,
            //                        PainterCalls=p.PainterCall
            //                    }).ToList();


            //var lead = _context.LeadGenerations.Join(_context.LeadFollowUps, lg => lg.Id, lf => lf.LeadGenerationId, (LeadGeneration, LeadFollowUp) => new { LeadGeneration, LeadFollowUp })
            //    .Select(p => new {
            //        LeadGeneration = p.LeadGeneration,
            //        LeadFollowUp = p.LeadFollowUp
            //    }).ToList();


            //var reportResult = new List<ActiveSummaryReportResultModel>()
            //{
            //    new ActiveSummaryReportResultModel
            //    {
            //        Activity="Journey Plan",
            //        Target=data.Select(p=>p.DealerSalesCall.DealerId).Count(x=>x>0).ToString(),
            //        Actual = data.Select(p=>p.DealerSalesCall.Id).Distinct().Count().ToString(),
            //        Variance=(data.Select(p=>p.DealerSalesCall.DealerId).Count(x=>x>0)-data.Select(p=>p.DealerSalesCall.Id).Distinct().Count(x=>x>0)).ToString(),
            //        BusinessGeneration="N/A",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()

            //    },
            //    new ActiveSummaryReportResultModel
            //    {

            //        Activity="SALES CALL- SUB DEALER",
            //        Target="0",
            //        Actual = data.Select(x => x.DealerSalesCall).Count(x=>x.IsSubDealerCall).ToString(),
            //        Variance="0",
            //        BusinessGeneration="N/A",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()

            //    },

            //    new ActiveSummaryReportResultModel
            //    {

            //        Activity="SALES CALL- DIRECT DEALER",
            //        Target=data.Select(x => x.DealerSalesCall.DealerId).Distinct().Count(x=>x>0).ToString(),
            //        Actual = data.Select(x => x.DealerSalesCall).Count(x => !x.IsSubDealerCall).ToString(),
            //        Variance=(data.Select(x => x.DealerSalesCall.DealerId).Distinct().Count(x=>x>0)-data.Select(x => x.DealerSalesCall).Count(x => !x.IsSubDealerCall)).ToString(),
            //        BusinessGeneration="N/A",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()

            //    },
            //    new ActiveSummaryReportResultModel
            //    {
            //        Activity="PAINTER CALL",
            //        Target="N/A",
            //        Actual = painter.Select(x => x.PainterCalls.Id).Distinct().Count(x=>x>0).ToString(),
            //        Variance="N/A",
            //        BusinessGeneration="N/A",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()

            //    },
            //    new ActiveSummaryReportResultModel
            //    {
            //        Activity="PAINTER REGISTRATION",
            //        Target="N/A",
            //        Actual = painter.Select(x => x.PainterRegistration.Id).Distinct().Count(x=>x>0).ToString(),
            //        Variance="0",
            //        BusinessGeneration="N/A",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()

            //    },
            //    new ActiveSummaryReportResultModel
            //    {

            //        Activity="DEALER ADHOC VISIT",
            //        Target="N/A",
            //        Actual = data.Where(p=>p.DealerSalesCall.JourneyPlanId!=0).Select(x => x.DealerSalesCall).Distinct().Count(x=>x>0).ToString(),
            //        Variance="N/A",
            //        BusinessGeneration="N/A",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()


            //    },
            //    new ActiveSummaryReportResultModel
            //    {

            //        Activity="LEAD GENERATION",
            //        Target="N/A",
            //        Actual =lead.Select(p=>p.LeadGeneration.Id).Distinct().Count(x=>x>0).ToString(),
            //        Variance="N/A",
            //        BusinessGeneration="0",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()

            //    },
            //    new ActiveSummaryReportResultModel
            //    {
            //        Activity="LEAD FOLLOWUP",
            //        Target="N/A",
            //        Actual =lead.Select(p=>p.LeadFollowUp.Id).Distinct().Count(x=>x>0).ToString(),
            //        Variance="N/A",
            //        BusinessGeneration="0",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()


            //    },
            //    new ActiveSummaryReportResultModel
            //    {

            //        Activity="TOTAL COLLECTION VALUE",
            //        Target="N/A",
            //        //Actual =(await _collectionDataService.GetTotalCollectionValue(dealerIds)).ToString(),
            //        Actual ="0",
            //        Variance="N/A",
            //        BusinessGeneration="0",
            //        //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()

            //    }
            //}

            //.Skip(this.SkipCount(query)).Take(query.PageSize).ToList();
            //var queryResult = new QueryResultModel<ActiveSummaryReportResultModel>();
            //queryResult.Items = reportResult;
            //queryResult.TotalFilter = reportResult.Count();
            //queryResult.Total = reportResult.Count();


            //return queryResult;

        }

        public async Task<QueryResultModel<LogInReportResultModel>> GetLogInReportAsync(LogInReportSearchModel query)
        {
            var reportResult = new List<LogInReportResultModel>();

            var loginInfo = await (from ll in _context.LoginLogs
                                   join u in _context.UserInfos on ll.UserId equals u.Id into uleft
                                   from uInfo in uleft.DefaultIfEmpty()
                                   where (
                                      (!query.UserId.HasValue || uInfo.Id == query.UserId.Value)
                                      && (!query.FromDate.HasValue || ll.LoggedInTime.Date >= query.FromDate.Value.Date)
                                      && (!query.ToDate.HasValue || ll.LoggedInTime <= query.ToDate.Value.Date)
                                   )
                                   select new
                                   {
                                       uInfo.Email,
                                       ll.LoggedInTime,
                                       ll.LoggedOutTime,
                                       ll.IsLoggedIn
                                   }).ToListAsync();

            var loginInfoGroup = loginInfo.GroupBy(x => new { 
                x.Email,
                LoggedInTime = x.LoggedInTime.Date
            }).Select(x => new
            {
                userId = x.Key?.Email ?? string.Empty,
                startDate = CustomConvertExtension.ObjectToDateString(x.Min(x => x.LoggedInTime)),
                startTime = CustomConvertExtension.ObjectToTimeString(x.Min(x => x.LoggedInTime)),
                closedDate = CustomConvertExtension.ObjectToDateString(x.Max(x => x.LoggedOutTime)),
                closedTime = CustomConvertExtension.ObjectToTimeString(x.Max(x => x.LoggedOutTime)),
                totalUseTime = Decimal.Round(CustomConvertExtension.ObjectToDecimal((CustomConvertExtension.ObjectToCurrentDateTime(x.Max(x => x.LoggedOutTime)) - CustomConvertExtension.ObjectToCurrentDateTime(x.Min(x => x.LoggedInTime))).TotalHours), 2)
            }).OrderByDescending(x => x.startDate).ThenByDescending(x => x.startTime).ToList();

            reportResult = loginInfoGroup.Select(x => new LogInReportResultModel 
            {
                UserId = x.userId,
                StartDate = x.startDate,
                StartTime = x.startTime,
                ClosedDate = x.closedDate,
                ClosedTime = x.closedTime,
                TotalUseTime = x.totalUseTime
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<LogInReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = loginInfoGroup.Count();
            queryResult.Total = loginInfoGroup.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<MerchendizingSnapShotReportResultModel>> GetSnapShotReportAsync(MerchendizingSnapShotReportSearchModel query)
        {
            var reportResult = new List<MerchendizingSnapShotReportResultModel>();

            var merchendizingSnapShot = await (from ms in _context.MerchandisingSnapShots
                                               join di in _context.DealerInfos on ms.DealerId equals di.Id into dileft
                                               from diInfo in dileft.DefaultIfEmpty()
                                               join de in _context.Depots on diInfo.BusinessArea equals de.Werks into deleft
                                               from deInfo in deleft.DefaultIfEmpty()
                                               join t in _context.Territory on diInfo.Territory equals t.Code into tleft
                                               from tInfo in tleft.DefaultIfEmpty()
                                               join z in _context.Zone on diInfo.CustZone equals z.Code into zleft
                                               from zInfo in zleft.DefaultIfEmpty()
                                               join u in _context.UserInfos on ms.UserId equals u.Id into uleft
                                               from uInfo in uleft.DefaultIfEmpty()
                                               join ddcat in _context.DropdownDetails on ms.MerchandisingSnapShotCategoryId equals ddcat.Id into ddcatleft
                                               from ddcatInfo in ddcatleft.DefaultIfEmpty()
                                               where (
                                                   (!query.UserId.HasValue || uInfo.Id == query.UserId.Value)
                                                   && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                                   && (!query.FromDate.HasValue || ms.CreatedTime.Date >= query.FromDate.Value.Date)
                                                   && (!query.ToDate.HasValue || ms.CreatedTime.Date <= query.ToDate.Value.Date)
                                                   && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                                   && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                                   && (!query.DealerId.HasValue || ms.DealerId == query.DealerId.Value)
                                               )
                                               select new
                                               {
                                                   ms.CreatedTime,
                                                   ms.DealerId,
                                                   ms.ImageUrl,
                                                   ms.OthersSnapShotCategoryName,
                                                   diInfo.CustomerNo,
                                                   diInfo.CustomerName,
                                                   depot = deInfo.Name1,
                                                   territory = tInfo.Name,
                                                   zone = zInfo.Name,
                                                   uInfo.Email,
                                                   categoryName = ddcatInfo.DropdownName,
                                                   ms.Remarks
                                               }).ToListAsync();

            var groupmerchendizingSnapShot = merchendizingSnapShot.GroupBy(x => new { x.Email, x.DealerId, x.CustomerName, x.territory, x.zone })
                .Select(x => new
                {
                    email = x.Key.Email,
                    dealerId = x.FirstOrDefault()?.CustomerNo,
                    dealerName = x.Key.CustomerName,
                    territoryName = x.Key.territory,
                    zoneName = x.Key.zone,
                    snapShotDate = x.FirstOrDefault()?.CreatedTime,
                    competitionDisplay = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.CompetitionDisplay)?.ImageUrl,
                    cDRemarks = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.CompetitionDisplay)?.Remarks,
                    glowSignBoard = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.GlowSignBoard)?.ImageUrl,
                    gSRemarks = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.GlowSignBoard)?.Remarks,
                    productDisplay = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.ProductDisplay)?.ImageUrl,
                    pDRemarks = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.ProductDisplay)?.Remarks,
                    scheme = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.Scheme)?.ImageUrl,
                    sRemarks = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.Scheme)?.Remarks,
                    brochure = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.Brochure)?.ImageUrl,
                    bRemarks = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.Brochure)?.Remarks,
                    others = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.Others)?.ImageUrl,
                    oRemarks = x.FirstOrDefault(y => y.categoryName == ConstantSnapShotValue.Others)?.Remarks,
                    otherSnapshotTypeName = x.FirstOrDefault()?.OthersSnapShotCategoryName
                });

            reportResult = groupmerchendizingSnapShot.Select(x => new MerchendizingSnapShotReportResultModel
            {
                UserId = x.email,
                DealerId = x.dealerId.ToString(),
                DealerName = x.dealerName,
                Territory = x.territoryName,
                Zone = x.zoneName,
                SnapShotDate = CustomConvertExtension.ObjectToDateString(x.snapShotDate),
                CompetitionDisplay = x.competitionDisplay,
                CRemarks = x.cDRemarks,
                GlowSignBoard = x.glowSignBoard,
                GRemarks = x.gSRemarks,
                ProductDisplay = x.productDisplay,
                PRemarks = x.pDRemarks,
                Scheme = x.scheme,
                SRemarks = x.sRemarks,
                Brochure = x.brochure,
                BRemarks = x.bRemarks,
                Others = x.others,
                ORemarks = x.oRemarks,
                OtherSnapshotTypeName = x.otherSnapshotTypeName,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<MerchendizingSnapShotReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = groupmerchendizingSnapShot.Count();
            queryResult.Total = groupmerchendizingSnapShot.Count();

            return queryResult;
        }

        public dynamic GetSnapShotReportBySp(MerchendizingSnapShotReportSearchModel query) 
        {
            var territory = new StringBuilder();
            foreach (var item in query.Territories)
            {
                territory.Append(',');
                territory.Append(item);
            }

            var zone = new StringBuilder();
            foreach (var item in query.Zones)
            {
                zone.Append(',');
                zone.Append(item);
            }

            var parameters = new Dictionary<string, object>
            {
                { "@UserId", query.UserId },
                { "@Depot", query.Depot },
                { "@FromDate", query.FromDate.ToString() },
                { "@ToDate", query.ToDate.ToString() },
                { "@DealerId", query.DealerId },
                { "@Territories", territory.ToString().TrimStart(',') },
                { "@Zones", zone.ToString().TrimStart(',') }
            };

            return _leadGenerationRepository.DynamicListFromSql("GetDynamicSnapshotReport", parameters, true);
        }

        public dynamic GetPainterCallReportBySp(PainterCallReportSearchModel query)
        {
            var territory = new StringBuilder();
            foreach (var item in query.Territories)
            {
                territory.Append(',');
                territory.Append(item);
            }

            var zone = new StringBuilder();
            foreach (var item in query.Zones)
            {
                zone.Append(',');
                zone.Append(item);
            }

            var parameters = new Dictionary<string, object>
            {
                { "@UserId", query.UserId },
                { "@Depot", query.Depot },
                { "@FromDate", query.FromDate.ToString() },
                { "@ToDate", query.ToDate.ToString() },
                { "@Territories", territory.ToString().TrimStart(',') },
                { "@Zones", zone.ToString().TrimStart(',') },
                { "@PainterId", query.PainterId },
                { "@PainterTypeId", query.PainterType },
                { "@PageNumber", query.Page },
                { "@RowspPage", query.PageSize }
            };

            return _leadGenerationRepository.DynamicListFromSql("GetDynamicpPainterCallReport", parameters, true);
        }

        public dynamic GetDealerSalesCallReportBySp(DealerSalesCallReportSearchModel query)
        {
            var territory = new StringBuilder();
            foreach (var item in query.Territories)
            {
                territory.Append(',');
                territory.Append(item);
            }

            var zone = new StringBuilder();
            foreach (var item in query.Zones)
            {
                zone.Append(',');
                zone.Append(item);
            }

            var parameters = new Dictionary<string, object>
            {
                { "@IsSubDealarCall", 0 },
                { "@UserId", query.UserId },
                { "@Depot", query.Depot },
                { "@FromDate", query.FromDate.ToString() },
                { "@ToDate", query.ToDate.ToString() },
                { "@DealerId", query.DealerId },
                { "@Territories", territory.ToString().TrimStart(',') },
                { "@Zones", zone.ToString().TrimStart(',') },
                { "@PageNumber", query.Page },
                { "@RowspPage", query.PageSize }
            };

            return _leadGenerationRepository.DynamicListFromSql("GetDynamicDealerSalesCallReport", parameters, true);
        }

        public dynamic GetSubDealerSalesCallReportBySp(SubDealerSalesCallReportSearchModel query)
        {
            var territory = new StringBuilder();
            foreach (var item in query.Territories)
            {
                territory.Append(',');
                territory.Append(item);
            }

            var zone = new StringBuilder();
            foreach (var item in query.Zones)
            {
                zone.Append(',');
                zone.Append(item);
            }

            var parameters = new Dictionary<string, object>
            {
                { "@IsSubDealarCall", 1 },
                { "@UserId", query.UserId },
                { "@Depot", query.Depot },
                { "@FromDate", query.FromDate.ToString() },
                { "@ToDate", query.ToDate.ToString() },
                { "@DealerId", query.SubDealerId },
                { "@Territories", territory.ToString().TrimStart(',') },
                { "@Zones", zone.ToString().TrimStart(',') },
                { "@PageNumber", query.Page },
                { "@RowspPage", query.PageSize }
            };

            return _leadGenerationRepository.DynamicListFromSql("GetDynamicDealerSalesCallReport", parameters, true);
        }

        public dynamic GetAddhocDealerSalesCallReportBySp(DealerSalesCallReportSearchModel query)
        {
            var territory = new StringBuilder();
            foreach (var item in query.Territories)
            {
                territory.Append(',');
                territory.Append(item);
            }

            var zone = new StringBuilder();
            foreach (var item in query.Zones)
            {
                zone.Append(',');
                zone.Append(item);
            }

            var parameters = new Dictionary<string, object>
            {
                { "@IsSubDealarCall", 0 },
                { "@UserId", query.UserId },
                { "@Depot", query.Depot },
                { "@FromDate", query.FromDate.ToString() },
                { "@ToDate", query.ToDate.ToString() },
                { "@DealerId", query.DealerId },
                { "@Territories", territory.ToString().TrimStart(',') },
                { "@Zones", zone.ToString().TrimStart(',') },
                { "@PageNumber", query.Page },
                { "@RowspPage", query.PageSize }
            };

            return _leadGenerationRepository.DynamicListFromSql("GetDynamicAdhocDealerSalesCallReport", parameters, true);
        }

        public dynamic GetAddhocSubDealerSalesCallReportBySp(SubDealerSalesCallReportSearchModel query)
        {
            var territory = new StringBuilder();
            foreach (var item in query.Territories)
            {
                territory.Append(',');
                territory.Append(item);
            }

            var zone = new StringBuilder();
            foreach (var item in query.Zones)
            {
                zone.Append(',');
                zone.Append(item);
            }

            var parameters = new Dictionary<string, object>
            {
                { "@IsSubDealarCall", 1 },
                { "@UserId", query.UserId },
                { "@Depot", query.Depot },
                { "@FromDate", query.FromDate.ToString() },
                { "@ToDate", query.ToDate.ToString() },
                { "@DealerId", query.SubDealerId },
                { "@Territories", territory.ToString().TrimStart(',') },
                { "@Zones", zone.ToString().TrimStart(',') },
                { "@PageNumber", query.Page },
                { "@RowspPage", query.PageSize }
            };

            return _leadGenerationRepository.DynamicListFromSql("GetDynamicDealerSalesCallReport", parameters, true);
        }


    }
}
