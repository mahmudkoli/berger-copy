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
            //var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            //{
            //    ["createdTime"] = v => v.CreatedTime,
            //};

            var reportResult = new List<LeadSummaryReportResultModel>();

            var leads = await _leadGenerationRepository.GetAllIncludeAsync(x => x,
                            x => (!query.UserId.HasValue || x.UserId == query.UserId.Value)
                                //&& (!query.EmployeeRole.HasValue || x.User.EmployeeRole == query.EmployeeRole.Value)
                                && (string.IsNullOrWhiteSpace(query.Depot) || x.Depot == query.Depot)
                                && (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                                && (!query.Zones.Any() || query.Zones.Contains(x.Zone))
                                && (!query.FromDate.HasValue || x.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || x.CreatedTime.Date <= query.ToDate.Value.Date),
                            x => x.OrderByDescending(o => o.CreatedTime),
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
                var leadFollowUps = x.SelectMany(x => x.LeadFollowUps.Where(lf => 
                                (!query.FromDate.HasValue || lf.CreatedTime.Date >= query.FromDate.Value.Date)
                                && (!query.ToDate.HasValue || lf.CreatedTime.Date <= query.ToDate.Value.Date)));
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
            //var columnsMap = new Dictionary<string, Expression<Func<LeadGeneration, object>>>()
            //{
            //    ["createdTime"] = v => v.CreatedTime,
            //};

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
            }).OrderByDescending(o => o.LeadCreatedDate).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<LeadGenerationDetailsReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = leads.Count();
            queryResult.Total = leads.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<LeadFollowUpDetailsReportResultModel>> GetLeadFollowUpDetailsReportAsync(LeadFollowUpDetailsReportSearchModel query)
        {
            //var columnsMap = new Dictionary<string, Expression<Func<LeadFollowUp, object>>>()
            //{
            //    ["createdTime"] = v => v.CreatedTime,
            //};

            var reportResult = new List<LeadFollowUpDetailsReportResultModel>();

            var leadFollowUp = await (from lf in _context.LeadFollowUps
                              join lg in _context.LeadGenerations on lf.LeadGenerationId equals lg.Id
                              join ui in _context.UserInfos on lg.UserId equals ui.Id
                              join las in _context.LeadActualVolumeSold on lf.Id equals las.LeadFollowUpId into lasleft
                              from lasInfo in lasleft.DefaultIfEmpty()
                              join bi in _context.BrandInfos on lasInfo.BrandInfoId equals bi.Id into bileft
                              from biInfo in bileft.DefaultIfEmpty()
                              join bfi in _context.BrandFamilyInfos on biInfo.MaterialGroupOrBrand equals bfi.MatarialGroupOrBrand into bfileft
                              from bfiInfo in bfileft.DefaultIfEmpty()
                              join lba in _context.LeadBusinessAchievements on lf.BusinessAchievementId equals lba.Id into lbaleft
                              from lbaInfo in lbaleft.DefaultIfEmpty()
                              join dd in _context.DropdownDetails on lf.SwappingCompetitionId equals dd.Id into ddleft
                              from ddInfo in ddleft.DefaultIfEmpty()
                              join dt in _context.DropdownDetails on lf.TypeOfClientId equals dt.Id into dtleft
                              from dtInfo in dtleft.DefaultIfEmpty()
                              join dps in _context.DropdownDetails on lf.ProjectStatusId equals dps.Id into dpsleft
                              from dpsInfo in dpsleft.DefaultIfEmpty()
                              join d in _context.Depots on lg.Depot equals d.Werks into dleftjoin
                              from dinfo in dleftjoin.DefaultIfEmpty()
                              where (
                                   (!query.UserId.HasValue || lg.UserId == query.UserId.Value)
                                  && (string.IsNullOrWhiteSpace(query.Depot) || lg.Depot == query.Depot)
                                  && (!query.Territories.Any() || query.Territories.Contains(lg.Territory))
                                  && (!query.Zones.Any() || query.Zones.Contains(lg.Zone))
                                  && (!query.FromDate.HasValue || lf.CreatedTime.Date >= query.FromDate.Value.Date)
                                  && (!query.ToDate.HasValue || lf.CreatedTime.Date <= query.ToDate.Value.Date)
                                  && (string.IsNullOrWhiteSpace(query.ProjectName) || lg.ProjectName.Contains(query.ProjectName))
                                  && (string.IsNullOrWhiteSpace(query.ProjectCode) || lg.Code.Contains(query.ProjectCode))
                                  && (!query.ProjectStatusId.HasValue || lf.ProjectStatusId == query.ProjectStatusId.Value)
                              )
                              select new
                              {
                                  projectCode = lg.Code,
                                  projectName = lg.ProjectName,
                                  depot = lg.Depot,
                                  depotName = dinfo.Name1,
                                  userId = ui.Email,
                                  territory = lg.Territory,
                                  zone = lg.Zone,
                                  planVistDate = lf.NextVisitDatePlan,
                                  actualVisitDate = lf.ActualVisitDate,
                                  typeOfClient = dtInfo.DropdownName,
                                  projectAddress = lg.ProjectAddress,
                                  keyContactPerson = lf.KeyContactPersonName,
                                  keyContactPersonMobile = lf.KeyContactPersonMobile,
                                  paintContractorName = lf.PaintContractorName,
                                  paintContractorMobile = lf.PaintContractorMobile,
                                  numberOfStoriedBuilding = lf.NumberOfStoriedBuilding,
                                  expectedValue = lf.ExpectedValue,
                                  expectedMonthlyBusinessValue = lf.ExpectedMonthlyBusinessValue,
                                  swappingCompetition = ddInfo.DropdownName,
                                  swappingCompetitionAnotherCompetitorName = lf.SwappingCompetitionAnotherCompetitorName,
                                  upTradingFromBrandName = lf.UpTradingFromBrandName,
                                  upTradingToBrandName = lf.UpTradingToBrandName,
                                  brandUsedInteriorBrandName = lf.BrandUsedInteriorBrandName,
                                  brandUsedExteriorBrandName = lf.BrandUsedExteriorBrandName,
                                  brandUsedUnderCoatBrandName = lf.BrandUsedUnderCoatBrandName,
                                  brandUsedTopCoatBrandName = lf.BrandUsedTopCoatBrandName,
                                  
                                  totalPaintingAreaSqftInterior = lf.TotalPaintingAreaSqftInterior,
                                  totalPaintingAreaSqftExterior = lf.TotalPaintingAreaSqftExterior,
                                  actualPaintJobCompletedInterior = lf.ActualPaintJobCompletedInteriorPercentage,
                                  actualPaintJobCompletedExterior = lf.ActualPaintJobCompletedExteriorPercentage,

                                  bergerValueSales = lbaInfo.BergerValueSales,
                                  bergerPremiumBrandSalesValue = lbaInfo.BergerPremiumBrandSalesValue,
                                  competitionValueSales = lbaInfo.CompetitionValueSales,
                                  projectStatus = dpsInfo.DropdownName,
                                  isColorSchemeGiven = lbaInfo.IsColorSchemeGiven,
                                  isProductSampling = lbaInfo.IsProductSampling,
                                  comments = lbaInfo.RemarksOrOutcome,
                                  nextVisitDate = lbaInfo.NextVisitDate,
                                  imageUrl = lbaInfo.PhotoCaptureUrl,

                                  materialGroupOrBrandId = biInfo.MaterialGroupOrBrand,
                                  matarialGroupOrBrandName = bfiInfo.MatarialGroupOrBrandName,
                                  materialDescription = biInfo.MaterialDescription,
                                  quantity = lasInfo.Quantity,
                                  totalAmount = lasInfo.TotalAmount,
                                  actualVolumeSoldType = lasInfo.ActualVolumeSoldType,

                                  leadfolowupId = lf.Id,
                              }).ToListAsync();

            var groupOfleadFollowUp = leadFollowUp.GroupBy(x => new { x.leadfolowupId }).Select(x => new {
                projectCode = x.FirstOrDefault().projectCode,
                projectName = x.FirstOrDefault().projectName,
                depot = x.FirstOrDefault().depot,
                depotName = x.FirstOrDefault().depotName,
                userId = x.FirstOrDefault().userId,
                territory = x.FirstOrDefault().territory,
                zone = x.FirstOrDefault().zone,
                planVistDate = x.FirstOrDefault().planVistDate,
                actualVisitDate = x.FirstOrDefault().actualVisitDate,
                typeOfClient = x.FirstOrDefault().typeOfClient,
                projectAddress = x.FirstOrDefault().projectAddress,
                keyContactPerson = x.FirstOrDefault().keyContactPerson,
                keyContactPersonMobile = x.FirstOrDefault().keyContactPersonMobile,
                paintContractorName = x.FirstOrDefault().paintContractorName,
                paintContractorMobile = x.FirstOrDefault().paintContractorMobile,
                numberOfStoriedBuilding = x.FirstOrDefault().numberOfStoriedBuilding,
                expectedValue = x.FirstOrDefault().expectedValue,
                expectedMonthlyBusinessValue = x.FirstOrDefault().expectedMonthlyBusinessValue,
                swappingCompetition = x.FirstOrDefault().swappingCompetition,
                swappingCompetitionAnotherCompetitorName = x.FirstOrDefault().swappingCompetitionAnotherCompetitorName,
                upTradingFromBrandName = x.FirstOrDefault().upTradingFromBrandName,
                upTradingToBrandName = x.FirstOrDefault().upTradingToBrandName,
                brandUsedInteriorBrandName = x.FirstOrDefault().brandUsedInteriorBrandName,
                brandUsedExteriorBrandName = x.FirstOrDefault().brandUsedExteriorBrandName,
                brandUsedUnderCoatBrandName = x.FirstOrDefault().brandUsedUnderCoatBrandName,
                brandUsedTopCoatBrandName = x.FirstOrDefault().brandUsedTopCoatBrandName,

                totalPaintingAreaSqftInterior = x.FirstOrDefault().totalPaintingAreaSqftInterior,
                totalPaintingAreaSqftExterior = x.FirstOrDefault().totalPaintingAreaSqftExterior,
                actualPaintJobCompletedInterior = x.FirstOrDefault().actualPaintJobCompletedInterior,
                actualPaintJobCompletedExterior = x.FirstOrDefault().actualPaintJobCompletedExterior,

                actualVolumeSoldInteriorLitre = x.Where(x => x.actualVolumeSoldType == EnumLeadActualVolumeSoldType.Interior && x.materialDescription.EndsWith("L")).Sum(x => x.totalAmount),
                actualVolumeSoldInteriorKg = x.Where(x => x.actualVolumeSoldType == EnumLeadActualVolumeSoldType.Interior && x.materialDescription.EndsWith("KG")).Sum(x => x.totalAmount),
                actualVolumeSoldExteriorLitre = x.Where(x => x.actualVolumeSoldType == EnumLeadActualVolumeSoldType.Exterior && x.materialDescription.EndsWith("L")).Sum(x => x.totalAmount),
                actualVolumeSoldExteriorKg = x.Where(x => x.actualVolumeSoldType == EnumLeadActualVolumeSoldType.Exterior && x.materialDescription.EndsWith("KG")).Sum(x => x.totalAmount),
                actualVolumeSoldUnderCoatGallon = x.Where(x => x.actualVolumeSoldType == EnumLeadActualVolumeSoldType.UnderCoat).Sum(x => x.totalAmount),
                actualVolumeSoldTopCoatGallon = x.Where(x => x.actualVolumeSoldType == EnumLeadActualVolumeSoldType.TopCoat).Sum(x => x.totalAmount),

                bergerValueSales = x.FirstOrDefault().bergerValueSales,
                bergerPremiumBrandSalesValue = x.FirstOrDefault().bergerPremiumBrandSalesValue,
                competitionValueSales = x.FirstOrDefault().competitionValueSales,
                projectStatus = x.FirstOrDefault().projectStatus,
                isColorSchemeGiven = x.FirstOrDefault().isColorSchemeGiven,
                isProductSampling = x.FirstOrDefault().isProductSampling,
                comments = x.FirstOrDefault().comments,
                nextVisitDate = x.FirstOrDefault().nextVisitDate,
                imageUrl = x.FirstOrDefault().imageUrl,
            }).OrderByDescending(o => o.actualVisitDate);

            reportResult = groupOfleadFollowUp.Select(x =>
            {
                var reportModel = new LeadFollowUpDetailsReportResultModel();
                reportModel.ProjectCode = x.projectCode;
                reportModel.ProjectName = x.projectName;
                reportModel.Depot = x.depot;
                reportModel.DepotName = x.depotName;
                reportModel.UserId = x.userId;
                reportModel.Territory = x.territory;
                reportModel.Zone = x.zone;
                reportModel.PlanVisitDatePlan = CustomConvertExtension.ObjectToDateString(x.planVistDate);
                reportModel.ActualVisitDate = CustomConvertExtension.ObjectToDateString(x.actualVisitDate);
                reportModel.TypeOfClient = x.typeOfClient;
                reportModel.ProjectAddress = x.projectAddress;
                reportModel.KeyContactPersonName = x.keyContactPerson;
                reportModel.KeyContactPersonMobile = x.keyContactPersonMobile;
                reportModel.PaintContractorName = x.paintContractorName;
                reportModel.PaintContractorMobile = x.paintContractorMobile;
                reportModel.NumberOfStoriedBuilding = x?.numberOfStoriedBuilding ?? (double)0;
                reportModel.ExpectedValue = x?.expectedValue ?? (decimal)0;
                reportModel.ExpectedMonthlyBusinessValue = x?.expectedMonthlyBusinessValue ?? (decimal)0;
                reportModel.SwappingCompetition = x.swappingCompetition;
                reportModel.SwappingCompetitionAnotherCompetitorName = x.swappingCompetitionAnotherCompetitorName;
                reportModel.UpTradingFromBrandName = x.upTradingFromBrandName;
                reportModel.UpTradingToBrandName = x.upTradingToBrandName;
                reportModel.BrandUsedInteriorBrandName = x.brandUsedInteriorBrandName;
                reportModel.BrandUsedExteriorBrandName = x.brandUsedExteriorBrandName;
                reportModel.BrandUsedUnderCoatBrandName = x.brandUsedUnderCoatBrandName;
                reportModel.BrandUsedTopCoatBrandName = x.brandUsedTopCoatBrandName;

                reportModel.TotalPaintingAreaSqftInterior = x?.totalPaintingAreaSqftInterior ?? (int)0;
                reportModel.TotalPaintingAreaSqftExterior = x?.totalPaintingAreaSqftExterior ?? (int)0;
                reportModel.ActualPaintJobCompletedInterior = x?.actualPaintJobCompletedInterior ?? (decimal)0;
                reportModel.ActualPaintJobCompletedExterior = x?.actualPaintJobCompletedExterior ?? (decimal)0;

                reportModel.ActualVolumeSoldInteriorLitre = x?.actualVolumeSoldInteriorLitre ?? (decimal)0;
                reportModel.ActualVolumeSoldInteriorKg = x?.actualVolumeSoldInteriorKg ?? (decimal)0;
                reportModel.ActualVolumeSoldExteriorLitre = x?.actualVolumeSoldExteriorLitre ?? (decimal)0;
                reportModel.ActualVolumeSoldExteriorKg = x?.actualVolumeSoldExteriorKg ?? (decimal)0;
                reportModel.ActualVolumeSoldUnderCoatGallon = x?.actualVolumeSoldUnderCoatGallon ?? (decimal)0;
                reportModel.ActualVolumeSoldTopCoatGallon = x?.actualVolumeSoldTopCoatGallon ?? (decimal)0;

                reportModel.BergerValueSales = x?.bergerValueSales ?? (decimal)0;
                reportModel.BergerPremiumBrandSalesValue = x?.bergerPremiumBrandSalesValue ?? (decimal)0; ;
                reportModel.CompetitionValueSales = x?.competitionValueSales ?? (decimal)0;
                reportModel.ProjectStatus = x.projectStatus;
                reportModel.IsColorSchemeGiven = x?.isColorSchemeGiven ?? false ? "YES" : "NO";
                reportModel.IsProductSampling = x?.isProductSampling ?? false ? "YES" : "NO";
                reportModel.Comments = x.comments ?? string.Empty;
                reportModel.NextVisitDate = CustomConvertExtension.ObjectToDateString(x?.nextVisitDate);
                reportModel.ImageUrl = x?.imageUrl ?? string.Empty;
                return reportModel;
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<LeadFollowUpDetailsReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = groupOfleadFollowUp.Count();
            queryResult.Total = groupOfleadFollowUp.Count();

            return queryResult;
        }

        public async Task<QueryResultModel<LeadBusinessReportResultModel>> GetLeadBusinessUpdateReportAsync(LeadBusinessReportSearchModel query)
        {
            var reportResult = new List<LeadBusinessReportResultModel>();
            var reportData = new List<LeadBusinessReportResultModel>();

            var leadBusiness = await (from lf in _context.LeadFollowUps
                                      join lg in _context.LeadGenerations on lf.LeadGenerationId equals lg.Id
                                      join lba in _context.LeadBusinessAchievements on lf.BusinessAchievementId equals lba.Id
                                      join dd in _context.DropdownDetails on lba.ProductSourcingId equals dd.Id
                                      join ui in _context.UserInfos on lg.UserId equals ui.Id
                                      join las in _context.LeadActualVolumeSold on lf.Id equals las.LeadFollowUpId into lasleft
                                      from lasInfo in lasleft.DefaultIfEmpty()
                                      join bi in _context.BrandInfos on lasInfo.BrandInfoId equals bi.Id into bileft
                                      from biInfo in bileft.DefaultIfEmpty()
                                      join bfi in _context.BrandFamilyInfos on biInfo.MaterialGroupOrBrand equals bfi.MatarialGroupOrBrand into bfileft
                                      from bfiInfo in bfileft.DefaultIfEmpty()
                                      where (
                                           (!query.UserId.HasValue || lg.UserId == query.UserId.Value)
                                           && (string.IsNullOrWhiteSpace(query.Depot) || lg.Depot == query.Depot)
                                           && (!query.Territories.Any() || query.Territories.Contains(lg.Territory))
                                           && (!query.Zones.Any() || query.Zones.Contains(lg.Zone))
                                           && (!query.FromDate.HasValue || lf.CreatedTime.Date >= query.FromDate.Value.Date)
                                           && (!query.ToDate.HasValue || lf.CreatedTime.Date <= query.ToDate.Value.Date)
                                           && (string.IsNullOrWhiteSpace(query.ProjectName) || lg.ProjectName.Contains(query.ProjectName))
                                           && (string.IsNullOrWhiteSpace(query.ProjectCode) || lg.Code.Contains(query.ProjectCode))
                                           && (!query.ProjectStatusId.HasValue || lf.ProjectStatusId == query.ProjectStatusId.Value)
                                      )
                                      select new
                                      {
                                          leadfolowupId = lf.Id,
                                          ui.Email,
                                          lg.Depot,
                                          lg.Code,
                                          lg.ProjectName,
                                          lg.ProjectAddress,
                                          lg.Territory,
                                          lg.Zone,
                                          lf.ActualVisitDate,
                                          biInfo.MaterialGroupOrBrand,
                                          bfiInfo.MatarialGroupOrBrandName,
                                          biInfo.MaterialDescription,
                                          lasInfo.Quantity,
                                          lasInfo.TotalAmount,
                                          dd.DropdownName,
                                          lba.ProductSourcingRemarks,
                                          lba.BergerValueSales
                                      }).OrderByDescending(x => x.ActualVisitDate).ToListAsync();

            var groupOfLeadBusiness = leadBusiness.GroupBy(x => new { x.leadfolowupId });

            foreach (var item in groupOfLeadBusiness)
            {
                foreach (var i in item.Where(x => !string.IsNullOrEmpty(x.MaterialGroupOrBrand)))
                {
                    reportResult.Add(new LeadBusinessReportResultModel { 
                        UserId = i.Email,
                        Depot = i.Depot,
                        ProjectCode = i.Code,
                        ProjectName = i.ProjectName,
                        Address = i.ProjectAddress,
                        Territory = i.Territory,
                        Zone = i.Zone,
                        VisitDate = CustomConvertExtension.ObjectToDateString(i.ActualVisitDate),
                        BrandName = i.MatarialGroupOrBrandName + '(' + i.MaterialGroupOrBrand + ')',
                        BrandDescription = i.MaterialDescription,
                        Quantity = i.Quantity,
                        TotalAmount = i.TotalAmount,
                        ProductSourcing = i.DropdownName,
                        DealerIdAndName = i.ProductSourcingRemarks
                    });
                }
                reportResult.Add(new LeadBusinessReportResultModel
                {
                    UserId = "Sub Total",
                    Depot = item.FirstOrDefault().Depot,
                    ProjectCode = item.FirstOrDefault().Code,
                    ProjectName = item.FirstOrDefault().ProjectName,
                    Address = item.FirstOrDefault().ProjectAddress,
                    Territory = item.FirstOrDefault().Territory,
                    Zone = item.FirstOrDefault().Zone,
                    VisitDate = CustomConvertExtension.ObjectToDateString(item.FirstOrDefault().ActualVisitDate),
                    //BrandName = item.FirstOrDefault().MatarialGroupOrBrandName + '(' + item.FirstOrDefault().MaterialGroupOrBrand + ')',
                    //BrandDescription = item.FirstOrDefault().MaterialDescription,
                    //Quantity = item.Sum(x => x.Quantity),
                    TotalAmount = item.Select(x => new { x.leadfolowupId, x.BergerValueSales }).Distinct().Sum(x => x.BergerValueSales),
                    //ProductSourcing = "",
                    //DealerIdAndName = ""
                });
            }

            // for grand total
            if (reportResult.Any())
            {
                reportResult.Add(new LeadBusinessReportResultModel
                {
                    UserId = "Grand Total",
                    TotalAmount = reportResult.Where(x => x.UserId == "Sub Total").Sum(x => x.TotalAmount),
                });
            }

            reportData = reportResult.Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

            var queryResult = new QueryResultModel<LeadBusinessReportResultModel>();
            queryResult.Items = reportData;
            queryResult.TotalFilter = reportResult.Count();
            queryResult.Total = reportResult.Count();

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
                                  //join adp in _context.AttachedDealerPainters on p.AttachedDealerCd equals adp.Id.ToString() into adpleftjoin
                                  //from adpInfo in adpleftjoin.DefaultIfEmpty()
                                  //join di in _context.DealerInfos on adpInfo.DealerId equals di.Id into dileftjoin
                                  //from diInfo in dileftjoin.DefaultIfEmpty()
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
                                     && (!query.SalesGroups.Any() || query.SalesGroups.Contains(p.SaleGroup))
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
                                      PainterId=p.Id,
                                      userInfo.Email,
                                      territoryName = tinfo.Name,
                                      zoneName = zinfo.Name,
                                      painterId = p.Id.ToString(),
                                      painterNo = p.PainterNo,
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
                                      //p.AttachedDealerId,
                                      //diInfo.CustomerName,
                                      appInstalledStatus = p.IsAppInstalled ? "Installed" : "Not Installed",
                                      p.Remark,
                                      avgMonthlyUse = p.AvgMonthlyVal.ToString(),
                                      bergerLoyalty = p.Loyality.ToString(),
                                      p.PainterImageUrl
                                  }).OrderByDescending(o => o.CreatedTime).ToListAsync();

            var attachDealers = await (from adp in _context.AttachedDealerPainters
                                       join p in _context.Painters on adp.PainterId equals p.Id into pleftjoin
                                       from pInfo in pleftjoin.DefaultIfEmpty()
                                       join di in _context.DealerInfos on adp.DealerId equals di.Id into dileftjoin
                                       from diInfo in dileftjoin.DefaultIfEmpty()
                                       join u in _context.UserInfos on pInfo.EmployeeId equals u.EmployeeId into uleftjoin
                                       from userInfo in uleftjoin.DefaultIfEmpty()
                                       join d in _context.DropdownDetails on pInfo.PainterCatId equals d.Id into dleftjoin
                                       from dropDownInfo in dleftjoin.DefaultIfEmpty()
                                       join dep in _context.Depots on pInfo.Depot equals dep.Werks into depleftjoin
                                       from depinfo in depleftjoin.DefaultIfEmpty()
                                       join sg in _context.SaleGroup on pInfo.SaleGroup equals sg.Code into sgleftjoin
                                       from sginfo in sgleftjoin.DefaultIfEmpty()
                                       join t in _context.Territory on pInfo.Territory equals t.Code into tleftjoin
                                       from tinfo in tleftjoin.DefaultIfEmpty()
                                       join z in _context.Zone on pInfo.Zone equals z.Code into zleftjoin
                                       from zinfo in zleftjoin.DefaultIfEmpty()
                                       where (
                                          (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                          && (string.IsNullOrWhiteSpace(query.Depot) || pInfo.Depot == query.Depot)
                                          && (!query.SalesGroups.Any() || query.SalesGroups.Contains(pInfo.SaleGroup))
                                          && (!query.Territories.Any() || query.Territories.Contains(pInfo.Territory))
                                          && (!query.Zones.Any() || query.Zones.Contains(pInfo.Zone))
                                          && (!query.FromDate.HasValue || pInfo.CreatedTime.Date >= query.FromDate.Value.Date)
                                          && (!query.ToDate.HasValue || pInfo.CreatedTime.Date <= query.ToDate.Value.Date)
                                          && (!query.PainterId.HasValue || pInfo.Id == query.PainterId.Value)
                                          && (!query.PainterType.HasValue || pInfo.PainterCatId == query.PainterType.Value)
                                          && (string.IsNullOrWhiteSpace(query.PainterMobileNo) || pInfo.Phone == query.PainterMobileNo)
                                       )
                                       select new
                                       {
                                           adp.PainterId,
                                           DealerId=diInfo.CustomerNo,
                                           DealerName = diInfo.CustomerName
                                       }).ToListAsync();

            reportResult = painters.Select(x => new PainterRegistrationReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                Territory = x.territoryName,
                Zone = x.zoneName,
                //PainterId = x.painterId,
                PainterId = x.painterNo,
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
                AttachedTaggedDealerId = string.Join(", ", attachDealers.Where(y=>x.PainterId==y.PainterId).Select(s => s.DealerId).Distinct()),
                AttachedTaggedDealerName = string.Join(", ", attachDealers.Where(y => x.PainterId == y.PainterId).Select(s => s.DealerName).Distinct()),
                APPInstalledStatus = x.appInstalledStatus,
                APPNotInstalledReason = x.Remark,
                AverageMonthlyUse = x.avgMonthlyUse,
                BergerLoyalty = x.bergerLoyalty,
                PainterImageUrl =x.PainterImageUrl,
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
                                   && (!query.SalesGroups.Any() || query.SalesGroups.Contains(d.SaleGroup))
                                   && (!query.Zones.Any() || query.Zones.Contains(d.Zone))
                                   && (!query.FromDate.HasValue || d.CreatedTime.Date >= query.FromDate.Value.Date)
                                   && (!query.ToDate.HasValue || d.CreatedTime.Date <= query.ToDate.Value.Date)
                                 )
                                 orderby d.CreatedTime descending
                                 select new
                                 {
                                     uinfo.Email,
                                     dealerId = d.Id.ToString(),
                                     code = d.Code,
                                     d.BusinessArea,
                                     businessAreaName = depinfo.Name1,
                                     salesOffice = sginfo.Name,
                                     saleGroupName = sginfo.Name,
                                     territoryName = tinfo.Code,
                                     zoneName = zinfo.Code,
                                     d.EmployeeId,
                                     d.DealerOpeningStatus
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
                DealershipOpeningApplicationForm = dealerAttachments.FirstOrDefault(y => (y.attachmentName ?? "").ToLower() == ConstantDealerOpeningValue.Application_Form.ToLower() && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                TradeLicensee = dealerAttachments.FirstOrDefault(y => (y.attachmentName ?? "").ToLower() == ConstantDealerOpeningValue.Trade_Licensee.ToLower() && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                IdentificationNo = dealerAttachments.FirstOrDefault(y => (y.attachmentName ?? "").ToLower() == ConstantDealerOpeningValue.NID_Passport_Birth.ToLower() && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                PhotographOfproprietor = dealerAttachments.FirstOrDefault(y => (y.attachmentName ?? "").ToLower() == ConstantDealerOpeningValue.Photograph_of_proprietor.ToLower() && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                NomineeIdentificationNo = dealerAttachments.FirstOrDefault(y => (y.attachmentName ?? "").ToLower() == ConstantDealerOpeningValue.Nominee_NID_PASSPORT_BIRTH.ToLower() && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                NomineePhotograph = dealerAttachments.FirstOrDefault(y => (y.attachmentName ?? "").ToLower() == ConstantDealerOpeningValue.Nominee_Photograph.ToLower() && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                Cheque = dealerAttachments.FirstOrDefault(y => (y.attachmentName ?? "").ToLower() == ConstantDealerOpeningValue.Cheque.ToLower() && y.dealerOpeningId == x.dealerId)?.Path ?? string.Empty,
                CurrentStatusOfThisApplication = ((DealerOpeningStatus)x.DealerOpeningStatus).ToString(),
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
                                 join dep in _context.Depots on p.Depot equals dep.Werks into depleftjoin
                                 from depinfo in depleftjoin.DefaultIfEmpty()
                                 where (
                                   dctinfo.DropdownCode == ConstantsCustomerTypeValue.DealerDropdownCode
                                   && (string.IsNullOrEmpty(query.Depot) || query.Depot == p.Depot)
                                   //&& (!query.SalesGroups.Any() || query.SalesGroups.Contains(uareaInfo.AreaId))
                                   && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                   && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                   && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                   && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                   && (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                   && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                   && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                 )
                                 orderby p.CollectionDate descending
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
                                     territory = p.Territory,
                                     zone = p.Zone,
                                     p.DealerId,
                                     dinfo.CustomerNo
                                 }).Distinct().ToListAsync();

            reportResult = dealers.Select(x => new DealerCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                DepotId = x.depotId,
                DepotName = x.depotName,
                Territory = x.territory,
                Zone = x.zone,
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.CollectionDate),
                TypeOfCustomer = x.customerType,
                DealerId = x.CustomerNo,
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
                                    join dep in _context.Depots on p.Depot equals dep.Werks into depleftjoin
                                    from depinfo in depleftjoin.DefaultIfEmpty()
                                    where (
                                      dctinfo.DropdownCode == ConstantsCustomerTypeValue.SubDealerDropdownCode
                                      && (string.IsNullOrEmpty(query.Depot) || query.Depot == p.Depot)
                                      //&& (!query.SalesGroups.Any() || query.SalesGroups.Contains(uareaInfo.AreaId))
                                      && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                      && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                      && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                      && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                      && (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                      && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                      && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                    )
                                    orderby p.CollectionDate descending
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
                                        territory = p.Territory,
                                        zone = p.Zone,
                                        p.DealerId,
                                        dinfo.CustomerNo
                                    }).Distinct().ToListAsync();

            reportResult = subDealers.Select(x => new SubDealerCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                DepotId = x.depotId,
                DepotName = x.depotName,
                Territory = x.territory,
                Zone = x.zone,
                CollectionDate = CustomConvertExtension.ObjectToDateString(x.CollectionDate),
                TypeOfCustomer = x.customerType,
                SubDealerId = x.CustomerNo,
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
                                   join dep in _context.Depots on p.Depot equals dep.Werks into depleftjoin
                                   from depinfo in depleftjoin.DefaultIfEmpty()
                                   where (
                                     dctinfo.DropdownCode == ConstantsCustomerTypeValue.CustomerDropdownCode
                                      && (string.IsNullOrEmpty(query.Depot) || query.Depot == p.Depot)
                                      //&& (!query.SalesGroups.Any() || query.SalesGroups.Contains(uareaInfo.AreaId))
                                      && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                      && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                     && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                     && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                     //&& (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                     && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                     && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                   )
                                   //orderby p.CollectionDate descending
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
                                       territory = p.Territory,
                                       zone = p.Zone
                                   }).Distinct().OrderByDescending(o => o.CollectionDate).ToListAsync();

            reportResult = customers.Select(x => new CustomerCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                DepotId = x.depotId,
                DepotName = x.depotName,
                Territory = x.territory,
                Zone = x.zone,
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
                                        join dep in _context.Depots on p.Depot equals dep.Werks into depleftjoin
                                        from depinfo in depleftjoin.DefaultIfEmpty()
                                        where (
                                          dctinfo.DropdownCode == ConstantsCustomerTypeValue.DirectProjectDropdownCode
                                          && (string.IsNullOrEmpty(query.Depot) || query.Depot == p.Depot)
                                          //&& (!query.SalesGroups.Any() || query.SalesGroups.Contains(uareaInfo.AreaId))
                                          && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                          && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                          && (!query.PaymentMethodId.HasValue || p.PaymentMethodId == query.PaymentMethodId.Value)
                                          && (!query.UserId.HasValue || uinfo.Id == query.UserId.Value)
                                          //&& (!query.DealerId.HasValue || p.DealerId == query.DealerId.Value.ToString())
                                          && (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                          && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                        )
                                        orderby p.CollectionDate descending
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
                                            depotId = depinfo.Werks,
                                            depotName = depinfo.Name1,
                                            territory = p.Territory,
                                            zone = p.Zone
                                        }).Distinct().ToListAsync();

            reportResult = directProjects.Select(x => new DirectProjectCollectionReportResultModel
            {
                UserId = x?.Email ?? string.Empty,
                DepotId = x.depotId,
                DepotName = x.depotName,
                Territory = x.territory,
                Zone = x.zone,
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
                                       //join adp in _context.AttachedDealerPainters on pinfo.AttachedDealerCd equals adp.Id.ToString() into adpleftjoin
                                       //from adpInfo in adpleftjoin.DefaultIfEmpty()
                                       //join di in _context.DealerInfos on adpInfo.DealerId equals di.Id into dileftjoin
                                       //from diInfo in dileftjoin.DefaultIfEmpty()
                                       where (
                                       (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                       && (string.IsNullOrWhiteSpace(query.Depot) || pinfo.Depot == query.Depot)
                                       && (!query.SalesGroups.Any() || query.SalesGroups.Contains(pcinfo.SaleGroup))
                                       && (!query.Territories.Any() || query.Territories.Contains(pcinfo.Territory))
                                       && (!query.Zones.Any() || query.Zones.Contains(pcinfo.Zone))
                                       && (!query.FromDate.HasValue || pcinfo.CreatedTime.Date >= query.FromDate.Value.Date)
                                       && (!query.ToDate.HasValue || pcinfo.CreatedTime.Date <= query.ToDate.Value.Date)
                                       && (!query.PainterId.HasValue || pinfo.Id == query.PainterId.Value)
                                       && (!query.PainterType.HasValue || pinfo.PainterCatId == query.PainterType.Value)
                                   )
                                   orderby pcinfo.CreatedTime descending
                                       select new
                                       {
                                           PainterCallId=pcinfo.Id,
                                           userInfo.Email,
                                           painterId = pcinfo.PainterId.ToString(),
                                           painterNo = pinfo.PainterNo,
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
                                           //pinfo.AttachedDealerCd,
                                           //diInfo.CustomerName,
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

            var attachDealers = await (from adp in _context.AttachedDealerPainterCalls
                                       join pc in _context.PainterCalls on adp.PainterCallId equals pc.Id into pcleftjoin
                                       from pcinfo in pcleftjoin.DefaultIfEmpty()
                                       join p in _context.Painters on pcinfo.PainterId equals p.Id into pleftjoin
                                       from pInfo in pleftjoin.DefaultIfEmpty()
                                       join di in _context.DealerInfos on adp.DealerId equals di.Id into dileftjoin
                                       from diInfo in dileftjoin.DefaultIfEmpty()
                                       join u in _context.UserInfos on pInfo.EmployeeId equals u.EmployeeId into uleftjoin
                                       from userInfo in uleftjoin.DefaultIfEmpty()
                                       join d in _context.DropdownDetails on pInfo.PainterCatId equals d.Id into dleftjoin
                                       from dropDownInfo in dleftjoin.DefaultIfEmpty()
                                       join dep in _context.Depots on pInfo.Depot equals dep.Werks into depleftjoin
                                       from depinfo in depleftjoin.DefaultIfEmpty()
                                       join sg in _context.SaleGroup on pInfo.SaleGroup equals sg.Code into sgleftjoin
                                       from sginfo in sgleftjoin.DefaultIfEmpty()
                                       join t in _context.Territory on pInfo.Territory equals t.Code into tleftjoin
                                       from tinfo in tleftjoin.DefaultIfEmpty()
                                       join z in _context.Zone on pInfo.Zone equals z.Code into zleftjoin
                                       from zinfo in zleftjoin.DefaultIfEmpty()
                                       where (
                                          (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                           && (string.IsNullOrWhiteSpace(query.Depot) || pInfo.Depot == query.Depot)
                                           && (!query.SalesGroups.Any() || query.SalesGroups.Contains(pcinfo.SaleGroup))
                                           && (!query.Territories.Any() || query.Territories.Contains(pcinfo.Territory))
                                           && (!query.Zones.Any() || query.Zones.Contains(pcinfo.Zone))
                                           && (!query.FromDate.HasValue || pcinfo.CreatedTime.Date >= query.FromDate.Value.Date)
                                           && (!query.ToDate.HasValue || pcinfo.CreatedTime.Date <= query.ToDate.Value.Date)
                                           && (!query.PainterId.HasValue || pInfo.Id == query.PainterId.Value)
                                           && (!query.PainterType.HasValue || pInfo.PainterCatId == query.PainterType.Value)
                                       )
                                       select new
                                       {
                                           adp.PainterCallId,
                                           DealerId = diInfo.CustomerNo,
                                           DealerName = diInfo.CustomerName
                                       }).ToListAsync();

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
                //PainterId = x?.painterId ?? string.Empty,
                PainterId = x?.painterNo ?? string.Empty,
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
                AttachedTaggedDealerId = string.Join(", ", attachDealers.Where(y => x.PainterCallId == y.PainterCallId).Select(s => s.DealerId).Distinct()),
                AttachedTaggedDealerName = string.Join(", ", attachDealers.Where(y => x.PainterCallId == y.PainterCallId).Select(s => s.DealerName).Distinct()),
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
                                       && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                       && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                       && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                       && (!query.DealerId.HasValue || jpd.DealerId == query.DealerId.Value)
                                     )
                                     select new DealerVisitReport
                                     {
                                         EmployeeId = jpminfo.EmployeeId,
                                         DealerId = jpd.DealerId,
                                         Email = userInfo.Email,
                                         BusinessArea = diInfo.BusinessArea,
                                         depot = depinfo.Name1,
                                         territoryName = tinfo.Code,
                                         zoneName = zinfo.Code,
                                         CustomerNo=diInfo.CustomerNo,
                                         CustomerName=diInfo.CustomerName,
                                         Date=jpminfo.PlanDate,
                                         JourneyPlanId = dscinfo.JourneyPlanId,
                                         IsAdhocVisit = false
                                     }).Distinct().ToListAsync();

            var adHocDealerCalls = await (from dsc in _context.DealerSalesCalls
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
                                        (dsc.CreatedTime.Month == month && dsc.CreatedTime.Year == year)
                                       && (dsc.JourneyPlanId == null)
                                       && (!query.UserId.HasValue || dsc.UserId == query.UserId.Value)
                                       && (string.IsNullOrWhiteSpace(query.Depot) || diInfo.BusinessArea == query.Depot)
                                       && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                       && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                       && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                       && (!query.DealerId.HasValue || dsc.DealerId == query.DealerId.Value)
                                     )
                                     select new DealerVisitReport
                                     {
                                         EmployeeId = userInfo.EmployeeId,
                                         DealerId = dsc.DealerId,
                                         Email = userInfo.Email,
                                         BusinessArea = diInfo.BusinessArea,
                                         depot = depinfo.Name1,
                                         territoryName = tinfo.Code,
                                         zoneName = zinfo.Code,
                                         CustomerNo = diInfo.CustomerNo,
                                         CustomerName = diInfo.CustomerName,
                                         Date = dsc.CreatedTime,
                                         JourneyPlanId = dsc.JourneyPlanId,
                                         IsAdhocVisit = true
                                     }).ToListAsync();

            var allVisitData = new List<DealerVisitReport>();
            if (dealerVisit.Any()) allVisitData.AddRange(dealerVisit);
            if (adHocDealerCalls.Any()) allVisitData.AddRange(adHocDealerCalls);
            var dayNumber = 1;

            var allVisitDataGroup = allVisitData.GroupBy(x => new { x.EmployeeId, x.DealerId }).ToList();

            var dealerVisitGroup = new List<DealerVisitReportResultModel>();

            foreach (var x in allVisitDataGroup)
            {
                dayNumber = 1;
                var res = new DealerVisitReportResultModel();

                res.UserId = x.FirstOrDefault()?.Email;
                res.DepotId = x.FirstOrDefault()?.BusinessArea;
                res.DepotName = x.FirstOrDefault()?.depot ?? string.Empty;
                res.Territory = x.FirstOrDefault()?.territoryName;
                res.Zone = x.FirstOrDefault()?.zoneName;
                res.DealerId = x.FirstOrDefault()?.CustomerNo ?? string.Empty;
                res.DealerName = x.FirstOrDefault()?.CustomerName;

                res.D1 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D2 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D3 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D4 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D5 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D6 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D7 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D8 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D9 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D10 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D11 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D12 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D13 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D14 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D15 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D16 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D17 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D18 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D19 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D20 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D21 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D22 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D23 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D24 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D25 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D26 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D27 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D28 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D29 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D30 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;
                res.D31 = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && c?.Date.Day == dayNumber) > 0 ? "Visited" :
                        x.Count(c => (!c.IsAdhocVisit && c?.JourneyPlanId == null) && c?.Date.Day == dayNumber) > 0 ? "Not Visited" : "";
                dayNumber++;

                res.TargetVisits = tvist = x.Count(c => (c?.Date.Month == month && c?.Date.Year == year));
                res.ActualVisits = avisit = x.Count(c => ((!c.IsAdhocVisit && c?.JourneyPlanId != null) || c.IsAdhocVisit) && (c?.Date.Month == month && c?.Date.Year == year));
                res.NotVisits = (tvist - avisit);

                dealerVisitGroup.Add(res);
            }

            reportResult = dealerVisitGroup.Skip(this.SkipCount(query)).Take(query.PageSize).ToList();

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
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.DealerId.HasValue || dsc.DealerId == query.DealerId.Value)
                                     )
                                     orderby jpm.PlanDate descending
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
                                         jpm.PlanDate,
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
                VisitDate = CustomConvertExtension.ObjectToDateString(x.PlanDate),
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
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.SubDealerId.HasValue || dsc.DealerId == query.SubDealerId.Value)
                                     )
                                     orderby jpm.PlanDate descending
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
                                         jpm.PlanDate,
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
                VisitDate = CustomConvertExtension.ObjectToDateString(x.PlanDate),
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
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.DealerId.HasValue || dsc.DealerId == query.DealerId.Value)
                                     )
                                     orderby dsc.CreatedTime descending
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
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.SubDealerId.HasValue || dsc.DealerId == query.SubDealerId.Value)
                                     )
                                     orderby dsc.CreatedTime descending
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
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (!query.DealerId.HasValue || dscInfo.DealerId == query.DealerId.Value)
                                     )
                                     select new
                                     {
                                         userInfo.Email,
                                         diInfo.BusinessArea,
                                         depotId = depInfo.Werks,
                                         depot = depInfo.Name1,
                                         territory = tInfo.Code,
                                         zone = zInfo.Code,
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
                                         issueCategory = dscddInfo.DropdownCode,
                                         dsi.DealerSalesCallId
                                     }).ToListAsync();

            var groupdealerIssue = dealerIssue.GroupBy(x => x.DealerSalesCallId).Select(x => new
            {
                dealerSalesCallId = x.Key,
                userId = x.FirstOrDefault()?.Email,
                depotId = x.FirstOrDefault()?.depotId,
                depotName = x.FirstOrDefault()?.depot,
                territoryName = x.FirstOrDefault()?.territory,
                zoneName = x.FirstOrDefault()?.zone,
                dealerId = x.FirstOrDefault()?.CustomerNo,
                dealerName = x.FirstOrDefault()?.CustomerName,
                visitDate = x.FirstOrDefault()?.CreatedTime,
                pcMaterial = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaintDropdownCode)?.MaterialName,
                pcMaterialGroup = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaintDropdownCode)?.MaterialGroup,
                pcQuantity = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaintDropdownCode)?.Quantity,
                pcBatchNumber = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaintDropdownCode)?.BatchNumber,
                pcComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaintDropdownCode)?.Comments,
                pcPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ProductComplaintDropdownCode)?.priority,
                posComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShortDropdownCode)?.Comments,
                posPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShortDropdownCode)?.priority,
                shadeComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCardDropdownCode)?.Comments,
                shadePriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCardDropdownCode)?.priority,
                shopsignComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplainDropdownCode)?.Comments,
                shopsignPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplainDropdownCode)?.priority,
                deliveryComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssueDropdownCode)?.Comments,
                deliveryPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssueDropdownCode)?.priority,
                damageMaterial = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProductDropdownCode)?.MaterialName,
                damageMaterialGroup = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProductDropdownCode)?.MaterialGroup,
                damageQuantity = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProductDropdownCode)?.Quantity,
                damageComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProductDropdownCode)?.Comments,
                damagePriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DamageProductDropdownCode)?.priority,
                cbmStatus = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.HasCBMachineMantainance ?? false,
                cbmMaintatinanceFrequency = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.maintinaceFrequency,
                cbmRemarks = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.CBMachineMantainanceRegularReason,
                cbmPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.priority,
                othersComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.OthersDropdownCode)?.Comments,
                othersPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.OthersDropdownCode)?.priority
            }).OrderByDescending(o => o.visitDate).ToList();

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
                                           && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                           && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                           && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                           && (!query.SubDealerId.HasValue || dscInfo.DealerId == query.SubDealerId.Value)
                                        )
                                        select new
                                        {
                                            userInfo.Email,
                                            diInfo.BusinessArea,
                                            depotId = depInfo.Werks,
                                            depot = depInfo.Name1,
                                            territory = tInfo.Code,
                                            zone = zInfo.Code,
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
                                            issueCategory = dscddInfo.DropdownCode,
                                            dsi.DealerSalesCallId
                                        }).ToListAsync();

            var groupSubDealerIssue = subDealerIssue.GroupBy(x => x.DealerSalesCallId).Select(x => new
            {
                dealerSalesCallId = x.Key,
                userId = x.FirstOrDefault()?.Email,
                depotId = x.FirstOrDefault()?.depotId,
                depotName = x.FirstOrDefault()?.depot,
                territoryName = x.FirstOrDefault()?.territory,
                zoneName = x.FirstOrDefault()?.zone,
                dealerId = x.FirstOrDefault()?.CustomerNo,
                dealerName = x.FirstOrDefault()?.CustomerName,
                visitDate = x.FirstOrDefault()?.CreatedTime,
                posComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShortDropdownCode)?.Comments,
                posPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.POSMaterialShortDropdownCode)?.priority,
                shadeComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCardDropdownCode)?.Comments,
                shadePriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShadeCardDropdownCode)?.priority,
                shopsignComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplainDropdownCode)?.Comments,
                shopsignPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.ShopSignComplainDropdownCode)?.priority,
                deliveryComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssueDropdownCode)?.Comments,
                deliveryPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.DeliveryIssueDropdownCode)?.priority,
                cbmStatus = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.HasCBMachineMantainance ?? false,
                cbmMaintatinanceFrequency = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.maintinaceFrequency,
                cbmRemarks = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.CBMachineMantainanceRegularReason,
                cbmPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.CBMachineMantainanceDropdownCode)?.priority,
                othersComments = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.OthersDropdownCode)?.Comments,
                othersPriority = x.FirstOrDefault(y => y.issueCategory == ConstantIssuesValue.OthersDropdownCode)?.priority
            }).OrderByDescending(o => o.visitDate).ToList();

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

        public async Task<QueryResultModel<ActiveSummaryReportResultModel>> GetActiveSummeryReportIssueAsync(ActiveSummeryReportSearchModel query)
        {
            UserInfo userinfo = new UserInfo();

            string territory = string.Join(",", query.Territories);//    query.Territories.Count > 0 ? query.Territories[0] : string.Empty;
            string zone = string.Join(",", query.Zones);//    query.Territories.Count > 0 ? query.Territories[0] : string.Empty;
            //string zone = query.Zones.Count > 0 ? query.Zones[0] : string.Empty;
            query.ActivitySummary = string.IsNullOrWhiteSpace(query.ActivitySummary) ? "" : query.ActivitySummary;
            //IList<int> userDealerIds = await _service.GetDealerByUserId(AppIdentity.AppUser.UserId);
            //  var userDealerIds = new List<string>();

            //if (query.UserId.HasValue)
            //    userDealerIds = (await _service.GetDealerByUserId(query.UserId.Value)).ToList();

            //var dbResult = await _dealerInfoRepository.FindByCondition(x =>
            //    (!query.Territories.Any() || query.Territories.Contains(x.Territory))
            //    && (!query.SalesGroups.Any() || query.SalesGroups.Contains(x.SalesGroup))
            //    && (!query.Zones.Any() || query.Zones.Contains(x.CustZone))
            //    && (string.IsNullOrWhiteSpace(query.Depot) || query.Depot == x.BusinessArea)
            //    && (!query.UserId.HasValue || userDealerIds.Contains(x.CustomerNo))
            //).Select(x => new
            //{
            //    //x.Territory,
            //    x.CustomerNo,
            //    //x.CustZone,
            //    //x.CreditControlArea,
            //    x.CustomerName,
            //}).ToListAsync();

            // var dealerIds = dbResult.Select(x => x.CustomerNo).Distinct().ToList();

            if (query.UserId.HasValue)
            {
                userinfo = _context.UserInfos.FirstOrDefault(p => p.Id == query.UserId);

            }

            var data = _context.JourneyPlanMasters.Join(_context.JourneyPlanDetails, jpm => jpm.Id, jpd => jpd.PlanId, (JourneyPlanMaster, JourneyPlanDetail) => new { JourneyPlanMaster, JourneyPlanDetail })
                        .Join(_context.DealerSalesCalls.Include(p => p.Dealer), jpm => jpm.JourneyPlanMaster.Id, dsc => dsc.JourneyPlanId, (JourneyPlanMaster, DealerSalesCall) => new { JourneyPlanMaster, DealerSalesCall })
                        .Where(p =>
                            p.JourneyPlanMaster.JourneyPlanMaster.PlanStatus == PlanStatus.Approved &&
                           (!query.UserId.HasValue || userinfo.EmployeeId == p.JourneyPlanMaster.JourneyPlanMaster.EmployeeId)

                           && (!query.FromDate.HasValue || p.JourneyPlanMaster.JourneyPlanMaster.CreatedTime >= query.FromDate)
                           && (!query.ToDate.HasValue || p.JourneyPlanMaster.JourneyPlanMaster.CreatedTime <= query.ToDate)

                           && (query.Zones.Count == 0 || query.Zones.Contains(p.DealerSalesCall.Dealer.CustZone))
                           && (query.Territories.Count == 0 || query.Territories.Contains(p.DealerSalesCall.Dealer.Territory))

                           && (p.DealerSalesCall.Dealer.BusinessArea == query.Depot || string.IsNullOrWhiteSpace(query.Depot))
                           && (!query.SalesGroups.Any() || query.SalesGroups.Contains(p.DealerSalesCall.Dealer.SalesGroup))

                            )
                        .Select(p => new
                        {
                            JourneyPlanMaster = p.JourneyPlanMaster.JourneyPlanMaster,
                            JourneyPlanDetail = p.JourneyPlanMaster.JourneyPlanMaster.JourneyPlanDetail,
                            DealerSalesCall = p.DealerSalesCall
                        }).ToList();


            var dealerSalesCall = await _context.DealerSalesCalls.Join(_context.DealerInfos, p => p.DealerId, di => di.Id, (dealerSalesCall, dealerInfo) => new { dealerSalesCall, dealerInfo })
                .Where(p => p.dealerSalesCall.JourneyPlanId == null
                            && (!query.FromDate.HasValue || p.dealerSalesCall.CreatedTime.Date >= query.FromDate.Value.Date)
                            && (!query.ToDate.HasValue || p.dealerSalesCall.CreatedTime.Date <= query.ToDate.Value.Date)
                            && (query.Zones.Count == 0 || query.Zones.Contains(p.dealerInfo.CustZone))
                            && (query.Territories.Count == 0 || query.Territories.Contains(p.dealerInfo.Territory))
                            && (query.SalesGroups.Count == 0 || query.SalesGroups.Contains(p.dealerInfo.SalesGroup))
                            && (string.IsNullOrWhiteSpace(query.Depot) || query.Depot == p.dealerInfo.BusinessArea)
                            && (!query.UserId.HasValue || p.dealerSalesCall.UserId == query.UserId.Value)
                            ).Select(x => x.dealerSalesCall).ToListAsync();

            var painter = _context.Painters.Join(_context.PainterCalls, p => p.Id, pc => pc.PainterId, (painter, PainterCall) => new { Painter = painter, PainterCall })
                                .Where(p => (!query.FromDate.HasValue || p.PainterCall.CreatedTime.Date >= query.FromDate.Value.Date)
                                            && (!query.ToDate.HasValue || p.PainterCall.CreatedTime.Date <= query.ToDate.Value.Date)
                                            && (!query.FromDate.HasValue || p.Painter.CreatedTime.Date >= query.FromDate.Value.Date)
                                            && (!query.ToDate.HasValue || p.Painter.CreatedTime.Date <= query.ToDate.Value.Date)
                                            && (query.Zones.Count == 0 || query.Zones.Contains(p.Painter.Zone))
                                            && (query.Territories.Count == 0 || query.Territories.Contains(p.Painter.Territory))
                                            && (query.SalesGroups.Count == 0 || query.SalesGroups.Contains(p.Painter.SaleGroup))
                                            && (string.IsNullOrWhiteSpace(query.Depot) || query.Depot == p.Painter.Depot)
                                            && (!query.UserId.HasValue || p.Painter.EmployeeId == userinfo.EmployeeId)
                                            )
                                            .Select(p => new
                                            {
                                                PainterRegistration = p.Painter,
                                                PainterCalls = p.PainterCall
                                            }).ToList();

            var lead = _context.LeadGenerations.Join(_context.LeadFollowUps, lg => lg.Id, lf => lf.LeadGenerationId, (leadGeneration, leadFollowUp) => new { LeadGeneration = leadGeneration, LeadFollowUp = leadFollowUp })
                .Where(p => (!query.FromDate.HasValue || p.LeadGeneration.CreatedTime.Date >= query.FromDate.Value.Date)
                            && (!query.ToDate.HasValue || p.LeadGeneration.CreatedTime.Date <= query.ToDate.Value.Date)
                            && (!query.FromDate.HasValue || p.LeadFollowUp.CreatedTime.Date >= query.FromDate.Value.Date)
                            && (!query.ToDate.HasValue || p.LeadFollowUp.CreatedTime.Date <= query.ToDate.Value.Date)
                            && (query.Zones.Count == 0 || query.Zones.Contains(p.LeadGeneration.Zone))
                            && (query.Territories.Count == 0 || query.Territories.Contains(p.LeadGeneration.Territory))
                            && (p.LeadGeneration.Depot == query.Depot || string.IsNullOrWhiteSpace(query.Depot))
                            && (!query.UserId.HasValue || p.LeadGeneration.UserId == query.UserId.Value)
                )
                            .Select(p => new
                            {
                                LeadGeneration = p.LeadGeneration,
                                LeadFollowUp = p.LeadFollowUp
                            }).ToList();



            var collection = await (from p in _context.Payments
                                    join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                    from dctinfo in dctleftjoin.DefaultIfEmpty()
                                    join ui in _context.UserInfos on p.EmployeeId equals ui.EmployeeId into uileftjoin
                                    from uiInfo in uileftjoin.DefaultIfEmpty()
                                    join uarea in _context.UserZoneAreaMappings on uiInfo.Id equals uarea.UserInfoId into uarealeftjoin
                                    from uareaInfo in uarealeftjoin.DefaultIfEmpty()
                                    where (
                                        (!query.ToDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(uareaInfo.AreaId))
                                        && (!query.Territories.Any() || query.Territories.Contains(uareaInfo.TerritoryId))
                                        && (!query.Zones.Any() || query.Zones.Contains(uareaInfo.ZoneId))
                                        //&& (string.IsNullOrWhiteSpace(query.Depot) || query.Depot == uareaInfo.PlantId)
                                        && (string.IsNullOrWhiteSpace(query.Depot) || query.Depot == uareaInfo.PlantId)
                                        && (!query.UserId.HasValue || p.EmployeeId == userinfo.EmployeeId)
                                    )
                                    select new
                                    {
                                        CollectionId = p.Id,
                                        p.Amount
                                    }).Distinct().SumAsync(x => x.Amount);


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

                    Activity="LEAD GENERATION",
                    Target="N/A",
                    Actual =lead.Select(p=>p.LeadGeneration.Id).Distinct().Count(x=>x>0).ToString(),
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
                    Activity="LEAD FOLLOWUP",
                    Target="N/A",
                    Actual =lead.Select(p=>p.LeadFollowUp.Id).Distinct().Count(x=>x>0).ToString(),
                    Variance="N/A",
                    BusinessGeneration=lead.Sum(x=>x.LeadFollowUp?.BusinessAchievement?.BergerValueSales??0).ToString(),
                    //UserID=data.Select(x=>x.UserEmail).FirstOrDefault()
                    UserID=query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID=query.Depot,
                    Territory=territory,
                    Zone=zone

                },
                new ActiveSummaryReportResultModel
                {
                    Activity="TOTAL COLLECTION VALUE",
                    Target="N/A",
                    Actual =  (string.IsNullOrWhiteSpace(query.ActivitySummary) || query.ActivitySummary.ToLower()=="TOTAL COLLECTION VALUE".ToLower())?
                        collection.ToString():"0",
                    //Actual ="0",
                    Variance="N/A",
                    BusinessGeneration="N/A",
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

        public async Task<QueryResultModel<ActiveSummaryReportResultModel>> GetActiveSummeryReportAsync(ActiveSummeryReportSearchModel query)
        {
            UserInfo userinfo = new UserInfo();

            string territory = string.Join(",", query.Territories);
            string zone = string.Join(",", query.Zones);

            query.ActivitySummary = string.IsNullOrWhiteSpace(query.ActivitySummary) ? "" : query.ActivitySummary;

            if (query.UserId.HasValue)
            {
                userinfo = _context.UserInfos.FirstOrDefault(p => p.Id == query.UserId);
            }

            var dealerVisit = await (from jpd in _context.JourneyPlanDetails
                                     join jpm in _context.JourneyPlanMasters on jpd.PlanId equals jpm.Id into jpmleftjoin
                                     from jpminfo in jpmleftjoin.DefaultIfEmpty()
                                     join dsc in _context.DealerSalesCalls on jpd.PlanId equals dsc.JourneyPlanId into dscleftjoin
                                     from dscinfo in dscleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on jpd.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     join cg in _context.CustomerGroups on diInfo.AccountGroup equals cg.CustomerAccountGroup into cgleftjoin
                                     from cgInfo in cgleftjoin.DefaultIfEmpty()
                                     where (
                                         (!query.FromDate.HasValue || jpminfo.PlanDate.Date >= query.FromDate.Value.Date)
                                         && (!query.ToDate.HasValue || jpminfo.PlanDate.Date <= query.ToDate.Value.Date)
                                         && (jpminfo.PlanStatus == PlanStatus.Approved)
                                         && (string.IsNullOrEmpty(query.Depot) || query.Depot == diInfo.BusinessArea)
                                         && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                         && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                         && (!query.UserId.HasValue || userinfo.EmployeeId == jpminfo.EmployeeId)
                                     )
                                     select new
                                     {
                                         JourneyPlanDetailId = jpd.Id,
                                         IsSubDealer = cgInfo.Description.StartsWith("Subdealer"),
                                         DealerId = jpd.DealerId,
                                         IsVisited = dscinfo.Id > 0
                                     }).Distinct().ToListAsync();

            var adHocDealerVisit = await (from dsc in _context.DealerSalesCalls
                                          join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                          from diInfo in dileftjoin.DefaultIfEmpty()
                                          where (
                                              (!query.FromDate.HasValue || dsc.CreatedTime.Date >= query.FromDate.Value.Date)
                                              && (!query.ToDate.HasValue || dsc.CreatedTime.Date <= query.ToDate.Value.Date)
                                              && (dsc.JourneyPlanId == null)
                                                && (string.IsNullOrEmpty(query.Depot) || query.Depot == diInfo.BusinessArea)
                                              && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                              && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                                && (!query.UserId.HasValue || query.UserId.Value == dsc.UserId)
                                          )
                                          select new
                                          {
                                              DealerSalerCallId = dsc.Id,
                                              IsSubDealer = dsc.IsSubDealerCall,
                                              DealerId = dsc.DealerId
                                          }).Distinct().ToListAsync();

            var painterReg = await (from p in _context.Painters
                                     where (
                                        (!query.FromDate.HasValue || p.CreatedTime.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || p.CreatedTime.Date <= query.ToDate.Value.Date)
                                        && (string.IsNullOrEmpty(query.Depot) || query.Depot == p.Depot)
                                        && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                        && (!query.UserId.HasValue || userinfo.EmployeeId == p.EmployeeId)
                                    )
                                     select new
                                     {
                                         PainterRegId = p.Id
                                     }).Distinct().ToListAsync();

            var painterCall = await (from pc in _context.PainterCalls
                                     join p in _context.Painters on pc.PainterId equals p.Id into pleftjoin
                                     from pInfo in pleftjoin.DefaultIfEmpty()
                                     where (
                                        (!query.FromDate.HasValue || pc.CreatedTime.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || pc.CreatedTime.Date <= query.ToDate.Value.Date)
                                        && (string.IsNullOrEmpty(query.Depot) || query.Depot == pInfo.Depot)
                                        && (!query.Territories.Any() || query.Territories.Contains(pc.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(pc.Zone))
                                        && (!query.UserId.HasValue || userinfo.EmployeeId == pc.EmployeeId)
                                    )
                                     select new
                                     {
                                         PainterCallId = pc.Id
                                     }).Distinct().ToListAsync();

            var collection = await (from p in _context.Payments
                                    where (
                                        (!query.FromDate.HasValue || p.CollectionDate.Date >= query.FromDate.Value.Date)
                                        && (!query.ToDate.HasValue || p.CollectionDate.Date <= query.ToDate.Value.Date)
                                        && (string.IsNullOrEmpty(query.Depot) || query.Depot == p.Depot)
                                        && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                        && (!query.UserId.HasValue || userinfo.EmployeeId == p.EmployeeId)
                                    )
                                    select new
                                    {
                                        CollectionId = p.Id,
                                        Amount = p.Amount
                                    }).Distinct().ToListAsync();

            var leadGen = await (from lg in _context.LeadGenerations
                              where (
                                  (!query.FromDate.HasValue || lg.CreatedTime.Date >= query.FromDate.Value.Date)
                                  && (!query.ToDate.HasValue || lg.CreatedTime.Date <= query.ToDate.Value.Date)
                                    && (string.IsNullOrEmpty(query.Depot) || query.Depot == lg.Depot)
                                  && (!query.Territories.Any() || query.Territories.Contains(lg.Territory))
                                  && (!query.Zones.Any() || query.Zones.Contains(lg.Zone))
                                    && (!query.UserId.HasValue || query.UserId.Value == lg.UserId)
                              )
                              select new
                              {
                                  LeadGenerationId = lg.Id
                              }).Distinct().ToListAsync();

            var leadFollowup = await (from lf in _context.LeadFollowUps
                              join lg in _context.LeadGenerations on lf.LeadGenerationId equals lg.Id into lgleftjoin
                              from lgInfo in lgleftjoin.DefaultIfEmpty()
                              join lba in _context.LeadBusinessAchievements on lf.BusinessAchievementId equals lba.Id into lbaleftjoin
                              from lbaInfo in lbaleftjoin.DefaultIfEmpty()
                              where (
                                  (!query.FromDate.HasValue || lf.CreatedTime.Date >= query.FromDate.Value.Date)
                                  && (!query.ToDate.HasValue || lf.CreatedTime.Date <= query.ToDate.Value.Date)
                                    && (string.IsNullOrEmpty(query.Depot) || query.Depot == lgInfo.Depot)
                                  && (!query.Territories.Any() || query.Territories.Contains(lgInfo.Territory))
                                  && (!query.Zones.Any() || query.Zones.Contains(lgInfo.Zone))
                                    && (!query.UserId.HasValue || query.UserId.Value == lgInfo.UserId)
                              )
                              select new
                              {
                                  LeadFollowUpId = lf.Id,
                                  DGABusinessValue = lbaInfo.BergerValueSales
                              }).Distinct().ToListAsync();

            var target = 0;
            var actual = 0;

            var reportResult = new List<ActiveSummaryReportResultModel>()
            {
                new ActiveSummaryReportResultModel
                {
                    Activity = "JOURNEY PLAN",
                    Target = (target = dealerVisit.Select(x => new {x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Actual = (actual = dealerVisit.Where(x => x.IsVisited).Select(x => new {x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Variance = (target - actual).ToString(),
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "SALES CALL- DIRECT DEALER",
                    Target = (target = dealerVisit.Where(x => !x.IsSubDealer).Select(x => new {x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Actual = (actual = dealerVisit.Where(x => !x.IsSubDealer && x.IsVisited).Select(x => new {x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Variance = (target - actual).ToString(),
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "AD HOC VISIT IN DEALERS POINT",
                    Target = "N/A",
                    Actual = (adHocDealerVisit.Where(x => !x.IsSubDealer).Select(x => new {x.DealerSalerCallId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Variance = "N/A",
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },

                new ActiveSummaryReportResultModel
                {
                    Activity = "SALES CALL- SUB DEALER",
                    Target = (target = dealerVisit.Where(x => x.IsSubDealer).Select(x => new {x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Actual = (actual = dealerVisit.Where(x => x.IsSubDealer && x.IsVisited).Select(x => new {x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Variance = (target - actual).ToString(),
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "AD HOC VISIT IN SUB-DEALERS POINT",
                    Target = "N/A",
                    Actual = (adHocDealerVisit.Where(x => x.IsSubDealer).Select(x => new {x.DealerSalerCallId, x.DealerId }).Distinct().Count(x => x.DealerId > 0)).ToString(),
                    Variance = "N/A",
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "PAINTER REGISTRATION",
                    Target = "N/A",
                    Actual = (painterReg.Select(x => x.PainterRegId).Distinct().Count(x => x > 0)).ToString(),
                    Variance = "0",
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "PAINTER CALL",
                    Target = "N/A",
                    Actual = (painterCall.Select(x => x.PainterCallId).Distinct().Count(x => x > 0)).ToString(),
                    Variance = "N/A",
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "LEAD GENERATION",
                    Target = "N/A",
                    Actual = (leadGen.Select(x => x.LeadGenerationId).Distinct().Count(x => x > 0)).ToString(),
                    Variance = "N/A",
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "LEAD FOLLOWUP",
                    Target = "N/A",
                    Actual = (leadFollowup.Select(x => x.LeadFollowUpId).Distinct().Count(x => x > 0)).ToString(),
                    Variance = "N/A",
                    BusinessGeneration = (leadFollowup.Select(x => new { x.LeadFollowUpId, x.DGABusinessValue }).Distinct().Sum(x => x.DGABusinessValue)).ToString(),
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
                },
                new ActiveSummaryReportResultModel
                {
                    Activity = "TOTAL COLLECTION VALUE",
                    Target = "N/A",
                    Actual = collection.Select(x => new {x.CollectionId, x.Amount}).Distinct().Sum(x => x.Amount).ToString("0.00"),
                    Variance = "N/A",
                    BusinessGeneration = "N/A",
                    UserID = query.UserId.HasValue?userinfo.Email:string.Empty,
                    DepotID = query.Depot,
                    Territory = territory,
                    Zone = zone
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
                                   join uz in _context.UserZoneAreaMappings on uInfo.Id equals uz.UserInfoId into uzleft
                                   from uzInfo in uzleft.DefaultIfEmpty()
                                   where (
                                      (!query.UserId.HasValue || uInfo.Id == query.UserId.Value)
                                      && (!query.FromDate.HasValue || ll.LoggedInTime.Date >= query.FromDate.Value.Date)
                                      && (!query.ToDate.HasValue || ll.LoggedInTime <= query.ToDate.Value.Date)
                                        && (string.IsNullOrWhiteSpace(query.Depot) || uzInfo.PlantId == query.Depot)
                                        && (!query.SalesGroups.Any() || query.SalesGroups.Contains(uzInfo.AreaId))
                                        && (!query.Territories.Any() || query.Territories.Contains(uzInfo.TerritoryId))
                                        && (!query.Zones.Any() || query.Zones.Contains(uzInfo.ZoneId))
                                   )
                                   select new
                                   {
                                       uInfo.Email,
                                       ll.LoggedInTime,
                                       ll.LoggedOutTime,
                                       ll.IsLoggedIn
                                   }).ToListAsync();

            var loginInfoGroup = loginInfo.GroupBy(x => new
            {
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
                                                   && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
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
                }).OrderByDescending(o => o.snapShotDate);

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

            return _leadGenerationRepository.DynamicListFromSql("GetDynamicAdhocDealerSalesCallReport", parameters, true);
        }

        public async Task<QueryResultModel<InactivePainterReportResultModel>> GetInactivePainterReportAsync(InactivePainterReportSearchModel query)
        {
            var reportResult = new List<InactivePainterReportResultModel>();

            var inactivePainter = await (from psl in _context.PainterStatusLogs
                                         join p in _context.Painters on psl.PainterId equals p.Id
                                         where (psl.Status == 0 && p.Status == 0)
                                         select new
                                         {
                                             psl.PainterId,
                                             psl.CreatedTime,
                                         }).GroupBy(x => new { x.PainterId }).Select(x => new
                                         {
                                             painterId = x.Key.PainterId,
                                             createdTime = x.Max(x => x.CreatedTime)
                                         }).ToListAsync();

            var Painterdata = (from ip in inactivePainter
                               join psl in _context.PainterStatusLogs on ip.painterId equals psl.PainterId
                               join p in _context.Painters on ip.painterId equals p.Id
                               join pc in _context.PainterCalls on p.Id equals pc.PainterId into pcleft
                               from pcInfo in pcleft.DefaultIfEmpty()
                               join dep in _context.Depots on p.Depot equals dep.Werks
                               join dd in _context.DropdownDetails on p.PainterCatId equals dd.Id
                               join ddpc in _context.DropdownDetails on pcInfo?.PainterCatId equals ddpc.Id into ddpcleft
                               from ddpcinfo in ddpcleft.DefaultIfEmpty()
                               where (
                                    (ip.painterId == psl.PainterId && ip.createdTime == psl.CreatedTime)
                                    && (!query.UserId.HasValue || psl.UserId == query.UserId.Value)
                                    && (string.IsNullOrWhiteSpace(query.Depot) || p.Depot == query.Depot)
                                    && (!query.FromDate.HasValue || psl.CreatedTime.Date >= query.FromDate.Value.Date)
                                    && (!query.ToDate.HasValue || psl.CreatedTime.Date <= query.ToDate.Value.Date)
                                    && (!query.SalesGroups.Any() || query.SalesGroups.Contains(p.SaleGroup))
                                    && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                    && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                    && (!query.PainterId.HasValue || p.Id == query.PainterId.Value)
                                    && (!query.PainterType.HasValue || p.PainterCatId == query.PainterType.Value)
                               )
                               orderby psl.CreatedTime descending
                               select new
                               {
                                   p.Depot,
                                   dep.Name1,
                                   poTerritory = p.Territory,
                                   puTerritory = pcInfo?.Territory,
                                   poZone = p.Zone,
                                   puZone = pcInfo?.Zone,
                                   psl.PainterId,
                                   p.PainterNo,
                                   pcInfo?.Id,
                                   poName = p.PainterName,
                                   puName = pcInfo?.PainterName,
                                   poAddress = p.Address,
                                   puAddress = pcInfo?.Address,
                                   poPhone = p.Phone,
                                   puPhone = pcInfo?.Phone,
                                   poDropdownName = dd.DropdownName,
                                   puDropdownName = ddpcinfo?.DropdownName,
                                   poAccDbblNumber = p.AccDbblNumber,
                                   puAccDbblNumber = pcInfo?.AccDbblNumber,
                                   psl.Reason
                               }).ToList();

            var reportData = Painterdata.GroupBy(x => x.PainterId).Select(x => new
            {
                depotIdAndName = $"{x.FirstOrDefault()?.Depot} {x.FirstOrDefault()?.Name1}",
                territory = x.FirstOrDefault()?.puTerritory ?? x.FirstOrDefault()?.poTerritory,
                zone = x.FirstOrDefault()?.puZone ?? x.FirstOrDefault()?.poZone,
                painterId = x.FirstOrDefault().PainterNo,
                painterName = x.FirstOrDefault()?.puName ?? x.FirstOrDefault()?.poName,
                painterAddress = x.FirstOrDefault()?.puAddress ?? x.FirstOrDefault()?.poAddress,
                painterMobileNumber = x.FirstOrDefault()?.puPhone ?? x.FirstOrDefault()?.poPhone,
                painerType = x.FirstOrDefault()?.puDropdownName ?? x.FirstOrDefault()?.poDropdownName,
                rocketDataNumber = x.FirstOrDefault()?.puAccDbblNumber ?? x.FirstOrDefault()?.poAccDbblNumber,
                inactiveReason = x.FirstOrDefault()?.Reason
            }).ToList();

            reportResult = reportData.Select(x => new InactivePainterReportResultModel
            {
                DepotIdAndName = x.depotIdAndName,
                Territory = x.territory,
                Zone = x.zone,
                PainterId = x.painterId,
                PainterName = x.painterName,
                PainterAddress = x.painterAddress,
                PainterMobileNumber = x.painterMobileNumber,
                PainerType = x.painerType,
                RocketDataNumber = x.rocketDataNumber,
                InactiveReason = x.inactiveReason
            }).Skip(this.SkipCount(query)).Take(query.PageSize).ToList();


            var queryResult = new QueryResultModel<InactivePainterReportResultModel>();
            queryResult.Items = reportResult;
            queryResult.TotalFilter = reportData.Count();
            queryResult.Total = reportData.Count();

            return queryResult;
        }

    }

    public class DealerVisitReport
    {
        public string EmployeeId { get; set; }
        public int DealerId { get; set; }
        public string Email { get; set; }
        public string BusinessArea { get; set; }
        public string depot { get; set; }
        public string territoryName { get; set; }
        public string zoneName { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public int? JourneyPlanId { get; set; }
        public bool IsAdhocVisit { get; set; }
    }
}
