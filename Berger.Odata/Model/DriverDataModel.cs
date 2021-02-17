using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class DriverDataModel
    {
        public string BillSl { get; internal set; }
        public string PlantOrBusinessArea { get; internal set; }
        public string InvoiceNoOrBillNo { get; internal set; }
        public string Date { get; internal set; }
        public string DriverName { get; internal set; }
        public string DriverMobileNo { get; internal set; }
        public string Vehicle { get; internal set; }
    }
}
