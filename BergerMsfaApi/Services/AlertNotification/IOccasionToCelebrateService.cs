using Berger.Data.MsfaEntity.AlertNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
{
   public interface IOccasionToCelebrateService
    {
        Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrate> occasions);
        //Task<bool> GetById(OccasionToCelebrate occasions);
        Task<bool> UpdateOccasionToCelebrate(IList<OccasionToCelebrate> occasions);
        Task<bool> GetOccasionToCelebrate(IList<string> customer);
    }
}
