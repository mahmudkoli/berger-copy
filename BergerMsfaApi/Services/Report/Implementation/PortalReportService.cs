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
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

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
        private readonly IRepository<DSC.DealerCompetitionSales> _dealerCompetitionSaleRepository;
        private readonly IRepository<DSC.DealerSalesIssue> _dealerSaleIssueRepository;
        private readonly IDropdownService _dropdownService;
        private readonly IMapper _mapper;
        
        private readonly ApplicationDbContext _context;

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
                IRepository<DSC.DealerCompetitionSales> dealerCompetitionSaleRepository,
                IRepository<DSC.DealerSalesIssue> dealerSaleIssueRepository,
                IDropdownService dropdownService,
                IMapper mapper,

                ApplicationDbContext context
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
            this._mapper = mapper;

            this._context = context;
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
                                && (string.IsNullOrWhiteSpace(query.DepotId) || x.Depot == query.DepotId)
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

            var leads = await _leadGenerationRepository.GetAllIncludeAsync(x => x,
                            x => (!query.UserId.HasValue || x.UserId == query.UserId.Value)
                                //&& (!query.EmployeeRole.HasValue || x.User.EmployeeRole == query.EmployeeRole.Value)
                                && (string.IsNullOrWhiteSpace(query.DepotId) || x.Depot == query.DepotId)
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
            queryResult.TotalFilter = leads.TotalFilter;
            queryResult.Total = leads.Total;

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
                                && (string.IsNullOrWhiteSpace(query.DepotId) || x.LeadGeneration.Depot == query.DepotId)
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

            var painters = (from p in await _painterRepository.GetAllAsync()
                            join u in await _userInfoRepository.GetAllAsync() on p.EmployeeId equals u.EmployeeId into uleftjoin
                            from userInfo in uleftjoin.DefaultIfEmpty()
                            join d in await _dorpDownDetailsRepository.GetAllAsync() on p.PainterCatId equals d.Id into dleftjoin
                            from dropDownInfo in dleftjoin.DefaultIfEmpty()
                            join adp in await _attachmentDealerRepository.GetAllAsync() on p.AttachedDealerCd equals adp.Id.ToString() into adpleftjoin
                            from adpInfo in adpleftjoin.DefaultIfEmpty()
                            join di in await _dealerInfoRepository.GetAllAsync() on adpInfo?.Dealer equals di.Id into dileftjoin
                            from diInfo in dileftjoin.DefaultIfEmpty()
                            join dep in await _depotSvc.GetAllAsync() on p.Depot equals dep.Werks into depleftjoin
                            from depinfo in depleftjoin.DefaultIfEmpty()
                            join sg in await _saleGroupSvc.GetAllAsync() on p.SaleGroup equals sg.Code into sgleftjoin
                            from sginfo in sgleftjoin.DefaultIfEmpty()
                            join t in await _territorySvc.GetAllAsync() on p.Territory equals t.Code into tleftjoin
                            from tinfo in tleftjoin.DefaultIfEmpty()
                            join z in await _zoneSvc.GetAllAsync() on p.Zone equals z.Code into zleftjoin
                            from zinfo in zleftjoin.DefaultIfEmpty()
                            where (
                               (!query.UserId.HasValue || userInfo?.Id == query.UserId.Value)
                               && (string.IsNullOrWhiteSpace(query.DepotId) || p.Depot == query.DepotId)
                               && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                               && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                               && (!query.FromDate.HasValue || p.CreatedTime.Date >= query.FromDate.Value.Date)
                               && (!query.ToDate.HasValue || p.CreatedTime.Date <= query.ToDate.Value.Date)
                               && (!query.PainterId.HasValue || p?.Id == query.PainterId.Value)
                               && (!query.PainterType.HasValue || p.PainterCatId == query.PainterType.Value)
                               && (string.IsNullOrWhiteSpace(query.PainterMobileNo) || p.Phone == query.PainterMobileNo)
                            )
                            select new { p, userInfo, dropDownInfo, diInfo, depinfo, sginfo, tinfo, zinfo }).ToList();

            reportResult = painters.Select(x => new PainterRegistrationReportResultModel
            {
                UserId = x.userInfo?.Email ?? string.Empty,
                Territory = x.tinfo.Name,
                Zone = x.zinfo.Name,
                PainterId = x.p.Id.ToString(),
                PainerRegistrationDate = CustomConvertExtension.ObjectToDateString(x.p.CreatedTime),
                TypeOfPainer = x.dropDownInfo?.DropdownName,
                DepotName = x.depinfo?.Name1,
                SalesGroup = x.sginfo.Name,
                PainterName = x.p.PainterName,
                PainterAddress = x.p.Address,
                MobileNumber = x.p.Phone,
                NoOfPaintingAttached = x.p.NoOfPainterAttached,
                DBBLRocketAccountStatus = x.p.HasDbbl ? "Created" : "Not Created",
                AccountNumber = x.p.AccDbblNumber,
                AccountHolderName = x.p.AccDbblHolderName,
                IdentificationNo = !string.IsNullOrEmpty(x.p.NationalIdNo) ? x.p.NationalIdNo
                        : (!string.IsNullOrEmpty(x.p.PassportNo) ? x.p.PassportNo
                        : (!string.IsNullOrEmpty(x.p.BrithCertificateNo)) ? x.p.BrithCertificateNo : string.Empty),
                AttachedTaggedDealerId = x.p.AttachedDealerCd,
                AttachedTaggedDealerName = x.diInfo?.CustomerName,
                APPInstalledStatus = x.p.IsAppInstalled ? "Installed" : "Not Installed",
                APPNotInstalledReason = x.p.Remark,
                AverageMonthlyUse = x.p.AvgMonthlyVal.ToString(),
                BergerLoyalty = x.p.Loyality.ToString(),
                PainterImageUrl = x.p.PainterImageUrl,
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

            var dealers = (from d in await _dealerOpening.GetAllAsync()
                           join u in await _userInfoRepository.GetAllAsync() on d.EmployeeId equals u.EmployeeId into uleftjoin
                           from uinfo in uleftjoin.DefaultIfEmpty()
                           join dep in await _depotSvc.GetAllAsync() on d.BusinessArea equals dep.Werks into depleftjoin
                           from depinfo in depleftjoin.DefaultIfEmpty()
                           join so in await _saleOfficeSvc.GetAllAsync() on d.SaleOffice equals so.Code into soleftjoin
                           from soinfo in soleftjoin.DefaultIfEmpty()
                           join sg in await _saleGroupSvc.GetAllAsync() on d.SaleGroup equals sg.Code into sgleftjoin
                           from sginfo in sgleftjoin.DefaultIfEmpty()
                           join t in await _territorySvc.GetAllAsync() on d.Territory equals t.Code into tleftjoin
                           from tinfo in tleftjoin.DefaultIfEmpty()
                           join z in await _zoneSvc.GetAllAsync() on d.Zone equals z.Code into zleftjoin
                           from zinfo in zleftjoin.DefaultIfEmpty()
                           where (
                             (!query.UserId.HasValue || uinfo?.Id == query.UserId.Value)
                             && (string.IsNullOrWhiteSpace(query.DepotId) || d.BusinessArea == query.DepotId)
                             && (!query.Territories.Any() || query.Territories.Contains(d.Territory))
                             && (!query.Zones.Any() || query.Zones.Contains(d.Zone))
                             && (!query.FromDate.HasValue || d.CreatedTime.Date >= query.FromDate.Value.Date)
                             && (!query.ToDate.HasValue || d.CreatedTime.Date <= query.ToDate.Value.Date)
                           )
                           select new { d, uinfo, depinfo, soinfo, sginfo, tinfo, zinfo }).ToList();

            var dealerAttachments = (from doa in await _dealerOpeningAttachmentSvc.GetAllAsync()
                                     join di in await _dealerInfoRepository.GetAllAsync() on doa.DealerOpeningId equals di.Id into dileftjoin
                                     from diinfo in dileftjoin.DefaultIfEmpty()
                                     select new { doa, diinfo }).ToList();

            var dealerId = "";
            reportResult = dealers.Select(x => new DealerOpeningReportResultModel
            {
                UserId = x.uinfo?.Email ?? string.Empty,
                DealrerOpeningId = dealerId = x.d?.Id.ToString(),
                BusinessArea = x.d?.BusinessArea,
                BusinessAreaName = x.depinfo?.Name1,
                SalesOffice = x.sginfo?.Name,
                SalesGroup = x.sginfo?.Name,
                Territory = x.tinfo?.Name,
                Zone = x.zinfo?.Name,
                EmployeeId = x.d?.EmployeeId,
                DealershipOpeningApplicationForm = dealerAttachments.FirstOrDefault(x => x.doa.Name == "Application Form" && x.doa.DealerOpeningId.ToString() == dealerId)?.doa?.Path ?? string.Empty,
                TradeLicensee = dealerAttachments.FirstOrDefault(x => x.doa.Name == "Trade Licensee" && x.doa.DealerOpeningId.ToString() == dealerId)?.doa?.Path ?? string.Empty,
                IdentificationNo = dealerAttachments.FirstOrDefault(x => x.doa.Name == "NID/Passport/Birth" && x.doa.DealerOpeningId.ToString() == dealerId)?.doa?.Path ?? string.Empty,
                PhotographOfproprietor = dealerAttachments.FirstOrDefault(x => x.doa.Name == "Photograph of proprietor" && x.doa.DealerOpeningId.ToString() == dealerId)?.doa?.Path ?? string.Empty,
                NomineeIdentificationNo = dealerAttachments.FirstOrDefault(x => x.doa.Name == "Nominee NID/PASSPORT/BIRTH" && x.doa.DealerOpeningId.ToString() == dealerId)?.doa?.Path ?? string.Empty,
                NomineePhotograph = dealerAttachments.FirstOrDefault(x => x.doa.Name == "Nominee/Photograph" && x.doa.DealerOpeningId.ToString() == dealerId)?.doa?.Path ?? string.Empty,
                Cheque = dealerAttachments.FirstOrDefault(x => x.doa.Name == "Cheque" && x.doa.DealerOpeningId.ToString() == dealerId)?.doa?.Path ?? string.Empty,
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

            var dealers = (from p in await _paymentRepository.GetAllAsync()
                           join u in await _userInfoRepository.GetAllAsync() on p.EmployeeId equals u.EmployeeId into uleftjoin
                           from uinfo in uleftjoin.DefaultIfEmpty()
                           join dct in await _dorpDownDetailsRepository.GetAllAsync() on p.CustomerTypeId equals dct.Id into dctleftjoin
                           from dctinfo in dctleftjoin.DefaultIfEmpty()
                           join dpm in await _dorpDownDetailsRepository.GetAllAsync() on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                           from dpminfo in dpmleftjoin.DefaultIfEmpty()
                           join ca in await _creditControlAreaRepository.GetAllAsync() on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                           from cainfo in caleftjoin.DefaultIfEmpty()
                           join d in await _dealerInfoRepository.GetAllAsync() on p.Code equals d.Id.ToString() into dleftjoin
                           from dinfo in dleftjoin.DefaultIfEmpty()
                           join t in await _territorySvc.GetAllAsync() on dinfo?.Territory equals t.Code into tleftjoin
                           from tinfo in tleftjoin.DefaultIfEmpty()
                           join z in await _zoneSvc.GetAllAsync() on dinfo?.CustZone equals z.Code into zleftjoin
                           from zinfo in zleftjoin.DefaultIfEmpty()
                           join dep in await _depotSvc.GetAllAsync() on dinfo?.BusinessArea equals dep.Werks into depleftjoin
                           from depinfo in depleftjoin.DefaultIfEmpty()
                           where (
                             dctinfo.DropdownName == ConstantsCustomerTypeValue.CustomerTypeDealer
                             && (!query.UserId.HasValue || uinfo?.Id == query.UserId.Value)
                             && (!query.Territories.Any() || query.Territories.Contains(dinfo.Territory))
                             && (!query.Zones.Any() || query.Zones.Contains(dinfo.CustZone))
                             && (!query.PaymentMethodId.HasValue || p?.PaymentMethodId == query.PaymentMethodId.Value)
                             && (!query.DealerId.HasValue || p?.Code == query.DealerId.Value.ToString())
                             && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                             && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                           )
                           select new { p, uinfo, dctinfo, dpminfo, cainfo, tinfo, zinfo, depinfo }).ToList();

            reportResult = dealers.Select(x => new DealerCollectionReportResultModel
            {
                UserId = x.uinfo?.Email ?? string.Empty,
                DepotId = x.depinfo.Werks,
                DepotName = x.depinfo.Name1,
                Territory = x.tinfo.Name,
                Zone = x.zinfo.Name,
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.p.CollectionDate),
                TypeOfCustomer = x.dctinfo?.DropdownName,
                DealerId = x.p.Code,
                DealerName = x.p.Name,
                PaymentMethod = x.dpminfo?.DropdownName,
                CreditControlArea = x.cainfo?.Description,
                BankName = x.p.BankName,
                ChequeNumber = x.p.Number,
                CashAmount = x.p.Amount,
                ManualMrNumber = x.p.ManualNumber,
                Remarks = x.p.Remarks
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

            var subDealers = (from p in await _paymentRepository.GetAllAsync()
                              join u in await _userInfoRepository.GetAllAsync() on p.EmployeeId equals u.EmployeeId into uleftjoin
                              from uinfo in uleftjoin.DefaultIfEmpty()
                              join dct in await _dorpDownDetailsRepository.GetAllAsync() on p.CustomerTypeId equals dct.Id into dctleftjoin
                              from dctinfo in dctleftjoin.DefaultIfEmpty()
                              join dpm in await _dorpDownDetailsRepository.GetAllAsync() on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                              from dpminfo in dpmleftjoin.DefaultIfEmpty()
                              join ca in await _creditControlAreaRepository.GetAllAsync() on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                              from cainfo in caleftjoin.DefaultIfEmpty()
                              join d in await _dealerInfoRepository.GetAllAsync() on p.Code equals d.Id.ToString() into dleftjoin
                              from dinfo in dleftjoin.DefaultIfEmpty()
                              join t in await _territorySvc.GetAllAsync() on dinfo?.Territory equals t.Code into tleftjoin
                              from tinfo in tleftjoin.DefaultIfEmpty()
                              join z in await _zoneSvc.GetAllAsync() on dinfo?.CustZone equals z.Code into zleftjoin
                              from zinfo in zleftjoin.DefaultIfEmpty()
                              join dep in await _depotSvc.GetAllAsync() on dinfo?.BusinessArea equals dep.Werks into depleftjoin
                              from depinfo in depleftjoin.DefaultIfEmpty()
                              where (
                                dctinfo.DropdownName == ConstantsCustomerTypeValue.CustomerTypeSubDealer
                                && (!query.UserId.HasValue || uinfo?.Id == query.UserId.Value)
                                && (!query.Territories.Any() || query.Territories.Contains(dinfo.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(dinfo.CustZone))
                                && (!query.PaymentMethodId.HasValue || p?.PaymentMethodId == query.PaymentMethodId.Value)
                                && (!query.DealerId.HasValue || p?.Code == query.DealerId.Value.ToString())
                                && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                              )
                              select new { p, uinfo, dctinfo, dpminfo, cainfo, tinfo, zinfo, depinfo }).ToList();

            reportResult = subDealers.Select(x => new SubDealerCollectionReportResultModel
            {
                UserId = x.uinfo?.Email ?? string.Empty,
                DepotId = x.depinfo.Werks,
                DepotName = x.depinfo.Name1,
                Territory = x.tinfo.Name,
                Zone = x.zinfo.Name,
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.p.CollectionDate),
                TypeOfCustomer = x.dctinfo?.DropdownName,
                SubDealerCode = x.p.Code,
                SubDealerName = x.p.Name,
                SubDealerMobileNumber = x.p.MobileNumber,
                SubDealerAddress = x.p.Address,
                PaymentMethod = x.dpminfo?.DropdownName,
                CreditControlArea = x.cainfo?.Description,
                BankName = x.p.BankName,
                ChequeNumber = x.p.Number,
                CashAmount = x.p.Amount,
                ManualMrNumber = x.p.ManualNumber,
                Remarks = x.p.Remarks
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

            var customers = (from p in await _paymentRepository.GetAllAsync()
                             join u in await _userInfoRepository.GetAllAsync() on p.EmployeeId equals u.EmployeeId into uleftjoin
                             from uinfo in uleftjoin.DefaultIfEmpty()
                             join dct in await _dorpDownDetailsRepository.GetAllAsync() on p.CustomerTypeId equals dct.Id into dctleftjoin
                             from dctinfo in dctleftjoin.DefaultIfEmpty()
                             join dpm in await _dorpDownDetailsRepository.GetAllAsync() on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                             from dpminfo in dpmleftjoin.DefaultIfEmpty()
                             join ca in await _creditControlAreaRepository.GetAllAsync() on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                             from cainfo in caleftjoin.DefaultIfEmpty()
                             where (
                               dctinfo.DropdownName == ConstantsCustomerTypeValue.CustomerTypeCustomer
                               && (!query.UserId.HasValue || uinfo?.Id == query.UserId.Value)
                               && (!query.PaymentMethodId.HasValue || p?.PaymentMethodId == query.PaymentMethodId.Value)
                               && (!query.DealerId.HasValue || p?.Code == query.DealerId.Value.ToString())
                               && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                               && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                             )
                             select new { p, uinfo, dctinfo, dpminfo, cainfo }).ToList();

            reportResult = customers.Select(x => new CustomerCollectionReportResultModel
            {
                UserId = x.uinfo?.Email ?? string.Empty,
                DepotId = "",
                DepotName = "",
                Territory = "",
                Zone = "",
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.p.CollectionDate),
                TypeOfCustomer = x.dctinfo?.DropdownName,
                CustomerName = x.p.Name,
                CustomerMobileNumber = x.p.MobileNumber,
                CustomerAddress = x.p.Address,
                PaymentMethod = x.dpminfo?.DropdownName,
                CreditControlArea = x.cainfo?.Description,
                BankName = x.p.BankName,
                ChequeNumber = x.p.Number,
                CashAmount = x.p.Amount,
                ManualMrNumber = x.p.ManualNumber,
                Remarks = x.p.Remarks
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

            var projects = (from p in await _paymentRepository.GetAllAsync()
                            join u in await _userInfoRepository.GetAllAsync() on p.EmployeeId equals u.EmployeeId into uleftjoin
                            from uinfo in uleftjoin.DefaultIfEmpty()
                            join dct in await _dorpDownDetailsRepository.GetAllAsync() on p.CustomerTypeId equals dct.Id into dctleftjoin
                            from dctinfo in dctleftjoin.DefaultIfEmpty()
                            join dpm in await _dorpDownDetailsRepository.GetAllAsync() on p.PaymentMethodId equals dpm.Id into dpmleftjoin
                            from dpminfo in dpmleftjoin.DefaultIfEmpty()
                            join ca in await _creditControlAreaRepository.GetAllAsync() on p.CreditControlAreaId equals ca.CreditControlAreaId into caleftjoin
                            from cainfo in caleftjoin.DefaultIfEmpty()
                            where (
                              dctinfo.DropdownName == ConstantsCustomerTypeValue.CustomerTypeDirectProject
                              && (!query.UserId.HasValue || uinfo?.Id == query.UserId.Value)
                              && (!query.PaymentMethodId.HasValue || p?.PaymentMethodId == query.PaymentMethodId.Value)
                              && (!query.DealerId.HasValue || p?.Code == query.DealerId.Value.ToString())
                              && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                              && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                            )
                            select new { p, uinfo, dctinfo, dpminfo, cainfo }).ToList();

            reportResult = projects.Select(x => new DirectProjectCollectionReportResultModel
            {
                UserId = x.uinfo?.Email ?? string.Empty,
                DepotId = "",
                DepotName = "",
                Territory = "",
                Zone = "",
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.p.CollectionDate),
                TypeOfCustomer = x.dctinfo?.DropdownName,
                ProjectSapId = x.p.SapId,
                ProjectName = x.p.Name,
                ProjectAddress = x.p.Address,
                PaymentMethod = x.dpminfo?.DropdownName,
                CreditControlArea = x.cainfo?.Description,
                BankName = x.p.BankName,
                ChequeNumber = x.p.Number,
                CashAmount = x.p.Amount,
                ManualMrNumber = x.p.ManualNumber,
                Remarks = x.p.Remarks
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DirectProjectCollectionReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = projects.Count();
            queryResult.Total = projects.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<PainterCallReportResultModel>> GetPainterCallReportAsync(PainterCallReportSearchModel query)
        {
            var reportResult = new List<PainterCallReportResultModel>();

            var painterCalls = (from pcm in await _painterCompanyMtdRepository.GetAllAsync()
                                join pc in await _painterCallRepository.GetAllAsync() on pcm.PainterCallId equals pc.Id into pcleftjoin
                                from pcinfo in pcleftjoin.DefaultIfEmpty()
                                join p in await _painterRepository.GetAllAsync() on pcinfo.PainterId equals p.Id into pleftjoin
                                from pinfo in pleftjoin.DefaultIfEmpty()
                                join u in await _userInfoRepository.GetAllAsync() on pinfo.EmployeeId equals u.EmployeeId into uleftjoin
                                from userInfo in uleftjoin.DefaultIfEmpty()
                                join ddc in await _dorpDownDetailsRepository.GetAllAsync() on pinfo.PainterCatId equals ddc.Id into ddcleftjoin
                                from ddcinfo in ddcleftjoin.DefaultIfEmpty()
                                join dep in await _depotSvc.GetAllAsync() on pinfo.Depot equals dep.Werks into depleftjoin
                                from depinfo in depleftjoin.DefaultIfEmpty()
                                join sg in await _saleGroupSvc.GetAllAsync() on pinfo.SaleGroup equals sg.Code into sgleftjoin
                                from sginfo in sgleftjoin.DefaultIfEmpty()
                                join t in await _territorySvc.GetAllAsync() on pinfo.Territory equals t.Code into tleftjoin
                                from tinfo in tleftjoin.DefaultIfEmpty()
                                join z in await _zoneSvc.GetAllAsync() on pinfo.Zone equals z.Code into zleftjoin
                                from zinfo in zleftjoin.DefaultIfEmpty()
                                join adp in await _attachmentDealerRepository.GetAllAsync() on pinfo.AttachedDealerCd equals adp.Id.ToString() into adpleftjoin
                                from adpInfo in adpleftjoin.DefaultIfEmpty()
                                join di in await _dealerInfoRepository.GetAllAsync() on adpInfo?.Dealer equals di.Id into dileftjoin
                                from diInfo in dileftjoin.DefaultIfEmpty()
                                where (
                                (!query.UserId.HasValue || userInfo?.Id == query.UserId.Value)
                                && (string.IsNullOrWhiteSpace(query.DepotId) || pinfo.Depot == query.DepotId)
                                && (!query.Territories.Any() || query.Territories.Contains(pinfo.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(pinfo.Zone))
                                && (!query.FromDate.HasValue || pcinfo.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || pcinfo.CreatedTime.Date <= query.ToDate.Value.Date)
                                && (!query.PainterId.HasValue || pinfo?.Id == query.PainterId.Value)
                                && (!query.PainterType.HasValue || pinfo.PainterCatId == query.PainterType.Value)
                            )
                                select new { pcinfo, pinfo, userInfo, ddcinfo, depinfo, sginfo, tinfo, zinfo, diInfo }).Distinct().ToList();

            var painterCallMtd = (from pmtd in await _painterCompanyMtdRepository.GetAllAsync()
                                  join dd in await _dorpDownDetailsRepository.GetAllAsync() on pmtd.CompanyId equals dd.Id into ddleftjoin
                                  from ddinfo in ddleftjoin.DefaultIfEmpty()
                                  where (
                                  (!query.FromDate.HasValue || pmtd.CreatedTime.Date >= query.FromDate.Value.Date)
                                  && (!query.ToDate.HasValue || pmtd.CreatedTime.Date <= query.ToDate.Value.Date)
                                  )
                                  select new { pmtd, ddinfo }).ToList();

            reportResult = painterCalls.Select(x => new PainterCallReportResultModel
            {
                UserId = x.userInfo?.Email ?? string.Empty,
                PainterId = x.pcinfo?.PainterId.ToString(),
                PainterVisitDate = CustomConvertExtension.ObjectToDateString(x.pcinfo.CreatedTime),
                TypeOfPainter = x.ddcinfo?.DropdownName,
                DepotName = x.depinfo?.Name1,
                SalesGroup = x.sginfo?.Name,
                Territory = x.tinfo?.Name,
                Zone = x.zinfo?.Name,
                PainterName = x.pinfo?.PainterName,
                PainterAddress = x.pinfo?.Address,
                MobileNumber = x.pinfo?.Phone,
                NoOfPainterAttached = x.pinfo?.NoOfPainterAttached.ToString(),
                DbblRocketAccountStatus = x.pinfo.HasDbbl ? "Created" : "Not Created",
                AccountNumber = x.pinfo?.AccDbblNumber,
                AcccountHolderName = x.pinfo?.AccDbblHolderName,
                IdentificationNo = !string.IsNullOrEmpty(x.pinfo.NationalIdNo) ? x.pinfo.NationalIdNo
                        : (!string.IsNullOrEmpty(x.pinfo.PassportNo) ? x.pinfo.PassportNo
                        : (!string.IsNullOrEmpty(x.pinfo.BrithCertificateNo)) ? x.pinfo.BrithCertificateNo : string.Empty),
                AttachedTaggedDealerId = x.pinfo?.AttachedDealerCd,
                AttachedTaggedDealerName = x.diInfo?.CustomerName,
                ShamparkaAppInstallStatus = x.pinfo.IsAppInstalled ? "Installed" : "Not Installed",
                BergerLoyalty = x.pinfo?.Loyality.ToString(),
                PainterSchemeCommunication = x.pcinfo.HasSchemeComnunaction ? "Yes" : "No",
                PremiumProductBriefing = x.pcinfo.HasPremiumProtBriefing ? "Yes" : "No",
                NewProductBriefing = x.pcinfo.HasNewProBriefing ? "Yes" : "No",
                EpToolsUsage = x.pcinfo.HasUsageEftTools ? "Yes" : "No",
                PainterAppUsage = x.pcinfo.HasAppUsage ? "Yes" : "No",
                WorkInHandNo = x.pcinfo?.WorkInHandNumber.ToString(),
                ApMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorAsianPaints).Sum(x => x.pmtd.Value).ToString(),
                ApCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorAsianPaints).Sum(x => x.pmtd.CountInPercent).ToString(),
                NerolacMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNerolac).Sum(x => x.pmtd.Value).ToString(),
                NerolacCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNerolac).Sum(x => x.pmtd.CountInPercent).ToString(),
                EliteMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorElitePaints).Sum(x => x.pmtd.Value).ToString(),
                EliteCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorElitePaints).Sum(x => x.pmtd.CountInPercent).ToString(),
                NipponMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNippon).Sum(x => x.pmtd.Value).ToString(),
                NipponCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNippon).Sum(x => x.pmtd.CountInPercent).ToString(),
                DuluxMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorDulux).Sum(x => x.pmtd.Value).ToString(),
                DuluxCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorDulux).Sum(x => x.pmtd.CountInPercent).ToString(),
                MoonstarMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorMoonstar).Sum(x => x.pmtd.Value).ToString(),
                MoonstarCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorMoonstar).Sum(x => x.pmtd.CountInPercent).ToString(),
                OthersMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorOthers).Sum(x => x.pmtd.Value).ToString(),
                OthersCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorOthers).Sum(x => x.pmtd.CountInPercent).ToString(),
                TotalMtdValue = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorAsianPaints
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNerolac
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorElitePaints
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNippon
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorDulux
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorMoonstar
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorOthers).Sum(x => x.pmtd.Value).ToString(),
                TotalCount = painterCallMtd.Where(x => x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorAsianPaints
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNerolac
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorElitePaints
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorNippon
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorDulux
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorMoonstar
                                                    || x.ddinfo.DropdownName == SwappingCompetitionValue.CompetitorOthers).Sum(x => x.pmtd.CountInPercent).ToString(),
                IssueWithDbblAccount = x.pcinfo.HasDbblIssue ? "Yes" : "No",
                RemarkIssueWithDbblAccount = "",
                Comments = x.pcinfo?.Comment,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<PainterCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = painterCalls.Count();
            queryResult.Total = painterCalls.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerVisitReportResultModel>> GetDealerVisitReportAsync(DealerVisitReportSearchModel query)
        {
            var reportResult = new List<DealerVisitReportResultModel>();
            int? month = (!query.Month.HasValue) ? DateTime.Now.Month : query.Month;
            int? year = (!query.Year.HasValue) ? DateTime.Now.Year : query.Year;
            int tvist = 0;
            int avisit = 0;

            var dealerVisits = (from jpd in await _journeyPlanDetailRepository.GetAllAsync()
                                join jpm in await _journeyPlanMasterRepository.GetAllAsync() on jpd.PlanId equals jpm.Id into jpmleftjoin
                                from jpminfo in jpmleftjoin.DefaultIfEmpty()
                                join dsc in await _dealerSalesCallRepository.GetAllAsync() on jpd.PlanId equals dsc.JourneyPlanId into dscleftjoin
                                from dscinfo in dscleftjoin.DefaultIfEmpty()
                                join u in await _userInfoRepository.GetAllAsync() on jpminfo?.EmployeeId equals u.EmployeeId into uleftjoin
                                from userInfo in uleftjoin.DefaultIfEmpty()
                                join di in await _dealerInfoRepository.GetAllAsync() on jpd?.DealerId equals di.Id into dileftjoin
                                from diInfo in dileftjoin.DefaultIfEmpty()
                                join dep in await _depotSvc.GetAllAsync() on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                from depinfo in depleftjoin.DefaultIfEmpty()
                                join t in await _territorySvc.GetAllAsync() on diInfo.Territory equals t.Code into tleftjoin
                                from tinfo in tleftjoin.DefaultIfEmpty()
                                join z in await _zoneSvc.GetAllAsync() on diInfo.CustZone equals z.Code into zleftjoin
                                from zinfo in zleftjoin.DefaultIfEmpty()
                                where (
                                  (jpminfo.PlanDate.Month == month && jpminfo.PlanDate.Year == year)
                                  && (!query.UserId.HasValue || userInfo?.Id == query.UserId.Value)
                                  && (string.IsNullOrWhiteSpace(query.DepotId) || diInfo.BusinessArea == query.DepotId)
                                  && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                  && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                  && (!query.DealerId.HasValue || jpd?.DealerId == query.DealerId.Value)
                                )
                                select new { jpd, jpminfo, dscinfo, userInfo, diInfo, depinfo, tinfo, zinfo }).ToList();

            reportResult = dealerVisits
                        .GroupBy(x => new { x.jpminfo.EmployeeId, x.jpd.DealerId })
                        .Select(x => new DealerVisitReportResultModel
                        {
                            UserId = x.FirstOrDefault()?.userInfo?.Email,
                            DepotId = x.FirstOrDefault()?.diInfo?.BusinessArea,
                            DepotName = x.FirstOrDefault()?.depinfo?.Name1,
                            Territory = x.FirstOrDefault()?.tinfo?.Name,
                            Zone = x.FirstOrDefault()?.zinfo?.Name,
                            DealerId = x.Key.DealerId.ToString(),
                            DealerName = x.FirstOrDefault()?.diInfo?.CustomerName,
                            D1 = x.Count(c => c.jpminfo?.PlanDate.Day == 1) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 1) > 0 ? "Visited" : "Not Visited" : "",
                            D2 = x.Count(c => c.jpminfo?.PlanDate.Day == 2) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 2) > 0 ? "Visited" : "Not Visited" : "",
                            D3 = x.Count(c => c.jpminfo?.PlanDate.Day == 3) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 3) > 0 ? "Visited" : "Not Visited" : "",
                            D4 = x.Count(c => c.jpminfo?.PlanDate.Day == 4) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 4) > 0 ? "Visited" : "Not Visited" : "",
                            D5 = x.Count(c => c.jpminfo?.PlanDate.Day == 5) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 5) > 0 ? "Visited" : "Not Visited" : "",
                            D6 = x.Count(c => c.jpminfo?.PlanDate.Day == 6) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 6) > 0 ? "Visited" : "Not Visited" : "",
                            D7 = x.Count(c => c.jpminfo?.PlanDate.Day == 7) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 7) > 0 ? "Visited" : "Not Visited" : "",
                            D8 = x.Count(c => c.jpminfo?.PlanDate.Day == 8) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 8) > 0 ? "Visited" : "Not Visited" : "",
                            D9 = x.Count(c => c.jpminfo?.PlanDate.Day == 9) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 9) > 0 ? "Visited" : "Not Visited" : "",
                            D10 = x.Count(c => c.jpminfo?.PlanDate.Day == 10) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 10) > 0 ? "Visited" : "Not Visited" : "",
                            D11 = x.Count(c => c.jpminfo?.PlanDate.Day == 11) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 11) > 0 ? "Visited" : "Not Visited" : "",
                            D12 = x.Count(c => c.jpminfo?.PlanDate.Day == 12) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 12) > 0 ? "Visited" : "Not Visited" : "",
                            D13 = x.Count(c => c.jpminfo?.PlanDate.Day == 13) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 13) > 0 ? "Visited" : "Not Visited" : "",
                            D14 = x.Count(c => c.jpminfo?.PlanDate.Day == 14) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 14) > 0 ? "Visited" : "Not Visited" : "",
                            D15 = x.Count(c => c.jpminfo?.PlanDate.Day == 15) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 15) > 0 ? "Visited" : "Not Visited" : "",
                            D16 = x.Count(c => c.jpminfo?.PlanDate.Day == 16) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 16) > 0 ? "Visited" : "Not Visited" : "",
                            D17 = x.Count(c => c.jpminfo?.PlanDate.Day == 17) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 17) > 0 ? "Visited" : "Not Visited" : "",
                            D18 = x.Count(c => c.jpminfo?.PlanDate.Day == 18) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 18) > 0 ? "Visited" : "Not Visited" : "",
                            D19 = x.Count(c => c.jpminfo?.PlanDate.Day == 19) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 19) > 0 ? "Visited" : "Not Visited" : "",
                            D20 = x.Count(c => c.jpminfo?.PlanDate.Day == 20) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 20) > 0 ? "Visited" : "Not Visited" : "",
                            D21 = x.Count(c => c.jpminfo?.PlanDate.Day == 21) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 21) > 0 ? "Visited" : "Not Visited" : "",
                            D22 = x.Count(c => c.jpminfo?.PlanDate.Day == 22) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 22) > 0 ? "Visited" : "Not Visited" : "",
                            D23 = x.Count(c => c.jpminfo?.PlanDate.Day == 23) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 23) > 0 ? "Visited" : "Not Visited" : "",
                            D24 = x.Count(c => c.jpminfo?.PlanDate.Day == 24) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 24) > 0 ? "Visited" : "Not Visited" : "",
                            D25 = x.Count(c => c.jpminfo?.PlanDate.Day == 25) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 25) > 0 ? "Visited" : "Not Visited" : "",
                            D26 = x.Count(c => c.jpminfo?.PlanDate.Day == 26) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 26) > 0 ? "Visited" : "Not Visited" : "",
                            D27 = x.Count(c => c.jpminfo?.PlanDate.Day == 27) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 27) > 0 ? "Visited" : "Not Visited" : "",
                            D28 = x.Count(c => c.jpminfo?.PlanDate.Day == 28) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 28) > 0 ? "Visited" : "Not Visited" : "",
                            D29 = x.Count(c => c.jpminfo?.PlanDate.Day == 29) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 29) > 0 ? "Visited" : "Not Visited" : "",
                            D30 = x.Count(c => c.jpminfo?.PlanDate.Day == 30) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 30) > 0 ? "Visited" : "Not Visited" : "",
                            D31 = x.Count(c => c.jpminfo?.PlanDate.Day == 31) > 0 ?
                                        x.Count(c => c.dscinfo?.JourneyPlanId != null && c.jpminfo?.PlanDate.Day == 31) > 0 ? "Visited" : "Not Visited" : "",
                            TargetVisits = tvist = x.Count(c => c.jpminfo?.PlanDate.Month == month && c.jpminfo?.PlanDate.Year == year),
                            ActualVisits = avisit = x.Count(c => c.dscinfo?.JourneyPlanId != null && (c.jpminfo.PlanDate.Month == month && c.jpminfo.PlanDate.Year == year)),
                            NotVisits = (tvist - avisit)
                        }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerVisitReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = reportResult.Count();
            queryResult.Total = reportResult.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<DealerSalesCallReportResultModel>> GetDealerSalesCallReportAsync(DealerSalesCallReportSearchModel query)
        {
            var reportResult = new List<DealerSalesCallReportResultModel>();

            var dealerCall = (from dsc in await _dealerSalesCallRepository.GetAllAsync()
                              join ssdd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.SecondarySalesRatingsId equals ssdd.Id into ssddleftjoin
                              from ssddinfo in ssddleftjoin.DefaultIfEmpty()
                              join ppldd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.PremiumProductLiftingId equals ppldd.Id into pplddleftjoin
                              from pplddinfo in pplddleftjoin.DefaultIfEmpty()
                              join mdd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.MerchendisingId equals mdd.Id into mddleftjoin
                              from mddinfo in mddleftjoin.DefaultIfEmpty()
                              join sdidd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.SubDealerInfluenceId equals sdidd.Id into sdiddleftjoin
                              from sdiddinfo in sdiddleftjoin.DefaultIfEmpty()
                              join pidd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.PainterInfluenceId equals pidd.Id into piddleftjoin
                              from piddinfo in piddleftjoin.DefaultIfEmpty()
                              join dsdd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.DealerSatisfactionId equals dsdd.Id into dsddleftjoin
                              from dsddinfo in dsddleftjoin.DefaultIfEmpty()
                              join di in await _dealerInfoRepository.GetAllAsync() on dsc?.DealerId equals di.Id into dileftjoin
                              from diInfo in dileftjoin.DefaultIfEmpty()
                              join dep in await _depotSvc.GetAllAsync() on diInfo?.BusinessArea equals dep.Werks into depleftjoin
                              from depinfo in depleftjoin.DefaultIfEmpty()
                              join t in await _territorySvc.GetAllAsync() on diInfo?.Territory equals t.Code into tleftjoin
                              from tinfo in tleftjoin.DefaultIfEmpty()
                              join z in await _zoneSvc.GetAllAsync() on diInfo?.CustZone equals z.Code into zleftjoin
                              from zinfo in zleftjoin.DefaultIfEmpty()
                              join u in await _userInfoRepository.GetAllAsync() on dsc?.UserId equals u.Id into uleftjoin
                              from userInfo in uleftjoin.DefaultIfEmpty()
                              where (
                                 (dsc.IsSubDealerCall == false)
                                 && (!query.UserId.HasValue || userInfo?.Id == query.UserId.Value)
                                 && (string.IsNullOrWhiteSpace(query.DepotId) || diInfo.BusinessArea == query.DepotId)
                                 && (!query.FromDate.HasValue || dsc.CreatedTime.Date >= query.FromDate.Value.Date)
                                 && (!query.ToDate.HasValue || dsc.CreatedTime.Date <= query.ToDate.Value.Date)
                                 && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                 && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                 && (!query.DealerId.HasValue || dsc?.DealerId == query.DealerId.Value)
                              )
                              select new { dsc, ssddinfo, pplddinfo, mddinfo, sdiddinfo, piddinfo, dsddinfo, diInfo, depinfo, tinfo, zinfo, userInfo }).ToList();

            var dealerCompitition = (from dcs in await _dealerCompetitionSaleRepository.GetAllAsync()
                                     join dd in await _dorpDownDetailsRepository.GetAllAsync() on dcs.CompanyId equals dd.Id into ddleft
                                     from ddinfo in ddleft.DefaultIfEmpty()
                                     select new { dcs, ddinfo }).ToList();

            reportResult = dealerCall.Select(x => new DealerSalesCallReportResultModel
            {
                UserId = x.userInfo?.Email ?? string.Empty,
                DepotId = x.diInfo?.BusinessArea,
                DepotName = x.depinfo?.Name1,
                Territory = x.tinfo?.Name,
                Zone = x.zinfo?.Name,
                DealerId = x.dsc?.DealerId.ToString(),
                DealerName = x.diInfo?.CustomerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.dsc.CreatedTime),
                TradePromotion = x.dsc.IsTargetPromotionCommunicated ? "Yes" : "No",
                Target = x.dsc.IsTargetCommunicated ? "Yes" : "No",
                SsStatus = x.ssddinfo?.DropdownName,
                SsReasonForPourOrAverage = x.dsc.SecondarySalesReasonRemarks,
                OsCommunication = x.dsc.IsOSCommunicated ? "Yes" : "No",
                SlippageCommunication = x.dsc.IsSlippageCommunicated ? "Yes" : "No",
                UspCommunication = x.dsc.IsPremiumProductCommunicated ? "Yes" : "No",
                ProductLiftingStatus = x.pplddinfo?.DropdownName,
                ReasonForNotLifting = x.dsc.PremiumProductLiftingOthers,
                CbMachineStatus = x.dsc.IsCBInstalled ? "Yes" : "No",
                CbProductivity = x.dsc.IsCBProductivityCommunicated ? "Yes" : "No",
                Merchendising = x.mddinfo?.DropdownName,
                SubDealerInfluence = x.dsc.HasSubDealerInfluence ? "Yes" : "No",
                SdInfluecePercent = x.sdiddinfo?.DropdownName,
                PainterInfluence = x.dsc.HasPainterInfluence ? "Yes" : "No",
                PainterInfluecePercent = x.piddinfo?.DropdownName,
                ProductKnoledge = x.dsc.IsShopManProductKnowledgeDiscussed ? "Yes" : "No",
                SalesTechniques = x.dsc.IsShopManSalesTechniquesDiscussed ? "Yes" : "No",
                MerchendisingImprovement = x.dsc.IsShopManMerchendizingImprovementDiscussed ? "Yes" : "No",
                CompetitionPresence = x.dsc.HasCompetitionPresence ? "Yes" : "No",
                CompetitionService = x.dsc.IsCompetitionServiceBetterThanBPBL ? "Yes" : "No",
                CsRemarks = x.dsc.CompetitionServiceBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingStatus = x.dsc.IsCompetitionProductDisplayBetterThanBPBL ? "Yes" : "No",
                PdmRemarks = x.dsc.CompetitionProductDisplayBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingImage = x.dsc.CompetitionProductDisplayImageUrl,
                SchemeModality = x.dsc.CompetitionSchemeModalityComments,
                SchemeModalityImage = x.dsc.CompetitionSchemeModalityImageUrl,
                ShopBoy = x.dsc.CompetitionShopBoysComments,
                ApAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAP).dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                ApActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAP).dcs?.ActualMTDSales.ToString() ?? string.Empty,
                NerolacAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNerolac).dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                NerolacActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNerolac)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                NipponAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNippon)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                NipponActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNippon)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                DuluxAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyDulux)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                DuluxActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyDulux)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                JotunAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyJotun)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                JotunActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyJotun)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                MoonstarAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyMoonstar)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                MoonstarActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyMoonstar)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                EliteAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyElite)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                EliteActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyElite)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                AlkarimAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAlKarim)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                AlkarimActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAlKarim)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                OthersAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyOthers)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                OthersActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyOthers)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                TotalAvrgMonthlySales = dealerCompitition.Where(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id).Sum(y => y.dcs?.AverageMonthlySales).ToString(),
                TotalActualMtdSales = dealerCompitition.Where(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id).Sum(y => y.dcs?.ActualMTDSales).ToString(),
                DealerIssueStatus = x.dsc.HasDealerSalesIssue ? "Yes" : "No",
                DealerSatisfactionStatus = x.dsddinfo?.DropdownName,
                DealerDissatisfactionReason = x.dsc.DealerSatisfactionReason,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<DealerSalesCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealerCall.Count();
            queryResult.Total = dealerCall.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<SubDealerSalesCallReportResultModel>> GetSubDealerSalesCallReportAsync(SubDealerSalesCallReportSearchModel query)
        {
            var reportResult = new List<SubDealerSalesCallReportResultModel>();

            var dealerCall = (from dsc in await _dealerSalesCallRepository.GetAllAsync()
                              join ssdd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.SecondarySalesRatingsId equals ssdd.Id into ssddleftjoin
                              from ssddinfo in ssddleftjoin.DefaultIfEmpty()
                              join ppldd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.PremiumProductLiftingId equals ppldd.Id into pplddleftjoin
                              from pplddinfo in pplddleftjoin.DefaultIfEmpty()
                              join mdd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.MerchendisingId equals mdd.Id into mddleftjoin
                              from mddinfo in mddleftjoin.DefaultIfEmpty()
                              join sdidd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.SubDealerInfluenceId equals sdidd.Id into sdiddleftjoin
                              from sdiddinfo in sdiddleftjoin.DefaultIfEmpty()
                              join pidd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.PainterInfluenceId equals pidd.Id into piddleftjoin
                              from piddinfo in piddleftjoin.DefaultIfEmpty()
                              join dsdd in await _dorpDownDetailsRepository.GetAllAsync() on dsc?.DealerSatisfactionId equals dsdd.Id into dsddleftjoin
                              from dsddinfo in dsddleftjoin.DefaultIfEmpty()
                              join di in await _dealerInfoRepository.GetAllAsync() on dsc?.DealerId equals di.Id into dileftjoin
                              from diInfo in dileftjoin.DefaultIfEmpty()
                              join dep in await _depotSvc.GetAllAsync() on diInfo?.BusinessArea equals dep.Werks into depleftjoin
                              from depinfo in depleftjoin.DefaultIfEmpty()
                              join t in await _territorySvc.GetAllAsync() on diInfo?.Territory equals t.Code into tleftjoin
                              from tinfo in tleftjoin.DefaultIfEmpty()
                              join z in await _zoneSvc.GetAllAsync() on diInfo?.CustZone equals z.Code into zleftjoin
                              from zinfo in zleftjoin.DefaultIfEmpty()
                              join u in await _userInfoRepository.GetAllAsync() on dsc?.UserId equals u.Id into uleftjoin
                              from userInfo in uleftjoin.DefaultIfEmpty()
                              where (
                                 (dsc.IsSubDealerCall == true)
                                 && (!query.UserId.HasValue || userInfo?.Id == query.UserId.Value)
                                 && (string.IsNullOrWhiteSpace(query.DepotId) || diInfo.BusinessArea == query.DepotId)
                                 && (!query.FromDate.HasValue || dsc.CreatedTime.Date >= query.FromDate.Value.Date)
                                 && (!query.ToDate.HasValue || dsc.CreatedTime.Date <= query.ToDate.Value.Date)
                                 && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                 && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                 && (!query.SubDealerId.HasValue || dsc?.DealerId == query.SubDealerId.Value)
                              )
                              select new { dsc, ssddinfo, pplddinfo, mddinfo, sdiddinfo, piddinfo, dsddinfo, diInfo, depinfo, tinfo, zinfo, userInfo }).ToList();

            var dealerCompitition = (from dcs in await _dealerCompetitionSaleRepository.GetAllAsync()
                                     join dd in await _dorpDownDetailsRepository.GetAllAsync() on dcs.CompanyId equals dd.Id into ddleft
                                     from ddinfo in ddleft.DefaultIfEmpty()
                                     select new { dcs, ddinfo }).ToList();

            reportResult = dealerCall.Select(x => new SubDealerSalesCallReportResultModel
            {
                UserId = x.userInfo?.Email ?? string.Empty,
                DepotId = x.diInfo?.BusinessArea,
                DepotName = x.depinfo?.Name1,
                Territory = x.tinfo?.Name,
                Zone = x.zinfo?.Name,
                SubDealerId = x.dsc?.DealerId.ToString(),
                SubDealerName = x.diInfo?.CustomerName,
                VisitDate = CustomConvertExtension.ObjectToDateString(x.dsc.CreatedTime),
                TradePromotion = x.dsc.IsTargetPromotionCommunicated ? "Yes" : "No",
                //Target = x.dsc.IsTargetCommunicated ? "Yes" : "No",
                SsStatus = x.ssddinfo?.DropdownName,
                SsReasonForPourOrAverage = x.dsc.SecondarySalesReasonRemarks,
                OsStatus = x.dsc.HasOS ? "Yes" : "No",
                OsActivity = x.dsc.IsSlippageCommunicated ? "Yes" : "No",
                UspCommunication = x.dsc.IsPremiumProductCommunicated ? "Yes" : "No",
                ProductLiftingStatus = x.pplddinfo?.DropdownName,
                ReasonForNotLifting = x.dsc.PremiumProductLiftingOthers,
                Merchendising = x.mddinfo?.DropdownName,
                PainterInfluence = x.dsc.HasPainterInfluence ? "Yes" : "No",
                PainterInfluecePercent = x.piddinfo?.DropdownName,
                ProductKnoledge = x.dsc.IsShopManProductKnowledgeDiscussed ? "Yes" : "No",
                SalesTechniques = x.dsc.IsShopManSalesTechniquesDiscussed ? "Yes" : "No",
                MerchendisingImprovement = x.dsc.IsShopManMerchendizingImprovementDiscussed ? "Yes" : "No",
                BergerAvrgMonthlySales = x.dsc.BPBLAverageMonthlySales.ToString(),
                BergerActualMtdSales = x.dsc.BPBLActualMTDSales.ToString(),
                CompetitionPresence = x.dsc.HasCompetitionPresence ? "Yes" : "No",
                CompetitionService = x.dsc.IsCompetitionServiceBetterThanBPBL ? "Yes" : "No",
                CsRemarks = x.dsc.CompetitionServiceBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingStatus = x.dsc.IsCompetitionProductDisplayBetterThanBPBL ? "Yes" : "No",
                PdmRemarks = x.dsc.CompetitionProductDisplayBetterThanBPBLRemarks,
                ProductDisplayAndMerchendizingImage = x.dsc.CompetitionProductDisplayImageUrl,
                SchemeModality = x.dsc.CompetitionSchemeModalityComments,
                SchemeModalityImage = x.dsc.CompetitionSchemeModalityImageUrl,
                ShopBoy = x.dsc.CompetitionShopBoysComments,
                ApAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAP).dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                ApActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAP).dcs?.ActualMTDSales.ToString() ?? string.Empty,
                NerolacAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNerolac).dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                NerolacActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNerolac)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                NipponAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNippon)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                NipponActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyNippon)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                DuluxAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyDulux)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                DuluxActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyDulux)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                JotunAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyJotun)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                JotunActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyJotun)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                MoonstarAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyMoonstar)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                MoonstarActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyMoonstar)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                EliteAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyElite)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                EliteActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyElite)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                AlkarimAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAlKarim)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                AlkarimActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyAlKarim)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                OthersAvrgMonthlySales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyOthers)?.dcs?.AverageMonthlySales.ToString() ?? string.Empty,
                OthersActualMtdSales = dealerCompitition.FirstOrDefault(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id && y.ddinfo?.DropdownName == ConstantCompanyValue.companyOthers)?.dcs?.ActualMTDSales.ToString() ?? string.Empty,
                TotalAvrgMonthlySales = dealerCompitition.Where(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id).Sum(y => y.dcs?.AverageMonthlySales).ToString(),
                TotalActualMtdSales = dealerCompitition.Where(y => y.dcs?.DealerSalesCallId == x?.dsc?.Id).Sum(y => y.dcs?.ActualMTDSales).ToString(),
                SubDealerIssueStatus = x.dsc.HasDealerSalesIssue ? "Yes" : "No",
                DealerSatisfactionStatus = x.dsddinfo?.DropdownName,
                DealerDissatisfactionReason = x.dsc.DealerSatisfactionReason,
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<SubDealerSalesCallReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = dealerCall.Count();
            queryResult.Total = dealerCall.Count();

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
                                        && (string.IsNullOrWhiteSpace(query.DepotId) || diInfo.BusinessArea == query.DepotId)
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
                dealerId = x.FirstOrDefault()?.DealerId,
                dealerName = x.FirstOrDefault()?.CustomerName,
                visitDate = x.FirstOrDefault()?.CreatedTime,
                pcMaterial = x.FirstOrDefault(y => y.issueCategory == "")?.MaterialName,
                pcMaterialGroup = x.FirstOrDefault(y => y.issueCategory == "")?.MaterialGroup,
                pcQuantity = x.FirstOrDefault(y => y.issueCategory == "")?.Quantity,
                pcBatchNumber = x.FirstOrDefault(y => y.issueCategory == "")?.BatchNumber,
                pcComments = x.FirstOrDefault(y => y.issueCategory == "")?.Comments,
                pcPriority = x.FirstOrDefault(y => y.issueCategory == "")?.priority,
                posComments = x.FirstOrDefault(y => y.issueCategory == "POS Material Short")?.Comments,
                posPriority = x.FirstOrDefault(y => y.issueCategory == "POS Material Short")?.priority,
                shadeComments = x.FirstOrDefault(y => y.issueCategory == "Shade Card")?.Comments,
                shadePriority = x.FirstOrDefault(y => y.issueCategory == "Shade Card")?.priority,
                shopsignComments = x.FirstOrDefault(y => y.issueCategory == "Shop Sign Complain")?.Comments,
                shopsignPriority = x.FirstOrDefault(y => y.issueCategory == "Shop Sign Complain")?.priority,
                deliveryComments = x.FirstOrDefault(y => y.issueCategory == "Delivery Issue")?.Comments,
                deliveryPriority = x.FirstOrDefault(y => y.issueCategory == "Delivery Issue")?.priority,
                damageMaterial = x.FirstOrDefault(y => y.issueCategory == "")?.MaterialName,
                damageMaterialGroup = x.FirstOrDefault(y => y.issueCategory == "")?.MaterialGroup,
                damageQuantity = x.FirstOrDefault(y => y.issueCategory == "")?.Quantity,
                damageComments = x.FirstOrDefault(y => y.issueCategory == "")?.Comments,
                damagePriority = x.FirstOrDefault(y => y.issueCategory == "")?.priority,
                cbmStatus = x.FirstOrDefault(y => y.issueCategory == "")?.HasCBMachineMantainance ?? false,
                cbmMaintatinanceFrequency = x.FirstOrDefault(y => y.issueCategory == "")?.maintinaceFrequency,
                cbmRemarks = x.FirstOrDefault(y => y.issueCategory == "")?.CBMachineMantainanceRegularReason,
                cbmPriority = x.FirstOrDefault(y => y.issueCategory == "")?.priority,
                othersComments = x.FirstOrDefault(y => y.issueCategory == "Others")?.Comments,
                othersPriority = x.FirstOrDefault(y => y.issueCategory == "Others")?.priority
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
            queryResult.TotalFilter = reportResult.Count();
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
                                        && (string.IsNullOrWhiteSpace(query.DepotId) || diInfo.BusinessArea == query.DepotId)
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
                dealerId = x.FirstOrDefault()?.DealerId,
                dealerName = x.FirstOrDefault()?.CustomerName,
                visitDate = x.FirstOrDefault()?.CreatedTime,
                posComments = x.FirstOrDefault(y => y.issueCategory == "POS Material Short")?.Comments,
                posPriority = x.FirstOrDefault(y => y.issueCategory == "POS Material Short")?.priority,
                shadeComments = x.FirstOrDefault(y => y.issueCategory == "Shade Card")?.Comments,
                shadePriority = x.FirstOrDefault(y => y.issueCategory == "Shade Card")?.priority,
                shopsignComments = x.FirstOrDefault(y => y.issueCategory == "Shop Sign Complain")?.Comments,
                shopsignPriority = x.FirstOrDefault(y => y.issueCategory == "Shop Sign Complain")?.priority,
                deliveryComments = x.FirstOrDefault(y => y.issueCategory == "Delivery Issue")?.Comments,
                deliveryPriority = x.FirstOrDefault(y => y.issueCategory == "Delivery Issue")?.priority,
                cbmStatus = x.FirstOrDefault(y => y.issueCategory == "")?.HasCBMachineMantainance ?? false,
                cbmMaintatinanceFrequency = x.FirstOrDefault(y => y.issueCategory == "")?.maintinaceFrequency,
                cbmRemarks = x.FirstOrDefault(y => y.issueCategory == "")?.CBMachineMantainanceRegularReason,
                cbmPriority = x.FirstOrDefault(y => y.issueCategory == "")?.priority,
                othersComments = x.FirstOrDefault(y => y.issueCategory == "Others")?.Comments,
                othersPriority = x.FirstOrDefault(y => y.issueCategory == "Others")?.priority
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
            queryResult.TotalFilter = reportResult.Count();
            queryResult.Total = groupSubDealerIssue.Count();

            return queryResult;
        }



    }
}
