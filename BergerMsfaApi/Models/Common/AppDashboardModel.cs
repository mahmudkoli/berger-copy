using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Common
{
    public class AppDashboardModel
    {
        public string Title { get; set; }
        public decimal Value { get; set; }
        public AppDashboardType Type { get; set; }
    }

    public enum AppDashboardType
    {
        Percentage = 1,
        Flat = 2
    }
}
