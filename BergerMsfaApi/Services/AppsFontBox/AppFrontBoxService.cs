using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Sync;
using Berger.Odata.Extensions;
using Berger.Odata.Repositories;
using Berger.Odata.Services;
using BergerMsfaApi.Models.AppFontBox;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BergerMsfaApi.Services.AppsFontBox
{
    public class AppFrontBoxService : IAppFrontBoxService
    {
        private readonly IRepository<SyncDailySalesLog> _syncDailySalesLogRepository;
        private readonly IRepository<SyncDailyTargetLog> _syncDailyTargetLogRepository;
        private readonly IRepository<BrandInfo> _brandInfoRepository;
        private readonly IODataService _oDataService;
        private readonly IRepository<LeadGeneration> _leadGenerationRepository;
        private readonly IRepository<LeadFollowUp> _leadFollowUpRepository;
        private readonly IAuthService _authService;
        private readonly IRepository<SyncSetup> _syncSetupRepository;
        private readonly IRepository<Berger.Data.MsfaEntity.DealerSalesCall.DealerSalesCall> _dealerSalesCallRepository;
        private readonly IRepository<PainterCall> _painterCallsRepository;
        private readonly IODataSAPRepository<CustomerInvoiceReport> _customerInvoiceReportRepository;

        public AppFrontBoxService(IRepository<SyncDailySalesLog> syncDailySalesLogRepository,
            IRepository<SyncDailyTargetLog> syncDailyTargetLogRepository,
            IRepository<BrandInfo> brandInfoRepository, IODataService oDataService,
            IRepository<LeadGeneration> leadGenerationRepository, IRepository<LeadFollowUp> leadFollowUpRepository,
            IAuthService authService,
            IRepository<SyncSetup> syncSetupRepository,
            IRepository<Berger.Data.MsfaEntity.DealerSalesCall.DealerSalesCall> dealerSalesCallRepository,
            IRepository<PainterCall> painterCallsRepository,
            IODataSAPRepository<CustomerInvoiceReport> customerInvoiceReportRepository

            )
        {
            _syncDailySalesLogRepository = syncDailySalesLogRepository;
            _syncDailyTargetLogRepository = syncDailyTargetLogRepository;
            _brandInfoRepository = brandInfoRepository;
            _oDataService = oDataService;
            _leadGenerationRepository = leadGenerationRepository;
            _leadFollowUpRepository = leadFollowUpRepository;
            _authService = authService;
            _syncSetupRepository = syncSetupRepository;
            _dealerSalesCallRepository = dealerSalesCallRepository;
            _painterCallsRepository = painterCallsRepository;
            _customerInvoiceReportRepository = customerInvoiceReportRepository;
        }

        public async Task<AppFrontBoxModel> GetAppFontBoxValue(AreaSearchCommonModel area)
        {

            var returnModel = new AppFrontBoxModel { FontBoxItem = new List<string>(), LastSyncDateTime = "" };

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;

            var leadCreatedCount = await GetLeadCreatedCount();

            var leadFollowUpCount = await GetLeadFollowUpCount();

            if (employeeRole == EnumEmployeeRole.ZO)
            {
                returnModel.FontBoxItem.Add($"Lead Create: {leadCreatedCount}");
                returnModel.FontBoxItem.Add($"Number of Painter Call: {await GetNumberOfPainterCall()}");
                returnModel.FontBoxItem.Add($"Lead Followup: {leadFollowUpCount}");
                returnModel.FontBoxItem.Add($"Sub-dealer Sales Call: {await GetNumberOfDealerSalesCall()}");
            }

            else if (employeeRole == EnumEmployeeRole.AM || employeeRole == EnumEmployeeRole.TM_TO ||
                employeeRole == EnumEmployeeRole.BM_BSI ||
                employeeRole == EnumEmployeeRole.DSM || employeeRole == EnumEmployeeRole.RSM ||
                employeeRole == EnumEmployeeRole.GM)
            {

                var sumOfTotalSales = await GetMonthlySalesAmount();

                var numberOfBillingDealer = await GetNoOfBillingDealer();

                var sumOfTotalTarget = await GetTotalTarget();

                var sumOfPremiumBrandSales = await GetPremiumBrandSalesAmount();

                var targetVal = string.Format("{0:0.00}", _oDataService.GetAchivement(CustomConvertExtension.ObjectToDecimal(sumOfTotalTarget),
                                                            CustomConvertExtension.ObjectToDecimal(sumOfTotalSales)));
                var premVal = string.Format("{0:0.00}", _oDataService.GetPercentage(CustomConvertExtension.ObjectToDecimal(sumOfTotalSales),
                                                            CustomConvertExtension.ObjectToDecimal(sumOfPremiumBrandSales)));

                var targetAchv = $"Target Achv: {targetVal}%";
                var premiumSalesPercentage = $"Premium Brand Value Sales: {premVal}%";
                var leadFollowupString = $"Lead Created: {leadCreatedCount} \n Lead Followup: {leadFollowUpCount}";
                var numberOfBillingDealerString = $"Number of Billing Dealer: {numberOfBillingDealer}";

                returnModel.FontBoxItem.Add(targetAchv);
                returnModel.FontBoxItem.Add(premiumSalesPercentage);
                returnModel.FontBoxItem.Add(leadFollowupString);
                returnModel.FontBoxItem.Add(numberOfBillingDealerString);

                returnModel.LastSyncDateTime = await GetLastSyncTime();

            }

            return returnModel;

        }

        private async Task<string> GetLastSyncTime()
        {
            var lastSyncTime = await _syncSetupRepository.FindByCondition(x => true).Select(x => x.LastSyncTime).FirstOrDefaultAsync();
            return lastSyncTime?.ToString("dd/MM/yyyy h:mm tt") ?? "";
        }
        private async Task<int> GetLeadFollowUpCount()
        {

            var area = _authService.GetLoggedInUserArea();

            var dates = GetComparableDates();
            var firstDateOfMonth = dates.Item1;
            var lastDateOfMonth = dates.Item2;

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;


            return await _leadFollowUpRepository.FindByCondition(x => x.CreatedTime >= firstDateOfMonth
                     && x.CreatedTime <= lastDateOfMonth
                     && (employeeRole == EnumEmployeeRole.GM || ((!area.Depots.Any() || area.Depots.Contains(x.LeadGeneration.Depot))
                     && (!area.Territories.Any() || area.Territories.Contains(x.LeadGeneration.Territory))
                     && (!area.Zones.Any() || area.Zones.Contains(x.LeadGeneration.Zone)))))
                 .CountAsync();
        }

        private async Task<int> GetLeadCreatedCount()
        {
            var area = _authService.GetLoggedInUserArea();

            var dates = GetComparableDates();
            var firstDateOfMonth = dates.Item1;
            var lastDateOfMonth = dates.Item2;

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;

            return await _leadGenerationRepository.FindByCondition(x => x.CreatedTime >= firstDateOfMonth
                    && x.CreatedTime <= lastDateOfMonth && (employeeRole == EnumEmployeeRole.GM || ((!area.Depots.Any() || area.Depots.Contains(x.Depot))
                    && (!area.Territories.Any() || area.Territories.Contains(x.Territory))
                    && (!area.Zones.Any() || area.Zones.Contains(x.Zone)))))
                .CountAsync();
        }

        private async Task<double> GetPremiumBrandSalesAmount()
        {
            //List<string> premiumBrandList = await _brandInfoRepository.FindByCondition(x => x.IsPremium)
            //    .Select(x => x.MaterialGroupOrBrand).Distinct().ToListAsync();

            //Expression<Func<SyncDailySalesLog, bool>> premiumBranCheckExpression =
            //    x => premiumBrandList.Contains(x.BrandCode);

            //var premiumBranCheckFullExpression = GetSalesGeneralWhereExpression().AndAlso(premiumBranCheckExpression);

            return (double)(await _customerInvoiceReportRepository
                .FindByCondition(GetSalesGeneralWhereExpression())
                .SumAsync(x => x.PremiumValue));
        }

        private async Task<double> GetTotalTarget()
        {
            return await _syncDailyTargetLogRepository
                .FindByCondition(GetTargetWhereClause())
                .SumAsync(x => x.TargetValue);
        }

        private Expression<Func<SyncDailyTargetLog, bool>> GetTargetWhereClause()
        {

            var area = _authService.GetLoggedInUserArea();

            var dates = GetComparableDates();
            var firstDateOfMonth = dates.Item1;

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;

            return x => x.Month == firstDateOfMonth.Month && x.Year == firstDateOfMonth.Year
                                                          && (employeeRole == EnumEmployeeRole.GM || ((!area.Depots.Any() || area.Depots.Contains(x.BusinessArea))
                                                        && (!area.SalesOffices.Any() || area.SalesOffices.Contains(x.SalesOffice))
                                                        && (!area.SalesGroups.Any() || area.SalesGroups.Contains(x.SalesGroup))
                                                        && (!area.Territories.Any() || area.Territories.Contains(x.TerritoryCode))
                                                        && (!area.Zones.Any() || area.Zones.Contains(x.Zone))));
        }

        private async Task<int> GetNoOfBillingDealer()
        {
            return await _customerInvoiceReportRepository
                .FindByCondition(GetSalesGeneralWhereExpression())
                .Select(x => x.CustomerNo).Distinct().CountAsync();
        }

        private async Task<double> GetMonthlySalesAmount()
        {
            return (double)(await _customerInvoiceReportRepository
                .FindByCondition(GetSalesGeneralWhereExpression())
                .SumAsync(x => x.Value));
        }

        private Expression<Func<CustomerInvoiceReport, bool>> GetSalesGeneralWhereExpression()
        {
            var area = _authService.GetLoggedInUserArea();
            var dates = GetComparableDates();
            var firstDateOfMonth = dates.Item1;
            var lastDateOfMonth = dates.Item2;

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;

            return x => x.Date >= firstDateOfMonth.Date && x.Date <= lastDateOfMonth.Date && (employeeRole == EnumEmployeeRole.GM ||
                    ((!area.Depots.Any() || area.Depots.Contains(x.Depot))
                     && (!area.SalesOffices.Any() || area.SalesOffices.Contains(x.SalesOffice))
                     && (!area.SalesGroups.Any() || area.SalesGroups.Contains(x.SalesGroup))
                     && (!area.Territories.Any() || area.Territories.Contains(x.Territory))
                     && (!area.Zones.Any() || area.Zones.Contains(x.Zone))));
        }

        private Tuple<DateTime, DateTime> GetComparableDates()
        {
            var firstDateOfMonth = DateTime.Now.GetMonthFirstDate();
            var lastDateOfMonth = DateTime.Now.GetMonthLastDate();
            return Tuple.Create(firstDateOfMonth, lastDateOfMonth);
        }


        private async Task<int> GetNumberOfDealerSalesCall()
        {
            var (fromDate, toDate) = GetComparableDates();
            return await _dealerSalesCallRepository.FindByCondition(x => x.UserId == AppIdentity.AppUser.UserId
                                                                         && x.IsSubDealerCall && x.CreatedTime >= fromDate && x.CreatedTime <= toDate).CountAsync();
        }

        private async Task<int> GetNumberOfPainterCall()
        {
            var (fromDate, toDate) = GetComparableDates();

            return await _painterCallsRepository.FindByCondition(x => x.EmployeeId == AppIdentity.AppUser.EmployeeId && x.CreatedTime >= fromDate && x.CreatedTime <= toDate)
                .CountAsync();
        }


    }
}