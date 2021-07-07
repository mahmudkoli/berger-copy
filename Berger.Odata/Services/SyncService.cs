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

        public async Task<IList<SalesDataModel>> GetDailySalesData()
        {
            var date = DateTime.Now.SalesSearchDateFormat();
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
                .AddProperty(DataColumnDef.DistributionChannel);

            return await _odataService.GetSalesDataByDate(selectQueryBuilder, date);
        }

    }
}