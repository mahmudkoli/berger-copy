using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class CustomerDeliveryDataModel
    {
        public string InvoiceDate { get; set; }
        public string InvoiceCreateTime { get; set; }
        public string InvoiceNumber { get; set; }
        public string Volume { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNo { get; set; }
        public string CustomerNo { get; set; }
    }
}
