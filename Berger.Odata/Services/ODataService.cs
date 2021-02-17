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
        #endregion

        #region Get selectable data
        public async Task<IList<DriverDataModel>> GetDriverDataByInvoiceNos(List<string> invoiceNos)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.Driver_InvoiceNoOrBillNo)
                                .AddProperty(DataColumnDef.Driver_DriverName)
                                .AddProperty(DataColumnDef.Driver_DriverMobileNo);

            //var topQuery = $"$top=5";

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.Driver_InvoiceNoOrBillNo, invoiceNos.FirstOrDefault());

            foreach (var invoiceNo in invoiceNos.Skip(1))
            {
                filterQueryBuilder.Or().Equal(DataColumnDef.Driver_InvoiceNoOrBillNo, invoiceNo);
            }

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetDriverData(queryBuilder.Query);

            return data;
        }

        public async Task<IList<BrandFamilyDataModel>> GetBrandFamilyDataByBrands(List<string> brands, bool isFamily = false)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily)
                                .AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamilyName)
                                .AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.BrandFamily_MatarialGroupOrBrandName);

            //var topQuery = $"$top=5";

            var colName = isFamily ? DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily : DataColumnDef.BrandFamily_MatarialGroupOrBrand;

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(colName, brands.FirstOrDefault());

            foreach (var brand in brands.Skip(1))
            {
                filterQueryBuilder.Or().Equal(colName, brand);
            }

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetBrandFamilyData(queryBuilder.Query);

            return data;
        }

        public async Task<IList<SalesDataModel>> GetSalesDataByCustomerAndDivision(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string startDate, string endDate, string division = "-1")
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

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetSalesData(queryBuilder.Query)).ToList();

            return data;
        }

        public async Task<IList<MTSDataModel>> GetMTSDataByCustomerAndDate(SelectQueryOptionBuilder selectQueryBuilder,
            string customerNo, string date)
        {
            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, customerNo)
                                .And()
                                .Equal(DataColumnDef.MTS_Date, date);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = (await GetMTSData(queryBuilder.Query)).ToList();

            return data;
        }
        #endregion
    }
}

