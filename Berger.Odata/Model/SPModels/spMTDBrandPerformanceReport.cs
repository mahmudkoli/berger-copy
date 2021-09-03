using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Model.SPModels
{
    public class spMTDTBrnadPerformanceReport
    {
        public string Depot { get; set; }
        public string Brand { get; set; }
        public decimal LYMTD { get; set; }
        public decimal CMActual { get; set; }
        public decimal TillDateLYActual { get; set; }
        public decimal TillDateCYActual { get; set; }
        public IList<string> Depots { get; set; }
        public string TempBrand { get; set; }
    }
}
