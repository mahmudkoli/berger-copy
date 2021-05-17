namespace BergerMsfaApi.Models.Dealer
{
    public class DealerListSearchModel
    {
        public string DepoId { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public int? CustomerNo { get; set; }
        public string[] Territories { get; set; }
        public string[] CustZones { get; set; }
        public string[] SalesGroup { get; set; }

    }
}
