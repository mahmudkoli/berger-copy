using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CustomerCreditStatusSearchModel
    {
        public string CustomerNo { get; set; }
        public string CreditControlArea { get; set; }
    }
}
