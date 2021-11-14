using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Report.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Data.MsfaEntity.Target;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Services;
using BergerMsfaApi.Services.Interfaces;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Services.Report.Implementation
{
    public class KPIReportService : IKPIReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISalesDataService _salesDataService;
        private readonly IFinancialDataService _financialDataService;
        private readonly ICollectionDataService _collectionDataService;

        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IColorBankInstallMachine _colorBankInstallMachine;
        private readonly IRepository<ColorBankInstallationTarget> _colorBankInstallRepository;
        private readonly IODataService _oDataService;
        private readonly IRepository<BrandInfo> _brandRepository;
        private readonly IRepository<DealerInfo> _dealerInfoRepository;
        private readonly IKpiDataService _kpiDataService;

        public Dictionary<int, (int Start, int End)> WeeklyCalculationDict = new Dictionary<int, (int Start, int End)>()
        {
            { 7, (1 , 7) },
            { 14, (8 , 14) },
            { 21, (15 , 21) },
            { 28, (22 , 28) },
        };

        public KPIReportService(
                ApplicationDbContext context,
                ISalesDataService salesDataService,
                IFinancialDataService financialDataService,
                ICollectionDataService collectionDataService,
                IAuthService authService,
                IMapper mapper,
                IColorBankInstallMachine colorBankInstallMachine,
                IRepository<ColorBankInstallationTarget> colorBankInstallRepository,
                IODataService oDataService,
                IRepository<BrandInfo> brandRepository,
                IRepository<DealerInfo> dealerInfoRepository, IKpiDataService kpiDataService)
        {
            this._context = context;
            this._salesDataService = salesDataService;
            this._financialDataService = financialDataService;
            this._collectionDataService = collectionDataService;

            this._authService = authService;
            this._mapper = mapper;
            _colorBankInstallMachine = colorBankInstallMachine;
            _colorBankInstallRepository = colorBankInstallRepository;
            _oDataService = oDataService;
            _brandRepository = brandRepository;
            _dealerInfoRepository = dealerInfoRepository;
            _kpiDataService = kpiDataService;
        }

        private int SkipCount(QueryObjectModel query) => (query.Page - 1) * query.PageSize;

        public async Task<IList<StrikeRateKPIReportResultModel>> PremiumBrandBillingStrikeRateKPIReportAsync(StrikeRateKPIReportSearchModel query, EnumReportFor reportFor)
        {
            var reportResult = new List<StrikeRateKPIReportResultModel>();
            var customerType = query.ReportType switch
            {
                EnumStrikeRateReportType.Exclusive => ConstantsODataValue.CustomerClassificationExclusive,
                EnumStrikeRateReportType.NonExclusive => ConstantsODataValue.CustomerClassificationNonExclusive,
                _ => String.Empty
            };
            var dealerVisit = await (from jpd in _context.JourneyPlanDetails
                                     join jpm in _context.JourneyPlanMasters on jpd.PlanId equals jpm.Id into jpmleftjoin
                                     from jpminfo in jpmleftjoin.DefaultIfEmpty()
                                     join dsc in _context.DealerSalesCalls on jpd.PlanId equals dsc.JourneyPlanId into dscleftjoin
                                     from dscinfo in dscleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on jpd.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     where (
                                         (jpminfo.PlanDate.Month == query.Month && jpminfo.PlanDate.Year == query.Year)
                                         && (diInfo.BusinessArea == query.Depot)
                                         && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                         && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                         && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                         && (jpminfo.PlanStatus == PlanStatus.Approved)
                                         && (dscinfo.JourneyPlanId.HasValue)
                                         && (diInfo.CustomerClasification.Contains(customerType))
                                     )
                                     select new
                                     {
                                         DealerClasification = diInfo.CustomerClasification,
                                         VisitDate = jpminfo.PlanDate,
                                         CustomerNo = diInfo.CustomerNo
                                     }).ToListAsync();

            var dealerVisitAdhoc = await (from dsc in _context.DealerSalesCalls
                                     join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     where (
                                         (dsc.CreatedTime.Month == query.Month && dsc.CreatedTime.Year == query.Year)
                                         && (diInfo.BusinessArea == query.Depot)
                                         && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                         && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                         && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                         && (!dsc.JourneyPlanId.HasValue)
                                         && (diInfo.CustomerClasification.Contains(customerType))
                                     )
                                     select new
                                     {
                                         DealerClasification = diInfo.CustomerClasification,
                                         VisitDate = dsc.CreatedTime,
                                         CustomerNo = diInfo.CustomerNo
                                     }).ToListAsync();

            if (dealerVisitAdhoc.Any())
                dealerVisit.AddRange(dealerVisitAdhoc);

            var fromDate = new DateTime(query.Year, query.Month, 01);
            var toDate = new DateTime(query.Year, query.Month, DateTime.DaysInMonth(query.Year, query.Month));

            var premiumBrands = _context.BrandInfos.Where(x => x.IsPremium).Select(x => x.MaterialGroupOrBrand).Distinct().ToList();


            var depotList = string.IsNullOrWhiteSpace(query.Depot) ? new List<string>() : new List<string> { query.Depot };


            var billingOData = await _kpiDataService.GetKpiPerformanceReport(x => new KPIPerformanceReport()
            {
                CustomerNo = x.CustomerNo,
                Date = x.Date,
                CustomerClassification = x.CustomerClassification
            }, fromDate.DateFormat(),
                toDate.DateFormat(),
                depotList, query.SalesGroups, query.Territories, brands: premiumBrands);

            var visitDealerNos = dealerVisit.Select(x => x.CustomerNo).Distinct();
            billingOData = billingOData.Where(x => visitDealerNos.Contains(x.CustomerNo)).ToList();

            // var billingOData = await _salesDataService.GetKPIStrikeRateKPIReport(query.Year, query.Month, query.Depot, query.SalesGroups, query.Territories, query.Zones, premiumBrands);

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                var reportModel = new StrikeRateKPIReportResultModel();

                var actualVisit = dealerVisit.Where(x => x.VisitDate.Date == date.Date
                                                        && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                            (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                                x.DealerClasification == ConstantsODataValue.CustomerClassificationExclusive :
                                                                x.DealerClasification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                    .Select(x => x.CustomerNo).Distinct();

                var actualVisitCount = actualVisit.Count();

                var billing = billingOData.Where(x => x.Date.Date == date.Date
                                                    && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                        (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                            x.CustomerClassification == ConstantsODataValue.CustomerClassificationExclusive :
                                                            x.CustomerClassification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                .Select(x => x.CustomerNo).Distinct();

                var billingCount = actualVisit.Where(x => billing.Contains(x)).Count();

                reportModel.Date = date.ToString("dd-MM-yyyy");
                reportModel.DateTime = date;
                reportModel.NoOfCallActual = actualVisitCount;
                reportModel.NoOfPremiumBrandBilling = billingCount;
                reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);

                reportResult.Add(reportModel);

                if (WeeklyCalculationDict.TryGetValue(date.Day, out (int Start, int End) dateRange))
                {
                    var actualVisitWeekly = dealerVisit.Where(x => (x.VisitDate.Day >= dateRange.Start && x.VisitDate.Day <= dateRange.End)
                                                            && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                                (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                                    x.DealerClasification == ConstantsODataValue.CustomerClassificationExclusive :
                                                                    x.DealerClasification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                        .Select(x => new { x.CustomerNo, VisitDate = x.VisitDate.Date }).Distinct();

                    var actualVisitCountWeekly = actualVisitWeekly.Count();

                    var billingWeekly = billingOData.Where(x => (x.Date.Day >= dateRange.Start && x.Date.Day <= dateRange.End)
                                                        && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                            (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                                x.CustomerClassification == ConstantsODataValue.CustomerClassificationExclusive :
                                                                x.CustomerClassification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                    .Select(x => x.CustomerNo).Distinct();

                    var billingCountWeekly = actualVisitWeekly.Where(x => billingWeekly.Any(y => y == x.CustomerNo)).Select(x => x.CustomerNo).Distinct().Count();

                    reportModel = new StrikeRateKPIReportResultModel
                    {
                        Date = $"Week {(WeeklyCalculationDict.Keys.ToList().IndexOf(date.Day) + 1)}",
                        DateTime = default(DateTime),
                        NoOfCallActual = actualVisitCountWeekly,
                        NoOfPremiumBrandBilling = billingCountWeekly
                    };
                    reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);
                    reportResult.Add(reportModel);
                }
                else if (date.Day > 28 && date.Day == DateTime.DaysInMonth(date.Year, date.Month))
                {
                    var actualVisitWeekly = dealerVisit.Where(x => (x.VisitDate.Day > 28)
                                                            && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                                (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                                    x.DealerClasification == ConstantsODataValue.CustomerClassificationExclusive :
                                                                    x.DealerClasification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                        .Select(x => new { x.CustomerNo, VisitDate = x.VisitDate.Date }).Distinct();

                    var actualVisitCountWeekly = actualVisit.Count();

                    var billingWeekly = billingOData.Where(x => (x.Date.Day > 28)
                                                        && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                            (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                                x.CustomerClassification == ConstantsODataValue.CustomerClassificationExclusive :
                                                                x.CustomerClassification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                    .Select(x => x.CustomerNo).Distinct();

                    var billingCountWeekly = actualVisitWeekly.Where(x => billingWeekly.Any(y => y == x.CustomerNo)).Select(x => x.CustomerNo).Distinct().Count();

                    reportModel = new StrikeRateKPIReportResultModel
                    {
                        Date = $"Week 5",
                        DateTime = default(DateTime),
                        NoOfCallActual = actualVisitCountWeekly,
                        NoOfPremiumBrandBilling = billingCountWeekly
                    };
                    reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);
                    reportResult.Add(reportModel);
                }
            }

            var weekResults = reportResult.Where(x => x.Date.StartsWith("Week")).ToList();

            var actualVisitMonthly = dealerVisit.Where(x => ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                        (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                            x.DealerClasification == ConstantsODataValue.CustomerClassificationExclusive :
                                                            x.DealerClasification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                .Select(x => new { x.CustomerNo, VisitDate = x.VisitDate.Date }).Distinct();

            var actualVisitCountMonthly = actualVisitMonthly.Count();

            var billingMonthly = billingOData.Where(x => ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                    (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                        x.CustomerClassification == ConstantsODataValue.CustomerClassificationExclusive :
                                                        x.CustomerClassification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                            .Select(x => x.CustomerNo).Distinct();

            var billingCountMonthly = actualVisitMonthly.Where(x => billingMonthly.Any(y => y == x.CustomerNo)).Select(x => x.CustomerNo).Distinct().Count();

            var businessCallWebKpiReportResultModel = new StrikeRateKPIReportResultModel
            {
                NoOfCallActual = actualVisitCountMonthly,
                NoOfPremiumBrandBilling = billingCountMonthly,
                BillingPercentage = this.GetAchivement(actualVisitCountMonthly, billingCountMonthly),
                Date = "Total",
            };

            reportResult.Add(businessCallWebKpiReportResultModel);
            weekResults.Add(businessCallWebKpiReportResultModel);

            return reportFor == EnumReportFor.App ? weekResults : reportResult;
        }

        public async Task<IList<BusinessCallBaseKPIReportResultModel>> GetBusinessCallKPIReportAsync(BusinessCallKPIReportSearchModel query, EnumReportFor reportFor)
        {
            var reportResult = new List<BusinessCallWebKPIReportResultModel>();

            var customerType = query.Category switch
            {
                EnumStrikeRateReportType.Exclusive => ConstantsODataValue.CustomerClassificationExclusive,
                EnumStrikeRateReportType.NonExclusive => ConstantsODataValue.CustomerClassificationNonExclusive,
                _ => String.Empty
            };

            var dealerVisit = await (from jpd in _context.JourneyPlanDetails
                                     join jpm in _context.JourneyPlanMasters on jpd.PlanId equals jpm.Id into jpmleftjoin
                                     from jpmInfo in jpmleftjoin.DefaultIfEmpty()
                                     join dsc in _context.DealerSalesCalls on jpd.PlanId equals dsc.JourneyPlanId into dscleftjoin
                                     from descInfo in dscleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on jpd.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     where (
                                         (jpmInfo.PlanDate.Month == query.Month && jpmInfo.PlanDate.Year == query.Year)
                                         && (string.IsNullOrEmpty(query.Depot) || diInfo.BusinessArea == query.Depot)
                                         //&& (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                         && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                         //&& (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                         && (jpmInfo.PlanStatus == PlanStatus.Approved)
                                         && (diInfo.CustomerClasification.Contains(customerType))
                                     )
                                     select new
                                     {
                                         jpd.DealerId,
                                         DealerClasification = diInfo.CustomerClasification,
                                         JourneyPlanDate = jpmInfo.PlanDate,
                                         JourneyPlanId = (int?)descInfo.JourneyPlanId,
                                     }).ToListAsync();


            var adHocDealerVisit = await (from dsc in _context.DealerSalesCalls
                                          join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                          from diInfo in dileftjoin.DefaultIfEmpty()
                                          where (
                                              (dsc.CreatedTime.Month == query.Month && dsc.CreatedTime.Year == query.Year)
                                              && (string.IsNullOrEmpty(query.Depot) || diInfo.BusinessArea == query.Depot)
                                              //&& (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                              && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                              //&& (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                              && dsc.JourneyPlanId == null
                                              && (diInfo.CustomerClasification.Contains(customerType))
                                          )
                                          select new
                                          {
                                              dsc.DealerId,
                                              DealerClasification = diInfo.CustomerClasification,
                                              JourneyPlanDate = dsc.CreatedTime,
                                              JourneyPlanId = (int?)null
                                          }).Distinct().ToListAsync();


            //dealerVisit.AddRange(adHocDealerVisit);

            var fromDate = new DateTime(query.Year, query.Month, 01);
            var toDate = new DateTime(query.Year, query.Month, DateTime.DaysInMonth(query.Year, query.Month));

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                BusinessCallWebKPIReportResultModel reportModel = new BusinessCallWebKPIReportResultModel();

                //var data = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date).ToList();
                var callActual = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date && x.JourneyPlanId.HasValue).ToList();
                var callTarget = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date).ToList();


                var adHocVisit = adHocDealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date).ToList();

                callActual.AddRange(adHocVisit); //considering adhoc with actual visit
                callTarget.AddRange(adHocVisit);//considering adhoc with target


                reportModel.Date = date.ToString("dd-MM-yyyy");
                reportModel.DateTime = date;

                reportModel.NoOfCallTarget = callTarget.Select(x => x.DealerId).Distinct().Count();
                reportModel.NoOfCallActual = callActual.Select(x => x.DealerId).Distinct().Count();
                reportModel.Achivement = this.GetAchivement(reportModel.NoOfCallTarget, reportModel.NoOfCallActual);

                if (reportFor == EnumReportFor.Web)
                {
                    reportModel.ExclusiveNoOfCallTarget = callTarget.Where(x =>
                            x.DealerClasification == ConstantsODataValue.CustomerClassificationExclusive)
                        .Select(x => x.DealerId).Distinct().Count();

                    reportModel.ExclusiveNoOfCallActual = callActual.Where(x =>
                            x.DealerClasification == ConstantsODataValue.CustomerClassificationExclusive)
                        .Select(x => x.DealerId).Distinct().Count();

                    reportModel.ExclusiveAchivement = this.GetAchivement(reportModel.ExclusiveNoOfCallTarget,
                        reportModel.ExclusiveNoOfCallActual);

                    reportModel.NonExclusiveNoOfCallTarget = callTarget.Where(x =>
                            x.DealerClasification == ConstantsODataValue.CustomerClassificationNonExclusive)
                        .Select(x => x.DealerId).Distinct().Count();

                    reportModel.NonExclusiveNoOfCallActual = callActual.Where(x =>
                            x.DealerClasification == ConstantsODataValue.CustomerClassificationNonExclusive)
                        .Select(x => x.DealerId).Distinct().Count();

                    reportModel.NonExclusiveAchivement = this.GetAchivement(reportModel.NonExclusiveNoOfCallTarget,
                        reportModel.NonExclusiveNoOfCallActual);
                }



                reportResult.Add(reportModel);

                if (WeeklyCalculationDict.TryGetValue(date.Day, out (int Start, int End) dateRange))
                {
                    reportModel = new BusinessCallWebKPIReportResultModel
                    {
                        Date = $"Week {(WeeklyCalculationDict.Keys.ToList().IndexOf(date.Day) + 1)}",
                        DateTime = default(DateTime),
                        NoOfCallTarget = reportResult
                            .Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End)
                            .Sum(x => x.NoOfCallTarget),
                        NoOfCallActual = reportResult
                            .Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End)
                            .Sum(x => x.NoOfCallActual)
                    };

                    reportModel.Achivement = this.GetAchivement(reportModel.NoOfCallTarget, reportModel.NoOfCallActual);

                    if (reportFor == EnumReportFor.Web)
                    {
                        reportModel.ExclusiveNoOfCallTarget = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.ExclusiveNoOfCallTarget);
                        reportModel.ExclusiveNoOfCallActual = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.ExclusiveNoOfCallActual);
                        reportModel.ExclusiveAchivement = this.GetAchivement(reportModel.ExclusiveNoOfCallTarget, reportModel.ExclusiveNoOfCallActual);
                        reportModel.NonExclusiveNoOfCallTarget = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.NonExclusiveNoOfCallTarget);
                        reportModel.NonExclusiveNoOfCallActual = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.NonExclusiveNoOfCallActual);
                        reportModel.NonExclusiveAchivement = this.GetAchivement(reportModel.NonExclusiveNoOfCallTarget, reportModel.NonExclusiveNoOfCallActual);
                    }

                    reportResult.Add(reportModel);
                }
                else if (date.Day > 28 && date.Day == DateTime.DaysInMonth(date.Year, date.Month))
                {
                    reportModel = new BusinessCallWebKPIReportResultModel
                    {
                        Date = $"Week 5",
                        DateTime = default(DateTime),
                        NoOfCallTarget = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NoOfCallTarget),
                        NoOfCallActual = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NoOfCallActual)
                    };

                    if (reportFor == EnumReportFor.Web)
                    {
                        reportModel.Achivement = this.GetAchivement(reportModel.NoOfCallTarget, reportModel.NoOfCallActual);
                        reportModel.ExclusiveNoOfCallTarget = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.ExclusiveNoOfCallTarget);
                        reportModel.ExclusiveNoOfCallActual = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.ExclusiveNoOfCallActual);
                        reportModel.ExclusiveAchivement = this.GetAchivement(reportModel.ExclusiveNoOfCallTarget, reportModel.ExclusiveNoOfCallActual);
                        reportModel.NonExclusiveNoOfCallTarget = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NonExclusiveNoOfCallTarget);
                        reportModel.NonExclusiveNoOfCallActual = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NonExclusiveNoOfCallActual);
                        reportModel.NonExclusiveAchivement = this.GetAchivement(reportModel.NonExclusiveNoOfCallTarget, reportModel.NonExclusiveNoOfCallActual);
                    }

                    reportResult.Add(reportModel);
                }
            }


            var weekResults = reportResult.Where(x => x.Date.StartsWith("Week")).ToList();

            int target = 0;
            int actual = 0;
            var businessCallWebKpiReportResultModel = new BusinessCallWebKPIReportResultModel
            {
                NoOfCallActual = actual = weekResults.Sum(x => x.NoOfCallActual),
                NoOfCallTarget = target = weekResults.Sum(x => x.NoOfCallTarget),
                Achivement = this.GetAchivement(target, actual),

                ExclusiveNoOfCallTarget = target = weekResults.Sum(x => x.ExclusiveNoOfCallTarget),
                ExclusiveNoOfCallActual = actual = weekResults.Sum(x => x.ExclusiveNoOfCallActual),
                ExclusiveAchivement = this.GetAchivement(target, actual),

                NonExclusiveNoOfCallTarget = target = weekResults.Sum(x => x.NonExclusiveNoOfCallTarget),
                NonExclusiveNoOfCallActual = actual = weekResults.Sum(x => x.NonExclusiveNoOfCallActual),
                NonExclusiveAchivement = this.GetAchivement(target, actual),
                Date = "Total",
            };

            reportResult.Add(businessCallWebKpiReportResultModel);
            weekResults.Add(businessCallWebKpiReportResultModel);

            if (reportFor == EnumReportFor.App)
            {
                return _mapper.Map<IList<BusinessCallAPPKPIReportResultModel>>(weekResults).Cast<BusinessCallBaseKPIReportResultModel>().ToList();
            }

            return reportResult.Cast<BusinessCallBaseKPIReportResultModel>().ToList();
        }

        public async Task<IList<BillingAnalysisKPIReportResultModel>> GetBillingAnalysisKPIReportAsync(BillingAnalysisKPIReportSearchModel query)
        {
            var reportResult = new List<BillingAnalysisKPIReportResultModel>();

            var dealers = await (from diInfo in _context.DealerInfos
                                 where (
                                     (diInfo.Channel == ConstantsODataValue.DistrbutionChannelDealer
                                         && diInfo.Division == ConstantsODataValue.DivisionDecorative)
                                     && (diInfo.BusinessArea == query.Depot)
                                     && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                     && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                 )
                                 select diInfo).ToListAsync();

            var fromDate = new DateTime(query.Year, query.Month, 01);
            var toDate = new DateTime(query.Year, query.Month, DateTime.DaysInMonth(query.Year, query.Month));


            var depotList = string.IsNullOrWhiteSpace(query.Depot) ? new List<string>() : new List<string> { query.Depot };


            var billingOData = await _kpiDataService.GetKpiPerformanceReport(x => new KPIPerformanceReport()
            {
                CustomerNo = x.CustomerNo,
            }, fromDate.DateFormat(),
                toDate.DateFormat(),
                depotList, query.SalesGroups, query.Territories, division: ConstantsValue.DivisionDecorative);


            // var billingOData = await _salesDataService.GetKPIBusinessAnalysisKPIReport(query.Year, query.Month, query.Depot, query.SalesGroups, query.Territories);
            //var billingOData = new List<KPIBusinessAnalysisKPIReportResultModel>();
            var tempDealer = new List<string>();
            var tempNoOfDealer = 0;
            var tempNoOfBillingDealer = 0;

            var selectDictionary = new Dictionary<EnumBussinesCategory, (string Text, Func<DealerInfo, bool> Func)>()
            {
                { EnumBussinesCategory.Exclusive, ("Exclusive", x => x.BussinesCategoryType == EnumBussinesCategory.Exclusive) },
                { EnumBussinesCategory.NonExclusive, ("Non Exclusive", x => x.BussinesCategoryType == EnumBussinesCategory.NonExclusive) },
                { EnumBussinesCategory.NonAPNonExclusive, ("Non AP Non Exclusive", x => x.BussinesCategoryType == EnumBussinesCategory.NonAPNonExclusive) },
                { EnumBussinesCategory.NewDealer, ("CY New Dealer", x => x.BussinesCategoryType == EnumBussinesCategory.NewDealer) }
            };

            foreach (var item in selectDictionary)
            {
                reportResult.Add(new BillingAnalysisKPIReportResultModel
                {
                    //BillingAnalysisType = item.Key,
                    DealerType = item.Value.Text,
                    NoOfDealer = tempNoOfDealer = (tempDealer = dealers.Where(item.Value.Func)
                                                                        .Select(x => x.CustomerNo).Distinct().ToList()).Count(),
                    NoOfBillingDealer = tempNoOfBillingDealer = (billingOData.Where(x => tempDealer.Contains(x.CustomerNo))
                                                                        .Select(x => x.CustomerNo).Distinct().ToList()).Count(),
                    BillingPercentage = this.GetPercentage(tempNoOfDealer, tempNoOfBillingDealer),
                    //Details = dealers.Where(item.Value.Func).GroupBy(x => x.CustomerNo).Select(x =>
                    //{
                    //    var dt = new BillingAnalysisDetailsKPIReportResultModel();
                    //    dt.CustomerNo = x.FirstOrDefault()?.CustomerNo.ToString() ?? string.Empty;
                    //    dt.CustomerName = x.FirstOrDefault()?.CustomerName.ToString() ?? string.Empty;
                    //    dt.IsBilling = billingOData.Any(x => dt.CustomerNo == x.CustomerNo);
                    //    dt.IsBillingText = billingOData.Any(x => dt.CustomerNo == x.CustomerNo) ? "Has Billing" : "Does not have Billing";
                    //    return dt;
                    //}).ToList()
                });
            }

            //reportResult.Add(new BillingAnalysisKPIReportResultModel
            //{
            //    BillingAnalysisType = EnumBillingAnalysisType.Total,
            //    BillingAnalysisTypeText = "Total",
            //    NoOfDealer = tempNoOfDealer = (reportResult.Sum(x => x.NoOfDealer)),
            //    NoOfBillingDealer = tempNoOfBillingDealer = (reportResult.Sum(x => x.NoOfBillingDealer)),
            //    BillingPercentage = this.GetPercentage(tempNoOfDealer, tempNoOfBillingDealer),
            //    Details = dealers.GroupBy(x => x.CustomerNo).Select(x =>
            //    {
            //        var dt = new BillingAnalysisDetailsKPIReportResultModel();
            //        dt.CustomerNo = x.FirstOrDefault()?.CustomerNo.ToString() ?? string.Empty;
            //        dt.CustomerName = x.FirstOrDefault()?.CustomerName.ToString() ?? string.Empty;
            //        dt.IsBilling = billingOData.Any(x => dt.CustomerNo == x.CustomerNo);
            //        dt.IsBillingText = billingOData.Any(x => dt.CustomerNo == x.CustomerNo) ? "Has Billing" : "Does not have Billing";
            //        return dt;
            //    }).ToList()
            //});

            return reportResult;
        }


        public async Task<IList<ColorBankInstallationPlanVsActualKPIReportResultModel>> GetColorBankInstallationPlanVsActual(ColorBankInstallationPlanVsActualKpiReportSearchModel query)
        {
            var result = new List<ColorBankInstallationPlanVsActualKPIReportResultModel>();
            var bergerFyMonth = ConstantsValue.GetBergerFyMonth();
            DateTime dateTime = new DateTime(query.Year, 4, 1);

            var startDate = dateTime.GetCFYFD().DateTimeFormat();
            var endDate = dateTime.GetCFYLD().DateTimeFormat();

            var targetList =
                await _colorBankInstallRepository.FindByCondition(x =>
            x.BusinessArea == query.Depot &&
            (!query.Territories.Any() || query.Territories.Contains(x.Territory)) &&
            x.Year == query.Year && x.Month >= ConstantsValue.FyYearFirstMonth)
                .Select(x => new
                {
                    x.Month,
                    x.ColorBankInstallTarget
                }).ToListAsync();

            var targetList2 =
                await _colorBankInstallRepository.FindByCondition(x =>
            x.BusinessArea == query.Depot &&
            (!query.Territories.Any() || query.Territories.Contains(x.Territory)) &&
            x.Year == query.Year + 1 && x.Month <= ConstantsValue.FyYearLastMonth)
                .Select(x => new
                {
                    x.Month,
                    x.ColorBankInstallTarget
                }).ToListAsync();

            targetList.AddRange(targetList2);

            var customerList = await _dealerInfoRepository.FindByCondition(x => 
                                                                x.Division == ConstantsValue.DivisionDecorative
                                                                && x.Channel == ConstantsValue.DistrbutionChannelDealer
                                                                && string.IsNullOrEmpty(query.Depot) || x.BusinessArea == query.Depot
                                                                && (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                                                                //&& (!query.SalesGroups.Any() || query.Territories.Contains(x.SalesGroup))
                                                                //&& (!query.Zones.Any() || query.Territories.Contains(x.CustZone))
                                                            )
                                        .Select(x => x.CustomerNo).Distinct().ToListAsync();

            var colorBankMachineDataModels = await _colorBankInstallMachine.GetColorBankInstallMachine(query.Depot, startDate, endDate);
            int actual = 0, target = 0;

            foreach (var item in bergerFyMonth)
            {
                var addItem = new ColorBankInstallationPlanVsActualKPIReportResultModel()
                {
                    Actual = actual = colorBankMachineDataModels.Where(x => customerList.Contains(x.CustomerNo) 
                                        && CustomConvertExtension.ObjectToDateTime(x.InstallDate).Month == item.Key).Distinct().Count(),
                    Month = item.Value,
                    Target = target = targetList.FirstOrDefault(x => x.Month == item.Key)?.ColorBankInstallTarget ?? 0,
                    TargetAchievement = GetAchivement(target, actual)
                };
                result.Add(addItem);
            }

            result.Add(new ColorBankInstallationPlanVsActualKPIReportResultModel
            {
                Month = "Total",
                Target = target = result.Sum(x => x.Target),
                Actual = actual = result.Sum(x => x.Actual),
                TargetAchievement = GetAchivement(target, actual)
            });
            return result;
        }

        public async Task<IList<ColorBankProductivityBase>> GetColorBankProductivity(ColorBankProductivityKpiReportSearchModel query, EnumReportFor reportFor)
        {
            DateTime today = DateTime.Now;
            int year = query.Year;
            DateTime compareDate = new DateTime(year, ConstantsValue.FyYearFirstMonth, 1);

            DateTime currentYearStartDate = compareDate.GetCFYFD();
            DateTime currentYearEndDate = compareDate.GetCFYLD();

            DateTime lastYearStartDate = compareDate.GetLFYFD();
            DateTime lastYearEndDate = compareDate.GetLFYLD();

            if ((year == today.Year && today.Month >= ConstantsValue.FyYearFirstMonth) || (year == today.Year-1 && today.Month <= ConstantsValue.FyYearLastMonth))
            {
                //currentYearEndDate = today.AddMonths(-1).GetCYLD();
                //lastYearEndDate = today.AddMonths(-1).GetLYLD();
                currentYearEndDate = today.GetCYLD();
                lastYearEndDate = today.GetLYLD();
            }

            var productivityTarget =
                await _colorBankInstallRepository.FindByCondition(x =>
                        x.BusinessArea == query.Depot &&
                        (!query.Territories.Any() || query.Territories.Contains(x.Territory)) &&
                        x.Year == query.Year && x.Month >= ConstantsValue.FyYearFirstMonth)
                        .Select(x => new
                        {
                            x.Month,
                            x.ColorBankProductivityTarget,
                            x.Territory
                        }).ToListAsync();

            var productivityTarget2 =
                await _colorBankInstallRepository.FindByCondition(x =>
                        x.BusinessArea == query.Depot &&
                        (!query.Territories.Any() || query.Territories.Contains(x.Territory)) &&
                        x.Year == query.Year + 1 && x.Month <= ConstantsValue.FyYearLastMonth)
                        .Select(x => new
                        {
                            x.Month,
                            x.ColorBankProductivityTarget,
                            x.Territory
                        }).ToListAsync();



            //var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();

            //selectCustomerQueryBuilder.AddProperty(DataColumnDef.MatrialCode).AddProperty(DataColumnDef.Volume).AddProperty(DataColumnDef.Territory);

            var depotList = new List<string>
            {
                query.Depot
            };

            //var dbCbBrandList = await _brandRepository.FindByCondition(x => x.IsCBInstalled).Select(x => x.MaterialCode).ToListAsync();


            var currentYearSales = await _salesDataService.GetCbProductReport(x => new ColorBankPerformanceReport
            {
                Territory = x.Territory,
                Volume = x.Volume,
                CustomerNo = x.CustomerNo
            }, null,
                startDate: currentYearStartDate.DateFormat(), endDate: currentYearEndDate.DateFormat(),
                depots: depotList, salesGroup: query.SalesGroups, territories: query.Territories,zones:query.Zones);

            var lastYearSales = await _salesDataService.GetCbProductReport(x => new ColorBankPerformanceReport
            {
                Territory = x.Territory,
                Volume = x.Volume,
                CustomerNo = x.CustomerNo
            }, null,
                startDate: lastYearStartDate.DateFormat(), endDate: lastYearEndDate.DateFormat(),
                depots: depotList, salesGroup: query.SalesGroups, territories: query.Territories, zones: query.Zones);



            //var currentYearSales = await _oDataService.GetSalesData(selectCustomerQueryBuilder, depots: depotList, startDate: currentYearStartDate.SalesSearchDateFormat(), endDate: currentYearEndDate.SalesSearchDateFormat(),
            //    salesGroups: query.SalesGroups, territories: query.Territories);


            //var lastYearSales = await _oDataService.GetSalesData(selectCustomerQueryBuilder, depots: depotList, startDate: lastYearStartDate.SalesSearchDateFormat(), endDate: lastYearEndDate.SalesSearchDateFormat(),
            //    salesGroups: query.SalesGroups, territories: query.Territories);

            //  currentYearSales = currentYearSales.Where(x => dbCbBrandList.Contains(x.Brand)).ToList();
            // lastYearSales = lastYearSales.Where(x => dbCbBrandList.Contains(x.Brand)).ToList();

            var customerList = await _dealerInfoRepository.FindByCondition(x =>
                                                                x.Division == ConstantsValue.DivisionDecorative
                                                                && x.Channel == ConstantsValue.DistrbutionChannelDealer
                                                                && x.BusinessArea == query.Depot 
                                                                && (!query.Territories.Any() || query.Territories.Contains(x.Territory)) 
                                                                //&& (!query.SalesGroups.Any() || query.Territories.Contains(x.SalesGroup)) 
                                                                //&& (!query.Zones.Any() || query.Territories.Contains(x.CustZone))
                                                            )
                                        .Select(x => new { x.CustomerNo, x.Territory }).Distinct().ToListAsync();

            var currentYearTotalMachineList = await _colorBankInstallMachine.GetColorBankInstallMachine(query.Depot, "", currentYearEndDate.DateTimeFormat());
            var lastYearTotalMachineList = await _colorBankInstallMachine.GetColorBankInstallMachine(query.Depot, "", lastYearEndDate.DateTimeFormat());

            int totalMonth = (currentYearEndDate.Year - currentYearStartDate.Year) * 12 + (currentYearEndDate.Month - currentYearStartDate.Month) + 1;
            var result = new List<ColorBankProductivityBase>();

            foreach (string territory in query.Territories)
            {
                var bankProductivityBase = reportFor == EnumReportFor.App ? new ColorBankProductivityBase() : new ColorBankProductivityWeb { Territory = territory };

                decimal currentYearTotalSales = currentYearSales.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Volume));
                decimal lastYearTotalSales = lastYearSales.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Volume));

                int lastYearTotalMachine = lastYearTotalMachineList.Where(x => customerList.Any(y => y.Territory == territory && y.CustomerNo == x.CustomerNo)).Distinct().Count();
                int currentYearTotalMachine = currentYearTotalMachineList.Where(x => customerList.Any(y => y.Territory == territory && y.CustomerNo == x.CustomerNo)).Distinct().Count();

                bankProductivityBase.CYActualProductivity = 0;
                bankProductivityBase.LYProductivity = 0;
                try
                {
                    bankProductivityBase.CYActualProductivity = (currentYearTotalSales / currentYearTotalMachine) / totalMonth;

                }
                catch
                {
                    // ignored
                }

                try
                {
                    bankProductivityBase.LYProductivity = (lastYearTotalSales / lastYearTotalMachine) / totalMonth;
                }
                catch
                {
                    // ignored
                }

                var productivityTargetSum = productivityTarget.Where(x => x.Territory == territory).Sum(x => x.ColorBankProductivityTarget)
                                            + productivityTarget2.Where(x => x.Territory == territory).Sum(x => x.ColorBankProductivityTarget);
                var productivityTargetMonthCount = productivityTarget.Where(x => x.Territory == territory && x.ColorBankProductivityTarget > 0)
                                                    .Select(x => x.Month).Distinct().Count()
                                                + productivityTarget2.Where(x => x.Territory == territory && x.ColorBankProductivityTarget > 0)
                                                    .Select(x => x.Month).Distinct().Count();
                productivityTargetMonthCount = productivityTargetMonthCount == 0 ? 1 : productivityTargetMonthCount;

                bankProductivityBase.ProductivityTarget = productivityTargetSum / productivityTargetMonthCount;

                bankProductivityBase.ProductivityGrowth = _oDataService.GetGrowth(bankProductivityBase.LYProductivity, bankProductivityBase.CYActualProductivity);

                result.Add(bankProductivityBase);
            }

            decimal currentYear;
            decimal lastYear;

            if (reportFor == EnumReportFor.App)
            {

                var colorBankProductivityBase = new ColorBankProductivityBase
                {
                    CYActualProductivity = currentYear = result.Sum(x => x.CYActualProductivity),
                    LYProductivity = lastYear = result.Sum(x => x.LYProductivity),
                    ProductivityTarget = result.Sum(x => x.ProductivityTarget),
                    ProductivityGrowth = _oDataService.GetGrowth(lastYear, currentYear)
                };

                result = new List<ColorBankProductivityBase>
                {
                    colorBankProductivityBase
                };
            }

            if (query.Territories.Count > 1)
            {
                result.Add(new ColorBankProductivityWeb
                {
                    Territory = "Total",
                    CYActualProductivity = currentYear = result.Sum(x => x.CYActualProductivity),
                    LYProductivity = lastYear = result.Sum(x => x.LYProductivity),
                    ProductivityTarget = result.Sum(x => x.ProductivityTarget),
                    ProductivityGrowth = _oDataService.GetGrowth(lastYear, currentYear)
                });
            }

            return result;
        }


        public async Task<IList<CollectionPlanKPIReportResultModel>> GetFinancialCollectionPlanKPIReportAsync(CollectionPlanKPIReportSearchModel query)
        {
            var reportResult = new List<CollectionPlanKPIReportResultModel>();



            foreach (var item in query.Territories)
            {
                var currentDate = DateTime.Now;

                var dealerIds = await _context.DealerInfos.
                                       Where(p =>
                                           p.BusinessArea == query.Depot
                                           && item == p.Territory &&
                                           p.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                                            p.Division == ConstantsODataValue.DivisionDecorative
                                           && query.SalesGroups.Count == 0 ? true : query.SalesGroups.Contains(p.SalesGroup)
                                       ).Select(p => p.CustomerNo).Distinct().ToListAsync();

                var fromDate = new DateTime(currentDate.Year, currentDate.Month, 01);
                var toDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                var lastMonthToDate = (new DateTime(currentDate.Year, currentDate.Month, 01)).AddDays(-1);

                var slippageData = await _financialDataService.GetCustomerSlippageAmount(dealerIds, lastMonthToDate);
                var slippageCustomerNos = slippageData.Select(x => x.CustomerNo).Distinct().ToList();
                dealerIds = dealerIds.Where(x => slippageCustomerNos.Contains(x)).ToList();
                var actualCollection = await _collectionDataService.GetCustomerCollectionAmount(dealerIds, fromDate, toDate);
                var targetAmount = (await _context.CollectionPlans.Where(x => x.BusinessArea == query.Depot && item == x.Territory
                                    && x.Year == currentDate.Year && x.Month == currentDate.Month).FirstOrDefaultAsync())?.CollectionTargetAmount ?? 0;

                reportResult.Add(new CollectionPlanKPIReportResultModel
                {
                    Territory = item,
                    ImmediateLMSlippageAmount = slippageData.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount)),
                    MTDCollectionPlan = targetAmount,
                    MTDActualCollection = actualCollection,
                    TargetAch = GetAchivement(targetAmount, actualCollection)
                });
            }


            var totalset = new CollectionPlanKPIReportResultModel
            {
                Territory = "Total",
                ImmediateLMSlippageAmount = reportResult.Sum(x => x.ImmediateLMSlippageAmount),
                MTDCollectionPlan = reportResult.Sum(x => x.MTDCollectionPlan),
                MTDActualCollection = reportResult.Sum(x => x.MTDActualCollection),
                TargetAch = GetAchivement(reportResult.Sum(x => x.MTDCollectionPlan), reportResult.Sum(x => x.MTDActualCollection))
            };

            reportResult.Add(totalset);

            return reportResult;
        }


        public async Task<CollectionPlanKPIReportResultModelForApp> GetFinancialCollectionPlanKPIReportForAppAsync(CollectionPlanKPIReportSearchModelForApp query)
        {
            var reportResult = new CollectionPlanKPIReportResultModelForApp();



            foreach (var item in query.Territory)
            {
                var currentDate = DateTime.Now;

                var dealerIds = await (from diInfo in _context.DealerInfos
                                       where (
                                           diInfo.BusinessArea == query.Depot
                                           && item == diInfo.Territory &&
                                           diInfo.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                                            diInfo.Division == ConstantsODataValue.DivisionDecorative
                                           && query.SalesGroups.Count == 0 ? true : query.SalesGroups.Contains(diInfo.SalesGroup)
                                       )
                                       select diInfo.CustomerNo).Distinct().ToListAsync();

                var fromDate = new DateTime(currentDate.Year, currentDate.Month, 01);
                var toDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                var lastMonthToDate = (new DateTime(currentDate.Year, currentDate.Month, 01)).AddDays(-1);

                var slippageData = await _financialDataService.GetCustomerSlippageAmount(dealerIds, lastMonthToDate);
                var slippageCustomerNos = slippageData.Select(x => x.CustomerNo).Distinct().ToList();
                dealerIds = dealerIds.Where(x => slippageCustomerNos.Contains(x)).ToList();
                var actualCollection = await _collectionDataService.GetCustomerCollectionAmount(dealerIds, fromDate, toDate);
                var targetAmount = (await _context.CollectionPlans.Where(x => x.BusinessArea == query.Depot && item == x.Territory
                                    && x.Year == currentDate.Year && x.Month == currentDate.Month).FirstOrDefaultAsync())?.CollectionTargetAmount ?? 0;


                reportResult.ImmediateLMSlippageAmount = reportResult.ImmediateLMSlippageAmount + slippageData.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount));
                reportResult.MTDCollectionPlan = reportResult.MTDCollectionPlan + targetAmount;
                reportResult.MTDActualCollection = reportResult.MTDActualCollection + actualCollection;

            }

            reportResult.TargetAch = GetAchivement(reportResult.MTDCollectionPlan, reportResult.MTDActualCollection);




            return reportResult;
        }



        public decimal GetPercentage(decimal total, decimal value)
        {
            return (value * 100) / (total == 0 ? 1 : total);
        }

        public decimal GetAchivement(decimal target, decimal actual)
        {
            return target > 0 ? ((actual / target)) * 100 : decimal.Zero;
        }
    }
}
