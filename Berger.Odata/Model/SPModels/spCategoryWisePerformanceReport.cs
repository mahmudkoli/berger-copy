using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Model.SPModels
{
    public class spCategoryWisePerformanceReport
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CYMTD { get; set; }
        public decimal LYYTD { get; set; }
        public decimal CYYTD { get; set; }
    }
}
