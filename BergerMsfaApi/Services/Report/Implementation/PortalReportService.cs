using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
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
        private readonly IRepository<CreditControlArea> _creditControlAreaRepository;
        private readonly IDropdownService _dropdownService;
        private readonly IMapper _mapper;

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
                IRepository<CreditControlArea> creditControlAreaRepository,
                IDropdownService dropdownService,
                IMapper mapper
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
            this._creditControlAreaRepository = creditControlAreaRepository;
            this._mapper = mapper;
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
                PainterId =  x.p.Id.ToString(),
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
                          where(
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
                           where (
                             dctinfo.DropdownName == ConstantsCustomerTypeValue.CustomerTypeDealer
                             && (!query.UserId.HasValue || uinfo?.Id == query.UserId.Value)
                             && (!query.PaymentMethodId.HasValue || p?.PaymentMethodId == query.PaymentMethodId.Value)
                             && (!query.DealerId.HasValue || p?.Code == query.DealerId.Value.ToString())
                             && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                             && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                           )
                           select new { p, uinfo, dctinfo, dpminfo, cainfo }).ToList();

            reportResult = dealers.Select(x => new DealerCollectionReportResultModel
            {
                UserId = x.uinfo?.Email ?? string.Empty,
                DepotId = "",
                DepotName = "",
                Territory = "",
                Zone = "",
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
                              where (
                                dctinfo.DropdownName == ConstantsCustomerTypeValue.CustomerTypeSubDealer
                                && (!query.UserId.HasValue || uinfo?.Id == query.UserId.Value)
                                && (!query.PaymentMethodId.HasValue || p?.PaymentMethodId == query.PaymentMethodId.Value)
                                && (!query.DealerId.HasValue || p?.Code == query.DealerId.Value.ToString())
                                && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                              )
                              select new { p, uinfo, dctinfo, dpminfo, cainfo }).ToList();

            reportResult = subDealers.Select(x => new SubDealerCollectionReportResultModel
            {
                UserId = x.uinfo?.Email ?? string.Empty,
                DepotId = "",
                DepotName = "",
                Territory = "",
                Zone = "",
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


    }
}
