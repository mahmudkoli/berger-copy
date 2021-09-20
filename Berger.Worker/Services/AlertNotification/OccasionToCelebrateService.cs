using Berger.Data.MsfaEntity.AlertNotification;
using Berger.Worker.Repositories;
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
        private readonly IApplicationRepository<OccasionToCelebrate> _repository;
        public OccasionToCelebrateService(IApplicationRepository<OccasionToCelebrate> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OccasionToCelebrate>> GetOccasionToCelebrate(IList<string> customer)
        {
            var result = _repository.Where(p => customer.Contains(p.CustomarNo) && p.NotificationDate.Date == DateTime.Today).ToList();
            return result;
        }

        public async Task<bool> GetByModel(OccasionToCelebrate occasionToCelebrate)
        {
            var res = _repository.Where(p => p.CustomarNo == occasionToCelebrate.CustomarNo).FirstOrDefault();
            var result = res.DOB != occasionToCelebrate.DOB ||
                res.SpouseDOB != occasionToCelebrate.SpouseDOB ||
                res.FirsChildDOB != occasionToCelebrate.FirsChildDOB ||
                res.SecondChildDOB != occasionToCelebrate.SecondChildDOB ||
                res.ThirdChildDOB != occasionToCelebrate.ThirdChildDOB;
            return result ;

        }

        public async Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrate> occasions)
        {
            bool res = false;
            foreach (var item in occasions)
            {
                var model = _repository.Where(p => p.CustomarNo == item.CustomarNo).FirstOrDefault();
                if (model == null)
                {
                    await _repository.CreateAsync(item);

                }
                else
                {

                    model.DOB = item.DOB;
                    model.FirsChildDOB = item.FirsChildDOB;
                    model.SecondChildDOB = item.SecondChildDOB;
                    model.ThirdChildDOB = item.ThirdChildDOB;
                    res=await UpdateOccasionToCelebrate(model);


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
