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
using BergerMsfaApi.Models.Common;
using System.Linq.Expressions;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class FocusDealerService : IFocusDealerService
    {
        private readonly IRepository<FocusDealer> _focusDealerRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<DealerInfo> _dealerInfoRepository;
        private readonly IRepository<DealerInfoStatusLog> _dealerInfoStatusLogRepository;
        private readonly IMapper _mapper;
        private readonly IExcelReaderService _excelReaderService;

        public FocusDealerService(
            IRepository<FocusDealer> focusDealerRepo,
            IRepository<UserInfo> userInfoRepo,
            IRepository<DealerInfo> dealerInfoRepo,
            IRepository<DealerInfoStatusLog> dealerInfoStatusLogRepo,
            IMapper mapper,
            IExcelReaderService excelReaderService
            )
        {
            _focusDealerRepository = focusDealerRepo;
            _userInfoRepository = userInfoRepo;
            _dealerInfoRepository = dealerInfoRepo;
            _dealerInfoStatusLogRepository = dealerInfoStatusLogRepo;
            _mapper = mapper;
            _excelReaderService = excelReaderService;
        }

        #region Focus Dealer
        public async Task<QueryResultModel<FocusDealerModel>> GetAllFocusDealersAsync(FocusDealerQueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<FocusDealerModel, object>>>()
            {
                ["createdTime"] = v => v.CreatedTime,
                ["dealerName"] = v => v.CustomerName,
                ["userFullName"] = v => v.FullName,
                ["validFromText"] = v => v.ValidFrom,
                ["validToText"] = v => v.ValidTo
            };
            //var loggedInUser = AppIdentity.AppUser;
            //var isAdminOrGMEmployeeRole = loggedInUser.EmployeeRole == (int)EnumEmployeeRole.Admin || loggedInUser.EmployeeRole == (int)EnumEmployeeRole.GM;

            var result = (
                            from fd in _focusDealerRepository.GetAll()
                            join ui in _userInfoRepository.GetAll() on fd.EmployeeId equals ui.EmployeeId
                            join di in _dealerInfoRepository.GetAll() on fd.DealerId equals di.Id
                            //where (ui.ManagerId == loggedInUser.EmployeeId || isAdminOrGMEmployeeRole)
                            select new FocusDealerModel
                            {
                                Id = fd.Id,
                                DealerId = fd.DealerId,
                                EmployeeId = fd.EmployeeId,
                                ValidFrom = fd.ValidFrom,
                                ValidTo = fd.ValidTo,
                                CustomerNo = di.CustomerNo,
                                CustomerName = di.CustomerName,
                                FullName = ui.FullName,
                                Depot = di.BusinessArea,
                                Territory = di.Territory,
                                Zone = di.CustZone,
                                CreatedTime = fd.CreatedTime
                            });

            Expression<Func<FocusDealerModel, object>> keySelector = columnsMap[query.SortBy];

            var total = await result.CountAsync();

            result = result.Where(x =>
                (string.IsNullOrEmpty(query.GlobalSearchValue) || x.CustomerName.Contains(query.GlobalSearchValue) || x.FullName.Contains(query.GlobalSearchValue))
                    && (string.IsNullOrEmpty(query.Depot) || query.Depot == x.Depot)
                    && (!query.Territories.Any() || query.Territories.Contains(x.Territory))
                    && (!query.Zones.Any() || query.Zones.Contains(x.Zone)));

            var filterCount = await result.CountAsync();


            result = query.IsSortAscending ? result.OrderBy(keySelector) : result.OrderByDescending(keySelector);

            result = result.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize);

            result = result.AsNoTracking();

            var items = await result.ToListAsync();

            var queryResult = new QueryResultModel<FocusDealerModel>
            {
                Items = items,
                TotalFilter = filterCount,
                Total = total
            };

            return queryResult;
        }

        public async Task<int> CreateFocusDealerAsync(SaveFocusDealerModel model)
        {
            var isAlreadyAssigned = await IsFocusDealerAlreadyAssigned(model);
            if (isAlreadyAssigned) throw new Exception("This dealer has been already assigned within this date.");
            var focusDealer = _mapper.Map<FocusDealer>(model);
            var result = await _focusDealerRepository.CreateAsync(focusDealer);
            return result.Id;
        }

        public async Task<int> UpdateFocusDealerAsync(SaveFocusDealerModel model)
        {
            var isAlreadyAssigned = await IsFocusDealerAlreadyAssigned(model);
            if (isAlreadyAssigned) throw new Exception("This dealer has been already assigned within this date.");
            var focusDealer = _mapper.Map<FocusDealer>(model);
            var result = await _focusDealerRepository.UpdateAsync(focusDealer);
            return result.Id;
        }

        public async Task<int> DeleteFocusDealerAsync(int id) => await _focusDealerRepository.DeleteAsync(s => s.Id == id);

        public async Task<bool> IsExistFocusDealerAsync(int id) => await _focusDealerRepository.IsExistAsync(f => f.Id == id);

        public async Task<FocusDealerModel> GetFocusDealerById(int id)
        {
            var result = (
                            from fd in _focusDealerRepository.GetAll()
                            join ui in _userInfoRepository.GetAll() on fd.EmployeeId equals ui.EmployeeId
                            join di in _dealerInfoRepository.GetAll() on fd.DealerId equals di.Id
                            select new FocusDealerModel
                            {
                                Id = fd.Id,
                                DealerId = fd.DealerId,
                                EmployeeId = fd.EmployeeId,
                                ValidFrom = fd.ValidFrom,
                                ValidTo = fd.ValidTo,
                                CustomerNo = di.CustomerNo,
                                CustomerName = di.CustomerName,
                                FullName = ui.FullName,
                                Depot = di.BusinessArea,
                                Territory = di.Territory,
                                Zone = di.CustZone,
                                CreatedTime = fd.CreatedTime
                            });

            var returnResult = await result.FirstOrDefaultAsync(x => x.Id == id);

            return returnResult;
        }

        private async Task<bool> IsFocusDealerAlreadyAssigned(SaveFocusDealerModel model)
        {
            var isExists = await _focusDealerRepository.AnyAsync(x => x.Id != model.Id && x.EmployeeId == model.EmployeeId && x.DealerId == model.DealerId
                   && (((Convert.ToDateTime(model.ValidFrom).Date >= x.ValidFrom.Date && Convert.ToDateTime(model.ValidFrom).Date <= x.ValidTo.Date)
                       || (Convert.ToDateTime(model.ValidTo).Date >= x.ValidFrom.Date && Convert.ToDateTime(model.ValidTo).Date <= x.ValidTo.Date))
                       || ((x.ValidFrom.Date >= Convert.ToDateTime(model.ValidFrom).Date && x.ValidFrom.Date <= Convert.ToDateTime(model.ValidTo).Date)
                       || (x.ValidTo.Date >= Convert.ToDateTime(model.ValidFrom).Date && x.ValidTo.Date <= Convert.ToDateTime(model.ValidTo).Date))));
            return isExists;
        }
        #endregion

        #region Dealer
        public async Task<QueryResultModel<DealerInfoPortalModel>> GetAllDealersAsync(DealerInfoQueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DealerInfo, object>>>()
            {
                ["customerName"] = v => v.CustomerName,
                ["customerNo"] = v => v.CustomerNo,
                ["isLastYearAppointedText"] = v => v.IsLastYearAppointed,
                ["clubSupremeTypeDropdown"] = v => v.ClubSupremeType,
                ["bussinesCategoryTypeDrodown"] = v => v.BussinesCategoryType,
            };

            var result = await _dealerInfoRepository.GetAllIncludeAsync(
                                x => x,
                                x => !x.IsDeleted &&
                                    x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                                    x.Division == ConstantsODataValue.DivisionDecorative &&
                                    ((string.IsNullOrEmpty(query.GlobalSearchValue) || x.CustomerName.Contains(query.GlobalSearchValue) ||
                                        x.CustomerNo.Contains(query.GlobalSearchValue)) &&
                                    (string.IsNullOrEmpty(query.Depot) || x.BusinessArea == query.Depot) &&
                                    (!query.Territories.Any() || query.Territories.Contains(x.Territory)) &&
                                    (query.DealerId==0 || query.DealerId==x.Id) &&

                                    (!query.Zones.Any() || query.Zones.Contains(x.CustZone))),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                null,
                                query.Page,
                                query.PageSize,
                                true
                            );

            var totalCount = await _dealerInfoRepository.CountFuncAsync(x => !x.IsDeleted &&
                                                                        x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                                                                        x.Division == ConstantsODataValue.DivisionDecorative);

            var modelResult = _mapper.Map<IList<DealerInfoPortalModel>>(result.Items);

            var queryResult = new QueryResultModel<DealerInfoPortalModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = totalCount;

            return queryResult;
        }

        public async Task<bool> DealerStatusUpdate(DealerInfoStatusModel dealer)
        {
            var userId = AppIdentity.AppUser.UserId;

            var find = await _dealerInfoRepository.FindAsync(f => f.Id == dealer.DealerId);
            if (find == null) return false;

            switch (dealer.PropertyName)
            {
                case "IsLastYearAppointed": find.IsLastYearAppointed = !find.IsLastYearAppointed; break;
                case "ClubSupremeType": find.ClubSupremeType = (EnumClubSupreme)Enum.Parse(typeof(EnumClubSupreme), dealer.PropertyValue.ToString()); break;
                case "BussinesCategoryType": find.BussinesCategoryType = (EnumBussinesCategory)Enum.Parse(typeof(EnumBussinesCategory), dealer.PropertyValue.ToString()); break;
                default: break;
            }

            await _dealerInfoRepository.UpdateAsync(find);

            // Create Dealer info status log
            await CreateDealerInfoStatusLog(dealer, userId, find);

            return true;
        }

        private async Task CreateDealerInfoStatusLog(DealerInfoStatusModel dealerStatus, int userId, DealerInfo find)
        {
            var dealerStatusLog = new DealerInfoStatusLog()
            {
                UserId = userId,
                DealerInfoId = find.Id,
                PropertyValue = GetPropertyValue(dealerStatus.PropertyName, find),
                PropertyName = GetPropertyName(dealerStatus.PropertyName)

            };

            await _dealerInfoStatusLogRepository.CreateAsync(dealerStatusLog);
        }

        private string GetPropertyValue(string propertyName, DealerInfo dealerInfo)
        {
            string value = "";
            switch (propertyName)
            {
                case "IsLastYearAppointed": value = (dealerInfo.IsLastYearAppointed ? "Yes" : "No"); break;
                case "ClubSupremeType": value = EnumExtension.GetEnumDescription(dealerInfo.ClubSupremeType); break;
                case "BussinesCategoryType": value = EnumExtension.GetEnumDescription(dealerInfo.BussinesCategoryType); break;
                default: break;
            }
            return value;
        }

        private string GetPropertyName(string propertyName)
        {
            string value = "";
            switch (propertyName)
            {
                case "IsLastYearAppointed": value = "Last Year Appointed"; break;
                case "ClubSupremeType": value = "Club Supreme"; break;
                case "BussinesCategoryType": value = "Bussines Category"; break;
                default: break;
            }
            return value;
        }

        public async Task<IList<DealerInfoStatusLogModel>> GetDealerInfoStatusLog(int dealerInfoId)
        {
            var result = await _dealerInfoStatusLogRepository.GetAllIncludeAsync(
                            x => x,
                            x => x.DealerInfoId == dealerInfoId,
                            x => x.OrderByDescending(b => b.CreatedTime),
                            x => x.Include(i => i.DealerInfo).Include(i => i.User),
                            true);

            var modelResult = _mapper.Map<IList<DealerInfoStatusLogModel>>(result);

            return modelResult;
        }
        #endregion

        #region Excel Dealer Status Update
        public async Task<DealerStatusExcelExportModel> DealerStatusUpdate(DealerStatusExcelImportModel model)
        {
            var result = new DealerStatusExcelExportModel();

            switch (model.Type)
            {
                
                case EnumDealerStatusExcelImportType.LastYearAppointed:
                    result = await this.DealerStatusLastYearAppointed(model.File);
                    break;
                case EnumDealerStatusExcelImportType.ClubSupreme:
                    result = await this.DealerStatusClubSupreme(model.File);
                    break;
                case EnumDealerStatusExcelImportType.BussinessCategory:
                    result = await this.DealerStatusBussinessCategory(model.File);
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
            var dbDealerInfoList = await _dealerInfoRepository.FindByCondition(x => !x.IsDeleted &&
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

            await _dealerInfoRepository.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLogRepository.CreateListAsync(dealerInfoStatusLogs);

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

        private async Task<DealerStatusExcelExportModel> DealerStatusBussinessCategory(IFormFile file)
        {
            var userId = AppIdentity.AppUser.UserId;
            var excelModelList = await _excelReaderService.LoadDataAsync<DealerStatusBussinessCategoryExcelModel>(file);
            excelModelList.ForEach(x =>
            {
                try { x.BussinesCategoryType = x.BussinesCategoryStatus.ToEnumFromDisplayName<EnumBussinesCategory>(); }
                catch (Exception ex) { x.BussinesCategoryType = null; }
            }
            );

            var dealerIdList = excelModelList.Select(x => x.DealerId).ToList();
            var dbDealerInfoList = await _dealerInfoRepository.FindByCondition(x => !x.IsDeleted &&
                    x.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
                    x.Division == ConstantsODataValue.DivisionDecorative &&
                    (dealerIdList.Contains(x.CustomerNo)
                    || x.BussinesCategoryType != EnumBussinesCategory.None
                    )
                    )
                .ToListAsync();

            List<DealerInfoStatusLog> dealerInfoStatusLogs = new List<DealerInfoStatusLog>();
            List<DealerInfo> updatedDealerInfos = new List<DealerInfo>();
            List<DealerStatusExcelExportDataModel> dealerStatusExportDataModels = new List<DealerStatusExcelExportDataModel>();

            #region dealer status excel export
            var dbDealerIdList = dbDealerInfoList.Select(x => x.CustomerNo);
            var notFoundDealers = excelModelList.Where(x => !dbDealerIdList.Contains(x.DealerId));
            foreach (var item in notFoundDealers)
            {
                dealerStatusExportDataModels.Add(new DealerStatusExcelExportDataModel { DealerId = item.DealerId, Status = item.BussinesCategoryStatus, Result = "Not Found" });
            }
            var typeMismatchExcelModels = excelModelList.Where(x => x.BussinesCategoryType == null);
            foreach (var item in typeMismatchExcelModels)
            {
                dealerStatusExportDataModels.Add(new DealerStatusExcelExportDataModel { DealerId = item.DealerId, Status = item.BussinesCategoryStatus, Result = "Type Mismatch" });
            }
            #endregion

            foreach (var dealerInfo in dbDealerInfoList)
            {
                var excelModel = excelModelList.FirstOrDefault(x => x.DealerId == dealerInfo.CustomerNo);

                if (excelModel != null && excelModel.BussinesCategoryType == null) continue; // check if type mismatch then no need to update;
                if (excelModel != null && dealerInfo.BussinesCategoryType == excelModel.BussinesCategoryType) continue; // check if already same status then no need to update;

                dealerInfo.BussinesCategoryType = excelModel?.BussinesCategoryType ?? EnumBussinesCategory.None;

                var dealerInfoStatusLog = new DealerInfoStatusLog()
                {
                    DealerInfoId = dealerInfo.Id,
                    UserId = userId,
                    PropertyName = "Bussinee Category",
                    PropertyValue = EnumExtension.GetEnumDescription((EnumBussinesCategory)dealerInfo.BussinesCategoryType)
                };

                updatedDealerInfos.Add(dealerInfo);
                dealerInfoStatusLogs.Add(dealerInfoStatusLog);
            }

            await _dealerInfoRepository.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLogRepository.CreateListAsync(dealerInfoStatusLogs);

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
                FileName = "Dealer_Status_Error_BussinesCategory"
            };
        }

        private async Task<DealerStatusExcelExportModel> DealerStatusExclusive(IFormFile file)
        {
            var userId = AppIdentity.AppUser.UserId;
            var excelModelList = await _excelReaderService.LoadDataAsync<DealerStatusExclusiveExcelModel>(file);
            excelModelList.ForEach(x => x.ExclusiveStatus = !string.IsNullOrWhiteSpace(x.DealerId) ? "Exclusive" : "Not Exclusive");

            var dealerIdList = excelModelList.Select(x => x.DealerId).ToList();
            var dbDealerInfoList = await _dealerInfoRepository.FindByCondition(x => !x.IsDeleted &&
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

            await _dealerInfoRepository.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLogRepository.CreateListAsync(dealerInfoStatusLogs);

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
            var dbDealerInfoList = await _dealerInfoRepository.FindByCondition(x => !x.IsDeleted &&
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

            await _dealerInfoRepository.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLogRepository.CreateListAsync(dealerInfoStatusLogs);

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
            var dbDealerInfoList = await _dealerInfoRepository.FindByCondition(x => !x.IsDeleted &&
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

            await _dealerInfoRepository.UpdateListAsync(updatedDealerInfos);
            await _dealerInfoStatusLogRepository.CreateListAsync(dealerInfoStatusLogs);

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
