using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Model
{
    public class WorkerSettingsModel
    {
        public string BaseAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerUrl { get; set; }
        public string BrandUrl { get; set; }
        public string BrandFamilyUrl { get; set; }
        public string LogUrl { get; set; }
    }

    public class WorkerConfig
    {
        public bool RunDailySalesNTargetDataWorker { get; set; }
        public bool RunDailyCustomerBrandDataWorker { get; set; }
    }
}
