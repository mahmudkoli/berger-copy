namespace Berger.Odata.Model
{
    public class MTSSearchModel
    {
        public string CustomerNo { get; set; }
        //public string Division { get; set; }
        public EnumVolumeOrValue VolumeOrValue { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
