using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;

namespace Berger.Odata.Model
{
    public class CustomerCreditStatusResultModel
    {
        public decimal CreditLimit { get; internal set; }
        public decimal CreditLimitUsed { get; internal set; }
        public decimal RemainingLimit { get; internal set; }
        public decimal CreditLimitUsedPercentage { get; internal set; }
        public decimal LastPayment { get; internal set; }
        public string LastPaymentDate { get; internal set; }

        public CustomerCreditStatusResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
