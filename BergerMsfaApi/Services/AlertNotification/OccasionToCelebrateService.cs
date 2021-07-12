using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Service.AlertNotification
{
   public class OccasionToCelebrateService: IOccasionToCelebrateService
    {
        private readonly IRepository<OccasionToCelebrate> _repository;
        public OccasionToCelebrateService(IRepository<OccasionToCelebrate> repository)
        {
            _repository = repository;
        }

        public Task<bool> GetById(Expression<Func<bool, bool>> predicate)
        {
            var res = _repository.
        }

        public async Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrate> occasions)
        {
            var res = await _repository.CreateListAsync(occasions.ToList());
            return res !=null;

        }

        public async Task<bool> UpdateOccasionToCelebrate(IList<OccasionToCelebrate> occasions)
        {
            var res=await _repository.UpdateListAsync(occasions.ToList());
            return res!=null;
        }
    }
}
