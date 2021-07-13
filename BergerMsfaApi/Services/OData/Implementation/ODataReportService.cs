using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Data.MsfaEntity;
using BergerMsfaApi.Services.OData.Interfaces;
using Berger.Odata.Services;
using Berger.Odata.Model;
using BergerMsfaApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Berger.Common.Model;
using Berger.Common.Enumerations;
using Berger.Common.Constants;
using Berger.Data.MsfaEntity.Master;

namespace BergerMsfaApi.Services.OData.Implementation
{
    public class ODataReportService : IODataReportService
    {
        private readonly IRepository<BrandInfo> _brandInfoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<DealerInfo> _dealerInfoRepository;
        private readonly IMTSDataService _mTSDataService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ISalesDataService _salesDataService;
        private readonly IAuthService _authService;
        private readonly ICollectionDataService _collectionDataService;
        private readonly IRepository<Depot> _depotRepository;

        public ODataReportService(
            IRepository<BrandInfo> brandInfoRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<DealerInfo> dealerInfoRepository,
            IMTSDataService mTSDataService,
            IMapper mapper,
            ApplicationDbContext context,
            ISalesDataService salesDataService,
            IAuthService authService, ICollectionDataService collectionDataService,
            IRepository<Depot> depotRepository
            )
        {
            _brandInfoRepository = brandInfoRepository;
            _userInfoRepository = userInfoRepository;
            _dealerInfoRepository = dealerInfoRepository;
            _mTSDataService = mTSDataService;
            _mapper = mapper;
            _context = context;
            _salesDataService = salesDataService;
            _authService = authService;
            _collectionDataService = collectionDataService;
            _depotRepository = depotRepository;
        }

        public async Task<TodaysActivitySummaryReportResultModel> TodaysActivitySummaryReport(AreaSearchCommonModel area)
        {
            var currentDate = DateTime.Now;

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
                                         (jpminfo.PlanDate.Date == currentDate.Date)
                                         && (jpminfo.PlanStatus == PlanStatus.Approved)
                                         && (!area.Depots.Any() || area.Depots.Contains(diInfo.BusinessArea))
                                         && (!area.SalesOffices.Any() || area.SalesOffices.Contains(diInfo.SalesOffice))
                                         && (!area.SalesGroups.Any() || area.SalesGroups.Contains(diInfo.SalesGroup))
                                         && (!area.Territories.Any() || area.Territories.Contains(diInfo.Territory))
                                         && (!area.Zones.Any() || area.Zones.Contains(diInfo.CustZone))
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
                                              (dsc.CreatedTime.Date == currentDate.Date)
                                              && (!area.Depots.Any() || area.Depots.Contains(diInfo.BusinessArea))
                                              && (!area.SalesOffices.Any() || area.SalesOffices.Contains(diInfo.SalesOffice))
                                              && (!area.SalesGroups.Any() || area.SalesGroups.Contains(diInfo.SalesGroup))
                                              && (!area.Territories.Any() || area.Territories.Contains(diInfo.Territory))
                                              && (!area.Zones.Any() || area.Zones.Contains(diInfo.CustZone))
                                          )
                                          select new
                                          {
                                              DealerSalerCallId = dsc.Id,
                                              IsSubDealer = dsc.IsSubDealerCall,
                                              DealerId = dsc.DealerId
                                          }).Distinct().ToListAsync();

            //TODO: need to update query after painter call modification
            var painterCall = await (from pc in _context.PainterCalls
                                     join p in _context.Painters on pc.PainterId equals p.Id into pleftjoin
                                     from pInfo in pleftjoin.DefaultIfEmpty()
                                     where (
                                        (pc.CreatedTime.Date == currentDate.Date)
                                        && (!area.Depots.Any() || area.Depots.Contains(pInfo.Depot))
                                        && (!area.SalesGroups.Any() || area.SalesGroups.Contains(pInfo.SaleGroup))
                                        && (!area.Territories.Any() || area.Territories.Contains(pInfo.Territory))
                                        && (!area.Zones.Any() || area.Zones.Contains(pInfo.Zone))
                                    )
                                     select new
                                     {
                                         PainterCallId = pc.Id
                                     }).Distinct().ToListAsync();

