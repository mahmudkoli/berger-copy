using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CustomerCreditResultModel
    {
        public decimal CreditLimit { get; internal set; }
        public decimal CreditLimitUsed { get; internal set; }
        public decimal Delta { get; internal set; }
        public decimal CreditLimitUsedPercentage { get; internal set; }
        public string CreditHorizonDate { get; internal set; } // DateTime.Now
        public decimal LastPayment { get; internal set; }
        public decimal Receivables { get; internal set; }
        public decimal OpenDeliveryValue { get; internal set; }
        public decimal OpenSalesOrderValue { get; internal set; }
        public decimal OpenBillDocValue { get; internal set; }
        public string LastPaymentDate { get; internal set; }

        public CustomerCreditResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
