using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Odata.Repositories;

namespace Berger.Odata.Services
{
    public class KpiDataService : IKpiDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;
        private readonly ApplicationDbContext _context;
        private readonly IODataSAPRepository<KPIPerformanceReport> _oDataKPIPerformanceReportRepository;
        public KpiDataService(
            IODataService odataService,
            IODataBrandService odataBrandService,
            ApplicationDbContext context, IODataSAPRepository<KPIPerformanceReport> oDataKpiPerformanceReportRepository)
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _context = context;
            _oDataKPIPerformanceReportRepository = oDataKpiPerformanceReportRepository;
        }

        public async Task<List<TerritoryTargetAchievementResultModel>> GetTerritoryTargetAchivement(SalesTargetAchievementSearchModel model)
        {
            var liquidBrands = new List<string>();
            var powderBrands = new List<string>();

            var liquidTarget = new List<MTSDataModel>();
            var powderTarget = new List<MTSDataModel>();
            var valueTarget = new List<MTSDataModel>();

            IList<KPIPerformanceReport> liquidActual = new List<KPIPerformanceReport>();
            IList<KPIPerformanceReport> powderActual = new List<KPIPerformanceReport>();
            IList<KPIPerformanceReport> valueActual = new List<KPIPerformanceReport>();

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

            var depotList = string.IsNullOrWhiteSpace(model.Depot) ? new List<string>() : new List<string> { model.Depot };


            liquidActual = await GetKpiPerformanceReport(x => new KPIPerformanceReport()
            {
                Value = x.Value,
                Territory = x.Territory
            }, model.FromDate.DateFormat(),
                model.ToDate.DateFormat(),
                depotList, model.SalesGroups, model.Territories, brands: liquidBrands);

            powderActual = await GetKpiPerformanceReport(x => new KPIPerformanceReport()
            {
                Value = x.Value,
                Territory = x.Territory
            }, model.FromDate.DateFormat(),
                model.ToDate.DateFormat(),
                depotList, model.SalesGroups, model.Territories, brands: powderBrands);

            valueActual = await GetKpiPerformanceReport(x => new KPIPerformanceReport()
            {
                Value = x.Value,
                Territory = x.Territory
            }, model.FromDate.DateFormat(),
                model.ToDate.DateFormat(),
                depotList, model.SalesGroups, model.Territories);

            // liquidActual = valueActual.Where(x => liquidBrands.Contains(x.Brand)).ToList();
            // powderActual = valueActual.Where(x => powderBrands.Contains(x.Brand)).ToList();

            var territoies = (from a in liquidActual select a.Territory)
                .Union(from a in powderActual select a.Territory)
                .Union(from t in valueTarget select t.Territory).Distinct().ToList();

            decimal target = 0;
            decimal actual = 0;

            foreach (var territory in territoies)
            {
                var result = new TerritoryTargetAchievementResultModel()
                {
                    Territory = territory,

                    LiquidTarget = target = liquidTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    LiquidActual = actual = liquidActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Value)),
                    LiquidAcv = _odataService.GetAchivement(target, actual),

                    PowderTarget = target = powderTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    PowderActual = actual = powderActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Value)),
                    PowderAcv = _odataService.GetAchivement(target, actual),

                    ValueTarget = target = valueTarget.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    ValueActual = actual = valueActual.Where(x => x.Territory == territory).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Value)),
                    ValueAcv = _odataService.GetAchivement(target, actual),
                };

                reportResult.Add(result);
            }

            if (!model.ForApp && reportResult.Any() && reportResult.Count > 1)
            {
                var result = new TerritoryTargetAchievementResultModel()
                {
                    Territory = "Total",

                    LiquidTarget = target = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.LiquidTarget)),
                    LiquidActual = actual = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.LiquidActual)),
                    LiquidAcv = _odataService.GetAchivement(target, actual),

                    PowderTarget = target = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.PowderTarget)),
                    PowderActual = actual = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.PowderActual)),
                    PowderAcv = _odataService.GetAchivement(target, actual),

                    ValueTarget = target = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.ValueTarget)),
                    ValueActual = actual = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.ValueActual)),
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

            var resLiquid = new AppTargetAchievementResultModel
            {
                Category = "Liquid",
                Target = result.Sum(x => x.LiquidTarget),
                Actual = result.Sum(x => x.LiquidActual)
            };
            resLiquid.Achievement = _odataService.GetAchivement(resLiquid.Target, resLiquid.Actual);
            reportResult.Add(resLiquid);

            var resPowder = new AppTargetAchievementResultModel
            {
                Category = "Powder",
                Target = result.Sum(x => x.PowderTarget),
                Actual = result.Sum(x => x.PowderActual)
            };
            resPowder.Achievement = _odataService.GetAchivement(resPowder.Target, resPowder.Actual);
            reportResult.Add(resPowder);

            var resValue = new AppTargetAchievementResultModel
            {
                Category = "Value",
                Target = result.Sum(x => x.ValueTarget),
                Actual = result.Sum(x => x.ValueActual)
            };
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

            IList<KPIPerformanceReport> liquidActual = new List<KPIPerformanceReport>();
            IList<KPIPerformanceReport> powderActual = new List<KPIPerformanceReport>();
            IList<KPIPerformanceReport> valueActual = new List<KPIPerformanceReport>();

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



            var depotList = string.IsNullOrWhiteSpace(model.Depot) ? new List<string>() : new List<string> { model.Depot };


            valueActual = await GetKpiPerformanceReport(x => new KPIPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Territory = x.Territory
            }, model.FromDate.DateFormat(),
                model.ToDate.DateFormat(),
                depotList, model.SalesGroups, model.Territories, customerNo: model.CustomerNo);




            //valueActual = (await _odataService.GetSalesData(selectActualQueryBuilder,
            //    model.FromDate.SalesSearchDateFormat(), model.ToDate.SalesSearchDateFormat(),
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    customerNo: model.CustomerNo)).ToList();

            liquidActual = valueActual.Where(x => liquidBrands.Contains(x.Brand)).ToList();
            powderActual = valueActual.Where(x => powderBrands.Contains(x.Brand)).ToList();

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
                    LiquidActual = actual = liquidActual.Where(x => x.CustomerNo == customerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Value)),
                    LiquidAcv = _odataService.GetAchivement(target, actual),

                    PowderTarget = target = powderTarget.Where(x => x.CustomerNo == customerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    PowderActual = actual = powderActual.Where(x => x.CustomerNo == customerNo).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Value)),
                    PowderAcv = _odataService.GetAchivement(target, actual),

                    ValueTarget = target = valTar.Sum(x => CustomConvertExtension.ObjectToDecimal(x.TargetValue)),
                    ValueActual = actual = valAct.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Value)),
                    ValueAcv = _odataService.GetAchivement(target, actual),
                };

                reportResult.Add(result);
            }

            if (!model.ForApp && reportResult.Any() && reportResult.Count > 1)
            {
                var result = new DealerWiseTargetAchievementResultModel()
                {
                    Territory = "Total",

                    LiquidTarget = target = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.LiquidTarget)),
                    LiquidActual = actual = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.LiquidActual)),
                    LiquidAcv = _odataService.GetAchivement(target, actual),

                    PowderTarget = target = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.PowderTarget)),
                    PowderActual = actual = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.PowderActual)),
                    PowderAcv = _odataService.GetAchivement(target, actual),

                    ValueTarget = target = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.ValueTarget)),
                    ValueActual = actual = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.ValueActual)),
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
            IList<KPIPerformanceReport> valueActual = new List<KPIPerformanceReport>();
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

            //valueActual = (await _odataService.GetSalesData(selectActualQueryBuilder,
            //    model.FromDate.SalesSearchDateFormat(), model.ToDate.SalesSearchDateFormat(),
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: model.Brands, division: model.Division)).ToList();


            var depotList = string.IsNullOrWhiteSpace(model.Depot) ? new List<string>() : new List<string> { model.Depot };


            valueActual = await GetKpiPerformanceReport(x => new KPIPerformanceReport()
            {
                Brand = x.Brand,
                Value = x.Value,
                Volume = x.Volume
            }, model.FromDate.DateFormat(),
                model.ToDate.DateFormat(),
                depotList, model.SalesGroups, model.Territories, brands: model.Brands, division: model.Division);



            var brands = (from a in valueActual select new { MatarialGroupOrBrand = a.Brand })
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
                        ProductActual = actual = valueActual.Where(x => x.Brand == brand.MatarialGroupOrBrand).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Value)),
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
                        ProductActual = actual = valueActual.Where(x => x.Brand == brand.MatarialGroupOrBrand).Sum(x => CustomConvertExtension.ObjectToDecimal(x.Volume)),
                        ProductAcv = _odataService.GetAchivement(target, actual),
                    };
                    reportResult.Add(result);
                }
            }

            if (!model.ForApp && reportResult.Any() && reportResult.Count > 1)
            {
                var result = new ProductWiseTargetAchievementResultModel()
                {
                    BrandId = "Total",

                    ProductTarget = target = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.ProductTarget)),
                    ProductActual = actual = reportResult.Sum(x => CustomConvertExtension.ObjectToDecimal(x.ProductActual)),
                    ProductAcv = _odataService.GetAchivement(target, actual),
                };
                reportResult.Add(result);
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

        public async Task<IList<KPIPerformanceReport>> GetKpiPerformanceReport(Expression<Func<KPIPerformanceReport,
            KPIPerformanceReport>> selectProperty,
            string startDate, string endDate, List<string> depots = null, List<string> salesGroups = null, List<string> territories = null, List<string> brands = null,
            string customerNo = null,
            string division = null
            )
        {
            depots ??= new List<string>();
            salesGroups ??= new List<string>();
            territories ??= new List<string>();
            brands ??= new List<string>();

            DateTime stDate = DateTime.ParseExact(startDate, "yyyy.MM.dd", null);
            DateTime edDate = DateTime.ParseExact(endDate, "yyyy.MM.dd", null);

            return await _oDataKPIPerformanceReportRepository.GetAllIncludeAsync(selectProperty, x =>
                    (!brands.Any() || brands.Contains(x.Brand)) &&
                    (!territories.Any() || territories.Contains(x.Territory)) &&
                    (!salesGroups.Any() || salesGroups.Contains(x.SalesGroup)) &&
                    (!depots.Any() || depots.Contains(x.Depot)) &&
                    (string.IsNullOrWhiteSpace(customerNo) || x.CustomerNo == customerNo) &&
                    (string.IsNullOrWhiteSpace(division) || x.Division == division)
                    && x.Date >= stDate && x.Date <= edDate
                , null, null, true);

        }
    }
}
