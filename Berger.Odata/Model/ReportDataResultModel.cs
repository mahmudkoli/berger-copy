namespace Berger.Odata.Model
{
    public class TargetReportResultModel
    {
        public string TerritoryNumber { get; set; }
        public string Zone { get; set; }
        public string Brand { get; set; }
        public decimal LYSMAchieved { get; set; }
        public decimal TotalMTSTarget { get; set; }
        public decimal TillDateTarget { get; set; }
        public decimal TillDateMTSAchieved { get; set; }
        public decimal DayTarget { get; set; }
        public decimal DaySales { get; set; }
        public decimal TillDateIdealAchieved { get; set; }
        public decimal TillDateActualAchieved { get; set; }
    }
}
