using System;
using System.Collections.Generic;
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

        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var mtsBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<MTSDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectTargetQueryBuilder = new SelectQueryOptionBuilder();
            selectTargetQueryBuilder.AddProperty(DataColumnDef.MTS_Territory)
                                .AddProperty(DataColumnDef.MTS_CustomerNo)
                                .AddProperty(DataColumnDef.MTS_CustomerName)
                                .AddProperty(DataColumnDef.MTS_TargetValue);

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder.AddProperty(DataColumnDef.Territory)
                                .AddProperty(DataColumnDef.CustomerNo)
                                .AddProperty(DataColumnDef.CustomerName)
                                .AddProperty(DataColumnDef.NetAmount);

            mtsBrands = (await _odataBrandService.GetMTSBrandCodesAsync()).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var dateFull = fromDate.GetMonthDate(number).GetCYFD();
                var date = $"{string.Format("{0:0000}", dateFull.Year)}.{string.Format("{0:00}", dateFull.Month)}";

                var data = (await _odataService.GetMTSDataByTerritory(selectTargetQueryBuilder, date, model.Territory, mtsBrands)).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetCYFD().DateFormat();
                var endDate = fromDate.GetMonthDate(number).GetCYLD().DateFormat();

                var data = (await _odataService.GetSalesDataByTerritory(selectActualQueryBuilder, startDate, endDate, model.Territory, mtsBrands)).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }

            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
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
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = res.TotalTarget > 0 ? ((res.TotalActual / res.TotalTarget)) * 100 : decimal.Zero;

                result.Add(res);
            }

            return result;
        }

        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetEnamelPaintsQuarterlyGrowth(OdataQuartPerformSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var enamelBrands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                //.AddProperty(DataColumnDef.CustomerNo)
                .AddProperty(DataColumnDef.Volume);

            enamelBrands = (await _odataBrandService.GetEnamelBrandCodesAsync()).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var startDate = fromDate.GetMonthDate(number).GetCYFD().DateFormat();
                var endDate = fromDate.GetMonthDate(number).GetCYLD().DateFormat();

                var data = await _odataService.GetSalesDataByTerritory(selectActualQueryBuilder, startDate, endDate, model.Territory, enamelBrands);

                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetLYFD().DateFormat();
                var endDate = fromDate.GetMonthDate(number).GetLYLD().DateFormat();

                var data = (await _odataService.GetSalesDataByTerritory(selectActualQueryBuilder, startDate, endDate, model.Territory, enamelBrands)).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }


            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowthNew(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return result;
        }

        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsContribution(OdataQuartPerformSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var brands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                .AddProperty(DataColumnDef.Division)
                .AddProperty(DataColumnDef.NetAmount);

            brands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var startDate = fromDate.GetMonthDate(number).GetCYFD().DateFormat();
                var endDate = fromDate.GetMonthDate(number).GetCYLD().DateFormat();

                var data = await _odataService.GetSalesDataByTerritory(selectActualQueryBuilder, startDate, endDate, model.Territory, brands);

                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var monthName = fromDate.GetMonthName(number);

                var data = monthlyDictTarget[monthName];


                Division division = await _oDataDivisionRepository.FindAsync(x => x.Description == "Decorative");
                string divisionCode = division != null ? division.DivisionCode.ToString() : "";

                data = data.Where(x => x.Division == divisionCode).ToList();

                monthlyDictActual.Add(monthName, data);
            }


            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Premium Brand actual Sales at his Territory)",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} (Total Deco Sales at his Territory)",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.NetAmount))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetContribution(res.TotalActual, res.TotalTarget);

                result.Add(res);
            }

            return result;
        }


        public async Task<IList<QuarterlyPerformanceDataResultModel>> GetPremiumBrandsGrowth(OdataQuartPerformSearchModel model)
        {
            var fromDate = (new DateTime(model.FromYear, model.FromMonth, 1));
            //var toDate = (new DateTime(model.ToYear, model.ToMonth, 1)).GetCYLD().DateTimeFormat();
            var monthCount = 3;
            var brands = new List<string>();
            var monthlyDictTarget = new Dictionary<string, IList<SalesDataModel>>();
            var monthlyDictActual = new Dictionary<string, IList<SalesDataModel>>();

            var selectActualQueryBuilder = new SelectQueryOptionBuilder();
            selectActualQueryBuilder
                //.AddProperty(DataColumnDef.CustomerNo)
                .AddProperty(DataColumnDef.Volume);

            brands = (await _odataBrandService.GetPremiumBrandCodesAsync()).ToList();

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;

                var startDate = fromDate.GetMonthDate(number).GetCYFD().DateFormat();
                var endDate = fromDate.GetMonthDate(number).GetCYLD().DateFormat();

                var data = await _odataService.GetSalesDataByTerritory(selectActualQueryBuilder, startDate, endDate, model.Territory, brands);

                var monthName = fromDate.GetMonthName(number);

                monthlyDictTarget.Add(monthName, data);
            }

            for (var i = 0; i < monthCount; i++)
            {
                int number = i;
                var startDate = fromDate.GetMonthDate(number).GetLYFD().DateFormat();
                var endDate = fromDate.GetMonthDate(number).GetLYLD().DateFormat();

                var data = (await _odataService.GetSalesDataByTerritory(selectActualQueryBuilder, startDate, endDate, model.Territory, brands)).ToList();
                var monthName = fromDate.GetMonthName(number);

                monthlyDictActual.Add(monthName, data);
            }


            var result = new List<QuarterlyPerformanceDataResultModel>();

            if (monthlyDictTarget.Any())
            {
                var res = new QuarterlyPerformanceDataResultModel();

                for (var i = 0; i < monthCount; i++)
                {
                    int number = i;
                    var monthName = fromDate.GetMonthName(number);
                    var dictDataTarget = monthlyDictTarget[monthName].ToList();
                    var dictDataActual = monthlyDictActual[monthName].ToList();

                    res.MonthlyTargetData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} CY",
                        Amount = dictDataTarget.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });

                    res.MonthlyActualData.Add(new MonthlyDataModel()
                    {
                        MonthName = $"{monthName} LY",
                        Amount = dictDataActual.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Volume))
                    });
                }

                res.TotalTarget = res.MonthlyTargetData.Sum(s => s.Amount);
                res.TotalActual = res.MonthlyActualData.Sum(s => s.Amount);

                res.AchivementOrGrowth = _odataService.GetGrowthNew(res.TotalTarget, res.TotalActual);

                result.Add(res);
            }

            return result;
        }



    }
}
