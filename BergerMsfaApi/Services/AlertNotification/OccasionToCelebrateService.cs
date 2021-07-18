using Berger.Data.MsfaEntity.AlertNotification;
using BergerMsfaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
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
            var result = _repository.Where(p => customer.Contains(p.CustomarNo) && p.NotificationDate.Date==DateTime.Now.Date).ToList();
            return result;
        }

        //public async Task<OccasionToCelebrate> GetByModel(OccasionToCelebrate occasionToCelebrate)
        //{
        //    var res =  _repository.Where(p => p.CustomarNo == occasionToCelebrate.CustomarNo).FirstOrDefault();
        //    var result = res.DOB != occasionToCelebrate.DOB ||
        //        res.SpouseDOB != occasionToCelebrate.SpouseDOB ||
        //        res.FirsChildDOB != occasionToCelebrate.FirsChildDOB ||
        //        res.SecondChildDOB != occasionToCelebrate.SecondChildDOB ||
        //        res.ThirdChildDOB != occasionToCelebrate.ThirdChildDOB;

        //}

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
