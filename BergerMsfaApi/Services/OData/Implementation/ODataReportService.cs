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
using Berger.Data.ViewModel;

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
            var userId = AppIdentity.AppUser.UserId;
            var employeeId = AppIdentity.AppUser.EmployeeId;

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
                                         //&& (!area.SalesOffices.Any() || area.SalesOffices.Contains(diInfo.SalesOffice))
                                         //&& (!area.SalesGroups.Any() || area.SalesGroups.Contains(diInfo.SalesGroup))
                                         && (!area.Territories.Any() || area.Territories.Contains(diInfo.Territory))
                                         && (!area.Zones.Any() || area.Zones.Contains(diInfo.CustZone))
                                         && (jpminfo.EmployeeId == employeeId)
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
                                              && (dsc.JourneyPlanId == null)
                                              && (!area.Depots.Any() || area.Depots.Contains(diInfo.BusinessArea))
                                              //&& (!area.SalesOffices.Any() || area.SalesOffices.Contains(diInfo.SalesOffice))
                                              //&& (!area.SalesGroups.Any() || area.SalesGroups.Contains(diInfo.SalesGroup))
                                              && (!area.Territories.Any() || area.Territories.Contains(diInfo.Territory))
                                              && (!area.Zones.Any() || area.Zones.Contains(diInfo.CustZone))
                                              && (dsc.UserId == userId)
                                          )
                                          select new
                                          {
                                              DealerSalerCallId = dsc.Id,
                                              IsSubDealer = dsc.IsSubDealerCall,
                                              DealerId = dsc.DealerId
                                          }).Distinct().ToListAsync();

            var painterCall = await (from pc in _context.PainterCalls
                                     join p in _context.Painters on pc.PainterId equals p.Id into pleftjoin
                                     from pInfo in pleftjoin.DefaultIfEmpty()
                                     where (
                                        (pc.CreatedTime.Date == currentDate.Date)
                                        && (!area.Depots.Any() || area.Depots.Contains(pInfo.Depot))
                                        //&& (!area.SalesGroups.Any() || area.SalesGroups.Contains(pc.SaleGroup))
                                        && (!area.Territories.Any() || area.Territories.Contains(pc.Territory))
                                        && (!area.Zones.Any() || area.Zones.Contains(pc.Zone))
                                         && (pc.EmployeeId == employeeId)
                                    )
                                     select new
                                     {
                                         PainterCallId = pc.Id
                                     }).Distinct().ToListAsync();

            var collection = await (from p in _context.Payments
                                    join dct in _context.DropdownDetails on p.CustomerTypeId equals dct.Id into dctleftjoin
                                    from dctinfo in dctleftjoin.DefaultIfEmpty()
                                    where (
                                        (p.CollectionDate.Date == currentDate.Date)
                                        && (!area.Depots.Any() || area.Depots.Contains(p.Depot))
                                        //&& (!area.SalesOffices.Any() || area.SalesOffices.Contains(uareaInfo.SalesOfficeId))
                                        //&& (!area.SalesGroups.Any() || area.SalesGroups.Contains(uareaInfo.AreaId))
                                        && (!area.Territories.Any() || area.Territories.Contains(p.Territory))
                                        && (!area.Zones.Any() || area.Zones.Contains(p.Zone))
                                        && (p.EmployeeId == employeeId)
                                    )
                                    select new
                                    {
                                        CollectionId = p.Id,
                                        CustomerTypeId = p.CustomerTypeId,
                                        CustomerTypeCode = dctinfo.DropdownCode,
                                        Amount = p.Amount
                                    }).Distinct().ToListAsync();

            var leadGen = await (from lg in _context.LeadGenerations
                              where (
                                  (lg.CreatedTime.Date == currentDate.Date)
                                  && (!area.Depots.Any() || area.Depots.Contains(lg.Depot))
                                  && (!area.Territories.Any() || area.Territories.Contains(lg.Territory))
                                  && (!area.Zones.Any() || area.Zones.Contains(lg.Zone))
                                  && (lg.UserId == userId)
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
                                  (lf.CreatedTime.Date == currentDate.Date)
                                  && (!area.Depots.Any() || area.Depots.Contains(lgInfo.Depot))
                                  && (!area.Territories.Any() || area.Territories.Contains(lgInfo.Territory))
                                  && (!area.Zones.Any() || area.Zones.Contains(lgInfo.Zone))
                                  && (lgInfo.UserId == userId)
                              )
                              select new
                              {
                                  LeadFollowUpId = lf.Id,
                                  DGABusinessValue = lbaInfo.BergerValueSales
                              }).Distinct().ToListAsync();

            var noOfBillingDealer = await _salesDataService.NoOfBillingDealer(area, ConstantsODataValue.DivisionDecorative, ConstantsODataValue.DistrbutionChannelDealer);

            var result = new TodaysActivitySummaryReportResultModel
            {
                DealerVisitTarget = dealerVisit.Where(x => !x.IsSubDealer).Select(x => new { x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0),
                DealerVisitActual = dealerVisit.Where(x => !x.IsSubDealer && x.IsVisited).Select(x => new { x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0),
                SubDealerVisitTarget = dealerVisit.Where(x => x.IsSubDealer).Select(x => new { x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0),
                SubDealerVisitActual = dealerVisit.Where(x => x.IsSubDealer && x.IsVisited).Select(x => new { x.JourneyPlanDetailId, x.DealerId }).Distinct().Count(x => x.DealerId > 0),
                AdHocDealerVisit = adHocDealerVisit.Where(x => !x.IsSubDealer).Select(x => new { x.DealerSalerCallId, x.DealerId }).Distinct().Count(x => x.DealerId > 0),
                AdHocSubDealerVisit = adHocDealerVisit.Where(x => x.IsSubDealer).Select(x => new { x.DealerSalerCallId, x.DealerId }).Distinct().Count(x => x.DealerId > 0),
                NoOfBillingDealer = noOfBillingDealer,
                PainterCall = painterCall.Select(x => x.PainterCallId).Distinct().Count(x => x > 0),
                CollectionFromDealer = collection.Where(x => x.CustomerTypeCode == ConstantsCustomerTypeValue.DealerDropdownCode).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                CollectionFromSubDealer = collection.Where(x => x.CustomerTypeCode == ConstantsCustomerTypeValue.SubDealerDropdownCode).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                CollectionFromDirectProject = collection.Where(x => x.CustomerTypeCode == ConstantsCustomerTypeValue.DirectProjectDropdownCode).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                CollectionFromCustomer = collection.Where(x => x.CustomerTypeCode == ConstantsCustomerTypeValue.CustomerDropdownCode).Select(x => x.CollectionId).Distinct().Count(x => x > 0),
                LeadGenerationNo = leadGen.Select(x => x.LeadGenerationId).Distinct().Count(x => x > 0),
                LeadFollowupNo = leadFollowup.Select(x => x.LeadFollowUpId).Distinct().Count(x => x > 0),
                DGABusinessValue = leadFollowup.Select(x => new { x.LeadFollowUpId, x.DGABusinessValue }).Distinct().Sum(x => x.DGABusinessValue),
            };

            return result;
        }

        //public async Task<IList<ReportDealerPerformanceResultModel>> ReportDealerPerformance(DealerPerformanceResultSearchModel model, IList<string> dealerIds)
        //{
        //    var customerNoList = new List<string>();

        //    if (model.ReportType == DealerPerformanceReportType.LastYearAppointed)
        //    {
        //        customerNoList = await _dealerInfoRepository
        //            .FindByCondition(x => x.IsLastYearAppointed && x.Territory == model.Territory && dealerIds.Contains(x.CustomerNo))
        //            .Select(x => x.CustomerNo).ToListAsync();
        //    }
        //    else
        //    {
        //        customerNoList = await _dealerInfoRepository
        //            .FindByCondition(x => x.ClubSupremeType > 0 && x.Territory == model.Territory && dealerIds.Contains(x.CustomerNo))
        //            .Select(x => x.CustomerNo).ToListAsync();
        //    }


        //    if (!customerNoList.Any())
        //        return new List<ReportDealerPerformanceResultModel>(); // if no record found in db;


        //    var result = new List<ReportDealerPerformanceResultModel>
        //    {
        //        new ReportDealerPerformanceResultModel()
        //        {
        //            Territory = model.Territory,
        //            NumberOfDealer = customerNoList.Count()
        //        }
        //    };


        //    var reportLastYearAppointedNewDealerPerformanceInCurrentYear =
        //        await _salesDataService.GetReportDealerPerformance(customerNoList, model.ReportType);

        //    if (reportLastYearAppointedNewDealerPerformanceInCurrentYear.Any())
        //    {
        //        reportLastYearAppointedNewDealerPerformanceInCurrentYear.ToList().ForEach(x =>
        //       {
        //           x.NumberOfDealer = customerNoList.Count();
        //           x.Territory = model.Territory;
        //       });
        //        return reportLastYearAppointedNewDealerPerformanceInCurrentYear;
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}


        public async Task<IList<RptLastYearAppointDlerPerformanceSummaryResultModel>> ReportLastYearAppointedDealerPerformanceSummary(LastYearAppointedDealerPerformanceSearchModel model)
        {
            var depotList = await _depotRepository.FindByCondition(x => model.Depots.Contains(x.Werks)).Select(x => new
            {
                x.Werks,
                x.Name1
            }).ToListAsync();

            List<string> lastYearAppointedDealer = _dealerInfoRepository
                .Where(x => (!model.Depots.Any() || model.Depots.Contains(x.BusinessArea)) &&
                            (!model.Territories.Any() || model.Territories.Contains(x.Territory)) &&
                            (!model.Zones.Any() || model.Zones.Contains(x.CustZone)) &&
                            x.IsLastYearAppointed)
                .Select(x => x.CustomerNo).Distinct()
                .ToList();


            var result = await _salesDataService.GetReportLastYearAppointedDealerPerformanceSummary(model, lastYearAppointedDealer);

            //var result = depotList.Select(x => new RptLastYearAppointDlerPerformanceSummaryResultModel()
            //{
            //    CYMTD = 0,
            //    DepotCode = x.Werks,
            //    LYMTD = 0,
            //    Depot = x.Name1,
            //    GrowthMTD = 0,
            //    NumberOfDealer = 0,
            //}).ToList();


            //foreach (var item in result)
            //{
            //    var summaryResultModel = summaryResultModels.FirstOrDefault(x => x.DepotCode == item.DepotCode);
            //    if (summaryResultModel != null)
            //    {
            //        item.CYMTD = summaryResultModel.CYMTD;
            //        item.LYMTD = summaryResultModel.LYMTD;
            //        item.GrowthMTD = summaryResultModel.GrowthMTD;
            //        item.NumberOfDealer = summaryResultModel.NumberOfDealer;
            //        item.LYMTD = summaryResultModel.LYMTD;
            //        item.LYYTD = summaryResultModel.LYYTD;
            //        item.GrowthYTD = summaryResultModel.GrowthYTD;
            //    }
            //}

            foreach (var item in result)
            {
                var depot = depotList.FirstOrDefault(x => x.Werks == item.DepotCode);
                if (depot != null)
                {
                    item.DepotCode = depot.Werks;
                    item.Depot = depot.Name1;
                }
            }

            //if (depotList.Count == 1)
            //{
            //    result.ForEach(x =>
            //    {
            //        x.DepotCode = null;
            //        x.Depot = null;

            //    });
            //}

            return result;
        }


        public async Task<IList<RptLastYearAppointDlrPerformanceDetailResultModel>> ReportLastYearAppointedDealerPerformanceDetails(LastYearAppointedDealerPerformanceSearchModel model)
        {
            var depotList = await _depotRepository.FindByCondition(x => model.Depots.Contains(x.Werks)).Select(x => new
            {
                x.Werks,
                x.Name1
            }).ToListAsync();

            List<string> lastYearAppointedDealer = _dealerInfoRepository
                .Where(x => (!model.Depots.Any() || model.Depots.Contains(x.BusinessArea)) &&
                            (!model.Territories.Any() || model.Territories.Contains(x.Territory)) &&
                            (!model.Zones.Any() || model.Zones.Contains(x.CustZone)) &&
                            x.IsLastYearAppointed)
                .Select(x => x.CustomerNo).Distinct()
                .ToList();

            var result = await _salesDataService.GetReportLastYearAppointedDealerPerformanceDetail(model, lastYearAppointedDealer);

            foreach (var item in result)
            {
                var depot = depotList.FirstOrDefault(x => x.Werks == item.DepotCode);
                if (depot != null)
                {
                    item.DepotCode = depot.Werks;
                    item.Depot = depot.Name1;
                }
            }

            //if (depotList.Count == 1)
            //{
            //    result.ToList().ForEach(x =>
            //    {
            //        x.DepotCode = null;
            //        x.Depot = null;

            //    });
            //}

            return result;
        }

        public async Task<IList<ReportClubSupremePerformance>> ReportClubSupremePerformanceSummaryReport(ClubSupremePerformanceSearchModel model, ClubSupremeReportType reportType)
        {
            var clubSupremeDealers = _dealerInfoRepository
                .Where(x => x.ClubSupremeType > 0 && (x.ClubSupremeType == model.ClubStatus || model.ClubStatus == EnumClubSupreme.None)
                            && (!model.Depots.Any() || model.Depots.Contains(x.BusinessArea)) 
                            && (!model.Territories.Any() || model.Territories.Contains(x.Territory)) 
                            && (!model.Zones.Any() || model.Zones.Contains(x.CustZone)))
                .Select(x => new CustNClubMappingVm { CustomerNo = x.CustomerNo, ClubSupreme = x.ClubSupremeType, DepotCode = x.BusinessArea, Territory = x.Territory, Zone = x.CustZone, CustomerName = x.CustomerName }).Distinct()
                .ToList();

            var result = await _salesDataService.GetReportClubSupremePerformance(model, clubSupremeDealers, reportType);

            if (reportType == ClubSupremeReportType.Detail)
            {
                var depotList = await _depotRepository.FindByCondition(x => model.Depots.Contains(x.Werks)).Select(x => new
                {
                    x.Werks,
                    x.Name1
                }).ToListAsync();

                foreach (var reportClubSupremePerformance in result)
                {
                    var item = (ReportClubSupremePerformanceDetail)reportClubSupremePerformance;
                    var depot = depotList.FirstOrDefault(x => x.Werks == item.DepotCode);
                    if (depot != null)
                    {
                        item.DepotCode = depot.Werks;
                        item.Depot = depot.Name1;
                    }
                }
            }

            return result;
        }
    }
}
