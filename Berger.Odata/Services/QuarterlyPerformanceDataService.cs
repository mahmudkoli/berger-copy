﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Data.MsfaEntity.Master;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Berger.Odata.Repositories;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class QuarterlyPerformanceDataService : IQuarterlyPerformanceDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataBrandService _odataBrandService;
        private readonly IODataRepository<Division> _oDataDivisionRepository;

        public QuarterlyPerformanceDataService(
            IODataService odataService,
            IODataBrandService odataBrandService, IODataRepository<Division> oDataDivisionRepository
            )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _oDataDivisionRepository = oDataDivisionRepository;
        }

        #region App Report
        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<MTSDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder
                                .AddProperty(DataColumnDef.MTS_Date)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            mtsBrands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            monthlyDictTarget = (await this.GetQuarterlyTargetData(selectTargetQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: mtsBrands));

            monthlyDictActual = (await this.GetQuarterlyActualData(selectActualQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: mtsBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var res = new QuarterlyPerformanceDataResultModel();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var monthName = fromDate.GetMonthName(number);
                var dictDataTarget = monthlyDictTarget[monthName].ToList();
                var dictDataActual = monthlyDictActual[monthName].ToList();

                res.MonthlyTargetData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} Target",
                    Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.TargetValue))
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} Actual",
                    Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                });

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetAchivement(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return result;
        }

        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                            .AddProperty(DataColumnDef.Date)
                            .AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                            .AddProperty(DataColumnDef.NetAmount);

            monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var res = new QuarterlyPerformanceDataResultModel();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var monthName = fromDate.GetMonthName(number);
                var dictDataLY = monthlyDictLY[monthName].ToList();
                var dictDataCY = monthlyDictCY[monthName].ToList();

                res.MonthlyTargetData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} LY",
                    Amount = dictDataLY.Select(s => s.CustomerNoOrSoldToParty).Distinct().Count()
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} CY",
                    Amount = dictDataCY.Select(s => s.CustomerNoOrSoldToParty).Distinct().Count()
                });

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return result;
        }

        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var enamelBrands = new List<string>();
            var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                            .AddProperty(DataColumnDef.Date)
                            .AddProperty(DataColumnDef.Volume);

            enamelBrands = (await _odataBrandService.GetEnamelBrandCodesAsync()).ToList();

            monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: enamelBrands, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: enamelBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var res = new QuarterlyPerformanceDataResultModel();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var monthName = fromDate.GetMonthName(number);
                var dictDataLY = monthlyDictLY[monthName].ToList();
                var dictDataCY = monthlyDictCY[monthName].ToList();

                res.MonthlyTargetData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} LY",
                    Amount = dictDataLY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} CY",
                    Amount = dictDataCY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                });
            }

            res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
            res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

            res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

            result.Add(res);

            return result;
        }

        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var premiumBrands = new List<string>();
            var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                            .AddProperty(DataColumnDef.Date)
                            .AddProperty(DataColumnDef.Volume);

            premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: premiumBrands, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: premiumBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var res = new QuarterlyPerformanceDataResultModel();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var monthName = fromDate.GetMonthName(number);
                var dictDataLY = monthlyDictLY[monthName].ToList();
                var dictDataCY = monthlyDictCY[monthName].ToList();

                res.MonthlyTargetData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} LY",
                    Amount = dictDataLY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} CY",
                    Amount = dictDataCY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                });
            }

            res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
            res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

            res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

            result.Add(res);

            return result;
        }

        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var premiumBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                            .AddProperty(DataColumnDef.NetAmount)
                            .AddProperty(DataColumnDef.Division)
                            .AddProperty(DataColumnDef.Date);

            premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: premiumBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var res = new QuarterlyPerformanceDataResultModel();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var monthName = fromDate.GetMonthName(number);
                //var dictDataLY = monthlyDictLY[monthName].ToList();
                var dictDataLY = monthlyDictCY[monthName].Where(x => x.Division == ConstantsValue.DivisionDecorative).ToList();
                var dictDataCY = monthlyDictCY[monthName].ToList();

                res.MonthlyTargetData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} (Total Deco Sales at his Territory)",
                    Amount = dictDataLY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} (Premium Brand actual Sales at his Territory)",
                    Amount = dictDataCY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                });
            }

            res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
            res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

            res.AchivementOrGrowth = _odataService.GetContribution(res.TotalActual, res.TotalTarget);

            result.Add(res);

            return result;
        }
        #endregion

        #region Portal Report
        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<MTSDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder
                                .AddProperty(DataColumnDef.MTS_Territory)
                                .AddProperty(DataColumnDef.MTS_Date)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                                .AddProperty(DataColumnDef.Territory)
                                .AddProperty(DataColumnDef.Date)
                                .AddProperty(DataColumnDef.NetAmount);

            mtsBrands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            monthlyDictTarget = (await this.GetQuarterlyTargetData(selectTargetQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: mtsBrands));

            monthlyDictActual = (await this.GetQuarterlyActualData(selectActualQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: mtsBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var territories = monthlyDictTarget.SelectMany(x => x.Value).Select(x => x.Territory)
                                .Concat(monthlyDictActual.SelectMany(x => x.Value).Select(x => x.Territory))
                                    .Distinct().ToList();

            foreach (var territory in territories)
            {
                var res = new QuarterlyPerformanceDataResultModel();
                res.Territory = territory;

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].Where(x => x.Territory == territory).ToList();
                    var dictDataActual = monthlyDictActual[monthName].Where(x => x.Territory == territory).ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} Target",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.TargetValue))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} Actual",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetAchivement(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                            .AddProperty(DataColumnDef.Date)
                            .AddProperty(DataColumnDef.Territory)
                            .AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
                            .AddProperty(DataColumnDef.NetAmount);

            monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var territories = monthlyDictLY.SelectMany(x => x.Value).Select(x => x.Territory)
                                .Concat(monthlyDictCY.SelectMany(x => x.Value).Select(x => x.Territory))
                                    .Distinct().ToList();

            foreach (var territory in territories)
            {
                var res = new QuarterlyPerformanceDataResultModel();
                res.Territory = territory;

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataLY = monthlyDictLY[monthName].Where(x => x.Territory == territory).ToList();
                    var dictDataCY = monthlyDictCY[monthName].Where(x => x.Territory == territory).ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataLY.Select(s => s.CustomerNoOrSoldToParty).Distinct().Count()
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataCY.Select(s => s.CustomerNoOrSoldToParty).Distinct().Count()
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var enamelBrands = new List<string>();
            var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                .AddProperty(DataColumnDef.Territory)
                .AddProperty(DataColumnDef.Date)
                .AddProperty(DataColumnDef.Volume);

            enamelBrands = (await _odataBrandService.GetEnamelBrandCodesAsync()).ToList();

            monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: enamelBrands, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: enamelBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var territories = monthlyDictLY.SelectMany(x => x.Value).Select(x => x.Territory)
                                .Concat(monthlyDictCY.SelectMany(x => x.Value).Select(x => x.Territory))
                                    .Distinct().ToList();

            foreach (var territory in territories)
            {
                var res = new QuarterlyPerformanceDataResultModel();
                res.Territory = territory;

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataLY = monthlyDictLY[monthName].Where(x => x.Territory == territory).ToList();
                    var dictDataCY = monthlyDictCY[monthName].Where(x => x.Territory == territory).ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataLY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataCY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var premiumBrands = new List<string>();
            var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                .AddProperty(DataColumnDef.Date)
                .AddProperty(DataColumnDef.Volume);

            premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: premiumBrands, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: premiumBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var territories = monthlyDictLY.SelectMany(x => x.Value).Select(x => x.Territory)
                                .Concat(monthlyDictCY.SelectMany(x => x.Value).Select(x => x.Territory))
                                    .Distinct().ToList();

            foreach (var territory in territories)
            {
                var res = new QuarterlyPerformanceDataResultModel();
                res.Territory = territory;

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataLY = monthlyDictLY[monthName].Where(x => x.Territory == territory).ToList();
                    var dictDataCY = monthlyDictCY[monthName].Where(x => x.Territory == territory).ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataLY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataCY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var premiumBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                            .AddProperty(DataColumnDef.Division)
                            .AddProperty(DataColumnDef.NetAmount)
                            .AddProperty(DataColumnDef.Date);

            premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: premiumBrands));

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var territories = monthlyDictCY.SelectMany(x => x.Value).Select(x => x.Territory)
                                .Distinct().ToList();

            foreach (var territory in territories)
            {
                var res = new QuarterlyPerformanceDataResultModel();
                res.Territory = territory;

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    //var dictDataLY = monthlyDictLY[monthName].Where(x => x.Territory == territory).ToList();
                    var dictDataLY = monthlyDictCY[monthName].Where(x => x.Division == ConstantsValue.DivisionDecorative && x.Territory == territory).ToList();
                    var dictDataCY = monthlyDictCY[monthName].Where(x => x.Territory == territory).ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Total Deco Sales at his Territory)",
                        Amount = dictDataLY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Premium Brand actual Sales at his Territory)",
                        Amount = dictDataCY.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetContribution(res.TotalActual, res.TotalTarget);

                result.Add(res);
            }

            return await ToPortalModel(result);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> ToPortalModel(List<QuarterlyPerformanceDataResultModel> data)
        {
            var result = new List<PortalQuarterlyPerformanceDataResultModel>();

            foreach (var item in data)
            {
                var res = new PortalQuarterlyPerformanceDataResultModel();
                res.Territory = item.Territory;
                res.FirstMonthTargetName = item.MonthlyTargetData[0].MonthName;
                res.SecondMonthTargetName = item.MonthlyTargetData[1].MonthName;
                res.ThirdMonthTargetName = item.MonthlyTargetData[2].MonthName;
                res.FirstMonthTargetAmount = item.MonthlyTargetData[0].Amount;
                res.SecondMonthTargetAmount = item.MonthlyTargetData[1].Amount;
                res.ThirdMonthTargetAmount = item.MonthlyTargetData[2].Amount;

                res.FirstMonthActualName = item.MonthlyActualData[0].MonthName;
                res.SecondMonthActualName = item.MonthlyActualData[1].MonthName;
                res.ThirdMonthActualName = item.MonthlyActualData[2].MonthName;
                res.FirstMonthActualAmount = item.MonthlyActualData[0].Amount;
                res.SecondMonthActualAmount = item.MonthlyActualData[1].Amount;
                res.ThirdMonthActualAmount = item.MonthlyActualData[2].Amount;

                res.TotalTarget = item.TotalTarget;
                res.TotalActual = item.TotalActual;
                res.AchivementOrGrowth = item.AchivementOrGrowth;

                result.Add(res);
            }

            return result;
        }

        public async Task<Dictionary<string, IList<MTSDataModel>>> GetQuarterlyTargetData(SelectQueryOptionBuilder selectQueryBuilder, int fromYear, int fromMonth, 
            List<string> depots = null, List<string> salesGroups = null, List<string> territories = null, List<string> brands = null, bool isLastYear = false)
        {
            var fromDate = (new DateTime(fromYear, fromMonth, 1));
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<MTSDataModel>>();

            var fromDateStr = (isLastYear ? fromDate.GetMonthDate(0).GetLYFD() : fromDate.GetMonthDate(0).GetCYFD()).MTSSearchDateFormat();
            var toDateStr = (isLastYear ? fromDate.GetMonthDate(2).GetLYLD() : fromDate.GetMonthDate(2).GetCYLD()).MTSSearchDateFormat();

            var targetData = (await _odataService.GetMTSData(selectQueryBuilder, fromDateStr, toDateStr,
                depots: depots, salesGroups: salesGroups, territories: territories,
                brands: brands)).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var dateStr = (isLastYear ? fromDate.GetMonthDate(number).GetLYFD() : fromDate.GetMonthDate(number).GetCYFD()).MTSResultDateFormat();

                var data = targetData.Where(x => x.Date == dateStr).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            return monthlyDictTarget;
        }

        public async Task<Dictionary<string, IList<SalesDataModel>>> GetQuarterlyActualData(SelectQueryOptionBuilder selectQueryBuilder, int fromYear, int fromMonth, 
            List<string> depots = null, List<string> salesGroups = null, List<string> territories = null, List<string> brands = null, bool isLastYear = false)
        {
            var fromDate = (new DateTime(fromYear, fromMonth, 1));
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var fromDateStr = (isLastYear ? fromDate.GetMonthDate(0).GetLYFD() : fromDate.GetMonthDate(0).GetCYFD()).SalesSearchDateFormat();
            var toDateStr = (isLastYear ? fromDate.GetMonthDate(2).GetLYLD() : fromDate.GetMonthDate(2).GetCYLD()).SalesSearchDateFormat();

            var actualData = (await _odataService.GetSalesData(selectQueryBuilder, fromDateStr, toDateStr,
                depots: depots, salesGroups: salesGroups, territories: territories,
                brands: brands)).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = (isLastYear ? fromDate.GetMonthDate(number).GetLYFD() : fromDate.GetMonthDate(number).GetCYFD());
                var endDate = (isLastYear ? fromDate.GetMonthDate(number).GetLYLD() : fromDate.GetMonthDate(number).GetCYLD());

                var data = actualData.Where(x => x.Date.SalesResultDateFormat().Date >= startDate.Date && x.Date.SalesResultDateFormat().Date <= endDate.Date).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }

            return monthlyDictActual;
        }
        #endregion
    }
}
