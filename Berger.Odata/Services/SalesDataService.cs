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
    public class SalesDataService : ISalesDataService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ODataSettingsModel _appSettings;

        public SalesDataService(IHttpClientService httpClientService, IOptions<ODataSettingsModel> appSettings)
        {
            _httpClientService = httpClientService;
            _appSettings = appSettings.Value;
        }

        private async Task<IList<SalesDataModel>> GetSalesData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.SalesUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<SalesDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            return data;
        }

        public async Task<object> GetInvoiceHistory(InvoiceHistorySearchModel model)
        {
            //model.FromDate = "2011.09.01";//(new DateTime(2011, 09, 01)).DateFormat()
            //model.ToDate = "2011.10.01";//(new DateTime(2011, 10, 01)).DateFormat()
            //model.CustomerNo = "24";
            //model.Division = "10";

            var fromDate = model.FromDate.DateFormat();
            var toDate = model.ToDate.DateFormat();

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.CustomerNoOrSoldToParty, model.CustomerNo)
                                .And()
                                .Equal(DataColumnDef.Division, model.Division)
                                .And()
                                .StartGroup()
                                .GreaterThanOrEqual(DataColumnDef.Date, fromDate)
                                .And()
                                .LessThanOrEqual(DataColumnDef.Date, toDate)
                                .EndGroup();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerNo)
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.DivisionName)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetSalesData(queryBuilder.Query);

            var result = data.Select(x => 
                                new InvoiceHistoryResultModel()
                                {
                                    CustomerNoOrSoldToParty = x.CustomerNoOrSoldToParty,
                                    CustomerNo = x.CustomerNo,
                                    Division = x.Division,
                                    DivisionName = x.DivisionName,
                                    InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
                                    Date = x.Date,
                                    NetAmount = x.NetAmount }
                                ).ToList();

            return result;
        }

        public async Task<object> GetInvoiceItemDetails(InvoiceItemDetailsSearchModel model)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.InvoiceNoOrBillNo, model.InvoiceNo);

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Quantity)
                                .AddProperty(DataColumnDef.MatrialCode)
                                .AddProperty(DataColumnDef.MatarialDescription);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetSalesData(queryBuilder.Query);

            var result = data.Select(x => new InvoiceItemDetailsResultModel()
                                            {
                                                NetAmount = x.NetAmount,
                                                Quantity = x.Quantity,
                                                MatrialCode = x.MatrialCode,
                                                MatarialDescription = x.MatarialDescription,
                                            }
                                            ).ToList();

            return result;
        }

        public async Task<object> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model)
        {
            //var currendate = new DateTime(2011, 09, 21);
            var currendate = model.Date;
            var previousMonthCount = 3;

            var cyfd = currendate.GetCYFD().DateFormat();
            var cylcd = currendate.GetCYLCD().DateFormat();

            var lyfd = currendate.GetLYFD().DateFormat();
            var lylcd = currendate.GetLYLCD().DateFormat();

            var dataLy = new List<SalesDataModel>();
            var dataCy = new List<SalesDataModel>();
            var previousMonthDict = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                                .AddProperty(DataColumnDef.CustomerNo)
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.DivisionName)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount)
                                //.AddProperty(DataColumnDef.Volume)
                                //.AddProperty(DataColumnDef.VolumeUnit)
                                .AddProperty(DataColumnDef.MatrialCode)
                                .AddProperty(DataColumnDef.MatarialDescription);

            var topQuery = $"$top=5";

            #region Last Year MTD
            var filterLyQueryBuilder = new FilterQueryOptionBuilder();
            filterLyQueryBuilder.Equal(DataColumnDef.CustomerNoOrSoldToParty, model.CustomerNo)
                                .And()
                                .Equal(DataColumnDef.Division, model.Division)
                                .And()
                                .StartGroup()
                                .GreaterThanOrEqual(DataColumnDef.Date, lyfd)
                                .And()
                                .LessThanOrEqual(DataColumnDef.Date, lylcd)
                                .EndGroup();

            var queryLyBuilder = new QueryOptionBuilder();
            queryLyBuilder.AppendQuery(filterLyQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            dataLy = (await GetSalesData(queryLyBuilder.Query)).ToList();
            #endregion

            #region Current Year MTD
            var filterCyQueryBuilder = new FilterQueryOptionBuilder();
            filterCyQueryBuilder.Equal(DataColumnDef.CustomerNoOrSoldToParty, model.CustomerNo)
                                .And()
                                .Equal(DataColumnDef.Division, model.Division)
                                .And()
                                .StartGroup()
                                .GreaterThanOrEqual(DataColumnDef.Date, cyfd)
                                .And()
                                .LessThanOrEqual(DataColumnDef.Date, cylcd)
                                .EndGroup();

            var queryCyBuilder = new QueryOptionBuilder();
            queryCyBuilder.AppendQuery(filterCyQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            dataCy = (await GetSalesData(queryCyBuilder.Query)).ToList();
            #endregion

            #region Previous Month Data
            for (var i = 1; i <= previousMonthCount; i++)
            {
                int number = i * -1;
                var monthFilterQueryBuilder = new FilterQueryOptionBuilder();
                monthFilterQueryBuilder.Equal(DataColumnDef.CustomerNoOrSoldToParty, model.CustomerNo)
                                    .And()
                                    .Equal(DataColumnDef.Division, model.Division)
                                    .And()
                                    .StartGroup()
                                    .GreaterThanOrEqual(DataColumnDef.Date, currendate.GetMonthDate(number).GetCYFD().DateFormat())
                                    .And()
                                    .LessThanOrEqual(DataColumnDef.Date, currendate.GetMonthDate(number).GetCYLD().DateFormat())
                                    .EndGroup();

                var queryBuilder = new QueryOptionBuilder();
                queryBuilder.AppendQuery(monthFilterQueryBuilder.Filter)
                            //.AppendQuery(topQuery)
                            .AppendQuery(selectQueryBuilder.Select);

                var monthName = currendate.GetMonthName(number);
                var data = await GetSalesData(queryBuilder.Query);

                previousMonthDict.Add(monthName, data);
            }
            #endregion

            Func<SalesDataModel, decimal> calcFunc = x => CustomConvertExtension.ObjectToDecimal(x.NetAmount);
            var result = new List<BrandWiseMTDResultModel>();

            var brandCodes = dataLy.Select(x => x.MatrialCode)
                                .Concat(dataCy.Select(x => x.MatrialCode))
                                    .Concat(previousMonthDict.Values.SelectMany(x => x).Select(x => x.MatrialCode))
                                        .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new BrandWiseMTDResultModel();
                res.PreviousMonthData = new Dictionary<string, decimal>();

                if (dataLy.Any(x => x.MatrialCode == brandCode))
                {
                    var mtdAmtLy = dataLy.Where(x => x.MatrialCode == brandCode).Sum(calcFunc);
                    var brandNameLy = dataLy.FirstOrDefault(x => x.MatrialCode == brandCode).MatarialDescription;

                    res.BrandName = string.IsNullOrEmpty(res.BrandName) ? brandNameLy : res.BrandName;
                    res.LYMTD = mtdAmtLy;
                }

                if (dataCy.Any(x => x.MatrialCode == brandCode))
                {
                    var mtdAmtCy = dataCy.Where(x => x.MatrialCode == brandCode).Sum(calcFunc);
                    var brandNameCy = dataCy.FirstOrDefault(x => x.MatrialCode == brandCode).MatarialDescription;

                    res.BrandName = string.IsNullOrEmpty(res.BrandName) ? brandNameCy : res.BrandName;
                    res.CYMTD = mtdAmtCy;
                }

                for (var i = 1; i <= previousMonthCount; i++)
                {
                    int number = i * -1;
                    var monthName = currendate.GetMonthName(number);
                    var dictData = previousMonthDict[monthName].ToList();
                    var mtdAmt = decimal.Zero;

                    if (dictData.Any(x => x.MatrialCode == brandCode))
                    {
                        mtdAmt = dictData.Where(x => x.MatrialCode == brandCode).Sum(calcFunc);
                        var brandName = dictData.FirstOrDefault(x => x.MatrialCode == brandCode).MatarialDescription;

                        res.BrandName = string.IsNullOrEmpty(res.BrandName) ? brandName : res.BrandName;
                    }

                    res.PreviousMonthData.Add(monthName, mtdAmt);
                }

                res.Growth =  res.LYMTD > 0 ? ((res.CYMTD - res.LYMTD) * 100) / res.LYMTD : decimal.Zero;
                result.Add(res);
            }

            return result;
        }
    }
}

