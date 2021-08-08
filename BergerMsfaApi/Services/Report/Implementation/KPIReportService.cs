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
                IMapper mapper
            )
        {
            this._context = context;
            this._salesDataService = salesDataService;
            this._financialDataService = financialDataService;
            this._collectionDataService = collectionDataService;

            this._authService = authService;
            this._mapper = mapper;
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
                                     join dsc in _context.DealerSalesCalls.Select(x => new { x.JourneyPlanId }).Distinct() on jpd.PlanId equals dsc.JourneyPlanId into dscleftjoin
                                     from dscinfo in dscleftjoin.DefaultIfEmpty()
                                         //join u in _context.UserInfos on jpminfo.EmployeeId equals u.EmployeeId into uleftjoin
                                         //  from userInfo in uleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on jpd.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                         //join dep in _context.Depots on diInfo.BusinessArea equals dep.Werks into depleftjoin
                                         //from depinfo in depleftjoin.DefaultIfEmpty()
                                         //join t in _context.Territory on diInfo.Territory equals t.Code into tleftjoin
                                         //from tinfo in tleftjoin.DefaultIfEmpty()
                                         //join z in _context.Zone on diInfo.CustZone equals z.Code into zleftjoin
                                         //from zinfo in zleftjoin.DefaultIfEmpty()
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
                                         jpd.DealerId,
                                         DealerClasification = diInfo.CustomerClasification,
                                         JourneyPlanDate = jpminfo.PlanDate,
                                         JourneyPlanId = jpd.PlanId,
                                     }).ToListAsync();

            var fromDate = new DateTime(query.Year, query.Month, 01);
            var toDate = new DateTime(query.Year, query.Month, DateTime.DaysInMonth(query.Year, query.Month));

            var premiumBrands = _context.BrandInfos.Where(x => x.IsPremium).Select(x => x.MaterialGroupOrBrand).Distinct().ToList();
            var billingOData = await _salesDataService.GetKPIStrikeRateKPIReport(query.Year, query.Month, query.Depot, query.SalesGroups, query.Territories, query.Zones, premiumBrands);

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                var reportModel = new StrikeRateKPIReportResultModel();

                var actualVisitCount = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date
                                                        && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                            (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                                x.DealerClasification == ConstantsODataValue.CustomerClassificationExclusive :
                                                                x.DealerClasification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                    .Select(x => x.DealerId).Distinct().Count();

                var billingCount = billingOData.Where(x => x.DateTime.Date == date.Date
                                                    && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                        (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                            x.CustomerClassification == ConstantsODataValue.CustomerClassificationExclusive :
                                                            x.CustomerClassification == ConstantsODataValue.CustomerClassificationNonExclusive)))
                                                .Select(x => new { x.CustomerNo }).Distinct().Count();

                reportModel.Date = date.ToString("dd-MM-yyyy");
                reportModel.DateTime = date;
                reportModel.NoOfCallActual = actualVisitCount;
                reportModel.NoOfPremiumBrandBilling = billingCount;
                reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);

                reportResult.Add(reportModel);

                if (WeeklyCalculationDict.TryGetValue(date.Day, out (int Start, int End) dateRange))
                {
                    reportModel = new StrikeRateKPIReportResultModel
                    {
                        Date = $"Week {(WeeklyCalculationDict.Keys.ToList().IndexOf(date.Day) + 1)}",
                        DateTime = default(DateTime),
                        NoOfCallActual = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.NoOfCallActual),
                        NoOfPremiumBrandBilling = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.NoOfPremiumBrandBilling)
                    };
                    reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);
                    reportResult.Add(reportModel);
                }
                else if (date.Day > 28 && date.Day == DateTime.DaysInMonth(date.Year, date.Month))
                {
                    reportModel = new StrikeRateKPIReportResultModel
                    {
                        Date = $"Week 5",
                        DateTime = default(DateTime),
                        NoOfCallActual = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NoOfCallActual),
                        NoOfPremiumBrandBilling = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NoOfPremiumBrandBilling)
                    };
                    reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);
                    reportResult.Add(reportModel);
                }
            }


            var weekResults = reportResult.Where(x => x.Date.StartsWith("Week")).ToList();

            int noOfPremiumBrandBilling = 0;
            int actual = 0;
            var businessCallWebKpiReportResultModel = new StrikeRateKPIReportResultModel
            {
                NoOfCallActual = actual = weekResults.Sum(x => x.NoOfCallActual),
                NoOfPremiumBrandBilling = noOfPremiumBrandBilling = weekResults.Sum(x => x.NoOfPremiumBrandBilling),
                BillingPercentage = this.GetAchivement(actual, noOfPremiumBrandBilling),
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
                                     join dsc in _context.DealerSalesCalls.Select(x => new { x.JourneyPlanId }).Distinct() on jpd.PlanId equals dsc.JourneyPlanId into dscleftjoin
                                     from descInfo in dscleftjoin.DefaultIfEmpty()
                                     join di in _context.DealerInfos on jpd.DealerId equals di.Id into dileftjoin
                                     from diInfo in dileftjoin.DefaultIfEmpty()
                                     where (
                                         (jpmInfo.PlanDate.Month == query.Month && jpmInfo.PlanDate.Year == query.Year)
                                         && (diInfo.BusinessArea == query.Depot)
                                         && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                         && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                         && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                         && (jpmInfo.PlanStatus == PlanStatus.Approved)
                                         && (diInfo.CustomerClasification.Contains(customerType))
                                     )
                                     select new
                                     {
                                         jpd.DealerId,
                                         DealerClasification = diInfo.CustomerClasification,
                                         JourneyPlanDate = jpmInfo.PlanDate,
                                         JourneyPlanId = (int?)jpd.PlanId,
                                     }).ToListAsync();


            var adHocDealerVisit = await (from dsc in _context.DealerSalesCalls
                                          join di in _context.DealerInfos on dsc.DealerId equals di.Id into dileftjoin
                                          from diInfo in dileftjoin.DefaultIfEmpty()
                                          where (
                                              (dsc.CreatedTime.Month == query.Month && dsc.CreatedTime.Year == query.Year)
                                              && (!query.Depot.Any() || query.Depot.Contains(diInfo.BusinessArea))
                                              && (!query.SalesGroups.Any() || query.SalesGroups.Contains(diInfo.SalesGroup))
                                              && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                              && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
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


            dealerVisit.AddRange(adHocDealerVisit);

            var fromDate = new DateTime(query.Year, query.Month, 01);
            var toDate = new DateTime(query.Year, query.Month, DateTime.DaysInMonth(query.Year, query.Month));

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                BusinessCallWebKPIReportResultModel reportModel = new BusinessCallWebKPIReportResultModel();

                //var data = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date).ToList();
                var callActual = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date && x.JourneyPlanId.HasValue).ToList();
                var callTarget = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date).ToList();

                reportModel.Date = date.ToString("dd-MM-yyyy");
                reportModel.DateTime = date;

                reportModel.NoOfCallTarget = -1 * callTarget.Select(x => x.DealerId).Distinct().Count();
                reportModel.NoOfCallActual = -1 * callActual.Select(x => x.DealerId).Distinct().Count();
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

            //TODO: need to recheck
            var billingOData = await _salesDataService.GetKPIBusinessAnalysisKPIReport(query.Year, query.Month, query.Depot, query.SalesGroups, query.Territories);
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

        public async Task<IList<CollectionPlanKPIReportResultModel>> GetFinancialCollectionPlanKPIReportAsync(CollectionPlanKPIReportSearchModel query)
        {
            var reportResult = new List<CollectionPlanKPIReportResultModel>();
            var currentDate = DateTime.Now;

            var dealerIds = await (from diInfo in _context.DealerInfos
                                   where (
                                       diInfo.BusinessArea == query.Depot
                                       && query.Territory == diInfo.Territory
                                   )
                                   select diInfo.CustomerNo).Distinct().ToListAsync();

            var fromDate = new DateTime(currentDate.Year, currentDate.Month, 01);
            var toDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            var lastMonthToDate = (new DateTime(currentDate.Year, currentDate.Month, 01)).AddDays(-1);

            var slippageData = await _financialDataService.GetCustomerSlippageAmount(dealerIds, lastMonthToDate);
            var collectionData = await _collectionDataService.GetCustomerCollectionAmount(dealerIds, fromDate, toDate);

            var targetAmount = (await _context.CollectionPlans.Where(x => x.UserId == AppIdentity.AppUser.UserId
                                    && x.BusinessArea == query.Depot && query.Territory == x.Territory
                                    && x.Year == currentDate.Year && x.Month == currentDate.Month).FirstOrDefaultAsync())?.CollectionTargetAmount ?? 0;

            reportResult.Add(new CollectionPlanKPIReportResultModel
            {
                SlippageAmount = slippageData.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount)),
                CollectionTargetAmount = targetAmount,
                CollectionActualAmount = collectionData.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount)),
                CollectionActualSlippageAmount = collectionData.Where(x => slippageData.Any(y => x.CustomerNo == y.CustomerNo)).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount))
            });

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
