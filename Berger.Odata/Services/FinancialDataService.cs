using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class FinancialDataService : IFinancialDataService
    {
        private readonly IODataService _odataService;
        private readonly IODataCommonService _odataCommonService;

        public FinancialDataService(
            IODataService odataService,
            IODataCommonService odataCommonService
            )
        {
            _odataService = odataService;
            _odataCommonService = odataCommonService;
        }

        public async Task<IList<OutstandingDetailsResultModel>> GetOutstandingDetails(OutstandingDetailsSearchModel model)
        {
            //var currentDate = DateTime.Now;
            var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Amount);

            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate)).ToList();

            Func<FinancialDataModel, bool> predicateFunc = x => (model.Days switch
            {
                EnumOutstandingDetailsAgeDays._0_To_30_Days => CustomConvertExtension.ObjectToInt(x.Age) >= 0 && 
                                                                    CustomConvertExtension.ObjectToInt(x.Age) <= 30,
                EnumOutstandingDetailsAgeDays._31_To_60_Days => CustomConvertExtension.ObjectToInt(x.Age) >= 31 &&
                                                                    CustomConvertExtension.ObjectToInt(x.Age) <= 60,
                EnumOutstandingDetailsAgeDays._61_To_90_Days => CustomConvertExtension.ObjectToInt(x.Age) >= 61 &&
                                                                    CustomConvertExtension.ObjectToInt(x.Age) <= 90,
                EnumOutstandingDetailsAgeDays._GT_90_Days => CustomConvertExtension.ObjectToInt(x.Age) > 90,
                _ => true
            });
            Func<FinancialDataModel, bool> predicateFuncFinal = x => predicateFunc(x) && x.CreditControlArea == model.CreditControlArea;

            var result = data.Where(predicateFuncFinal).Select(x =>
                                new OutstandingDetailsResultModel()
                                {
                                    InvoiceNo = x.InvoiceNo,
                                    Age = x.Age,
                                    PostingDate = x.PostingDate.ReturnDateFormatDate(format: "yyyy-MM-ddTHH:mm:ssZ"),
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount)
                                }).ToList();

            return result;
        }

        public async Task<IList<OutstandingSummaryResultModel>> GetOutstandingSummary(OutstandingSummarySearchModel model)
        {
            //var currentDate = DateTime.Now;
            var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            foreach (var prop in typeof(CustomerDataModel).GetProperties())
            {
                selectCustomerQueryBuilder.AddProperty(prop.Name);
            }

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.Amount);

            var customerData = (await _odataService.GetCustomerDataByCustomerNo(selectCustomerQueryBuilder, model.CustomerNo)).ToList();
            var data = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, fromDate)).ToList();

            var groupData = data.GroupBy(x => x.CreditControlArea).ToList();

            var result = groupData.Select(x =>
                                    {
                                        var osModel = new OutstandingSummaryResultModel();
                                        osModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                                        osModel.DaysLimit = x.FirstOrDefault()?.DayLimit ?? string.Empty;
                                        osModel.ValueLimit = customerData.FirstOrDefault(f => f.CreditControlArea == osModel.CreditControlArea)?.CreditLimit ?? (decimal)0;
                                        osModel.NetDue = x.Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.Slippage = x.Where(w => CustomConvertExtension.ObjectToInt(w.DayLimit) > CustomConvertExtension.ObjectToInt(w.Age))
                                                            .Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount));
                                        osModel.HighestDaysInvoice = x.Max(m => CustomConvertExtension.ObjectToInt(m.Age)).ToString();
                                        return osModel;
                                    }).ToList();

            #region Credit Control Area 
            var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            foreach (var item in result)
            {
                item.CreditControlAreaName = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.CreditControlArea)?.Description ?? string.Empty;
            }
            #endregion

            return result;
        }
    }
}
