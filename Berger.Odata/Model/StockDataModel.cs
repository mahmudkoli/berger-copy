using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class StockDataModel
    {
        public string MaterialCode { get; set; }
        public string MaterialDiscription { get; set; }
        public string MaterialGroup { get; set; }
        public string Plant { get; set; }
        public string StorageLocation { get; set; }
        public string CurrentStock { get; set; }
    }
}
