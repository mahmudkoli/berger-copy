using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class FocusDealerService:IFocusDealerService
    {
        private readonly IRepository<FocusDealer> _focusDealer;
        private readonly IRepository<UserInfo> _userInfoSvc;
        private readonly IRepository<DealerInfo> _dealerInfo;
        public FocusDealerService(IRepository<FocusDealer> focusDealer, 
            IRepository<UserInfo> userInfoSvc,
            IRepository<DealerInfo> dealerInfo
            )
        {
            _focusDealer = focusDealer;
            _userInfoSvc=userInfoSvc;
            _dealerInfo = dealerInfo;
    }

        public async Task<IPagedList<FocusDealerModel>> GetFocusdealerListPaging(int index,int pageSize,string searchDate)

        {
            var result = await _focusDealer.FindAllPagedAsync(f => f.EmployeeRegId == "5", index, pageSize);
          
                if (!string.IsNullOrEmpty(searchDate))
                    result = result.Where(f => f.ValidFrom.Date.Equals(Convert.ToDateTime(searchDate).Date) || f.ValidTo.Date.Equals(Convert.ToDateTime(searchDate).Date)).ToPagedList();
            var  data = (from fd in result
                         join u in _userInfoSvc.GetAll()
                           on fd.EmployeeRegId equals u.EmployeeId
                         join d in _dealerInfo.GetAll()
                         on fd.Code equals d.Id
                         select new FocusDealerModel
                         {
                             Id = fd.Id,
                             EmployeeName = u.FirstName,
                             Code = fd.Code,
                             DealerName = d.CustomerName,
                             EmployeeRegId = fd.EmployeeRegId,
                             ValidFrom = fd.ValidFrom,
                             ValidTo = fd.ValidTo
                         }).ToPagedList();

            return data;
            //var result = from fd  in focusDealers
            //           join u in _userInfoSvc.GetAll()
            //             on fd.EmployeeRegId equals u.EmployeeId
            //             join d in _dealerInfo.GetAll()
            //             on fd.Code equals d.Id
            //             select new FocusDealerModel
            //             {
            //                 Id = fd.Id,
            //                 EmployeeName = u.FirstName,
            //                 Code = fd.Code,
            //                 DealerName = d.CustomerName,
            //                 EmployeeRegId = fd.EmployeeRegId,
            //                 ValidFrom = fd.ValidFrom,
            //                 ValidTo = fd.ValidTo
            //             };
           

        }

        public async Task<FocusDealerModel> CreateAsync(FocusDealerModel model)
        {
            var journeyPlan = model.ToMap<FocusDealerModel, FocusDealer>();
            var result = await _focusDealer.CreateAsync(journeyPlan);
            return result.ToMap<FocusDealer, FocusDealerModel>();
        }
        public async Task<FocusDealerModel> UpdateAsync(FocusDealerModel model)
        {
            var journeyPlan = model.ToMap<FocusDealerModel, FocusDealer>();
            var result = await _focusDealer.UpdateAsync(journeyPlan);
            return result.ToMap<FocusDealer, FocusDealerModel>();
        }
        public async Task<int> DeleteAsync(int id) => await _focusDealer.DeleteAsync(s => s.Id == id);

        public async Task<bool> IsExistAsync(int id) => await _focusDealer.IsExistAsync(f => f.Id == id);
        public async Task<FocusDealerModel> GetFocusDealerById(int id)
        {
            var result = await _focusDealer.FindAsync(f => f.Id == id);
            return result.ToMap<FocusDealer, FocusDealerModel>();
        }

    }
}
