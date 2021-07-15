using Berger.Data.MsfaEntity.AlertNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
{
   public interface IOccasionToCelebrateService
    {
        Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrate> occasions);
        //Task<bool> GetById(OccasionToCelebrate occasions);
        Task<bool> UpdateOccasionToCelebrate(OccasionToCelebrate occasions);
        Task<IEnumerable<OccasionToCelebrate>> GetOccasionToCelebrate(IList<string> customer);
    }
}
