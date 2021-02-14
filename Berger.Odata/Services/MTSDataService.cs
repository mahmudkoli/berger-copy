using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class MTSDataService : IMTSDataService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ODataSettingsModel _appSettings;

        public MTSDataService(IHttpClientService httpClientService, IOptions<ODataSettingsModel> appSettings)
        {
            _httpClientService = httpClientService;
            _appSettings = appSettings.Value;
        }

        private async Task<IList<MTSDataModel>> GetMTSData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.MTSUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<MTSDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            return data;
        }

        public async Task<IList<MTSResultModel>> GetMTSBrandsVolume(MTSSearchModel model)
        {
            var date = $"{string.Format("{0:000}", model.Month)}{string.Format("{0:0000}", model.Year)}"; ;

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, model.CustomerNo);
                                //.And()
                                //.Equal(DataColumnDef.MTS_Date, date);

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume)
                                .AddProperty(DataColumnDef.MTS_AverageSalesPrice);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetMTSData(queryBuilder.Query);

            var result = data.Select(x =>
                                new MTSResultModel()
                                {
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    MatarialGroupOrBrand = x.MatarialGroupOrBrand,
                                    TargetVolume = CustomConvertExtension.ObjectToDecimal(x.TargetVolume),
                                    ActualVolume = CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice),
                                    DifferenceVolume = CustomConvertExtension.ObjectToDecimal(x.TargetVolume) - CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice)
                                }).ToList();

            return result;
        }
    }
}

