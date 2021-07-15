using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class MaterialStockResultModel
    {
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public decimal Stock { get; set; }

        public MaterialStockResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
