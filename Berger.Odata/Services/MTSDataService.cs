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
            var fromDate = model.FromDate.DateFormat();
            var toDate = model.ToDate.DateFormat();

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, model.CustomerNo);
                                //.And()
                                //.StartGroup()
                                //.GreaterThanOrEqual(DataColumnDef.MTS_period, fromDate)
                                //.And()
                                //.LessThanOrEqual(DataColumnDef.MTS_period, toDate)
                                //.EndGroup();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_tarvol)
                                .AddProperty(DataColumnDef.MTS_asp);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetMTSData(queryBuilder.Query);

            //var result = data.Select(x =>
            //                    new MTSResultModel()
            //                    {
            //                        CustomerNo = x.CustomerNo,
            //                        CustomerName = x.CustomerName,
            //                        MatarialGroupOrBrand = x.MatarialGroupOrBrand,
            //                        TargetVolume = CustomConvertExtension.ObjectToDecimal(x.tarvol),
            //                        ActualVolume = CustomConvertExtension.ObjectToDecimal(x.asp),
            //                        DifferenceVolume = CustomConvertExtension.ObjectToDecimal(x.tarvol) - CustomConvertExtension.ObjectToDecimal(x.asp)
            //                    }).ToList();

            var result = data.GroupBy(x => x.MatarialGroupOrBrand).Select(x =>
                                new MTSResultModel()
                                {
                                    CustomerNo = x.FirstOrDefault().CustomerNo,
                                    CustomerName = x.FirstOrDefault().CustomerName,
                                    MatarialGroupOrBrand = x.FirstOrDefault().MatarialGroupOrBrand,
                                    TargetVolume = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.tarvol)),
                                    ActualVolume = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.asp)),
                                    DifferenceVolume = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.tarvol)) - x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.asp))
                                }).ToList();

            return result;
        }
    }
}

