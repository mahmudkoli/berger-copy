using System;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class PortalCreateJouneryModel
    {
        public int Id { get; set; }
        public int[] Dealers { get; set; }
        public DateTime VisitDate { get; set; }
    }

   
}