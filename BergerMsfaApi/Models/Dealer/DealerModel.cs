using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerModel
    {
        public DealerModel()
        {
            SubDealers = new List<SubDealerModel>();
        }
        public int Id { get; set; }
        public string AccountGroup { get; set; }
        public string CustomerName { get; set; }

        public string Description { get; set; }

        public List<SubDealerModel> SubDealers { get; set; }
    }

    public class SubDealerModel
    {
        public int Id { get; set; }
        public string AccountGroup { get; set; }
        public string CustomerName { get; set; }

        public string Description { get; set; }
    }
}
