using System;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class AppCreateJourneyModel
    {
        public int Id { get; set; }
        public int[] Dealers { get; set; }
        public string Status { get; set; }
        public DateTime VisitDate { get; set; }
    }
   
}