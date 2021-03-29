using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerOpeningStatusChangeModel
    {

            public int DealerOpeningId { get; set; }
            public int Status { get; set; }
            public string Comment { get; set; }
        
    }
}
