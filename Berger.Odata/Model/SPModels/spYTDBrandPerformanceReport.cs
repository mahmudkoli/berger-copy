using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Model.SPModels
{
    public class spYTDTBrnadPerformanceReport
    {
        public string Depot { get; set; }
        public string BrandOrDivision { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CYMTD { get; set; }
        public decimal LYYTD { get; set; }
        public decimal CYYTD { get; set; }
        public IList<string> Depots { get; set; }
        public string TempBrand { get; set; }
    }
}
