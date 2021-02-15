using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IDriverDataService _driverDataService;
        private readonly IBrandFamilyDataService _brandFamilyDataService;

        public SalesDataService(IHttpClientService httpClientService, IOptions<ODataSettingsModel> appSettings, IDriverDataService driverDataService, IBrandFamilyDataService brandFamilyDataService)
        {
            _httpClientService = httpClientService;
            _appSettings = appSettings.Value;
            _driverDataService = driverDataService;
            _brandFamilyDataService = brandFamilyDataService;
        }

        private async Task<IList<SalesDataModel>> GetSalesData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.SalesUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<SalesDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            return data;
        }

        public async Task<IList<InvoiceHistoryResultModel>> GetInvoiceHistory(InvoiceHistorySearchModel model)
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
                                .AddProperty(DataColumnDef.CustomerName)
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
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    Division = x.Division,
                                    DivisionName = x.DivisionName,
                                    InvoiceNoOrBillNo = x.InvoiceNoOrBillNo,
                                    Date = x.Date,
                                    NetAmount = x.NetAmount 
                                }
                                ).ToList();

            #region get driver data
            if(result.Any())
            {
                //Stopwatch st = new Stopwatch();
                //st.Start();
                //foreach (var item in result)
                //{
                //    var driverFilterQueryBuilder = new FilterQueryOptionBuilder();
                //    driverFilterQueryBuilder.Equal(DataColumnDef.Driver_InvoiceNoOrBillNo, item.InvoiceNoOrBillNo); 
                //    var driverDatas = (await _driverDataService.GetDriverData(driverFilterQueryBuilder));
                //    var driverData = driverDatas.FirstOrDefault();
                //    if (driverData != null)
                //    {
                //        item.DriverName = driverData.DRIVERNAME;
                //    }
                //}
                //st.Stop();
                //var time = TimeSpan.FromMilliseconds(st.ElapsedMilliseconds).Seconds;
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var allInvoiceNo = result.Select(x => x.InvoiceNoOrBillNo).Distinct();

                var driverFilterQueryBuilder = new FilterQueryOptionBuilder();
                driverFilterQueryBuilder.Equal(DataColumnDef.Driver_InvoiceNoOrBillNo, allInvoiceNo.FirstOrDefault());

                foreach (var invoiceNo in allInvoiceNo.Skip(1))
                {
                    driverFilterQueryBuilder.Or().Equal(DataColumnDef.Driver_InvoiceNoOrBillNo, invoiceNo);
                }

                var allDriverData = await _driverDataService.GetDriverData(driverFilterQueryBuilder);

                foreach (var item in result)
                {
                    var driverData = allDriverData.FirstOrDefault(x => x.InvoiceNoOrBillNo == item.InvoiceNoOrBillNo);
                    if (driverData != null)
                    {
                        item.DriverName = driverData.DriverName;
                        item.DriverMobileNo = driverData.DriverMobileNo;
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<InvoiceItemDetailsResultModel>> GetInvoiceItemDetails(InvoiceItemDetailsSearchModel model)
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

        public async Task<IList<BrandWiseMTDResultModel>> GetBrandWiseMTDDetails(BrandWiseMTDSearchModel model)
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
                                .AddProperty(DataColumnDef.Division)
                                .AddProperty(DataColumnDef.InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrand);

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

            var brandCodes = dataLy.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataCy.Select(x => x.MatarialGroupOrBrand))
                                    .Concat(previousMonthDict.Values.SelectMany(x => x).Select(x => x.MatarialGroupOrBrand))
                                        .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new BrandWiseMTDResultModel();
                res.PreviousMonthData = new Dictionary<string, decimal>();

                if (dataLy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdAmtLy = dataLy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(calcFunc);
                    var brandNameLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameLy : res.MatarialGroupOrBrand;
                    res.LYMTD = mtdAmtLy;
                }

                if (dataCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdAmtCy = dataCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(calcFunc);
                    var brandNameCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameCy : res.MatarialGroupOrBrand;
                    res.CYMTD = mtdAmtCy;
                }

                for (var i = 1; i <= previousMonthCount; i++)
                {
                    int number = i * -1;
                    var monthName = currendate.GetMonthName(number);
                    var dictData = previousMonthDict[monthName].ToList();
                    var mtdAmt = decimal.Zero;

                    if (dictData.Any(x => x.MatarialGroupOrBrand == brandCode))
                    {
                        mtdAmt = dictData.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(calcFunc);
                        var brandName = dictData.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;

                        res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandName : res.MatarialGroupOrBrand;
                    }

                    res.PreviousMonthData.Add(monthName, mtdAmt);
                }

                res.Growth =  res.LYMTD > 0 && res.CYMTD > 0 ? ((res.CYMTD - res.LYMTD) * 100) / res.LYMTD : 
                                res.LYMTD <= 0 && res.CYMTD > 0 ? decimal.Parse("100") : 
                                    decimal.Zero;
                result.Add(res);
            }

            #region get brand data
            if (result.Any())
            {
                var allMaterialBrand = result.Select(x => x.MatarialGroupOrBrand).Distinct();

                var brandFamilyFilterQueryBuilder = new FilterQueryOptionBuilder();
                brandFamilyFilterQueryBuilder.Equal(DataColumnDef.BrandFamily_MatarialGroupOrBrand, allMaterialBrand.FirstOrDefault());

                foreach (var matBrand in allMaterialBrand.Skip(1))
                {
                    brandFamilyFilterQueryBuilder.Or().Equal(DataColumnDef.BrandFamily_MatarialGroupOrBrand, matBrand);
                }

                var allBrandFamilyData = await _brandFamilyDataService.GetBrandFamilyData(brandFamilyFilterQueryBuilder);

                foreach (var item in result)
                {
                    var brandFamilyData = allBrandFamilyData.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand);
                    if (brandFamilyData != null)
                    {
                        item.MatarialGroupOrBrand = brandFamilyData.MatarialGroupOrBrandName;
                    }
                }
            }
            #endregion

            return result;
        }
    }
}

