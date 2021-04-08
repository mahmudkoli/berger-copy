using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class StockDataService : IStockDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataCommonService _odataCommonService;

        public StockDataService(
            IODataService odataService,
            IODataCommonService odataCommonService
            )
        {
            _odataService = odataService;
            _odataCommonService = odataCommonService;
        }

        public async Task<IList<StocksResultModel>> GetStockDetails(StocksSearchModel model)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            foreach (var prop in typeof(StockDataModel).GetProperties())
            {
                selectQueryBuilder.AddProperty(prop.Name);
            }

            var data = (await _odataService.GetStockData(selectQueryBuilder, plant: model.Plant, materialGroup: model.MaterialGroup, materialCode: model.MaterialCode)).ToList();

            var result = data.GroupBy(x => x.MaterialCode).Select(x =>
                                new StocksResultModel()
                                {
                                    MaterialCode = x.Key,
                                    MaterialDiscription = x.FirstOrDefault()?.MaterialDiscription??string.Empty,
                                    Stock = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.CurrentStock)),
                                    UOM = "NOS"
                                }).ToList();

            return result;
        }
    }
}
