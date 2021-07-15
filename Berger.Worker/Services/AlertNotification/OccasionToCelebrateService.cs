using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services.AlertNotification
{
   public class OccasionToCelebrateService: IOccasionToCelebrateService
    {
        private readonly IRepository<OccasionToCelebrate> _repository;
        public OccasionToCelebrateService(IRepository<OccasionToCelebrate> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OccasionToCelebrate>> GetOccasionToCelebrate(IList<string> customer)
        {
            var result = _repository.Where(p => customer.Contains(p.CustomarNo) && p.NotificationDate.Date == DateTime.Today).ToList();
            return result;
        }

        public async Task<OccasionToCelebrate> GetByModel(OccasionToCelebrate occasionToCelebrate)
        {
            var res = _repository.Where(p => p.CustomarNo == occasionToCelebrate.CustomarNo).FirstOrDefault();
            var result = res.DOB != occasionToCelebrate.DOB ||
                res.SpouseDOB != occasionToCelebrate.SpouseDOB ||
                res.FirsChildDOB != occasionToCelebrate.FirsChildDOB ||
                res.SecondChildDOB != occasionToCelebrate.SecondChildDOB ||
                res.ThirdChildDOB != occasionToCelebrate.ThirdChildDOB;

            return result ? res : null;

        }

        public async Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrate> occasions)
        {
            bool res = false;
            foreach (var item in occasions)
            {
                var result = await GetByModel(item);
                if (result == null)
                {
                    await _repository.CreateAsync(item);

                }
                else
                {
                    result.DOB = item.DOB;
                    result.FirsChildDOB = item.FirsChildDOB;
                    result.SecondChildDOB = item.SecondChildDOB;
                    result.ThirdChildDOB = item.ThirdChildDOB;
                    res=await UpdateOccasionToCelebrate(result);


                }
            }
            return res;

        }

        public async Task<bool> UpdateOccasionToCelebrate(OccasionToCelebrate occasions)
        {
            var res=await _repository.UpdateAsync(occasions);
            return res!=null;
        }
    }
}
