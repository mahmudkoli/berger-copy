﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.Sync;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Repositories;

namespace BergerMsfaApi.Services.Sync
{
    public class ApiSyncService : IApiSyncService
    {
        private readonly ISyncService _syncService;
        private readonly IRepository<SyncDailySalesLog> _syncDailySalesLogRepository;
        private readonly IRepository<SyncDailyTargetLog> _syncDailyTargetLogRepository;

        public ApiSyncService(ISyncService syncService, IRepository<SyncDailySalesLog> syncDailySalesLogRepository, IRepository<SyncDailyTargetLog> syncDailyTargetLogRepository)
        {
            _syncService = syncService;
            _syncDailySalesLogRepository = syncDailySalesLogRepository;
            _syncDailyTargetLogRepository = syncDailyTargetLogRepository;
        }

        public async Task SyncDailySalesData()
        {

            var date = DateTime.Now.AddMonths(-4).AddYears(-2);
            var firstDayOfMonth = date.GetMonthFirstDate();
            var lastDayOfMonth = date.GetMonthLastDate();

            var salesDataModels = await _syncService.GetDailySalesData(firstDayOfMonth, lastDayOfMonth);

            IList<MTSDataModel> monthlyTarget = await _syncService.GetMonthlyTarget(date, date);


            var syncDailyTargetLogs = monthlyTarget.ToList().Select(x => new SyncDailyTargetLog
            {
                Date = x.Date.DateFormatDate(),
                Zone = x.Zone,
                TerritoryCode = x.Territory,
                BusinessArea = x.PlantOrBusinessArea,
                SalesGroup = x.SalesGroup,
                TargetValue = CustomConvertExtension.ObjectToDouble(x.TargetValue),
                TargetVolume = CustomConvertExtension.ObjectToDouble(x.TargetVolume),
                Division = x.Division,
                CustNo = CustomConvertExtension.ObjectToInt(x.CustomerNo),
                SalesOffice = x.SalesOffice,
                AccountGroup = x.CustomerAccountGroup,
                BrandCode = x.MatarialGroupOrBrand,
                DistributionChannel = x.DistributionChannel

            }).ToList();


            var syncDailySalesLogs = salesDataModels.ToList().Select(x => new SyncDailySalesLog
            {
                Date = x.Date.DateFormatDate(),
                Zone = x.Zone,
                TerritoryCode = x.Territory,
                BusinessArea = x.PlantOrBusinessArea,
                SalesGroup = x.SalesGroup,
                Volume = CustomConvertExtension.ObjectToDouble(x.Volume),
                NetAmount = CustomConvertExtension.ObjectToDouble(x.NetAmount),
                Division = x.Division,
                CustNo = CustomConvertExtension.ObjectToInt(x.CustomerNoOrSoldToParty),
                SalesOffice = x.SalesOffice,
                AccountGroup = x.CustomerAccountGroup,
                BrandCode = x.MatarialGroupOrBrand,
                DistributionChannel = x.DistributionChannel
            }).ToList();

            try
            {
                await _syncDailySalesLogRepository.DeleteAsync(x => x.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth);
                await _syncDailyTargetLogRepository.DeleteAsync(x => x.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth);
                await _syncDailySalesLogRepository.BulkInsert(syncDailySalesLogs);
                await _syncDailyTargetLogRepository.BulkInsert(syncDailyTargetLogs);
            }
            catch (Exception e)
            {

            }

        }
    }
}