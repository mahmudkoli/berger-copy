using System;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.Sync;
using Berger.Odata.Services;
using BergerMsfaApi.Repositories;

namespace BergerMsfaApi.Services.Sync
{
    public class ApiSyncService : IApiSyncService
    {
        private readonly ISyncService _syncService;
        private readonly IRepository<SyncDailySalesLog> _syncDailySalesLogRepository;

        public ApiSyncService(ISyncService syncService, IRepository<SyncDailySalesLog> syncDailySalesLogRepository)
        {
            _syncService = syncService;
            _syncDailySalesLogRepository = syncDailySalesLogRepository;
        }

        public async Task SyncDailySalesData()
        {
            var salesDataModels = await _syncService.GetDailySalesData();
            var syncDailySalesLogs = salesDataModels.ToList().Select(x => new SyncDailySalesLog
            {
                Date = DateTime.Now,
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
                await _syncDailySalesLogRepository.DeleteAsync(x => x.Date == DateTime.Now.Date);
                await _syncDailySalesLogRepository.BulkInsert(syncDailySalesLogs);
            }
            catch (Exception e)
            {

            }

        }
    }
}