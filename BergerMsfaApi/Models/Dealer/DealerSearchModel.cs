using Berger.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Dealer
{
    public class AppDealerSearchModel
    {
        public string DealerName { get; set; }
        public EnumDealerCategory? DealerCategory { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public bool isDealerSubDealer { get; set; } = false;

        public AppDealerSearchModel()
        {
            this.isDealerSubDealer = false;
        }
    }

    public class AppAreaDealerSearchModel
    {
        public IList<string> Depots { get; set; }
        public IList<string> SalesOffices { get; set; }
        public IList<string> SalesGroups { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }
        public string DealerName { get; set; }
        public EnumDealerCategory? DealerCategory { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public bool isDealerSubDealer { get; set; } = false;

        public AppAreaDealerSearchModel()
        {
            this.Depots = new List<string>();
            this.SalesOffices = new List<string>();
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
            this.isDealerSubDealer = false;
        }
    }


    public class AreaDealerSearchModel
    {
        public string Depots { get; set; }
        public IList<string> SalesOffices { get; set; }
        public IList<string> SalesGroups { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }
        public EnumDealerCategory? DealerCategory { get; set; }
        public bool isDealerSubDealer { get; set; } = false;

        public AreaDealerSearchModel()
        {
            this.Depots = string.Empty;
            this.SalesOffices = new List<string>();
            this.SalesGroups = new List<string>();
            this.Territories = new List<string>();
            this.Zones = new List<string>();
            this.isDealerSubDealer = false;
        }


    }
}
