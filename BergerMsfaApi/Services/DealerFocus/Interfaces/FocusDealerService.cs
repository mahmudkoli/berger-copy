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
            var focusDealers = await _focusDealer.GetAllIncludeAsync(
                  select => select,
                  p => p.EmployeeId == AppIdentity.AppUser.EmployeeId,
                  o => o.OrderBy(f => f.ValidTo.Date),
                  null,
                  true
                  );
            //   var result = await _focusDealer.FindAllPagedAsync(f => f.EmployeeId ==AppIdentity.AppUser.EmployeeId, index, pageSize);
            var result = focusDealers.ToPagedList<FocusDealer>(index, pageSize);

            if (!string.IsNullOrEmpty(searchDate))
                    result = result.Where(f => f.ValidFrom.Date <= Convert.ToDateTime(searchDate).Date && f.ValidTo.Date >= Convert.ToDateTime(searchDate).Date).ToPagedList();
            var  final = (from fd in result
                         join u in _userInfoSvc.GetAll()
                           on fd.EmployeeId equals u.EmployeeId
                         join d in _dealerInfo.GetAll()
                         on fd.Code equals d.Id
                         select new FocusDealerModel
                         {
                             Id = fd.Id,
                             EmployeeName =$"{u.FirstName},{u.LastName}",
                             Code = fd.Code,
                             DealerName = d.CustomerName,
                             EmployeeId = fd.EmployeeId,
                             ValidFrom = fd.ValidFrom,
                             ValidTo = fd.ValidTo
                         }).ToPagedList();

            return final;
           

        }

        public async Task<FocusDealerModel> CreateAsync(FocusDealerModel model)
        {
            var journeyPlan = model.ToMap<FocusDealerModel, FocusDealer>();
            journeyPlan.EmployeeId = AppIdentity.AppUser.EmployeeId;
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
