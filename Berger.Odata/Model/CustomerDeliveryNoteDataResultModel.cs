using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;

namespace Berger.Odata.Model
{
    public class CustomerDeliveryNoteResultModel
    {
        public string InvoiceDate { get; set; }
        public string InvoiceCreateTime { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Volume { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNo { get; set; }

        public CustomerDeliveryNoteResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
