using Berger.Common.Enumerations;
using System;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class AppCreateJourneyModel
    {
        public int Id { get; set; }
        public int[] Dealers { get; set; }
        public PlanStatus Status { get; set; }
        public DateTime VisitDate { get; set; }
    }
   
}