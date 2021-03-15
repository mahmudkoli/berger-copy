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
    public class QuarterlyPerformanceDataService : IQuarterlyPerformanceDataService
    {
        private readonly IODataService _odataService;

        public QuarterlyPerformanceDataService(
            IODataService odataService
            )
        {
            _odataService = odataService;
        }

        public async Task<IList<MTSValueTargetAchivementResultModel>> GetMTSValueTargetAchivement(QuarterlyPerformanceSearchModel model)
        {
            //var fromDate = CustomConvertExtension.ObjectToDateTime(model.FromDate).DateTimeFormat();
            //var toDate = CustomConvertExtension.ObjectToDateTime(model.ToDate).DateTimeFormat();

            //var selectQueryBuilder = new SelectQueryOptionBuilder();
            //selectQueryBuilder.AddProperty(BalanceColDef.LineText)
            //                    .AddProperty(BalanceColDef.CustomerNo)
            //                    .AddProperty(BalanceColDef.CustomerName)
            //                    .AddProperty(BalanceColDef.CreditControlArea)
            //                    .AddProperty(BalanceColDef.PostingDate)
            //                    .AddProperty(BalanceColDef.Amount);

            //var data = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectQueryBuilder, model.Territory, fromDate, toDate, model.Territory, model.Territory)).ToList();

            //var groupData = data.GroupBy(x => x.PostingDate);

            //var result = groupData.Select(x =>
            //                    new BalanceConfirmationSummaryResultModel()
            //                    {
            //                        Date = CustomConvertExtension.ObjectToDateString(x.Key),
            //                        OpeningBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextOpening).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))),
            //                        InvoiceBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextTransaction && CustomConvertExtension.ObjectToDecimal(w.Amount) >= 0).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))),
            //                        PaymentBalance = ((x.Where(w => w.LineText == ConstantsValue.BalanceLineTextTransaction && CustomConvertExtension.ObjectToDecimal(w.Amount) < 0).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))) * -1),
            //                        ClosingBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextClosing).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount)))
            //                    }).ToList();

            return null;
        }
    }
}
