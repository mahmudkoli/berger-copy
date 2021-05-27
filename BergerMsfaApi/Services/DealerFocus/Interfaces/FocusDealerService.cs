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
using String = EllipticCurve.Utils.String;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class FocusDealerService : IFocusDealerService
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

        public async Task<IPagedList<FocusDealerModel>> GetFocusdealerListPaging(int index, int pageSize, string search, string depoId, string[] territories = null, string[] zones = null)

        {

            territories ??= new string[] { };
            zones ??= new string[] { };

            var focusDealers = (from f in _focusDealer.GetAll()
                                join u in _userInfoSvc.FindAll(f => f.ManagerId == AppIdentity.AppUser.EmployeeId)
                                on f.EmployeeId equals u.EmployeeId
                                join d in _dealerInfo.GetAll()
                                on f.Code equals d.Id
                                orderby f.ValidTo.Date descending
                                select new FocusDealerModel
                                {
                                    Id = f.Id,
                                    EmployeeName = $"{u.FullName}",
                                    Code = f.Code,
                                    DealerName = d.CustomerName,
                                    EmployeeId = f.EmployeeId,
                                    ValidFrom = f.ValidFrom.ToString("yyyy/MM/dd"),
                                    ValidTo = f.ValidTo.ToString("yyyy/MM/dd"),
                                    Territory = d.Territory,
                                    Zone = d.CustZone,
                                    DepoId = d.BusinessArea
                                }).Where(x => (!territories.Any() || territories.Contains(x.Territory)) &&
                                              (!zones.Any() || zones.Contains(x.Zone)) &&
                                              (string.IsNullOrWhiteSpace(depoId) || x.DepoId == depoId)).ToList();


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
        public async Task<IPagedList<DealerModel>> GetDalerListPaging(int index, int pazeSize, string search, string depoId = null, string[] territories = null, string[] custZones = null, string[] salesGroup = null)
        {

            territories ??= new string[] { };
            custZones ??= new string[] { };
            salesGroup ??= new string[] { };


            var dealers =  _dealerInfo.FindAll(x => !x.IsDeleted &&
                x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                x.Division == ConstantsODataValue.DivisionDecorative &&
                (string.IsNullOrWhiteSpace(depoId) || x.BusinessArea == depoId) &&
                (!territories.Any() || territories.Contains(x.Territory)) &&
                (!custZones.Any() || custZones.Contains(x.CustZone)) &&
                (!salesGroup.Any() || salesGroup.Contains(x.SalesGroup))
                )
            .Select(s => new DealerModel
            {
                Id = s.Id,
                CustomerName = s.CustomerName,
                CustomerNo = s.CustomerNo,
                Address = s.Address,
                AccountGroup = s.AccountGroup,
                ContactNo = s.ContactNo,
                Area = s.SalesGroup,
                CustZone = s.CustZone,
                BusinessArea = s.BusinessArea,
                IsExclusiveLabel = s.IsExclusive ? "Exclusive" : "Non Exclusive",
                IsCBInstalledLabel = s.IsCBInstalled ? "Installed" : "Not Installed",
                IsCBInstalled = s.IsCBInstalled,
                IsExclusive = s.IsExclusive,
                IsLastYearAppointedLabel = s.IsLastYearAppointed ? "Last Year Appointed" : "Not Appointed",
                //IsClubSupremeLabel = s.IsClubSupreme ? "Club Supreme" : "Not Club Supreme",
                IsLastYearAppointed = s.IsLastYearAppointed,
                ClubSupremeType = s.ClubSupremeType,
                Territory = s.Territory,
                IsAp = s.IsAP,
                IsApLabel = s.IsAP ? "Yes" : "No",
                SalesGroup = s.SalesGroup,
                SalesOffice = s.SalesOffice
            }).ToList();

            if (!string.IsNullOrEmpty(search)) dealers = dealers.Search(search);


            var result = dealers.OrderBy(o => o.CustomerNo).ToPagedList(index, pazeSize);
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
            find.ClubSupremeType = dealer.ClubSupremeType;
            find.IsAP = dealer.IsAP;
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
            string propertyName = "";
            if (find.IsExclusive != dealer.IsExclusive)
                propertyName = "Exclusive";
            else if (find.IsCBInstalled != dealer.IsCBInstalled)
                propertyName = "CB Installed";
            else if (find.IsLastYearAppointed != dealer.IsLastYearAppointed)
                propertyName = "Last Year Appointed";
            else if (find.ClubSupremeType != dealer.ClubSupremeType)
                propertyName = "Club Supreme";
            else if (find.IsAP != dealer.IsAP)
                propertyName = "AP";

            return propertyName;
        }
        public string GetPropertyValue(DealerInfo dealer, DealerInfo find)
        {
            string propertyValue = "";
            if (find.IsExclusive != dealer.IsExclusive)
            {
                propertyValue = dealer.IsExclusive ? "Yes" : "No";
            }
            else if (find.IsCBInstalled != dealer.IsCBInstalled)
            {
                propertyValue = dealer.IsCBInstalled ? "Yes" : "No";
            }
            else if (find.IsLastYearAppointed != dealer.IsLastYearAppointed)
            {
                propertyValue = dealer.IsLastYearAppointed ? "Yes" : "No";
            }
            else if (find.ClubSupremeType != dealer.ClubSupremeType)
            {
               // propertyValue = dealer.ClubSupremeType ? "Yes" : "No";
            }
            else if (find.IsAP != dealer.IsAP)
            {
                propertyValue = dealer.IsAP ? "Yes" : "No";
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
