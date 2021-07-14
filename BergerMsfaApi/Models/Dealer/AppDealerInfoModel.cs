using System;

namespace BergerMsfaApi.Models.Dealer
{
    public class AppDealerInfoModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Territory { get; set; } 
        public string PlanDate { get; set; }
        public bool IsFocused { get; set; }
        public bool IsSubdealer { get; set; }
        public int PlanId { get; set; }


    }

    public class AppAreaDealerResultModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public bool IsSubdealer { get; set; }
    }
}
