namespace BergerMsfaApi.Models.Dealer
{
    public class FocusDealerSearchModel
    {
        public string DepoId { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string[] Territories { get; set; }
        public string[] CustZones { get; set; }
    }
}
