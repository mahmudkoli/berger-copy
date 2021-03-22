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

    public class MySummaryReportResultModel
    {
        public int DealerVisitTarget { get; set; }
        public int ActualVisited { get; set; }
        public int SubDealerActuallyVisited { get; set; }
        public int PainterActuallyVisited { get; set; }
        public int AdHocVisitNo { get; set; }
        public int LeadGenerationNo { get; set; }
        public int LeadFollowupNo { get; set; }
        public int LeadFollowupValue { get; set; }
        public int NoOfBillingDealer { get; set; }
        public int TotalCollectionValue { get; set; }
    } 
}
