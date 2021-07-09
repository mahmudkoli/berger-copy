using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Common.Model;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Sync;
using Berger.Odata.Extensions;
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

        public AppFrontBoxService(IRepository<SyncDailySalesLog> syncDailySalesLogRepository,
            IRepository<SyncDailyTargetLog> syncDailyTargetLogRepository,
            IRepository<BrandInfo> brandInfoRepository, IODataService oDataService,
            IRepository<LeadGeneration> leadGenerationRepository, IRepository<LeadFollowUp> leadFollowUpRepository,
            IAuthService authService
        )
        {
            _syncDailySalesLogRepository = syncDailySalesLogRepository;
            _syncDailyTargetLogRepository = syncDailyTargetLogRepository;
            _brandInfoRepository = brandInfoRepository;
            _oDataService = oDataService;
            _leadGenerationRepository = leadGenerationRepository;
            _leadFollowUpRepository = leadFollowUpRepository;
            _authService = authService;
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
                returnModel.FontBoxItem.Add($"Lead Followup: {leadFollowUpCount}");
            }

            else if (employeeRole == EnumEmployeeRole.AM || employeeRole == EnumEmployeeRole.TM_TO ||
                employeeRole == EnumEmployeeRole.BM_BSI ||
                employeeRole == EnumEmployeeRole.DSM || employeeRole == EnumEmployeeRole.RSM ||
                employeeRole == EnumEmployeeRole.GM)
            {

                double sumOfTotalSales = await GetMonthlySalesAmount();

                double numberOfBillingDealer = await GetNoOfBillingDealer();

                double sumOfTotalTarget = await GetTotalTarget();

                var sumOfPremiumBrandSales = await GetPremiumBrandSalesAmount();


                string targetAchv = $"Target Achv: {_oDataService.GetAchivement(CustomConvertExtension.ObjectToDecimal(sumOfTotalTarget), CustomConvertExtension.ObjectToDecimal(sumOfTotalSales)):0.00}%";
                string premiumSalesPercentage = $"Premium Brand Value Sales: {_oDataService.GetPercentage(CustomConvertExtension.ObjectToDecimal(sumOfTotalSales), CustomConvertExtension.ObjectToDecimal(sumOfPremiumBrandSales))}%";
                string leadFollowupString = $"Lead Created: {leadCreatedCount} \n Lead Followup: {leadFollowUpCount}";
                string numberOfBillingDealerString = $"Number of Billing Dealer: {numberOfBillingDealer}";

                returnModel.FontBoxItem.Add(targetAchv);
                returnModel.FontBoxItem.Add(premiumSalesPercentage);
                returnModel.FontBoxItem.Add(leadFollowupString);
                returnModel.FontBoxItem.Add(numberOfBillingDealerString);

                returnModel.LastSyncDateTime = "";

            }

            return returnModel;

        }

        private async Task<int> GetLeadFollowUpCount()
        {

            var area = _authService.GetLoggedInUserArea();

            var dates = GetComparableDates();
            var firstDateOfMonth = dates.Item1;
            var lastDateOfMonth = dates.Item2;

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;


            int leadFollowUpCount = await _leadFollowUpRepository.FindByCondition(x => x.CreatedTime >= firstDateOfMonth
                    && x.CreatedTime <= lastDateOfMonth
                    && (employeeRole == EnumEmployeeRole.GM || ((!area.Depots.Any() || area.Depots.Contains(x.LeadGeneration.Depot))
                    && (!area.Territories.Any() || area.Territories.Contains(x.LeadGeneration.Territory))
                    && (!area.Zones.Any() || area.Zones.Contains(x.LeadGeneration.Zone)))))
                .CountAsync();
            return leadFollowUpCount;
        }

        private async Task<int> GetLeadCreatedCount()
        {
            var area = _authService.GetLoggedInUserArea();

            var dates = GetComparableDates();
            var firstDateOfMonth = dates.Item1;
            var lastDateOfMonth = dates.Item2;

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;

            int leadCreatedCount = await _leadGenerationRepository.FindByCondition(x => x.CreatedTime >= firstDateOfMonth
                    && x.CreatedTime <= lastDateOfMonth && (employeeRole == EnumEmployeeRole.GM || ((!area.Depots.Any() || area.Depots.Contains(x.Depot))
                    && (!area.Territories.Any() || area.Territories.Contains(x.Territory))
                    && (!area.Zones.Any() || area.Zones.Contains(x.Zone)))))
                .CountAsync();
            return leadCreatedCount;
        }

        private async Task<double> GetPremiumBrandSalesAmount()
        {
            List<string> premiumBrandList = await _brandInfoRepository.FindByCondition(x => x.IsPremium)
                .Select(x => x.MaterialGroupOrBrand).Distinct().ToListAsync();


            Expression<Func<SyncDailySalesLog, bool>> premiumBranCheckExpression =
                x => premiumBrandList.Contains(x.BrandCode);

            var premiumBranCheckFullExpression = GetSalesGeneralWhereExpression().AndAlso(premiumBranCheckExpression);

            double sumOfPremiumBrandSales = await _syncDailySalesLogRepository
                .FindByCondition(premiumBranCheckFullExpression)
                .SumAsync(x => x.NetAmount);
            return sumOfPremiumBrandSales;
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

            Expression<Func<SyncDailyTargetLog, bool>> targetWhereClause = x => x.Month == firstDateOfMonth.Month
                && x.Year == firstDateOfMonth.Year && (employeeRole == EnumEmployeeRole.GM || ((!area.Depots.Any() || area.Depots.Contains(x.BusinessArea))
                                                         && (!area.SalesOffices.Any() || area.SalesOffices.Contains(x.SalesOffice))
                                                         && (!area.SalesGroups.Any() || area.SalesGroups.Contains(x.SalesGroup))
                                                         && (!area.Territories.Any() || area.Territories.Contains(x.TerritoryCode))
                                                         && (!area.Zones.Any() || area.Zones.Contains(x.Zone))));
            return targetWhereClause;
        }

        private async Task<int> GetNoOfBillingDealer()
        {
            return await _syncDailySalesLogRepository
                .FindByCondition(GetSalesGeneralWhereExpression())
                .Select(x => x.CustNo).Distinct().CountAsync();
        }

        private async Task<double> GetMonthlySalesAmount()
        {
            return await _syncDailySalesLogRepository
                .FindByCondition(GetSalesGeneralWhereExpression())
                .SumAsync(x => x.NetAmount);
        }

        private Expression<Func<SyncDailySalesLog, bool>> GetSalesGeneralWhereExpression()
        {
            var area = _authService.GetLoggedInUserArea();

            var dates = GetComparableDates();
            var firstDateOfMonth = dates.Item1;
            var lastDateOfMonth = dates.Item2;

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)AppIdentity.AppUser.EmployeeRole;

            Expression<Func<SyncDailySalesLog, bool>> generalWhereExpression = x =>
                x.Date >= firstDateOfMonth.Date && x.Date <= lastDateOfMonth.Date && (employeeRole == EnumEmployeeRole.GM ||
                    ((!area.Depots.Any() || area.Depots.Contains(x.BusinessArea))
                     && (!area.SalesOffices.Any() || area.SalesOffices.Contains(x.SalesOffice))
                     && (!area.SalesGroups.Any() || area.SalesGroups.Contains(x.SalesGroup))
                     && (!area.Territories.Any() || area.Territories.Contains(x.TerritoryCode))
                     && (!area.Zones.Any() || area.Zones.Contains(x.Zone))));
            return generalWhereExpression;
        }

        private Tuple<DateTime, DateTime> GetComparableDates()
        {
            var firstDateOfMonth = DateTime.Now.GetMonthFirstDate();
            var lastDateOfMonth = DateTime.Now.GetMonthLastDate();
            return Tuple.Create(firstDateOfMonth, lastDateOfMonth);
        }
    }
}