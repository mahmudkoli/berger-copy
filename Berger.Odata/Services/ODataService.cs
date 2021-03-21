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
    public class ODataService : IODataService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ODataSettingsModel _appSettings;

        public ODataService(
            IHttpClientService httpClientService,
            IOptions<ODataSettingsModel> appSettings
            )
        {
            _httpClientService = httpClientService;
            _appSettings = appSettings.Value;
        }

        #region get data
        public async Task<IList<SalesDataModel>> GetSalesData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.SalesUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<SalesDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            //return await Task.FromResult(data);
            return await Task.Run(() => data);
        }

        public async Task<IList<MTSDataModel>> GetMTSData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.MTSUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<MTSDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            //return await Task.FromResult(data);
            return await Task.Run(() => data);
        }

        public async Task<IList<DriverDataModel>> GetDriverData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.DriverUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<DriverDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            //return await Task.FromResult(data);
            return await Task.Run(() => data);
        }

        public async Task<IList<BrandFamilyDataModel>> GetBrandFamilyData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.BrandFamilyUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<BrandFamilyDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            //return await Task.FromResult(data);
            return await Task.Run(() => data);
        }

        public async Task<IList<FinancialDataModel>> GetFinancialData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.FinancialUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<FinancialDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            //return await Task.FromResult(data);
            return await Task.Run(() => data);
        }

        public async Task<IList<BalanceDataModel>> GetBalanceData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.BalanceUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<BalanceDataRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            //return await Task.FromResult(data);
            return await Task.Run(() => data);
        }

        public async Task<IList<CustomerDataModel>> GetCustomerData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.CustomerUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<CustomerDataModel>.ParseJson(responseBody);
            var data = parsedData.Results.ToList();

            //return await Task.FromResult(data);
            return await Task.Run(() => data);
        }
        #endregion

        #region Get selectable data
        public async Task<DriverDataModel> GetDriverDataByInvoiceNo(string invoiceNo)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.Driver_InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Driver_DriverName)
                                .AddProperty(DataColumnDef.Driver_DriverMobileNo);

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.Driver_InvoiceNoOrBillNo, invoiceNo);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetDriverData(queryBuilder.Query);

            return data.FirstOrDefault();
        }

        public async Task<IList<BrandFamilyDataModel>> GetBrandFamilyDataByBrands(List<string> brands = null, bool isFamily = false)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily)
                                .AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamilyName)
                                .AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrandName);

            var filterQueryBuilder = new FilterQueryOptionBuilder();

            if (brands != null && brands.Any())
            {
                var colName = isFamily ? DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily : DataColumnDef.BrandFamily_MatarialGroupOrBrand;

                filterQueryBuilder.Equal(colName, brands.FirstOrDefault());

                foreach (var brand in brands.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(colName, brand);
                }
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetBrandFamilyData(queryBuilder.Query);

            return data;
        }

        public async Task<IList<SalesDataModel>> GetSalesDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate, string endDate, string division = "-1", List<string> materialCodes = null, List<string> brands = null)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.CustomerNoOrSoldToParty, customerNo)
                                .And()
                                .StartGroup()
                                .GreaterThanOrEqual(DataColumnDef.Date, startDate)
                                .And()
                                .LessThanOrEqual(DataColumnDef.Date, endDate)
                                .EndGroup();

            if (division != "-1")
            {
                filterQueryBuilder.And().Equal(DataColumnDef.Division, division);
            }

            if (materialCodes != null && materialCodes.Any())
            {
                filterQueryBuilder.And().StartGroup().Equal(DataColumnDef.MatrialCode, materialCodes.FirstOrDefault());

                foreach (var materialCode in materialCodes.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(DataColumnDef.MatrialCode, materialCode);
                }

                filterQueryBuilder.EndGroup();
            }

            if (brands != null && brands.Any())
            {
                filterQueryBuilder.And().StartGroup().Equal(DataColumnDef.MatarialGroupOrBrand, brands.FirstOrDefault());

                foreach (var brand in brands.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(DataColumnDef.MatarialGroupOrBrand, brand);
                }

                filterQueryBuilder.EndGroup();
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetSalesData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<SalesDataModel>> GetSalesDataByMultipleCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            IList<int> dealerList, string startDate, string endDate, string division = "-1", List<string> materialCodes = null, List<string> brands = null)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();

            if (dealerList.Any())
            {
                filterQueryBuilder.StartGroup();
                for (int i = 0; i < dealerList.Count; i++)
                {
                    filterQueryBuilder.Equal(DataColumnDef.CustomerNoOrSoldToParty, dealerList[i].ToString());

                    if (i + 1 != dealerList.Count)
                    {
                        filterQueryBuilder.Or();
                    }
                }
                filterQueryBuilder.EndGroup().And();
            }
            filterQueryBuilder.StartGroup()
                                .GreaterThanOrEqual(DataColumnDef.Date, startDate)
                                .And()
                                .LessThanOrEqual(DataColumnDef.Date, endDate)
                                .EndGroup();

            if (division != "-1")
            {
                filterQueryBuilder.And().Equal(DataColumnDef.Division, division);
            }

            if (materialCodes != null && materialCodes.Any())
            {
                filterQueryBuilder.And().StartGroup().Equal(DataColumnDef.MatrialCode, materialCodes.FirstOrDefault());

                foreach (var materialCode in materialCodes.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(DataColumnDef.MatrialCode, materialCode);
                }

                filterQueryBuilder.EndGroup();
            }

            if (brands != null && brands.Any())
            {
                filterQueryBuilder.And().StartGroup().Equal(DataColumnDef.MatarialGroupOrBrand, brands.FirstOrDefault());

                foreach (var brand in brands.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(DataColumnDef.MatarialGroupOrBrand, brand);
                }

                filterQueryBuilder.EndGroup();
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetSalesData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<SalesDataModel>> GetSalesDataByTerritory(SelectQueryOptionBuilder selectQueryBuilder,
            string startDate, string endDate, string territory = "-1", List<string> brands = null)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.StartGroup()
                                .GreaterThanOrEqual(DataColumnDef.Date, startDate)
                                .And()
                                .LessThanOrEqual(DataColumnDef.Date, endDate)
                                .EndGroup();

            if (territory != "-1")
            {
                filterQueryBuilder.And().Equal(DataColumnDef.Territory, territory);
            }

            if (brands != null && brands.Any())
            {
                filterQueryBuilder.And().StartGroup().Equal(DataColumnDef.MatarialGroupOrBrand, brands.FirstOrDefault());

                foreach (var brand in brands.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(DataColumnDef.MatarialGroupOrBrand, brand);
                }

                filterQueryBuilder.EndGroup();
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetSalesData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<MTSDataModel>> GetMTSDataByCustomerAndDate(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string date, List<string> brands = null)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, customerNo)
                                .And()
                                .Equal(DataColumnDef.MTS_Date, date);

            if (brands != null && brands.Any())
            {
                filterQueryBuilder.And().StartGroup().Equal(DataColumnDef.MTS_MatarialGroupOrBrand, brands.FirstOrDefault());

                foreach (var brand in brands.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(DataColumnDef.MTS_MatarialGroupOrBrand, brand);
                }

                filterQueryBuilder.EndGroup();
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetMTSData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<MTSDataModel>> GetMTSDataByTerritory(SelectQueryOptionBuilder selectQueryBuilder,
            string date, string territory = "-1", List<string> brands = null)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_Date, date);

            if (territory != "-1")
            {
                filterQueryBuilder.And().Equal(DataColumnDef.MTS_Territory, territory);
            }

            if (brands != null && brands.Any())
            {
                filterQueryBuilder.And().StartGroup().Equal(DataColumnDef.MTS_MatarialGroupOrBrand, brands.FirstOrDefault());

                foreach (var brand in brands.Skip(1))
                {
                    filterQueryBuilder.Or().Equal(DataColumnDef.MTS_MatarialGroupOrBrand, brand);
                }

                filterQueryBuilder.EndGroup();
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetMTSData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<FinancialDataModel>> GetFinancialDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "")
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(FinancialColDef.CompanyCode, "1000")
                                .And()
                                .Equal(FinancialColDef.CustomerLow, customerNo);
            //.And()
            //.Equal(FinancialColDef.CreditControlArea, creditControlArea)
            //.And()
            //.StartGroup()
            //.GreaterThanOrEqualDateTime(FinancialColDef.Date, startDate)
            //.And()
            //.LessThanOrEqualDateTime(FinancialColDef.Date, endDate)
            //.EndGroup();

            if (!string.IsNullOrEmpty(creditControlArea))
            {
                filterQueryBuilder.And().Equal(FinancialColDef.CreditControlArea, creditControlArea);
            }

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                filterQueryBuilder.And()
                                .StartGroup()
                                .GreaterThanOrEqualDateTime(FinancialColDef.Date, startDate)
                                .And()
                                .LessThanOrEqualDateTime(FinancialColDef.Date, endDate)
                                .EndGroup();
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                filterQueryBuilder.And().GreaterThanOrEqualDateTime(FinancialColDef.Date, startDate);
            }
            else if (!string.IsNullOrEmpty(endDate))
            {
                filterQueryBuilder.And().LessThanOrEqualDateTime(FinancialColDef.Date, endDate);
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetFinancialData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<BalanceDataModel>> GetBalanceDataByCustomerAndCreditControlArea(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate = "", string endDate = "", string creditControlArea = "")
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(BalanceColDef.CompanyCode, "1000")
                                .And()
                                .Equal(BalanceColDef.CustomerLow, customerNo);

            if (!string.IsNullOrEmpty(creditControlArea))
            {
                filterQueryBuilder.And().Equal(BalanceColDef.CreditControlArea, creditControlArea);
            }

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                filterQueryBuilder.And()
                                .StartGroup()
                                .GreaterThanOrEqualDateTime(BalanceColDef.PostingDate, startDate)
                                .And()
                                .LessThanOrEqualDateTime(BalanceColDef.PostingDate, endDate)
                                .EndGroup();
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                filterQueryBuilder.And().GreaterThanOrEqualDateTime(BalanceColDef.PostingDate, startDate);
            }
            else if (!string.IsNullOrEmpty(endDate))
            {
                filterQueryBuilder.And().LessThanOrEqualDateTime(BalanceColDef.PostingDate, endDate);
            }

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetBalanceData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<CustomerDataModel>> GetCustomerDataByCustomerNo(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(nameof(CustomerDataModel.CustomerNo), customerNo);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetCustomerData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<MTSDataModel>> GetMtsDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder, string customerNo, string compareMonth, string division = "-1")
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, customerNo)
                .And().Equal(DataColumnDef.MTS_Date, compareMonth);

            if (division != "-1")
            {
                filterQueryBuilder.And().Equal(DataColumnDef.Division, division);
            }

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                //.AppendQuery(topQuery)
                .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetMTSData(queryBuilder.Query)).ToList();

            return data;
        }
        public async Task<IList<MTSDataModel>> GetMtsDataByMultipleCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder, IList<int> dealerIds, string compareMonth, string division = "-1")
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();


            if (dealerIds.Any())
            {
                filterQueryBuilder.StartGroup();
                for (int i = 0; i < dealerIds.Count; i++)
                {
                    filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, dealerIds[i].ToString());

                    if (i + 1 != dealerIds.Count)
                    {
                        filterQueryBuilder.Or();
                    }
                }
                filterQueryBuilder.EndGroup().And();
            }
            filterQueryBuilder.Equal(DataColumnDef.MTS_Date, compareMonth);

            if (division != "-1")
            {
                filterQueryBuilder.And().Equal(DataColumnDef.Division, division);
            }

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                //.AppendQuery(topQuery)
                .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetMTSData(queryBuilder.Query)).ToList();

            return data;
        }

        #endregion

        #region calculate data
        public decimal GetGrowth(decimal first, decimal second)
        {
            return first > 0 && second > 0 ? ((second - first) * 100) / first :
                        first <= 0 && second > 0 ? decimal.Parse("100.000") :
                            decimal.Zero;
        }

        public decimal GetAchivement(decimal target, decimal actual)
        {
            return target > 0 ? ((actual / target)) * 100 : decimal.Zero;
        }

        public decimal GetTillDateGrowth(decimal first, decimal second, int totalDays, int countDays)
        {
            first = (first / totalDays) * countDays;

            return first > 0 && second > 0 ? ((second - first) * 100) / first :
                        first <= 0 && second > 0 ? decimal.Parse("100.000") :
                            decimal.Zero;
        }

        public decimal GetTillDateAchivement(decimal target, decimal actual, int totalDays, int countDays)
        {
            target = (target / totalDays) * countDays;

            return target > 0 ? ((actual / target)) * 100 : decimal.Zero;
        }
        #endregion
    }
}

