using Berger.Data.MsfaEntity.AlertNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.AlertNotification.Worker.AlertNotification
{
   public interface IOccasionToCelebrateService
    {
        Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrate> occasions);
    }
}
