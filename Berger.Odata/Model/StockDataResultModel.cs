using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class StocksResultModel
    {
        public string MaterialCode { get; set; }
        public string MaterialDiscription { get; set; }
        public decimal Stock { get; set; }
        public string UOM { get; set; }

        public StocksResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
