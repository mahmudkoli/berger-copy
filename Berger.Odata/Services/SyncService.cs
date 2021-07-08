using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public class SyncService : ISyncService
    {
        private readonly IODataService _odataService;
        public SyncService(IODataService odataService)
        {
            _odataService = odataService;
        }

        public async Task<IList<SalesDataModel>> GetDailySalesData(DateTime startDate, DateTime endDate)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder()
                .AddProperty(DataColumnDef.Division)
                .AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                .AddProperty(DataColumnDef.SalesGroup)
                .AddProperty(DataColumnDef.SalesOffice)
                .AddProperty(DataColumnDef.Division)
                .AddProperty(DataColumnDef.Zone)
                .AddProperty(DataColumnDef.Territory)
                .AddProperty(DataColumnDef.CustomerAccountGroup)
                .AddProperty(DataColumnDef.MatarialGroupOrBrand)
                .AddProperty(DataColumnDef.PlantOrBusinessArea)
                .AddProperty(DataColumnDef.Volume)
                .AddProperty(DataColumnDef.NetAmount)
                .AddProperty(DataColumnDef.DistributionChannel)
                .AddProperty(DataColumnDef.Date);

            return await _odataService.GetSalesData(selectQueryBuilder, startDate.SalesSearchDateFormat(), endDate.SalesSearchDateFormat(), null,
                 null, null, null, null, null, "", "");
        }

        public async Task<IList<MTSDataModel>> GetMonthlyTarget(DateTime fromDate,DateTime toDate)
        {
            DateTime currentDate = DateTime.Now;
            var startDate = $"{string.Format("{0:0000}", fromDate.Year)}.{string.Format("{0:00}", fromDate.Month)}";
            var endDate = $"{string.Format("{0:0000}", toDate.Year)}.{string.Format("{0:00}", toDate.Month)}";
            var selectQueryBuilder = new SelectQueryOptionBuilder()
                .AddProperty(DataColumnDef.MTS_Division)
                .AddProperty(DataColumnDef.MTS_CustomerNo)
                .AddProperty(DataColumnDef.MTS_SalesGroup)
                .AddProperty(DataColumnDef.MTS_SalesOffice)
                .AddProperty(DataColumnDef.MTS_Division)
                .AddProperty(DataColumnDef.MTS_Zone)
                .AddProperty(DataColumnDef.MTS_Territory)
                .AddProperty(DataColumnDef.MTS_CustomerAccountGroup)
                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                .AddProperty(DataColumnDef.MTS_PlantOrBusinessArea)
                .AddProperty(DataColumnDef.MTS_TargetVolume)
                .AddProperty(DataColumnDef.MTS_TargetValue)
                .AddProperty(DataColumnDef.MTS_DistributionChannel)
                .AddProperty(DataColumnDef.MTS_Date);

            return await _odataService.GetMTSData(selectQueryBuilder, startDate, endDate, null, null, null, null, null, null, "", "");

        }

    }
}