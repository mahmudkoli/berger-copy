using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerInfoModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerNo { get; set; }
        public string Territory { get; set; }
        public string AppVisitDate { get; set; }
        public DateTime VisitDate { get; set; }

    }
}
