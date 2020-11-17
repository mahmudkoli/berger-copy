using System;

namespace BergerMsfaApi.Models.Dealer
{
    public class DealerInfoModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerNo { get; set; }
        public string BusinessArea { get; set; }
        public string CreditControlArea { get; set; }
        public string AccountGroup { get; set; }
        public string Division { get; set; }
        public string Territory { get; set; }
        public string CustZone { get; set; }
        public DateTime VisitDate { get; set; }

    }
}
