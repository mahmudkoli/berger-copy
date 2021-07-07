using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Common.Model
{
    public class AreaSearchCommonModel
    {
        public IList<string> Depots { get; set; }
        public IList<string> SalesOffices { get; set; }
        public IList<string> SalesGroups { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }

        public AreaSearchCommonModel()
        {
            this.Depots = new List<string>();
            this.SalesOffices = new List<string>();
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }
}
