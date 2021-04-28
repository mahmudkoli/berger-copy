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

        public async Task<IList<StrikeRateKPIReportResultModel>> GetStrikeRateKPIReportAsync(StrikeRateKPIReportSearchModel query)
        {
            var reportResult = new List<StrikeRateKPIReportResultModel>();

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
                                        (jpminfo.PlanDate.Month == query.Month && jpminfo.PlanDate.Year == query.Year)
                                        && (diInfo.BusinessArea == query.Depot)
                                        && (!query.Territories.Any() || query.Territories.Contains(diInfo.Territory))
                                        && (!query.Zones.Any() || query.Zones.Contains(diInfo.CustZone))
                                        && (dscinfo.JourneyPlanId.HasValue)
                                        //&& ((query.ReportType == EnumStrikeRateReportType.All) ||
                                        //    (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                        //        diInfo.CustomerClasification == "01" :
                                        //        diInfo.CustomerClasification == "02"))
                                    )
                                    select new
                                    {
                                        UserId = userInfo.Id,
                                        EmployeeId = jpminfo.EmployeeId,
                                        DealerId = jpd.DealerId,
                                        DealerClasification = diInfo.CustomerClasification,
                                        UserEmail = userInfo.Email,
                                        Depot = diInfo.BusinessArea,
                                        DepotName = depinfo.Name1,
                                        Territory = diInfo.Territory,
                                        TerritoryName = tinfo.Name,
                                        Zone = diInfo.CustZone,
                                        ZoneName = zinfo.Name,
                                        CustomerName = diInfo.CustomerName,
                                        JourneyPlanDate = jpminfo.PlanDate,
                                        JourneyPlanId = dscinfo.JourneyPlanId,
                                        
                                    }).ToListAsync();

            //var dealerVisitGroup = dealerVisit.GroupBy(x => x.JourneyPlanDate).ToList();
            var fromDate = new DateTime(query.Year, query.Month, 01);
            var toDate = new DateTime(query.Year, query.Month, DateTime.DaysInMonth(query.Year, query.Month));

            var premiumBrands = _context.BrandInfos.Where(x => x.IsPremium).Select(x => x.MaterialGroupOrBrand).Distinct().ToList();
            var billingOData = await _salesDataService.GetKPIStrikeRateKPIReport(query.Year, query.Month, query.Depot, query.Territories, query.Zones, premiumBrands);
            
            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                var reportModel = new StrikeRateKPIReportResultModel();

                var actualVisitCount = dealerVisit.Where(x => x.JourneyPlanDate.Date == date.Date
                                                        && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                            (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                                x.DealerClasification == "01" :
                                                                x.DealerClasification == "02")))
                                                    .Select(x => x.DealerId).Distinct().Count();
                var billingCount = billingOData.Where(x => x.DateTime.Date == date.Date
                                                    && ((query.ReportType == EnumStrikeRateReportType.All) ||
                                                        (query.ReportType == EnumStrikeRateReportType.Exclusive ?
                                                            x.CustomerClassification == "01" :
                                                            x.CustomerClassification == "02")))
                                                .Select(x => new { x.CustomerNo, x.InvoiceNoOrBillNo }).Distinct().Count();
                
                reportModel.Date = date.ToString("dd-MM-yyyy");
                reportModel.DateTime = date;
                reportModel.NoOfCallActual = actualVisitCount;
                reportModel.NoOfPremiumBrandBilling = billingCount;
                reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);

                reportResult.Add(reportModel);

                if (WeeklyCalculationDict.TryGetValue(date.Day, out (int Start, int End) dateRange))
                {
                    reportModel = new StrikeRateKPIReportResultModel();
                    reportModel.Date = $"Week {(WeeklyCalculationDict.Keys.ToList().IndexOf(date.Day)+1)}";
                    reportModel.DateTime = default(DateTime);
                    reportModel.NoOfCallActual = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.NoOfCallActual);
                    reportModel.NoOfPremiumBrandBilling = reportResult.Where(x => x.DateTime.Day >= dateRange.Start && x.DateTime.Day <= dateRange.End).Sum(x => x.NoOfPremiumBrandBilling);
                    reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);
                    reportResult.Add(reportModel);
                }
                else if (date.Day > 28 && date.Day == DateTime.DaysInMonth(date.Year, date.Month))
                {
                    reportModel = new StrikeRateKPIReportResultModel();
                    reportModel.Date = $"Week 5";
                    reportModel.DateTime = default(DateTime);
                    reportModel.NoOfCallActual = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NoOfCallActual);
                    reportModel.NoOfPremiumBrandBilling = reportResult.Where(x => x.DateTime.Day > 28).Sum(x => x.NoOfPremiumBrandBilling);
                    reportModel.BillingPercentage = this.GetPercentage(reportModel.NoOfCallActual, reportModel.NoOfPremiumBrandBilling);

                    reportResult.Add(reportModel);
                }
            }

            return reportResult;
        }

        public decimal GetPercentage(decimal total, decimal value)
        {
            return (value * 100) / (total == 0 ? 1 : total);
        }
    }
}
