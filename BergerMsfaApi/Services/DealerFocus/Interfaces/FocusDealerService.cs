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
using Berger.Common.Enumerations;
using BergerMsfaApi.Services.Excel.Interface;
using Microsoft.AspNetCore.Http;
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
        private readonly IExcelReaderService _excelReaderService;

        public FocusDealerService(
            IRepository<FocusDealer> focusDealer,
            IRepository<UserInfo> userInfoSvc,
            IRepository<DealerInfo> dealerInfo,
            IRepository<DealerInfoStatusLog> dealerInfoStatusLog,
            IMapper mapper,
            IExcelReaderService excelReaderService
            )
        {
            _focusDealer = focusDealer;
            _userInfoSvc = userInfoSvc;
            _dealerInfo = dealerInfo;
            _dealerInfoStatusLog = dealerInfoStatusLog;
            _mapper = mapper;
            _excelReaderService = excelReaderService;
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
                                    DealerName = $"{d.CustomerName} ({d.CustomerNo})",
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
            var isAlreadyAssigned = await IsFocusDealerAlreadyAssigned(model);
            if (isAlreadyAssigned) throw new Exception("This dealer has been already assigned within this date.");
            var journeyPlan = model.ToMap<FocusDealerModel, FocusDealer>();
            var result = await _focusDealer.CreateAsync(journeyPlan);
            return result.ToMap<FocusDealer, FocusDealerModel>();
        }
        public async Task<FocusDealerModel> UpdateAsync(FocusDealerModel model)
        {
            var isAlreadyAssigned = await IsFocusDealerAlreadyAssigned(model);
            if (isAlreadyAssigned) throw new Exception("This dealer has been already assigned within this date.");
            var journeyPlan = model.ToMap<FocusDealerModel, FocusDealer>();
            var result = await _focusDealer.UpdateAsync(journeyPlan);
            return result.ToMap<FocusDealer, FocusDealerModel>();
        }
        private async Task<bool> IsFocusDealerAlreadyAssigned(FocusDealerModel model)
        {
            var isExists = await _focusDealer.AnyAsync(x => x.Id != model.Id && x.EmployeeId == model.EmployeeId && x.Code == model.Code
                   && (((Convert.ToDateTime(model.ValidFrom).Date >= x.ValidFrom.Date && Convert.ToDateTime(model.ValidFrom).Date <= x.ValidTo.Date)
                       || (Convert.ToDateTime(model.ValidTo).Date >= x.ValidFrom.Date && Convert.ToDateTime(model.ValidTo).Date <= x.ValidTo.Date))
                       || ((x.ValidFrom.Date >= Convert.ToDateTime(model.ValidFrom).Date && x.ValidFrom.Date <= Convert.ToDateTime(model.ValidTo).Date)
                       || (x.ValidTo.Date >= Convert.ToDateTime(model.ValidFrom).Date && x.ValidTo.Date <= Convert.ToDateTime(model.ValidTo).Date))));
            return isExists;
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


            var dealers = _dealerInfo.FindAll(x => !x.IsDeleted &&
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
                IsExclusiveLabel = s.IsExclusive ? "Exclusive" : "Not Exclusive",
                //IsCBInstalledLabel = s.IsCBInstalled ? "Installed" : "Not Installed",
                //IsCBInstalled = s.IsCBInstalled,
                IsExclusive = s.IsExclusive,
                IsLastYearAppointedLabel = s.IsLastYearAppointed ? "Last Year Appointed" : "Not Appointed",
               // IsClubSupremeLabel = s.IsClubSupreme ? "Club Supreme" : "Not Club Supreme",
                IsLastYearAppointed = s.IsLastYearAppointed,
                ClubSupremeType = s.ClubSupremeType,
                Territory = s.Territory,
                IsAp = s.IsAP,
                IsApLabel = s.IsAP ? "AP" : "Not AP",
                SalesGroup = s.SalesGroup,
                SalesOffice = s.SalesOffice
            }).ToList();

            if (!string.IsNullOrEmpty(search)) dealers = dealers.Search(search);


            var result = dealers.OrderBy(o => o.CustomerName).ToPagedList(index, pazeSize);
            return result;
        }

        public async Task<bool> DealerStatusUpdate(DealerInfo dealer)
        {
            var find = await _dealerInfo.FindAsync(f => f.Id == dealer.Id);
            if (find == null) return false;

            await CreateDealerInfoStatusLog(dealer);

            //find.IsCBInstalled = dealer.IsCBInstalled;
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
            //else if (find.IsCBInstalled != dealer.IsCBInstalled)
            //    propertyName = "CB Installed";
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
            //else if (find.IsCBInstalled != dealer.IsCBInstalled)
            //{
            //    propertyValue = dealer.IsCBInstalled ? "Yes" : "No";
            //}
            else if (find.IsLastYearAppointed != dealer.IsLastYearAppointed)
            {
                propertyValue = dealer.IsLastYearAppointed ? "Yes" : "No";
            }
            else if (find.ClubSupremeType != dealer.ClubSupremeType)
            {
                propertyValue = EnumExtension.GetEnumDescription((EnumClubSupreme)dealer.ClubSupremeType);
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

        #region Excel Dealer Status Update
        public async Task<DealerStatusExcelExportModel> DealerStatusUpdate(DealerStatusExcelImportModel model)
        {
            var result = new DealerStatusExcelExportModel();

            switch (model.Type)
            {
                case EnumDealerStatusExcelImportType.Exclusive:
                    result = await this.DealerStatusExclusive(model.File);
                    break;
                case EnumDealerStatusExcelImportType.LastYearAppointed:
                    result = await this.DealerStatusLastYearAppointed(model.File);
                    break;
                case EnumDealerStatusExcelImportType.ClubSupreme:
                    result = await this.DealerStatusClubSupreme(model.File);
                    break;
                case EnumDealerStatusExcelImportType.AP:
                    result = await this.DealerStatusAP(model.File);
                    break;
                default:
                    break;
            }

            return result;
        }

        private async Task<DealerStatusExcelExportModel> DealerStatusClubSupreme(IFormFile file)
        {
            var userId = AppIdentity.AppUser.UserId;
            var excelModelList = await _excelReaderService.LoadDataAsync<DealerStatusClubSupremeExcelModel>(file);
            excelModelList.ForEach(x =>
                { 
                    try { x.ClubSupremeType = x.ClubSupremeStatus.ToEnumFromDisplayName<EnumClubSupreme>(); }
                    catch (Exception ex) { x.ClubSupremeType = null; } 
                }
            );

            var dealerIdList = excelModelList.Select(x => x.DealerId).ToList();
            var dbDealerInfoList = await _dealerInfo.FindByCondition(x => !x.IsDeleted &&
                    x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                    x.Division == ConstantsODataValue.DivisionDecorative &&
                    (dealerIdList.Contains(x.CustomerNo) || x.ClubSupremeType != EnumClubSupreme.None))
                .ToListAsync();

            List<DealerInfoStatusLog> dealerInfoStatusLogs = new List<DealerInfoStatusLog>();
            List<DealerInfo> updatedDealerInfos = new List<DealerInfo>();
            List<DealerStatusExcelExportDataModel> dealerStatusExportDataModels = new List<DealerStatusExcelExportDataModel>();

            #region dealer status excel export
            var dbDealerIdList = dbDealerInfoList.Select(x => x.CustomerNo);
            var notFoundDealers = excelModelList.Where(x => !dbDealerIdList.Contains(x.DealerId));
            foreach (var item in notFoundDealers)
            {
                dealerStatusExportDataModels.Add(new DealerStatusExcelExportDataModel { DealerId = item.DealerId, Status = item.ClubSupremeStatus, Result = "Not Found" });
            }
            var typeMismatchExcelModels = excelModelList.Where(x => x.ClubSupremeType == null);
            foreach (var item in typeMismatchExcelModels)
            {
                dealerStatusExportDataModels.Add(new DealerStatusExcelExportDataModel { DealerId = item.DealerId, Status = item.ClubSupremeStatus, Result = "Type Mismatch" });
            }
            #endregion

            foreach (var dealerInfo in dbDealerInfoList)
            {
                var excelModel = excelModelList.FirstOrDefault(x => x.DealerId == dealerInfo.CustomerNo);

                if (excelModel != null && excelModel.ClubSupremeType == null) continue; // check if type mismatch then no need to update;
                if (excelModel != null && dealerInfo.ClubSupremeType == excelModel.ClubSupremeType) continue; // check if already same status then no need to update;

                dealerInfo.ClubSupremeType = excelModel?.ClubSupremeType ?? EnumClubSupreme.None;

                var dealerInfoStatusLog = new DealerInfoStatusLog()
                {
                    DealerInfoId = dealerInfo.Id,
                    UserId = userId,
                    PropertyName = "Club Supreme",
                    PropertyValue = EnumExtension.GetEnumDescription((EnumClubSupreme)dealerInfo.ClubSupremeType)
                };

                updatedDealerInfos.Add(dealerInfo);
                dealerInfoStatusLogs.Add(dealerInfoStatusLog);
            }

            await _dealerInfo.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLog.CreateListAsync(dealerInfoStatusLogs);

            var list = dealerStatusExportDataModels.Select(x => new
            {
                x.DealerId,
                x.Status,
                x.Result
            }).ToList();

            byte[] writeToFile = _excelReaderService.WriteToFile(list);

            return new DealerStatusExcelExportModel
            {
                File = Convert.ToBase64String(writeToFile),
                FileName = "Dealer_Status_Error_ClubSupreme"
            };
        }

        private async Task<DealerStatusExcelExportModel> DealerStatusExclusive(IFormFile file)
        {
            var userId = AppIdentity.AppUser.UserId;
            var excelModelList = await _excelReaderService.LoadDataAsync<DealerStatusExclusiveExcelModel>(file);
            excelModelList.ForEach(x => x.ExclusiveStatus = !string.IsNullOrWhiteSpace(x.DealerId) ? "Exclusive" : "Not Exclusive");

            var dealerIdList = excelModelList.Select(x => x.DealerId).ToList();
            var dbDealerInfoList = await _dealerInfo.FindByCondition(x => !x.IsDeleted &&
                    x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                    x.Division == ConstantsODataValue.DivisionDecorative && 
                    (dealerIdList.Contains(x.CustomerNo) || x.IsExclusive))
                .ToListAsync();

            List<DealerInfoStatusLog> dealerInfoStatusLogs = new List<DealerInfoStatusLog>();
            List<DealerInfo> updatedDealerInfos = new List<DealerInfo>();
            List<DealerStatusExcelExportDataModel> dealerStatusExportDataModels = new List<DealerStatusExcelExportDataModel>();

            #region dealer status excel export
            var dbDealerIdList = dbDealerInfoList.Select(x => x.CustomerNo);
            var notFoundDealers = excelModelList.Where(x => !dbDealerIdList.Contains(x.DealerId));
            foreach (var item in notFoundDealers)
            {
                dealerStatusExportDataModels.Add(new DealerStatusExcelExportDataModel { DealerId = item.DealerId, Status = "Exclusive", Result = "Not Found" });
            }
            #endregion

            foreach (var dealerInfo in dbDealerInfoList)
            {
                var excelModel = excelModelList.FirstOrDefault(x => x.DealerId == dealerInfo.CustomerNo);

                if (dealerInfo.IsExclusive && excelModel != null) continue; // check if already same status then no need to update;

                dealerInfo.IsExclusive = excelModel != null;

                var dealerInfoStatusLog = new DealerInfoStatusLog()
                {
                    DealerInfoId = dealerInfo.Id,
                    UserId = userId,
                    PropertyName = "AP",
                    PropertyValue = dealerInfo.IsExclusive ? "Yes" : "No"
                };

                updatedDealerInfos.Add(dealerInfo);
                dealerInfoStatusLogs.Add(dealerInfoStatusLog);
            }

            await _dealerInfo.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLog.CreateListAsync(dealerInfoStatusLogs);

            var list = dealerStatusExportDataModels.Select(x => new
            {
                x.DealerId,
                x.Status,
                x.Result
            }).ToList();

            byte[] writeToFile = _excelReaderService.WriteToFile(list);

            return new DealerStatusExcelExportModel
            {
                File = Convert.ToBase64String(writeToFile),
                FileName = "Dealer_Status_Error_Exclusive"
            };
        }

        private async Task<DealerStatusExcelExportModel> DealerStatusLastYearAppointed(IFormFile file)
        {
            var userId = AppIdentity.AppUser.UserId;
            var excelModelList = await _excelReaderService.LoadDataAsync<DealerStatusLastYearAppointedExcelModel>(file);
            excelModelList.ForEach(x => x.LastYearAppointedStatus = !string.IsNullOrWhiteSpace(x.DealerId) ? "Last Year Appointed" : "Not Last Year Appointed");

            var dealerIdList = excelModelList.Select(x => x.DealerId).ToList();
            var dbDealerInfoList = await _dealerInfo.FindByCondition(x => !x.IsDeleted &&
                    x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                    x.Division == ConstantsODataValue.DivisionDecorative && 
                    (dealerIdList.Contains(x.CustomerNo) || x.IsLastYearAppointed))
                .ToListAsync();

            List<DealerInfoStatusLog> dealerInfoStatusLogs = new List<DealerInfoStatusLog>();
            List<DealerInfo> updatedDealerInfos = new List<DealerInfo>();
            List<DealerStatusExcelExportDataModel> dealerStatusExportDataModels = new List<DealerStatusExcelExportDataModel>();

            #region dealer status excel export
            var dbDealerIdList = dbDealerInfoList.Select(x => x.CustomerNo);
            var notFoundDealers = excelModelList.Where(x => !dbDealerIdList.Contains(x.DealerId));
            foreach (var item in notFoundDealers)
            {
                dealerStatusExportDataModels.Add(new DealerStatusExcelExportDataModel { DealerId = item.DealerId, Status = "Last Year Appointed", Result = "Not Found" });
            }
            #endregion

            foreach (var dealerInfo in dbDealerInfoList)
            {
                var excelModel = excelModelList.FirstOrDefault(x => x.DealerId == dealerInfo.CustomerNo);

                if (dealerInfo.IsLastYearAppointed && excelModel != null) continue; // check if already same status then no need to update;

                dealerInfo.IsLastYearAppointed = excelModel != null;

                var dealerInfoStatusLog = new DealerInfoStatusLog()
                {
                    DealerInfoId = dealerInfo.Id,
                    UserId = userId,
                    PropertyName = "Last Year Appointed",
                    PropertyValue = dealerInfo.IsLastYearAppointed ? "Yes" : "No"
                };

                updatedDealerInfos.Add(dealerInfo);
                dealerInfoStatusLogs.Add(dealerInfoStatusLog);
            }

            await _dealerInfo.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLog.CreateListAsync(dealerInfoStatusLogs);

            var list = dealerStatusExportDataModels.Select(x => new
            {
                x.DealerId,
                x.Status,
                x.Result
            }).ToList();

            byte[] writeToFile = _excelReaderService.WriteToFile(list);

            return new DealerStatusExcelExportModel
            {
                File = Convert.ToBase64String(writeToFile),
                FileName = "Dealer_Status_Error_LastYearAppointed"
            };
        }

        private async Task<DealerStatusExcelExportModel> DealerStatusAP(IFormFile file)
        {
            var userId = AppIdentity.AppUser.UserId;
            var excelModelList = await _excelReaderService.LoadDataAsync<DealerStatusAPExcelModel>(file);
            excelModelList.ForEach(x => x.APStatus = !string.IsNullOrWhiteSpace(x.DealerId) ? "AP" : "Not AP");

            var dealerIdList = excelModelList.Select(x => x.DealerId).ToList();
            var dbDealerInfoList = await _dealerInfo.FindByCondition(x => !x.IsDeleted &&
                    x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                    x.Division == ConstantsODataValue.DivisionDecorative && 
                    (dealerIdList.Contains(x.CustomerNo) || x.IsAP))
                .ToListAsync();

            List<DealerInfoStatusLog> dealerInfoStatusLogs = new List<DealerInfoStatusLog>();
            List<DealerInfo> updatedDealerInfos = new List<DealerInfo>();
            List<DealerStatusExcelExportDataModel> dealerStatusExportDataModels = new List<DealerStatusExcelExportDataModel>();

            #region dealer status excel export
            var dbDealerIdList = dbDealerInfoList.Select(x => x.CustomerNo);
            var notFoundDealers = excelModelList.Where(x => !dbDealerIdList.Contains(x.DealerId));
            foreach (var item in notFoundDealers)
            {
                dealerStatusExportDataModels.Add(new DealerStatusExcelExportDataModel { DealerId = item.DealerId, Status = "AP", Result = "Not Found" });
            }
            #endregion

            foreach (var dealerInfo in dbDealerInfoList)
            {
                var excelModel = excelModelList.FirstOrDefault(x => x.DealerId == dealerInfo.CustomerNo);

                if (dealerInfo.IsAP && excelModel != null) continue; // check if already same status then no need to update;

                dealerInfo.IsAP = excelModel != null;

                var dealerInfoStatusLog = new DealerInfoStatusLog()
                {
                    DealerInfoId = dealerInfo.Id,
                    UserId = userId,
                    PropertyName = "AP",
                    PropertyValue = dealerInfo.IsAP ? "Yes" : "No"
                };

                updatedDealerInfos.Add(dealerInfo);
                dealerInfoStatusLogs.Add(dealerInfoStatusLog);
            }

            await _dealerInfo.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLog.CreateListAsync(dealerInfoStatusLogs);

            var list = dealerStatusExportDataModels.Select(x => new
            {
                x.DealerId,
                x.Status,
                x.Result
            }).ToList();

            byte[] writeToFile = _excelReaderService.WriteToFile(list);

            return new DealerStatusExcelExportModel
            {
                File = Convert.ToBase64String(writeToFile),
                FileName = "Dealer_Status_Error_AP"
            };
        }
        #endregion
    }
}
