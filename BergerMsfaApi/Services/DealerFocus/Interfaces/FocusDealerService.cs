﻿using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
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
        public FocusDealerService(
            IRepository<FocusDealer> focusDealer,
            IRepository<UserInfo> userInfoSvc,
            IRepository<DealerInfo> dealerInfo
            )
        {
            _focusDealer = focusDealer;
            _userInfoSvc = userInfoSvc;
            _dealerInfo = dealerInfo;
        }

        public async Task<IPagedList<FocusDealerModel>> GetFocusdealerListPaging(int index,int pageSize,string searchDate)

        {
            var focusDealers = (from f in _focusDealer.GetAll()
                                join u in _userInfoSvc.FindAll(f => f.ManagerId == AppIdentity.AppUser.EmployeeId)
                                on f.EmployeeId equals u.EmployeeId
                                join d in _dealerInfo.GetAll()
                                on f.Code equals d.Id
                                orderby f.ValidTo.Date   descending
                                select new FocusDealerModel
                                {
                                    Id = f.Id,
                                    EmployeeName = $"{u.FirstName},{u.LastName}",
                                    Code = f.Code,
                                    DealerName = d.CustomerName,
                                    EmployeeId = f.EmployeeId,
                                    ValidFrom = f.ValidFrom,
                                    ValidTo = f.ValidTo
                                });


            if (!string.IsNullOrEmpty(searchDate))
                focusDealers = focusDealers.Where(f => f.ValidFrom.Date <= Convert.ToDateTime(searchDate).Date && f.ValidTo.Date >= Convert.ToDateTime(searchDate).Date);
            var result = await focusDealers.ToPagedListAsync(index, pageSize);
            return result;
           

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

        public async Task<IPagedList<DealerModel>> GetDalerListPaging(int index, int pazeSize, string search)
        {


            var dealers = _dealerInfo.GetAll().Select(s => new DealerModel
            {
                Id=s.Id,
                CustomerName = s.CustomerName,
                CustomerNo = s.CustomerNo,
                Address = s.Address,
                AccountGroup = s.AccountGroup,
                ContactNo = s.ContactNo,
                Area = s.Area,
                CustZone=s.CustZone,
                BusinessArea=s.BusinessArea,
                IsExclusiveLabel=s.IsExclusive? "Exclusive": "Non Exclusive",
                IsCBInstalledLabel=s.IsCBInstalled ? "Installed" : "Not Installed",
                IsCBInstalled=s.IsCBInstalled,
                IsExclusive = s.IsExclusive
            });
            if (!string.IsNullOrEmpty(search))
            {
                
                var result = dealers.ToDictionary(s => s.Id, f => f.RowToString());
                var Keys = (from r in result where r.Value.Contains(search) select r.Key).ToList();
                dealers = dealers.Where(f => Keys.Contains(f.Id));
            }


            return dealers.OrderBy(o=>o.CustomerNo).ToPagedList(index,pazeSize);
        }
       
        public async Task<bool> DealerStatusUpdate(DealerInfo dealer)
        {
            var find = await _dealerInfo.FindAsync(f => f.CustomerNo == dealer.CustomerNo);
            if (find == null) return false;
            find.IsCBInstalled = dealer.IsCBInstalled;
            find.IsExclusive = dealer.IsExclusive;
            await _dealerInfo.UpdateAsync(find);
            return true;
        }
    }
}
