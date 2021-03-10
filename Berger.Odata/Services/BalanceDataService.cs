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
    public class BalanceDataService : IBalanceDataService
    {
        private readonly IODataService _odataService;

        public BalanceDataService(
            IODataService odataService
            )
        {
            _odataService = odataService;
        }

        public async Task<IList<BalanceConfirmationSummaryResultModel>> GetBalanceConfirmationSummary(BalanceConfirmationSummarySearchModel model)
        {
            var currentDate = CustomConvertExtension.ObjectToDateTime(model.PostingDate);
            var fromDate = currentDate.AddDays(-90).DateTimeFormat();
            var toDate = currentDate.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(BalanceColDef.LineText)
                                .AddProperty(BalanceColDef.CustomerNo)
                                .AddProperty(BalanceColDef.CustomerName)
                                .AddProperty(BalanceColDef.CreditControlArea)
                                .AddProperty(BalanceColDef.PostingDate)
                                .AddProperty(BalanceColDef.Amount);

            var data = (await _odataService.GetBalanceDataByCustomerAndCreditControlArea(selectQueryBuilder, model.CustomerNo, model.CreditControlArea, fromDate, toDate, model.FiscalYear)).ToList();

            var groupData = data.GroupBy(x => x.PostingDate);

            var result = groupData.Select(x =>
                                new BalanceConfirmationSummaryResultModel()
                                {
                                    Date = CustomConvertExtension.ObjectToDateString(x.Key),
                                    OpeningBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextOpening).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))),
                                    InvoiceBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextInvoice).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))),
                                    PaymentBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextPayment).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount))),
                                    ClosingBalance = (x.Where(w => w.LineText == ConstantsValue.BalanceLineTextClosing).Sum(s => CustomConvertExtension.ObjectToDecimal(s.Amount)))
                                }).ToList();

            return result;
        }
    }
}
