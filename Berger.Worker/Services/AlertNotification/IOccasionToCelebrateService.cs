using Berger.Data.MsfaEntity.AlertNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
{
   public interface IOccasionToCelebrateService
    {
        Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrateNotification> occasions);
        //Task<bool> GetById(OccasionToCelebrate occasions);
        Task<bool> UpdateOccasionToCelebrate(OccasionToCelebrateNotification occasions);
        Task<IEnumerable<OccasionToCelebrateNotification>> GetOccasionToCelebrate(IList<string> customer);
    }
}
