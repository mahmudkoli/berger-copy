using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class MaterialStockSearchModel
    {
        public string Plant { get; set; }
        public string MaterialGroupOrBrand { get; set; }
        public string MaterialCode { get; set; }
    }
}
