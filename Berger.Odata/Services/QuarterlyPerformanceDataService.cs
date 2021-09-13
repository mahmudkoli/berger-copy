using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPReports;
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
        private readonly IODataApplicationRepository<Division> _oDataDivisionRepository;
        private readonly IODataSAPRepository<QuarterlyPerformanceReport> _oDataQuartPerformRepository;

        public QuarterlyPerformanceDataService(
            IODataService odataService,
            IODataBrandService odataBrandService,
            IODataApplicationRepository<Division> oDataDivisionRepository,
            IODataSAPRepository<QuarterlyPerformanceReport> oDataQuartPerformRepository
            )
        {
            _odataService = odataService;
            _odataBrandService = odataBrandService;
            _oDataDivisionRepository = oDataDivisionRepository;
            _oDataQuartPerformRepository = oDataQuartPerformRepository;
        }

        #region App Report
        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<MTSDataModel>>();
            //var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder
                                .AddProperty(DataColumnDef.MTS_Date)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            //var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            //selectActualQueryBuilder
            //                    .AddProperty(DataColumnDef.Date)
            //                    .AddProperty(DataColumnDef.NetAmount);

            mtsBrands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            monthlyDictTarget = (await this.GetQuarterlyTargetData(selectTargetQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: mtsBrands));

            //monthlyDictActual = (await this.GetQuarterlyActualData(selectActualQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: mtsBrands));

            monthlyDictActual = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

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
                    Amount = dictDataActual.Sum(s => s.MTSValue)
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
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //                .AddProperty(DataColumnDef.Date)
            //                .AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                .AddProperty(DataColumnDef.NetAmount);

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, isLastYear: true));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
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
                    Amount = dictDataLY.Sum(s => s.NoOfBillingDealer)
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} CY",
                    Amount = dictDataCY.Sum(s => s.NoOfBillingDealer)
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
            //var enamelBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //                .AddProperty(DataColumnDef.Date)
            //                .AddProperty(DataColumnDef.Volume);

            //enamelBrands = (await _odataBrandService.GetEnamelBrandCodesAsync()).ToList();

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: enamelBrands, isLastYear: true));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
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
                    Amount = dictDataLY.Sum(s => s.EnamelVolume)
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} CY",
                    Amount = dictDataCY.Sum(s => s.EnamelVolume)
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
            //var premiumBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //                .AddProperty(DataColumnDef.Date)
            //                .AddProperty(DataColumnDef.Volume);

            //premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: premiumBrands, isLastYear: true));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: premiumBrands));

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
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
                    Amount = dictDataLY.Sum(s => s.PremiumVolume)
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} CY",
                    Amount = dictDataCY.Sum(s => s.PremiumVolume)
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
            //var premiumBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //                .AddProperty(DataColumnDef.NetAmount)
            //                .AddProperty(DataColumnDef.Division)
            //                .AddProperty(DataColumnDef.Date);

            //premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    division: ConstantsValue.DivisionDecorative));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: premiumBrands));

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

            monthlyDictCY = monthlyDictLY;

            var result = new List<QuarterlyPerformanceDataResultModel>();

            var res = new QuarterlyPerformanceDataResultModel();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var monthName = fromDate.GetMonthName(number);
                //var dictDataLY = monthlyDictLY[monthName].ToList();
                var dictDataLY = monthlyDictLY[monthName].ToList();
                var dictDataCY = monthlyDictCY[monthName].ToList();

                res.MonthlyTargetData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} (Total Deco Sales at his Territory)",
                    Amount = dictDataLY.Sum(s => s.DecorativeValue)
                });

                res.MonthlyActualData.Add(new MonthlyDataModel()
                {
                    MonthName = $"{monthName} (Premium Brand actual Sales at his Territory)",
                    Amount = dictDataCY.Sum(s => s.PremiumValue)
                });
            }

            res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
            res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

            res.AchivementOrGrowth = _odataService.GetContribution(res.TotalTarget, res.TotalActual);

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
            //var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder
                                .AddProperty(DataColumnDef.MTS_Territory)
                                .AddProperty(DataColumnDef.MTS_Date)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            //var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            //selectActualQueryBuilder
            //                    .AddProperty(DataColumnDef.Territory)
            //                    .AddProperty(DataColumnDef.Date)
            //                    .AddProperty(DataColumnDef.NetAmount);

            mtsBrands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            monthlyDictTarget = (await this.GetQuarterlyTargetData(selectTargetQueryBuilder, model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                brands: mtsBrands));

            //monthlyDictActual = (await this.GetQuarterlyActualData(selectActualQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: mtsBrands));

            monthlyDictActual = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

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
                        Amount = dictDataActual.Sum(s => s.MTSValue)
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetAchivement(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result, EnumQuarterlyPerformanceModel.MTSValueTargetAchivement);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetBillingDealerQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //                .AddProperty(DataColumnDef.Date)
            //                .AddProperty(DataColumnDef.Territory)
            //                .AddProperty(DataColumnDef.CustomerNoOrSoldToParty)
            //                .AddProperty(DataColumnDef.NetAmount);

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, isLastYear: true));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories, isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
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
                        Amount = dictDataLY.Sum(s => s.NoOfBillingDealer)
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataCY.Sum(s => s.NoOfBillingDealer)
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result, EnumQuarterlyPerformanceModel.BillingDealerQuarterlyGrowth);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            //var enamelBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //    .AddProperty(DataColumnDef.Territory)
            //    .AddProperty(DataColumnDef.Date)
            //    .AddProperty(DataColumnDef.Volume);

            //enamelBrands = (await _odataBrandService.GetEnamelBrandCodesAsync()).ToList();

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: enamelBrands, isLastYear: true));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: enamelBrands));

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
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
                        Amount = dictDataLY.Sum(s => s.EnamelVolume)
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataCY.Sum(s => s.EnamelVolume)
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result, EnumQuarterlyPerformanceModel.EnamelPaintsQuarterlyGrowt);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            //var premiumBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //    .AddProperty(DataColumnDef.Date)
            //    .AddProperty(DataColumnDef.Volume);

            //premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: premiumBrands, isLastYear: true));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: premiumBrands));

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
                isLastYear: true));

            monthlyDictCY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
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
                        Amount = dictDataLY.Sum(s => s.PremiumVolume)
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataCY.Sum(s => s.PremiumVolume)
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowth(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result, EnumQuarterlyPerformanceModel.PremiumBrandsGrowth);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(PortalQuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            var monthCount = 3;
            //var premiumBrands = new List<string>();
            //var monthlyDictLY = new Dictionary<string, IList<SalesDataModel>>();
            //var monthlyDictCY = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictLY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();
            var monthlyDictCY = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder
            //                .AddProperty(DataColumnDef.Division)
            //                .AddProperty(DataColumnDef.NetAmount)
            //                .AddProperty(DataColumnDef.Date);

            //premiumBrands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            //monthlyDictLY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    division: ConstantsValue.DivisionDecorative));

            //monthlyDictCY = (await this.GetQuarterlyActualData(selectQueryBuilder, model.FromYear, model.FromMonth,
            //    depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories,
            //    brands: premiumBrands));

            monthlyDictLY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

            monthlyDictCY = (await this.GetQuarterlyActualData(model.FromYear, model.FromMonth,
                depots: new List<string> { model.Depot }, salesGroups: model.SalesGroups, territories: model.Territories));

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
                    var dictDataLY = monthlyDictLY[monthName].Where(x => x.Territory == territory).ToList();
                    var dictDataCY = monthlyDictCY[monthName].Where(x => x.Territory == territory).ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Total Deco Sales at his Territory)",
                        Amount = dictDataLY.Sum(s => s.DecorativeValue)
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Premium Brand actual Sales at his Territory)",
                        Amount = dictDataCY.Sum(s => s.PremiumValue)
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetContribution(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return await ToPortalModel(result, EnumQuarterlyPerformanceModel.PremiumBrandsContribution);
        }

        public async Task<IList<PortalQuarterlyPerformanceDataResultModel>> ToPortalModel(List<QuarterlyPerformanceDataResultModel> data, EnumQuarterlyPerformanceModel reportType)
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

            #region for total
            if (result.Any() && result.Count() > 1)
            {
                var resO = new PortalQuarterlyPerformanceDataResultModel();
                resO.Territory = "Total";

                resO.FirstMonthTargetAmount = result.Sum(x => x.FirstMonthTargetAmount);
                resO.SecondMonthTargetAmount = result.Sum(x => x.SecondMonthTargetAmount);
                resO.ThirdMonthTargetAmount = result.Sum(x => x.ThirdMonthTargetAmount);

                resO.FirstMonthActualAmount = result.Sum(x => x.FirstMonthActualAmount);
                resO.SecondMonthActualAmount = result.Sum(x => x.SecondMonthActualAmount);
                resO.ThirdMonthActualAmount = result.Sum(x => x.ThirdMonthActualAmount);

                resO.TotalTarget = resO.FirstMonthTargetAmount + resO.SecondMonthTargetAmount + resO.ThirdMonthTargetAmount;
                resO.TotalActual = resO.FirstMonthActualAmount + resO.SecondMonthActualAmount + resO.ThirdMonthActualAmount;

                switch (reportType)
                {
                    case EnumQuarterlyPerformanceModel.MTSValueTargetAchivement:
                        resO.AchivementOrGrowth = _odataService.GetAchivement(resO.TotalTarget, resO.TotalActual);
                        break;
                    case EnumQuarterlyPerformanceModel.BillingDealerQuarterlyGrowth:
                        resO.AchivementOrGrowth = _odataService.GetGrowth(resO.TotalTarget, resO.TotalActual);
                        break;
                    case EnumQuarterlyPerformanceModel.EnamelPaintsQuarterlyGrowt:
                        resO.AchivementOrGrowth = _odataService.GetGrowth(resO.TotalTarget, resO.TotalActual);
                        break;
                    case EnumQuarterlyPerformanceModel.PremiumBrandsGrowth:
                        resO.AchivementOrGrowth = _odataService.GetGrowth(resO.TotalTarget, resO.TotalActual);
                        break;
                    case EnumQuarterlyPerformanceModel.PremiumBrandsContribution:
                        resO.AchivementOrGrowth = _odataService.GetContribution(resO.TotalTarget, resO.TotalActual);
                        break;
                }

                result.Add(resO);
            }
            #endregion

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

        //public async Task<Dictionary<string, IList<SalesDataModel>>> GetQuarterlyActualData(SelectQueryOptionBuilder selectQueryBuilder, int fromYear, int fromMonth,
        //    List<string> depots = null, List<string> salesGroups = null, List<string> territories = null, List<string> brands = null, bool isLastYear = false, string division = "")
        //{
        //    var fromDate = (new DateTime(fromYear, fromMonth, 1));
        //    var monthCount = 3;
        //    var mtsBrands = new List<string>();
        //    var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

        //    var fromDateStr = (isLastYear ? fromDate.GetMonthDate(0).GetLYFD() : fromDate.GetMonthDate(0).GetCYFD()).SalesSearchDateFormat();
        //    var toDateStr = (isLastYear ? fromDate.GetMonthDate(2).GetLYLD() : fromDate.GetMonthDate(2).GetCYLD()).SalesSearchDateFormat();

        //    var actualData = (await _odataService.GetSalesData(selectQueryBuilder, fromDateStr, toDateStr,
        //        depots: depots, salesGroups: salesGroups, territories: territories,
        //        brands: brands, division: division)).ToList();

        //    for (var i = 0; i < monthCount; i++)
        //    {
        //        int number = i;
        //        var startDate = (isLastYear ? fromDate.GetMonthDate(number).GetLYFD() : fromDate.GetMonthDate(number).GetCYFD());
        //        var endDate = (isLastYear ? fromDate.GetMonthDate(number).GetLYLD() : fromDate.GetMonthDate(number).GetCYLD());

        //        var data = actualData.Where(x => x.Date.SalesResultDateFormat().Date >= startDate.Date && x.Date.SalesResultDateFormat().Date <= endDate.Date).ToList();
        //        var monthName = fromDate.GetMonthName(number);

        //        monthlyDictActual.Add(monthName, data);
        //    }

        //    return monthlyDictActual;
        //}

        public async Task<Dictionary<string, IList<QuarterlyPerformanceReport>>> GetQuarterlyActualData(int fromYear, int fromMonth,
            List<string> depots = null, List<string> salesGroups = null, List<string> territories = null, bool isLastYear = false)
        {

            depots ??= new List<string>();
            territories ??= new List<string>();
            salesGroups ??= new List<string>();

            var fromDate = (new DateTime(fromYear, fromMonth, 1));
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictActual = new Dictionary<string, IList<QuarterlyPerformanceReport>>();

            var fromDateAll = (isLastYear ? fromDate.GetMonthDate(0).GetLYFD() : fromDate.GetMonthDate(0).GetCYFD());
            var toDateAll = (isLastYear ? fromDate.GetMonthDate(2).GetLYLD() : fromDate.GetMonthDate(2).GetCYLD());

            List<string> dateTime = new List<string>();

            while (fromDateAll <= toDateAll)
            {
                dateTime.Add(fromDateAll.Year.ToString()+fromDateAll.Month.ToString());
                fromDateAll = fromDateAll.AddMonths(1);
            }

            var actualData = (await _oDataQuartPerformRepository.FindAllAsync(x =>
                dateTime.Contains((x.Year.ToString()+x.Month.ToString())) &&
                (!territories.Any() || territories.Contains(x.Territory)) &&
                (!depots.Any() || depots.Contains(x.Depot)) &&
                (!salesGroups.Any() || salesGroups.Contains(x.SalesGroup)))).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = (isLastYear ? fromDate.GetMonthDate(number).GetLYFD() : fromDate.GetMonthDate(number).GetCYFD());
                var endDate = (isLastYear ? fromDate.GetMonthDate(number).GetLYLD() : fromDate.GetMonthDate(number).GetCYLD());

                var data = actualData.Where(x => new DateTime(x.Year, x.Month, 01).Date >= startDate.Date
                                                && new DateTime(x.Year, x.Month, 01).Date <= endDate.Date).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }

            return monthlyDictActual;
        }
        #endregion
    }
}
