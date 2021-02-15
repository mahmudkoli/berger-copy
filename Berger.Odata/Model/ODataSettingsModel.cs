using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Model
{
    public class ODataSettingsModel
    {
        public string BaseAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SalesUrl { get; set; }
        public string DriverUrl { get; set; }
        public string MTSUrl { get; set; }
        public string BrandFamilyUrl { get; set; }
    }
}
