using System;

namespace Berger.Data.MsfaEntity.AlertNotification
{
   public class OccasionToCelebrateNotification
    {
        public Guid Id { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string CustomarNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? SpouseDOB { get; set; }
        public DateTime? FirsChildDOB { get; set; }
        public DateTime? SecondChildDOB { get; set; }
        public DateTime? ThirdChildDOB { get; set; }
        public DateTime NotificationDate { get; set; }

    }
}