            //TODO: need to update after dropdown modification
            var collection = await (from p in _context.Payments
                                    join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                    from dctinfo in dctleftjoin.DefaultIfEmpty()
                                    join ui in _context.UserInfos on p.EmployeeId equals ui.EmployeeId into uileftjoin
                                    from uiInfo in uileftjoin.DefaultIfEmpty()
                                    join uarea in _context.UserZoneAreaMappings on uiInfo.Id equals uarea.UserInfoId into uarealeftjoin
                                    from uareaInfo in uarealeftjoin.DefaultIfEmpty()
                                    where (
                                        (p.CollectionDate.Date == currentDate.Date)
                                        && (!area.Depots.Any() || area.Depots.Contains(uareaInfo.PlantId))
                                        && (!area.SalesOffices.Any() || area.SalesOffices.Contains(uareaInfo.SalesOfficeId))
                                        && (!area.SalesGroups.Any() || area.SalesGroups.Contains(uareaInfo.AreaId))
                                        && (!area.Territories.Any() || area.Territories.Contains(uareaInfo.TerritoryId))
                                        && (!area.Zones.Any() || area.Zones.Contains(uareaInfo.ZoneId))
                                    )
                                    select new
                                    {
                                        CollectionId = p.Id,
                                        CustomerTypeId = p.CustomerTypeId,
                                        CustomerTypeName = dctinfo.DropdownName,
                                        Amount = p.Amount
                                    }).Distinct().ToListAsync();

            //TODO: need to update after lead followup modification
            var lead = await (from lg in _context.LeadGenerations
                              join lf in _context.LeadFollowUps on lg.Id equals lf.LeadGenerationId into lfleftjoin
                              from lfInfo in lfleftjoin.DefaultIfEmpty()
                              where (
                                  (lg.CreatedTime.Date == currentDate.Date || lfInfo.CreatedTime.Date == currentDate.Date)
                                  && (!area.Depots.Any() || area.Depots.Contains(lg.Depot))
                                  && (!area.Territories.Any() || area.Territories.Contains(lg.Territory))
                                  && (!area.Zones.Any() || area.Zones.Contains(lg.Zone))
                              )
                              select new
                              {
                                  LeadGenerationId = lg.Id,
                                  LeadFollowUpId = lfInfo.Id,
                                  LeadGenerationDate = lg.CreatedTime,
                                  LeadFollowUpDate = lfInfo.CreatedTime,
                                  DGABusinessValue = lfInfo.ExpectedValue
                              }).Distinct().ToListAsync();

            var noOfBillingDealer = await _salesDataService.NoOfBillingDealer(area, ConstantsODataValue.DivisionDecorative, ConstantsODataValue.DistrbutionChannelDealer);

            var result = new TodaysActivitySummaryReportResultModel
            {
                DealerVisitTarget = dealerVisit.Where(x => !x.IsSubDealer).Select(x => x.DealerId).Distinct().Count(x => x > 0),
                DealerVisitActual = dealerVisit.Where(x => !x.IsSubDealer && x.IsVisited).Select(x => x.DealerId).Distinct().Count(x => x > 0),
                SubDealerVisitTarget = dealerVisit.Where(x => x.IsSubDealer).Select(x => x.DealerId).Distinct().Count(x => x > 0),
                SubDealerVisitActual = dealerVisit.Where(x => x.IsSubDealer && x.IsVisited).Select(x => x.DealerId).Distinct().Count(x => x > 0),
                AdHocDealerVisit = adHocDealerVisit.Where(x => !x.IsSubDealer).Select(x => x.DealerId).Distinct().Count(x => x > 0),
                AdHocSubDealerVisit = adHocDealerVisit.Where(x => x.IsSubDealer).Select(x => x.DealerId).Distinct().Count(x => x > 0),
                NoOfBillingDealer = noOfBillingDealer,
                PainterCall = painterCall.Select(x => x.PainterCallId).Distinct().Count(x => x > 0),
                CollectionFromDealer = collection.Where(x => x.CustomerTypeName == ConstantsCustomerTypeValue.Dealer).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                CollectionFromSubDealer = collection.Where(x => x.CustomerTypeName == ConstantsCustomerTypeValue.SubDealer).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                CollectionFromDirectProject = collection.Where(x => x.CustomerTypeName == ConstantsCustomerTypeValue.DirectProject).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                CollectionFromCustomer = collection.Where(x => x.CustomerTypeName == ConstantsCustomerTypeValue.Customer).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                LeadGenerationNo = lead.Where(x => x.LeadGenerationDate.Date == currentDate.Date).Select(x => x.LeadGenerationId).Distinct().Count(x => x > 0),
                LeadFollowupNo = lead.Where(x => x.LeadFollowUpDate.Date == currentDate.Date).Select(x => x.LeadFollowUpId).Distinct().Count(x => x > 0),
                DGABusinessValue = lead.Where(x => x.LeadFollowUpDate.Date == currentDate.Date).Select(x => new { x.LeadFollowUpId, x.DGABusinessValue }).Distinct().Sum(x => x.DGABusinessValue),
            };

