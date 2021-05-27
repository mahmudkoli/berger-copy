using BergerMsfaApi.Models.Common;

namespace BergerMsfaApi.Models.DealerSalesCall
{
    public class DealerSalesCallQueryObjectModel : QueryObjectModel
    {
        public DealerSalesCallQueryObjectModel()
        {
            Territories ??= new string[] { };
            CustZones ??= new string[] { };
            SalesGroup ??= new string[] { };
        }
        public string DepoId { get; set; }
        public int? DealerId { get; set; }
        public string[] Territories { get; set; }
        public string[] CustZones { get; set; }
        public string[] SalesGroup { get; set; }
    }
}
