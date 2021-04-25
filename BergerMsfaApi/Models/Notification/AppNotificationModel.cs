using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Notification
{
    public class AppLeadFollowUpNotificationModel
    {
        public int UserId { get; set; }
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAddress { get; set; }
    }
}
