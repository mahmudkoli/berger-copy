using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.ViewModel
{
    public class WorkerSettingsModel
    {
        public string BaseAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerUrl { get; set; }
        public string BrandUrl { get; set; }
    }
}
