using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public class KpiDataService : IKpiDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;
        private readonly ApplicationDbContext _context;

        public KpiDataService(
            IODataService odataService,
            IODataBrandService odataBrandService,
            ApplicationDbContext context
        )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _context = context;
        }

        public async Task<List<TerritoryTargetAchievementResultModel>> GetTerritoryTargetAchivement(SalesTargetAchievementSearchModel model)
        {
            var liquidBrands = new List<string>();
            var powderBrands = new List<string>();

            var liquidTarget = new List<MTSDataModel>();
            var powderTarget = new List<MTSDataModel>();
            var valueTarget = new List<MTSDataModel>();

            var liquidActual = new List<SalesDataModel>();
            var powderActual = new List<SalesDataModel>();
            var valueActual = new List<SalesDataModel>();

            var reportResult = new List<TerritoryTargetAchievementResultModel>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder.AddProperty(DataColumnDef.MTS_Territory)
                                    .AddProperty(DataColumnDef.MTS_TargetValue)
                                    .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder.AddProperty(DataColumnDef.Territory)
                                    .AddProperty(DataColumnDef.NetAmount)
                                    .AddProperty(DataColumnDef.MatarialGroupOrBrand);

            liquidBrands = (await _odataBrandService.GetLiquidBrandCodesAsync()).ToList();
            powderBrands = (await _odataBrandService.GetPowderBrandCodesAsync()).ToList();

            valueTarget = (await _odataService.GetMTSData(selectTargetQueryBuilder, 
                            model.FromDate.MTSSearchDateFormat(), model.ToDate.MTSSearchDateFormat(),
                            depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories)).ToList();

            liquidTarget = valueTarget.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderTarget = valueTarget.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            valueActual = (await _odataService.GetSalesData(selectActualQueryBuilder, 
                            model.FromDate.SalesSearchDateFormat(), model.ToDate.SalesSearchDateFormat(),
                            depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories)).ToList();
            
            liquidActual = valueActual.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderActual = valueActual.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            var territoies = (from a in valueActual select a.Territory).Union(from t in valueTarget select t.Territory).Distinct().ToList();

            decimal target = 0;
            decimal actual = 0;

            foreach (var territory in territoies)
            {
                var result = new TerritoryTargetAchievementResultModel()
                {
                    Territory = territory,

                    LiquidTarget = target = liquidTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    LiquidActual = actual = liquidActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    LiquidAcv = _odataService.GetAchivement(target, actual),

                    PowderTarget = target = powderTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    PowderActual = actual = powderActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    PowderAcv = _odataService.GetAchivement(target, actual),

                    ValueTarget = target = valueTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    ValueActual = actual = valueActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    ValueAcv = _odataService.GetAchivement(target, actual),
                };

                reportResult.Add(result);
            }

            return reportResult;
        }

        public async Task<List<AppTargetAchievementResultModel>> GetAppSalesTargetAchievement(SalesTargetAchievementSearchModel model)
        {
            var result = await this.GetTerritoryTargetAchivement(model);

            var reportResult = new List<AppTargetAchievementResultModel>();

            var resLiquid = new AppTargetAchievementResultModel();
            resLiquid.Category = "Liquid";
            resLiquid.Target = result.Sum(x => x.LiquidTarget);
            resLiquid.Actual = result.Sum(x => x.LiquidActual);
            resLiquid.Achievement = _odataService.GetAchivement(resLiquid.Target, resLiquid.Actual);
            reportResult.Add(resLiquid);

            var resPowder = new AppTargetAchievementResultModel();
            resPowder.Category = "Powder";
            resPowder.Target = result.Sum(x => x.PowderTarget);
            resPowder.Actual = result.Sum(x => x.PowderActual);
            resPowder.Achievement = _odataService.GetAchivement(resPowder.Target, resPowder.Actual);
            reportResult.Add(resPowder);

            var resValue = new AppTargetAchievementResultModel();
            resValue.Category = "Value";
            resValue.Target = result.Sum(x => x.ValueTarget);
            resValue.Actual = result.Sum(x => x.ValueActual);
            resValue.Achievement = _odataService.GetAchivement(resValue.Target, resValue.Actual);
            reportResult.Add(resValue);

            return reportResult;
        }

        public async Task<List<DealerWiseTargetAchievementResultModel>> GetDealerWiseTargetAchivement(DealerWiseTargetAchievementSearchModel model)
        {
            var liquidBrands = new List<string>();
            var powderBrands = new List<string>();

            var liquidTarget = new List<MTSDataModel>();
            var powderTarget = new List<MTSDataModel>();
            var valueTarget = new List<MTSDataModel>();

            var liquidActual = new List<SalesDataModel>();
            var powderActual = new List<SalesDataModel>();
            var valueActual = new List<SalesDataModel>();

            var reportResult = new List<DealerWiseTargetAchievementResultModel>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder.AddProperty(DataColumnDef.MTS_Territory)
                                    .AddProperty(DataColumnDef.MTS_CustomerNo)
                                    .AddProperty(DataColumnDef.MTS_CustomerName)
                                    .AddProperty(DataColumnDef.MTS_TargetValue)
                                    .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder.AddProperty(DataColumnDef.Territory)
                                    .AddProperty(DataColumnDef.CustomerNo)
                                    .AddProperty(DataColumnDef.CustomerName)
                                    .AddProperty(DataColumnDef.NetAmount)
                                    .AddProperty(DataColumnDef.MatarialGroupOrBrand);

            liquidBrands = (await _odataBrandService.GetLiquidBrandCodesAsync()).ToList();
            powderBrands = (await _odataBrandService.GetPowderBrandCodesAsync()).ToList();

            valueTarget = (await _odataService.GetMTSData(selectTargetQueryBuilder, 
                model.FromDate.MTSSearchDateFormat(), model.ToDate.MTSSearchDateFormat(),
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, 
                customerNo: model.CustomerNo)).ToList();

            liquidTarget = valueTarget.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderTarget = valueTarget.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            valueActual = (await _odataService.GetSalesData(selectActualQueryBuilder, 
                model.FromDate.SalesSearchDateFormat(), model.ToDate.SalesSearchDateFormat(),
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, 
                customerNo: model.CustomerNo)).ToList();

            liquidActual = valueActual.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderActual = valueActual.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            var customerNos = (from a in valueActual select a.CustomerNo).Union(from t in valueTarget select t.CustomerNo).Distinct().ToList();

            decimal target = 0;
            decimal actual = 0;

            foreach (var customerNo in customerNos)
            {
                var valTar = valueTarget.Where(x => x.CustomerNo == customerNo);
                var valAct = valueActual.Where(x => x.CustomerNo == customerNo);

                var result = new DealerWiseTargetAchievementResultModel()
                {

                    CustomerNo = customerNo,
                    CustomerName = valTar.FirstOrDefault()?.CustomerName ?? valAct.FirstOrDefault()?.CustomerName,
                    Territory = valTar.FirstOrDefault()?.Territory ?? valAct.FirstOrDefault()?.Territory,

                    LiquidTarget = target = liquidTarget.Where(x => x.CustomerNo == customerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    LiquidActual = actual = liquidActual.Where(x => x.CustomerNo == customerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    LiquidAcv = _odataService.GetAchivement(target, actual),

                    PowderTarget = target = powderTarget.Where(x => x.CustomerNo == customerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    PowderActual = actual = powderActual.Where(x => x.CustomerNo == customerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    PowderAcv = _odataService.GetAchivement(target, actual),

                    ValueTarget = target = valTar.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    ValueActual = actual = valAct.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    ValueAcv = _odataService.GetAchivement(target, actual),
                };

                reportResult.Add(result);
            }

            return reportResult;
        }

        public async Task<List<AppTargetAchievementResultModel>> GetAppDealerWiseTargetAchievement(DealerWiseTargetAchievementSearchModel model)
        {
            var result = await this.GetDealerWiseTargetAchivement(model);

            var reportResult = new List<AppTargetAchievementResultModel>();

            var resLiquid = new AppTargetAchievementResultModel();
            resLiquid.Category = "Liquid";
            resLiquid.Target = result.Sum(x => x.LiquidTarget);
            resLiquid.Actual = result.Sum(x => x.LiquidActual);
            resLiquid.Achievement = _odataService.GetAchivement(resLiquid.Target, resLiquid.Actual);
            reportResult.Add(resLiquid);

            var resPowder = new AppTargetAchievementResultModel();
            resPowder.Category = "Powder";
            resPowder.Target = result.Sum(x => x.PowderTarget);
            resPowder.Actual = result.Sum(x => x.PowderActual);
            resPowder.Achievement = _odataService.GetAchivement(resPowder.Target, resPowder.Actual);
            reportResult.Add(resPowder);

            var resValue = new AppTargetAchievementResultModel();
            resValue.Category = "Value";
            resValue.Target = result.Sum(x => x.ValueTarget);
            resValue.Actual = result.Sum(x => x.ValueActual);
            resValue.Achievement = _odataService.GetAchivement(resValue.Target, resValue.Actual);
            reportResult.Add(resValue);

            return reportResult;
        }

        public async Task<List<ProductWiseTargetAchievementResultModel>> GetProductWiseTargetAchivement(ProductWiseTargetAchievementSearchModel model)
        {
            var valueTarget = new List<MTSDataModel>();
            var valueActual = new List<SalesDataModel>();
            var reportResult = new List<ProductWiseTargetAchievementResultModel>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder.AddProperty(DataColumnDef.MTS_Territory)
                                    .AddProperty(DataColumnDef.MTS_TargetValue)
                                    .AddProperty(DataColumnDef.MTS_TargetVolume)
                                    .AddProperty(DataColumnDef.MTS_MatarialGroupOrBrand);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder.AddProperty(DataColumnDef.Territory)
                                    .AddProperty(DataColumnDef.NetAmount)
                                    .AddProperty(DataColumnDef.Volume)
                                    .AddProperty(DataColumnDef.MatarialGroupOrBrand);

            valueTarget = (await _odataService.GetMTSData(selectTargetQueryBuilder, 
                model.FromDate.MTSSearchDateFormat(), model.ToDate.MTSSearchDateFormat(), 
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: model.Brands, division: model.Division)).ToList();

            valueActual = (await _odataService.GetSalesData(selectActualQueryBuilder, 
                model.FromDate.SalesSearchDateFormat(), model.ToDate.SalesSearchDateFormat(),
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: model.Brands, division: model.Division)).ToList();

            var brands = (from a in valueActual select new { a.MatarialGroupOrBrand })
                        .Union(from t in valueTarget select new { t.MatarialGroupOrBrand }).Distinct().ToList();

            var brandData = (from b in brands
                        join bfi in _context.BrandFamilyInfos.Select(x => new { x.MatarialGroupOrBrand, x.MatarialGroupOrBrandName }).Distinct() on b.MatarialGroupOrBrand equals bfi.MatarialGroupOrBrand into bfileftjoin
                        from bfiInfo in bfileftjoin.DefaultIfEmpty()
                        select new
                        {
                            MatarialGroupOrBrand = b.MatarialGroupOrBrand,
                            BrandName = bfiInfo?.MatarialGroupOrBrandName
                        }).ToList();

            decimal target = 0;
            decimal actual = 0;

            if (model.ResultType == KpiResultType.value)
            {
                foreach (var brand in brandData)
                {
                    var result = new ProductWiseTargetAchievementResultModel()
                    {
                        BrandId = brand.MatarialGroupOrBrand,
                        BrandName = brand.BrandName,
                        ProductTarget = target = valueTarget.Where(x => x.MatarialGroupOrBrand == brand.MatarialGroupOrBrand).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                        ProductActual = actual = valueActual.Where(x => x.MatarialGroupOrBrand == brand.MatarialGroupOrBrand).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                        ProductAcv = _odataService.GetAchivement(target, actual),
                    };
                    reportResult.Add(result);
                }
            }
            else if (model.ResultType == KpiResultType.volume)
            {
                foreach (var brand in brandData)
                {
                    var result = new ProductWiseTargetAchievementResultModel()
                    {
                        BrandId = brand.MatarialGroupOrBrand,
                        BrandName = brand.BrandName,
                        ProductTarget = target = valueTarget.Where(x => x.MatarialGroupOrBrand == brand.MatarialGroupOrBrand).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetVolume)),
                        ProductActual = actual = valueActual.Where(x => x.MatarialGroupOrBrand == brand.MatarialGroupOrBrand).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Volume)),
                        ProductAcv = _odataService.GetAchivement(target, actual),
                    };
                    reportResult.Add(result);
                }
            }

            return reportResult;
        }

        public async Task<List<AppProductWiseTargetAchievementResultModel>> GetAppProductWiseTargetAchievement(ProductWiseTargetAchievementSearchModel model)
        {
            var result = await this.GetProductWiseTargetAchivement(model);

            var reportResult = new List<AppProductWiseTargetAchievementResultModel>();

            foreach (var item in result)
            {
                var res = new AppProductWiseTargetAchievementResultModel();
                res.BrandName = $"{item.BrandName} ({item.BrandId})";
                res.Target = item.ProductTarget;
                res.Actual = item.ProductActual;
                res.Achievement = item.ProductAcv;

                reportResult.Add(res);
            }

            return reportResult;
        }
    }
}