            return result;
        }

        public async Task<IList<ReportDealerPerformanceResultModel>> ReportDealerPerformance(DealerPerformanceResultSearchModel model, IList<string> dealerIds)
        {
            var customerNoList = new List<string>();

            if (model.ReportType == DealerPerformanceReportType.LastYearAppointed)
            {
                customerNoList = await _dealerInfoRepository
                    .FindByCondition(x => x.IsLastYearAppointed && x.Territory == model.Territory && dealerIds.Contains(x.CustomerNo))
                    .Select(x => x.CustomerNo).ToListAsync();
            }
            else
            {
                customerNoList = await _dealerInfoRepository
                    .FindByCondition(x => x.ClubSupremeType > 0 && x.Territory == model.Territory && dealerIds.Contains(x.CustomerNo))
                    .Select(x => x.CustomerNo).ToListAsync();
            }


            if (!customerNoList.Any())
                return new List<ReportDealerPerformanceResultModel>(); // if no record found in db;


            var result = new List<ReportDealerPerformanceResultModel>
            {
                new ReportDealerPerformanceResultModel()
                {
                    Territory = model.Territory,
                    NumberOfDealer = customerNoList.Count()
                }
            };


            var reportLastYearAppointedNewDealerPerformanceInCurrentYear =
                await _salesDataService.GetReportDealerPerformance(customerNoList, model.ReportType);

            if (reportLastYearAppointedNewDealerPerformanceInCurrentYear.Any())
            {
                reportLastYearAppointedNewDealerPerformanceInCurrentYear.ToList().ForEach(x =>
               {
                   x.NumberOfDealer = customerNoList.Count();
                   x.Territory = model.Territory;
               });
                return reportLastYearAppointedNewDealerPerformanceInCurrentYear;
            }
            else
            {
                return result;
            }
        }


        public async Task<IList<RptLastYearAppointDlerPerformanceSummaryResultModel>> ReportLastYearAppointedDealerPerformance(LastYearAppointedDealerPerformanceSearchModel model)
        {
            var depotList = await _depotRepository.FindByCondition(x => model.Depots.Contains(x.Werks)).Select(x => new
            {
                x.Werks,
                x.Name1
            }).ToListAsync();

            var summaryResultModels = await _salesDataService.GetReportLastYearAppointedDealerPerformance(model);

            var result = depotList.Select(x => new RptLastYearAppointDlerPerformanceSummaryResultModel()
            {
                CYMTD = 0,
                DepotCode = x.Werks,
                LYMTD = 0,
                DepotName = x.Name1,
                GrowthMTD = 0,
                NumberOfDealer = 0,
            }).ToList();


            foreach (var item in result)
            {
                var summaryResultModel = summaryResultModels.FirstOrDefault(x => x.DepotCode == item.DepotCode);
                if (summaryResultModel != null)
                {
                    item.CYMTD = summaryResultModel.CYMTD;
                    item.LYMTD = summaryResultModel.LYMTD;
                    item.GrowthMTD = summaryResultModel.GrowthMTD;
                }
            }

            if (depotList.Count == 1)
            {
                result.ForEach(x =>
                {
                    x.DepotCode = null;
                    x.DepotName = null;

                });
            }

            return result;
        }
    }
}
