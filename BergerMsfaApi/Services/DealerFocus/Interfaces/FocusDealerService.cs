using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Berger.Common.Constants;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class FocusDealerService:IFocusDealerService
    {
        private readonly IRepository<FocusDealer> _focusDealer;
        private readonly IRepository<UserInfo> _userInfoSvc;
        private readonly IRepository<DealerInfo> _dealerInfo;
        private readonly IRepository<DealerInfoStatusLog> _dealerInfoStatusLog;
        private readonly IMapper _mapper;
        public FocusDealerService(
            IRepository<FocusDealer> focusDealer,
            IRepository<UserInfo> userInfoSvc,
            IRepository<DealerInfo> dealerInfo,
            IRepository<DealerInfoStatusLog> dealerInfoStatusLog,
            IMapper mapper
            )
        {
            _focusDealer = focusDealer;
            _userInfoSvc = userInfoSvc;
            _dealerInfo = dealerInfo;
            _dealerInfoStatusLog = dealerInfoStatusLog;
            _mapper = mapper;
        }

        public async Task<IPagedList<FocusDealerModel>> GetFocusdealerListPaging(int index,int pageSize,string search)

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
                                    EmployeeName = $"{u.FullName}",
                                    Code = f.Code,
                                    DealerName = d.CustomerName,
                                    EmployeeId = f.EmployeeId,
                                    ValidFrom = f.ValidFrom.ToString("yyyy/MM/dd"),
                                    ValidTo = f.ValidTo.ToString("yyyy/MM/dd")
                                }).ToList();


            if (!string.IsNullOrEmpty(search))
                focusDealers = focusDealers.Search(search);
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
            var f = await _focusDealer.FindAsync(f => f.Id == id);
            return new FocusDealerModel
            {
                Id = f.Id,
                Code = f.Code,
                EmployeeId = f.EmployeeId,
                ValidFrom = f.ValidFrom.ToString("yyyy-MM-dd"),
                ValidTo = f.ValidTo.ToString("yyyy-MM-dd")
            };

        }

        #region Dealer
        public async Task<IPagedList<DealerModel>> GetDalerListPaging(int index, int pazeSize, string search)
        {

            var dealers = _dealerInfo.FindAll(x => !x.IsDeleted && x.Channel == ConstantsODataValue.DistrbutionChannelDealer).Select(s => new DealerModel
            {
                Id = s.Id, CustomerName = s.CustomerName,CustomerNo = s.CustomerNo, Address = s.Address,
                AccountGroup = s.AccountGroup,ContactNo = s.ContactNo,Area = s.SalesGroup, CustZone = s.CustZone,
                BusinessArea = s.BusinessArea,IsExclusiveLabel = s.IsExclusive ? "Exclusive" : "Non Exclusive",
                IsCBInstalledLabel = s.IsCBInstalled ? "Installed" : "Not Installed", IsCBInstalled = s.IsCBInstalled,
                IsExclusive = s.IsExclusive, IsLastYearAppointedLabel = s.IsLastYearAppointed ? "Last Year Appointed" : "Not Appointed",
                IsClubSupremeLabel = s.IsClubSupreme ? "Club Supreme" : "Not Club Supreme", IsLastYearAppointed = s.IsLastYearAppointed,
                IsClubSupreme = s.IsClubSupreme
            }).ToList();

            if (!string.IsNullOrEmpty(search)) dealers = dealers.Search(search);
          

            var result= dealers.OrderBy(o=>o.CustomerNo).ToPagedList(index,pazeSize);
            return result;
        }
       
        public async Task<bool> DealerStatusUpdate(DealerInfo dealer)
        {
            var find = await _dealerInfo.FindAsync(f => f.Id == dealer.Id);
            if (find == null) return false;

            await CreateDealerInfoStatusLog(dealer);

            find.IsCBInstalled = dealer.IsCBInstalled;
            find.IsExclusive = dealer.IsExclusive;
            find.IsLastYearAppointed = dealer.IsLastYearAppointed;
            find.IsClubSupreme = dealer.IsClubSupreme;
            await _dealerInfo.UpdateAsync(find);
            return true;
        }
        public async Task<bool> CreateDealerInfoStatusLog(DealerInfo dealer)
        {
            var find = await _dealerInfo.FindAsync(f => f.Id == dealer.Id);
            if (find == null) return false;
            try
            {
                var dealerInfoStatusLog = new DealerInfoStatusLog()
                {
                    DealerInfoId = find.Id,
                    UserId = AppIdentity.AppUser.UserId,
                    PropertyName = GetPropertyName(dealer, find),
                    PropertyValue = GetPropertyValue(dealer, find)
                };
                
                await _dealerInfoStatusLog.CreateAsync(dealerInfoStatusLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public string GetPropertyName(DealerInfo dealer, DealerInfo find)
        {
            string propertyName="";
            if (find.IsExclusive != dealer.IsExclusive)
                propertyName = "Exclusive";
            else if (find.IsCBInstalled != dealer.IsCBInstalled)
                propertyName = "CB Installed";
            else if (find.IsLastYearAppointed != dealer.IsLastYearAppointed)
                propertyName = "Last Year Appointed";
            else if (find.IsClubSupreme != dealer.IsClubSupreme)
                propertyName = "Club Supreme";

            return propertyName;
        }
        public string GetPropertyValue(DealerInfo dealer, DealerInfo find)
        {
            string propertyValue = "";
            if (find.IsExclusive != dealer.IsExclusive)
            {
                if (dealer.IsExclusive)
                    propertyValue = "Yes";
                else
                    propertyValue = "No";
            }    
            else if (find.IsCBInstalled != dealer.IsCBInstalled)
            {
                if (dealer.IsCBInstalled)
                    propertyValue = "Yes";
                else
                    propertyValue = "No";
            }
            else if (find.IsLastYearAppointed != dealer.IsLastYearAppointed)
            {
                if (dealer.IsLastYearAppointed)
                    propertyValue = "Yes";
                else
                    propertyValue = "No";
            }
            else if (find.IsClubSupreme != dealer.IsClubSupreme)
            {
                if (dealer.IsClubSupreme)
                    propertyValue = "Yes";
                else
                    propertyValue = "No";
            }

            return propertyValue;
        }
        public async Task<IEnumerable<DealerInfoStatusLogModel>> GetDealerInfoStatusLog(int dealerInfoId)
        {

            var result = await _dealerInfoStatusLog.GetAllIncludeAsync(
                        dealer => dealer,
                        dealer => dealer.DealerInfoId == dealerInfoId,
                        dealer => dealer.OrderByDescending(b => b.CreatedTime),
                        dealer => dealer.Include(i => i.DealerInfo).Include(i => i.User),
                        true
                );
            var modelResult = _mapper.Map<IEnumerable<DealerInfoStatusLogModel>>(result);
            return modelResult;
        }
        #endregion
    }
}
