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
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;

        public MTSDataService(
            IODataService odataService,
            IODataBrandService odataBrandService
            )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
        }

        //TODO: need to update Actual value and volume
        public async Task<IList<MTSResultModel>> GetMTSBrandsVolume(MTSSearchModel model)
        {
            var currentdate = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var currentDateFull = (new DateTime(model.Year, model.Month, 1));
            var fromDate = currentDateFull.DateFormat();
            var toDate = currentDateFull.GetCYLD().DateFormat();
            var mtsBrandCodes = new List<string>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume);
                                //.AddProperty(DataColumnDef.MTS_AverageSalesPrice);

            //TODO: get MTS brand codes and add to list
            mtsBrandCodes = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            var data = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentdate, mtsBrandCodes)).ToList();

            var result = data.Select(x =>
                                new MTSResultModel()
                                {
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    MatarialGroupOrBrand = x.MatarialGroupOrBrand,
                                    TargetVolume = CustomConvertExtension.ObjectToDecimal(x.TargetVolume),
                                    //ActualVolume = CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice),
                                    //DifferenceVolume = CustomConvertExtension.ObjectToDecimal(x.TargetVolume) - CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice)
                                }).ToList();

            #region get actual data
            if(result.Any())
            {
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var brandsActualValueVolume = await GetSalesDataValueVolume(model.CustomerNo, fromDate, toDate, brands);

                foreach (var item in result)
                {
                    item.ActualVolume = brandsActualValueVolume.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand).ActualVolume;
                    item.DifferenceVolume = item.TargetVolume - item.ActualVolume;
                }
            }
            #endregion

            #region get brand data
            if (result.Any())
            {
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var allBrandFamilyData = (await _odataService.GetBrandFamilyDataByBrands(brands, true))
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
            //var lyDate = (new DateTime(model.Year, model.Month, 1)).GetLYFD();
            //var lysmDate =  $"{string.Format("{0:0000}", lyDate.Year)}.{string.Format("{0:00}", lyDate.Month)}";
            var currentDateFull = (new DateTime(model.Year, model.Month, 1));
            var lyFromDate = currentDateFull.GetLYFD().DateFormat();
            var lyToDate = currentDateFull.GetLYLD().DateFormat();
            var cyFromDate = currentDateFull.GetCYFD().DateFormat();
            var cyToDate = currentDateFull.GetCYLD().DateFormat();
            var premiumBrandCodes = new List<string>();

            //var dataLy = new List<MTSDataModel>();
            var dataCy = new List<MTSDataModel>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetVolume)
                                .AddProperty(DataColumnDef.MTS_AverageSalesPrice);

            //TODO: get premiumB brand codes and add to list
            premiumBrandCodes = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            //dataLy = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, lysmDate, premiumBrandCodes)).ToList();

            dataCy = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentdate, premiumBrandCodes)).ToList();
            
            //Func<MTSDataModel, decimal> actVolCalcFunc = x => CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice);
            Func<MTSDataModel, decimal> tarVolCalcFunc = x => CustomConvertExtension.ObjectToDecimal(x.TargetVolume);
            var result = new List<PerformanceResultModel>();

            //var brandCodes = dataLy.Select(x => x.MatarialGroupOrBrand)
            //                    .Concat(dataCy.Select(x => x.MatarialGroupOrBrand))
            //                            .Distinct().ToList();

            var brandCodes = dataCy.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

            foreach (var brandCode in brandCodes)
            {
                var res = new PerformanceResultModel();

                //if (dataLy.Any(x => x.MatarialGroupOrBrand == brandCode))
                //{
                //    var mtdAmtLy = dataLy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(actVolCalcFunc);
                //    var brandNameLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                //    var customerNoLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                //    var customerNameLy = dataLy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                //    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameLy : res.MatarialGroupOrBrand;
                //    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? customerNoLy : res.CustomerNo;
                //    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? customerNameLy : res.CustomerName;
                //    res.LYSMVolume = mtdAmtLy;
                //}

                if (dataCy.Any(x => x.MatarialGroupOrBrand == brandCode))
                {
                    var mtdTarAmtCy = dataCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(tarVolCalcFunc);
                    //var mtdActAmtCy = dataCy.Where(x => x.MatarialGroupOrBrand == brandCode).Sum(actVolCalcFunc);
                    var brandNameCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).MatarialGroupOrBrand;
                    var customerNoCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerNo;
                    var customerNameCy = dataCy.FirstOrDefault(x => x.MatarialGroupOrBrand == brandCode).CustomerName;

                    res.MatarialGroupOrBrand = string.IsNullOrEmpty(res.MatarialGroupOrBrand) ? brandNameCy : res.MatarialGroupOrBrand;
                    res.CustomerNo = string.IsNullOrEmpty(res.CustomerNo) ? customerNoCy : res.CustomerNo;
                    res.CustomerName = string.IsNullOrEmpty(res.CustomerName) ? customerNameCy : res.CustomerName;
                    res.TargetVolume = mtdTarAmtCy;
                    //res.ActualVolume = mtdActAmtCy;
                }

                //res.TargetAchievement = res.TargetVolume > 0 ? (res.ActualVolume / res.TargetVolume) : decimal.Zero;
                //res.TillDateGrowth = res.LYSMVolume > 0 ? (res.ActualVolume / res.LYSMVolume - 1) : decimal.Zero;

                result.Add(res);
            }

            #region get actual data
            if (result.Any())
            {
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var lyBrandsActualValueVolume = await GetSalesDataValueVolume(model.CustomerNo, lyFromDate, lyToDate, brands);
                var cyBrandsActualValueVolume = await GetSalesDataValueVolume(model.CustomerNo, cyFromDate, cyToDate, brands);

                foreach (var item in result)
                {
                    item.LYSMVolume = lyBrandsActualValueVolume.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand).ActualVolume;
                    item.ActualVolume = cyBrandsActualValueVolume.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand).ActualVolume;
                    item.TargetAchievement = item.TargetVolume > 0 ? (item.ActualVolume / item.TargetVolume) : decimal.Zero;
                    item.TillDateGrowth = item.LYSMVolume > 0 ? (item.ActualVolume / item.LYSMVolume - 1) : decimal.Zero;
                }
            }
            #endregion

            #region get brand data
            if (result.Any())
            {
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var allBrandFamilyData = (await _odataService.GetBrandFamilyDataByBrands(brands)).ToList();

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

        public async Task<IList<ValueTargetResultModel>> GetMonthlyValueTarget(MTSSearchModel model)
        {
            var currentdate = $"{string.Format("{0:0000}", model.Year)}.{string.Format("{0:00}", model.Month)}";
            var currentDateFull = (new DateTime(model.Year, model.Month, 1));
            var fromDate = currentDateFull.DateFormat();
            var toDate = currentDateFull.GetCYLD().DateFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.MTS_TargetValue)
                                .AddProperty(DataColumnDef.MTS_AverageSalesPrice);

            var data = (await _odataService.GetMTSDataByCustomerAndDate(selectQueryBuilder, model.CustomerNo, currentdate)).ToList();

            var result = data.Select(x =>
                                new ValueTargetTempResultModel()
                                {
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    MatarialGroupOrBrand = x.MatarialGroupOrBrand,
                                    TargetValue = CustomConvertExtension.ObjectToDecimal(x.TargetValue),
                                    //ActualValue = CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice),
                                    //DifferenceValue = CustomConvertExtension.ObjectToDecimal(x.TargetValue) - CustomConvertExtension.ObjectToDecimal(x.AverageSalesPrice)
                                }).ToList();

            #region get actual data
            if (result.Any())
            {
                var brands = result.Select(x => x.MatarialGroupOrBrand).Distinct().ToList();

                var brandsActualValueVolume = await GetSalesDataValueVolume(model.CustomerNo, fromDate, toDate, brands);

                foreach (var item in result)
                {
                    item.ActualValue = brandsActualValueVolume.FirstOrDefault(x => x.MatarialGroupOrBrand == item.MatarialGroupOrBrand).ActualValue;
                    item.DifferenceValue = item.TargetValue - item.ActualValue;
                }
            }
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

        private async Task<IList<(string MatarialGroupOrBrand, decimal ActualValue, decimal ActualVolume)>> GetSalesDataValueVolume(string customerNo, string fromDate, string toDate, List<string> brands)
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(DataColumnDef.CustomerNo)
                                .AddProperty(DataColumnDef.MatarialGroupOrBrand)
                                .AddProperty(DataColumnDef.NetAmount)
                                .AddProperty(DataColumnDef.Volume);

            var data = (await _odataService.GetSalesDataByCustomerAndDivision(selectQueryBuilder, customerNo, fromDate, toDate, brands: brands)).ToList();

            var result = data.GroupBy(x => x.MatarialGroupOrBrand).Select(x => 
                                                                (
                                                                    MatarialGroupOrBrand: x.Key,
                                                                    ActualValue: x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount)),
                                                                    ActualVolume: x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                                                                )).ToList();

            return result;
        }
    }
}

