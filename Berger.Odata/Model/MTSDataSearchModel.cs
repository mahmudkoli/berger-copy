namespace Berger.Odata.Model
{
    public class MTSSearchModel : MTSSearchModelBase
    {
        public EnumVolumeOrValue VolumeOrValue { get; set; }
    }


    public class MTSSearchModelBase
    {
        public string CustomerNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
