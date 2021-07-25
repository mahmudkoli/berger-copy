using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CustomerDeliveryNoteSearchModel
    {
        public string CustomerNo { get; set; }
        public DateTime DeliveryFromDate { get; set; }
        public DateTime DeliveryToDate { get; set; }
    }
}
