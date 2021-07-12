using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.AlertNotification.Worker.AlertNotification
{
   public class OccasionToCelebrateService: IOccasionToCelebrateService
    {
        private readonly IRepository<OccasionToCelebrate> _repository;
        public OccasionToCelebrateService(IRepository<OccasionToCelebrate> repository)
        {
            _repository = repository;
        }

        public async Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrate> occasions)
        {
            await _repository.CreateListAsync(occasions.ToList());
            var res = await _repository.SaveChangesAsync();
            return res > 0;

        }

    }
}
