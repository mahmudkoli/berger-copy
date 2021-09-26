using System;
using System.Collections.Generic;

namespace Berger.Odata.Common
{
    public class ConstantsValue
    {
        public const string BalanceLineTextOpening = "A Opening";
        //public const string BalanceLineTextTransaction = "B Transaction";
        public const string BalanceLineTextClosing = "C Closing";
        public const string BalanceDocTypeMoneyReceipt = "MR";
        public const string BalanceDocTypeInvoice = "IN";
        //public const string BalanceDocTypeCreditNote = "CN";
        public const string CollectionMoneyReceipt = "MR";
        public const string CollectionInvoice = "IN";
        //public const string CollectionCreditNote = "CN";
        public const string CustomerClassificationExclusive = "01";
        public const string CustomerClassificationNonExclusive = "02";
        public const string DistrbutionChannelDealer = "10";
        public const string DivisionDecorative = "10";
        public const string PriceGroupCreditBuyer = "01";
        public const string PriceGroupCashBuyer = "02";
        public const string PriceGroupFastPayCarry = "04";
        public const string BergerCompanyCode = "1000";
        public const string BergerSourceClient = "REP";
        public const string ChequeBounceStatus = "Z1";
        public const string ChequeDocTypeDA = "DA";
        public const string ChequeDocTypeDZ = "DZ";
        public const int FyYearFirstMonth = 4;
        public const int FyYearLastMonth = 3;

        public static Dictionary<int, string> GetBergerFyMonth(int year = 0)
        {
            var result = new Dictionary<int, string>();
            DateTime date = new DateTime(DateTime.Now.Year, 4, 1);

            if (year != 0)
            {
                date = new DateTime(year, 4, 1);
            }

            DateTime compareDate = date.AddMonths(12);

            while (date != compareDate)
            {
                result.Add(date.Month, date.ToString("MMM"));
                date = date.AddMonths(1);
            }

            return result;
        }

    }
}
