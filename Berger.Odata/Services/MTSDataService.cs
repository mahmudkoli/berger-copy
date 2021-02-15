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
        private readonly IBrandFamilyDataService _brandFamilyDataService;
        private readonly ODataSettingsModel _appSettings;

        public MTSDataService(IHttpClientService httpClientService, IOptions<ODataSettingsModel> appSettings, IBrandFamilyDataService brandFamilyDataService)
        {
            _httpClientService = httpClientService;
            _brandFamilyDataService = brandFamilyDataService;
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
            var currentdate = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, model.CustomerNo)
                                .And()
                                .Equal(DataColumnDef.MTS_Date, currentdate);

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

            #region get brand data
            if (result.Any())
            {
                var allMaterialBrand = result.Select(x => x.MatarialGroupOrBrand).Distinct();

                var brandFamilyFilterQueryBuilder = new FilterQueryOptionBuilder();
                brandFamilyFilterQueryBuilder.Equal(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily, allMaterialBrand.FirstOrDefault());

                foreach (var matBrand in allMaterialBrand.Skip(1))
                {
                    brandFamilyFilterQueryBuilder.Or().Equal(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily, matBrand);
                }

                var allBrandFamilyData = (await _brandFamilyDataService.GetBrandFamilyData(brandFamilyFilterQueryBuilder))
                                            .GroupBy(x => x.MatarialGroupOrBrandFamily).ToList();

                foreach (var item in result)
                {
                    var brandFamilyData = allBrandFamilyData.FirstOrDefault(x => x.Key == item.MatarialGroupOrBrand);
                    if (brandFamilyData != null)
                    {
                        item.MatarialGroupOrBrand = string.Join(", ", brandFamilyData.Select(x => x.MatarialGroupOrBrandName));
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<IList<PerformanceResultModel>> GetPremiumBrandPerformance(MTSSearchModel model)
        {
            var currentdate = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var previousDate = (new DateTime(model.Year, model.Month, 01)).GetLYFD();
            var lysmDate =  $"{string.Format("{0:0000}", previousDate.Year)}.{string.Format("{0:00}", previousDate.Month)}";

            var dataLy = new List<MTSDataModel>();
            var dataCy = new List<MTSDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume)
                                .AddProperty(DataColumnDef.MTS_AverageSalesPrice);

            //var topQuery = $"$top=5";

            #region Last Year Volume
            var filterLyQueryBuilder = new FilterQueryOptionBuilder();
            filterLyQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, model.CustomerNo)
                                .And()
                                .Equal(DataColumnDef.MTS_Date, lysmDate);

            var queryLyBuilder = new QueryOptionBuilder();
            queryLyBuilder.AppendQuery(filterLyQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            dataLy = (await GetMTSData(queryLyBuilder.Query)).ToList();
            #endregion

            #region Current Year Volume
            var filterCyQueryBuilder = new FilterQueryOptionBuilder();
            filterCyQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, model.CustomerNo)
                                .And()
                                .Equal(DataColumnDef.MTS_Date, currentdate);

            var queryCyBuilder = new QueryOptionBuilder();
            queryCyBuilder.AppendQuery(filterCyQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            dataCy = (await GetMTSData(queryCyBuilder.Query)).ToList();
            #endregion

            Func<MTSDataModel, decimal> actVolCalcFunc = x => CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice);
            Func<MTSDataModel, decimal> tarVolCalcFunc = x => CustomConvertExtension.ObjectToDecimal(x.TargetVolume);
            var result = new List<PerformanceResultModel>();

            var brandCodes = dataLy.Select(x => x.MatarialGroupOrBrand)
                                .Concat(dataCy.Select(x => x.MatarialGroupOrBrand))
                                        .Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new PerformanceResultModel();

                if (dataLy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdAmtLy = dataLy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(actVolCalcFunc);
                    var brandNameLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var customerNoLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var customerNameLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameLy : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? customerNoLy : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? customerNameLy : res.CustomerName;
                    res.LYSMVolume = mtdAmtLy;
                }

                if (dataCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdTarAmtCy = dataCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(tarVolCalcFunc);
                    var mtdActAmtCy = dataCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(actVolCalcFunc);
                    var brandNameCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var customerNoCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var customerNameCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameCy : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? customerNoCy : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? customerNameCy : res.CustomerName;
                    res.TargetVolume = mtdTarAmtCy;
                    res.ActualVolume = mtdActAmtCy;
                }

                res.TargetAchievement = res.TargetVolume > 0 ? (res.ActualVolume / res.TargetVolume) : decimal.Zero;
                res.TillDateGrowth = res.LYSMVolume > 0 ? (res.ActualVolume / res.LYSMVolume - 1) : decimal.Zero;

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

                var allBrandFamilyData = (await _brandFamilyDataService.GetBrandFamilyData(brandFamilyFilterQueryBuilder)).ToList();

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

        public async Task<IList<ValueTargetResultModel>> GetValueTarget(MTSSearchModel model)
        {
            var currentdate = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";

            var filterQueryBuilder = new FilterQueryOptionBuilder();
            filterQueryBuilder.Equal(DataColumnDef.MTS_CustomerNo, model.CustomerNo)
                                .And()
                                .Equal(DataColumnDef.MTS_Date, currentdate);

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetValue)
                                .AddProperty(DataColumnDef.MTS_AverageSalesPrice);

            //var topQuery = $"$top=5";

            var queryBuilder = new QueryOptionBuilder();
            queryBuilder.AppendQuery(filterQueryBuilder.Filter)
                        //.AppendQuery(topQuery)
                        .AppendQuery(selectQueryBuilder.Select);

            var data = await GetMTSData(queryBuilder.Query);

            var result = data.Select(x =>
                                new ValueTargetResultModel()
                                {
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    //MatarialGroupOrBrand = x.MatarialGroupOrBrand,
                                    TargetValue = CustomConvertExtension.ObjectToDecimal(x.TargetValue),
                                    ActualValue = CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice),
                                    DifferenceValue = CustomConvertExtension.ObjectToDecimal(x.TargetValue) - CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice)
                                }).ToList();

            #region get brand data
            //if (result.Any())
            //{
            //    var allMaterialBrand = result.Select(x => x.MatarialGroupOrBrand).Distinct();

            //    var brandFamilyFilterQueryBuilder = new FilterQueryOptionBuilder();
            //    brandFamilyFilterQueryBuilder.Equal(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily, allMaterialBrand.FirstOrDefault());

            //    foreach (var matBrand in allMaterialBrand.Skip(1))
            //    {
            //        brandFamilyFilterQueryBuilder.Or().Equal(DataColumnDef.BrandFamily_MatarialGroupOrBrandFamily, matBrand);
            //    }

            //    var allBrandFamilyData = (await _brandFamilyDataService.GetBrandFamilyData(brandFamilyFilterQueryBuilder)).ToList();

            //    foreach (var item in result)
            //    {
            //        var brandFamilyData = allBrandFamilyData.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand);
            //        if (brandFamilyData != null)
            //        {
            //            item.MatarialGroupOrBrand = brandFamilyData.MatarialGroupOrBrandName;
            //        }
            //    }
            //}
            #endregion

            var returnResult = new List<ValueTargetResultModel>();
            if(result.Any())
            {
                var res = new ValueTargetResultModel();
                res.CustomerNo = result.FirstOrDefault().CustomerNo;
                res.CustomerName = result.FirstOrDefault().CustomerName;
                res.TargetValue = result.Sum(x => x.TargetValue);
                res.ActualValue = result.Sum(x => x.ActualValue);
                res.DifferenceValue = res.TargetValue - res.ActualValue;
                returnResult.Add(res);
            }

            return returnResult;
        }
    }
}

