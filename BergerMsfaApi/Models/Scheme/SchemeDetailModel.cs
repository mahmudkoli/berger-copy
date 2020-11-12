namespace BergerMsfaApi.Models.Scheme
{
    public class SchemeDetailModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Slab { get; set; }
        public string Item { get; set; }
        public string Condition { get; set; }
        public string TargetVolume { get; set; }
        public string Benefit { get; set; }
        public int SchemeMasterId { get; set; }
        public string SchemeName { get; set; }
        public string GlobalCondition { get; set; }
        public string Date { get; set; }


    }
}
