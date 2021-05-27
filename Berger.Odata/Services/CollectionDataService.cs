using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public class CollectionDataService : ICollectionDataService
    {
        private readonly IODataService _oDataService;

        public CollectionDataService(IODataService oDataService)
        {
            _oDataService = oDataService;
        }

        public async Task<decimal> GetTotalCollectionValue(IList<string> dealerIds)
        {
            var selectQueryOptionBuilder = new SelectQueryOptionBuilder();

            selectQueryOptionBuilder.AddProperty(DataColumnDef.Collection_Amount);

            var currentDate = DateTime.Now;
            string date = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).DateTimeFormat();

            var data = await _oDataService.GetCollectionData(selectQueryOptionBuilder, dealerIds, date, date);

            var result = data.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount));

            return result;
        }

        public async Task<decimal> GetTotalCollectionValue(IList<string> dealerIds, DateTime? startDate, DateTime? endDate)
        {
            var selectQueryOptionBuilder = new SelectQueryOptionBuilder();

            selectQueryOptionBuilder.AddProperty(DataColumnDef.Collection_Amount);

            string startDateStr = startDate.HasValue ? startDate.Value.DateTimeFormat() : string.Empty;
            string endDateStr = endDate.HasValue ? endDate.Value.DateTimeFormat() : string.Empty;

            var data = await _oDataService.GetCollectionData(selectQueryOptionBuilder, dealerIds, startDateStr, endDateStr);

            var result = data.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount));

            return result;
        }

        public async Task<IList<CollectionDataModel>> GetCustomerCollectionAmount(IList<string> dealerIds, DateTime startDate, DateTime endDate)
        {
            string startDateStr = startDate.DateTimeFormat();
            string endDateStr = endDate.DateTimeFormat();

            var selectQueryOptionBuilder = new SelectQueryOptionBuilder();
            selectQueryOptionBuilder.AddProperty(DataColumnDef.Collection_Customer)
                                    .AddProperty(DataColumnDef.Collection_Amount);

            var data = await _oDataService.GetCollectionData(selectQueryOptionBuilder, dealerIds, startDateStr, endDateStr);

            return data;
        }
    }
}