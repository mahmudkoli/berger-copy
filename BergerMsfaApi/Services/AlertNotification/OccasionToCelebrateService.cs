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
    public class OccasionToCelebrateService : IOccasionToCelebrateService
    {
        private readonly IRepository<OccasionToCelebrateNotification> _repository;
        public OccasionToCelebrateService(IRepository<OccasionToCelebrateNotification> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OccasionToCelebrateNotification>> GetOccasionToCelebrate(IList<string> customer)
        {
            var tpDate = DateTime.Now;

            var result = _repository.Where(p => customer.Contains(p.CustomarNo) &&
                ((p.DOB.HasValue && (p.DOB.Value.Date.Month == tpDate.Month && p.DOB.Value.Day == tpDate.Day)) ||
                (p.FirsChildDOB.HasValue && (p.FirsChildDOB.Value.Date.Month == tpDate.Month && p.FirsChildDOB.Value.Day == tpDate.Day)) ||
                (p.SecondChildDOB.HasValue && (p.SecondChildDOB.Value.Date.Month == tpDate.Month && p.SecondChildDOB.Value.Day == tpDate.Day)) ||
                (p.ThirdChildDOB.HasValue && (p.ThirdChildDOB.Value.Date.Month == tpDate.Month && p.ThirdChildDOB.Value.Day == tpDate.Day)) ||
                (p.SpouseDOB.HasValue && (p.SpouseDOB.Value.Date.Month == tpDate.Month && p.SpouseDOB.Value.Day == tpDate.Day)))
            ).ToList();
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

        public async Task<bool> SaveOccasionToCelebrate(IList<OccasionToCelebrateNotification> occasions)
        {
            var res = await _repository.CreateListAsync(occasions.ToList());
            return res != null;

        }

        public async Task<bool> UpdateOccasionToCelebrate(IList<OccasionToCelebrateNotification> occasions)
        {
            var res = await _repository.UpdateListAsync(occasions.ToList());
            return res != null;
        }
    }
}
