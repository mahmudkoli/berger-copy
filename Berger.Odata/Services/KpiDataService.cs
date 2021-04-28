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

        public async Task<List<TerritoryTargetAchievementResultModel>> GetTerritoryTargetAchivement(TerritoryTargetAchievementSearchModel model)
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

            var startDate = $"{string.Format("{0:0000}", model.FromDate.Year)}.{string.Format("{0:00}", model.FromDate.Month)}";
            var endDate = $"{string.Format("{0:0000}", model.ToDate.Year)}.{string.Format("{0:00}", model.ToDate.Month)}";

            valueTarget = (await _odataService.GetMTSDataByMultipleTerritory(selectTargetQueryBuilder, startDate, endDate, model.Depot, model.Territory, model.Zone)).ToList();
            liquidTarget = valueTarget.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderTarget = valueTarget.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            valueActual = (await _odataService.GetSalesDataByMultipleTerritory(selectActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Depot, model.Territory, model.Zone)).ToList();
            liquidActual = valueActual.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderActual = valueActual.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            var territoies = (from a in valueActual select a.Territory).Union(from t in valueTarget select t.Territory).Distinct().ToList();

            decimal target;
            decimal actual;
            foreach (var territory in territoies)
            {
                var result = new TerritoryTargetAchievementResultModel()
                {
                    Territory = territory,
                    LiquidTargetInGallons = target = liquidTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    LiquidActualInGallons = actual = liquidActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    LiquidAcv = _odataService.GetAchivement(target, actual),

                    PowderTargetInKg = target = powderTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    PowderActualInKg = actual = powderActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    PowderAcv = _odataService.GetAchivement(target, actual),

                    ValueTargetInTk = target = valueTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    ValueActualInTk = actual = valueActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                    ValueAcv = _odataService.GetAchivement(target, actual),
                };
                reportResult.Add(result);
            }

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

            var startDate = $"{string.Format("{0:0000}", model.FromDate.Year)}.{string.Format("{0:00}", model.FromDate.Month)}";
            var endDate = $"{string.Format("{0:0000}", model.ToDate.Year)}.{string.Format("{0:00}", model.ToDate.Month)}";

            valueTarget = (await _odataService.GetMTSDataByMultipleTerritory(selectTargetQueryBuilder, startDate, endDate, model.Depot, model.Territory, model.Zone, model.DealerId.ToString())).ToList();
            liquidTarget = valueTarget.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderTarget = valueTarget.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            valueActual = (await _odataService.GetSalesDataByMultipleTerritory(selectActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Depot, model.Territory, model.Zone, model.DealerId.ToString())).ToList();
            liquidActual = valueActual.Where(x => liquidBrands.Contains(x.MatarialGroupOrBrand)).ToList();
            powderActual = valueActual.Where(x => powderBrands.Contains(x.MatarialGroupOrBrand)).ToList();

            var reportResult = new List<DealerWiseTargetAchievementResultModel>();

            decimal target;
            decimal actual;
            var result = new DealerWiseTargetAchievementResultModel()
            {
                LiquidTargetInGallons = target = liquidTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                LiquidActualInGallons = actual = liquidActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                LiquidAcv = _odataService.GetAchivement(target, actual),

                PowderTargetInKg = target = powderTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                PowderActualInKg = actual = powderActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                PowderAcv = _odataService.GetAchivement(target, actual),

                ValueTargetInTk = target = valueTarget.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                ValueActualInTk = actual = valueActual.Sum(x => CustomConvertExtension.ObjectToDecimal(x.NetAmount)),
                ValueAcv = _odataService.GetAchivement(target, actual),
            };

            reportResult.Add(result);

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

            var startDate = $"{string.Format("{0:0000}", model.FromDate.Year)}.{string.Format("{0:00}", model.FromDate.Month)}";
            var endDate = $"{string.Format("{0:0000}", model.ToDate.Year)}.{string.Format("{0:00}", model.ToDate.Month)}";

            valueTarget = (await _odataService.GetMTSDataByMultipleTerritory(selectTargetQueryBuilder, startDate, endDate, model.Depot, model.Territory, model.Zone)).ToList();
            valueActual = (await _odataService.GetSalesDataByMultipleTerritory(selectActualQueryBuilder, model.FromDate.DateFormat(), model.ToDate.DateFormat(), model.Depot, model.Territory, model.Zone)).ToList();

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

            decimal target;
            decimal actual;
            if (model.resutType == (int)KpiResultType.value)
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

            if (model.resutType == (int)KpiResultType.volume)
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

    }
}
