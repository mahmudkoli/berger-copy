using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class AppChequeBounceNotificationModel
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string DocumentNo { get; set; }
        public string InstrumentNo { get; set; }
        public string ReversalDate { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string CreditControlArea { get; set; }
        public string CreditControlAreaName { get; internal set; }
        public string Reason { get; set; }
        //public string Remarks { get; set; }

        public AppChequeBounceNotificationModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class AppCreditLimitCrossNotificationModel
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalDue { get; set; }
        public string CreditControlArea { get; set; }
        public string CreditControlAreaName { get; internal set; }

        public AppCreditLimitCrossNotificationModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class AppPaymentFollowUpNotificationModel
    {
        public EnumPaymentFollowUpTypeModel PaymentFollowUpType { get; set; }
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string InvoiceNo { get; internal set; }
        public string InvoiceDate { get; internal set; }
        public string InvoiceAge { get; internal set; }
        public string DayLimit { get; internal set; }
        public string RPRSDate { get; internal set; }
        public string NotificationDate { get; internal set; }

        public AppPaymentFollowUpNotificationModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }

    public class AppCustomerOccasionNotificationModel
    {
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }
        public string DOB { get; internal set; }
        public string SpouseDOB { get; internal set; }
        public string ChildDOB { get; internal set; }

        public AppCustomerOccasionNotificationModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
