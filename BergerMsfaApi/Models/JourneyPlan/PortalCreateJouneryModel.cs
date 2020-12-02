using Berger.Common.Enumerations;
using System;

namespace BergerMsfaApi.Models.JourneyPlan
{
    public class PortalCreateJouneryModel
    {
        public int Id { get; set; }
        public int[] Dealers { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public DateTime VisitDate { get; set; }
    }
   
}