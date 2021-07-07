using AutoMapper;
using Berger.Common;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.MerchandisingSnapShot;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.MerchandisingSnapShot.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Services.MerchandisingSnapShot.Implementation
{
    public class MerchandisingSnapShotService : IMerchandisingSnapShotService
    {
        private readonly IRepository<DSC.MerchandisingSnapShot> _merchandisingSnapShotRepository;
        private readonly IDropdownService _dropdownService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IRepository<UserInfo> _userInfo;
        private readonly IRepository<DealerInfo> dealerInfo;

        public MerchandisingSnapShotService(
                IRepository<DSC.MerchandisingSnapShot> merchandisingSnapShotRepository,
                IDropdownService dropdownService,
                IFileUploadService fileUploadService,
                IMapper mapper,
                IRepository<UserInfo> userInfo,
                IRepository<DealerInfo> dealerInfo
            )
        {
            this._merchandisingSnapShotRepository = merchandisingSnapShotRepository;
            this._dropdownService = dropdownService;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
           _userInfo= userInfo;
            this.dealerInfo = dealerInfo;
        }

        public async Task<int> AddAsync(SaveMerchandisingSnapShotModel model)
        {
            var merchandisingSnapShot = _mapper.Map<DSC.MerchandisingSnapShot>(model);

            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                var fileName = merchandisingSnapShot.DealerId + "_" + Guid.NewGuid().ToString();
                merchandisingSnapShot.ImageUrl = await _fileUploadService.SaveImageAsync(model.ImageUrl, fileName, FileUploadCode.MerchandisingSnapShot, 1200, 800);
            }

            #region check existing data and update
            var existingData = await _merchandisingSnapShotRepository.FindAsync(x => x.CreatedTime.Date == DateTime.Now
                                                    && merchandisingSnapShot.DealerId == x.DealerId
                                                    && merchandisingSnapShot.UserId == x.UserId
                                                    && merchandisingSnapShot.MerchandisingSnapShotCategoryId == x.MerchandisingSnapShotCategoryId);

            var id = 0;
            if (existingData == null)
            {
                var result = await _merchandisingSnapShotRepository.CreateAsync(merchandisingSnapShot);
                id = result.Id;
            }
            else
            {
                existingData.ImageUrl = merchandisingSnapShot.ImageUrl;
                existingData.Remarks = merchandisingSnapShot.Remarks;
                existingData.OthersSnapShotCategoryName = merchandisingSnapShot.OthersSnapShotCategoryName;
                var result = await _merchandisingSnapShotRepository.UpdateAsync(existingData);
                id = result.Id;
            }
            #endregion

            return id;
        }

        public async Task<bool> AddRangeAsync(List<SaveMerchandisingSnapShotModel> models)
        {
            var merchandisingSnapShots = new List<DSC.MerchandisingSnapShot>();

            foreach (var model in models)
            {
                var merchandisingSnapShot = _mapper.Map<DSC.MerchandisingSnapShot>(model);

                if (!string.IsNullOrWhiteSpace(model.ImageUrl))
                {
                    var fileName = merchandisingSnapShot.DealerId + "_" + Guid.NewGuid().ToString();
                    merchandisingSnapShot.ImageUrl = await _fileUploadService.SaveImageAsync(model.ImageUrl, fileName, FileUploadCode.MerchandisingSnapShot, 1200, 800);
                }

                merchandisingSnapShot.CreatedTime = DateTime.Now;

                merchandisingSnapShots.Add(merchandisingSnapShot);
            }

            var result = await _merchandisingSnapShotRepository.CreateListAsync(merchandisingSnapShots);

            return true;
        }

        public async Task<QueryResultModel<MerchandisingSnapShotModel>> GetAllAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DSC.MerchandisingSnapShot, object>>>()
            {
                ["dealerName"] = v => v.Dealer.CustomerName,
                ["userFullName"] = v => v.User.FullName,
            };

            var result = await _merchandisingSnapShotRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.User.FullName.Contains(query.GlobalSearchValue) || x.Dealer.CustomerName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.User).Include(i => i.Dealer),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<MerchandisingSnapShotModel>>(result.Items);

            var queryResult = new QueryResultModel<MerchandisingSnapShotModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<IList<AppMerchandisingSnapShotModel>> GetAllByUserIdAsync(int userId)
        {
            var result = await _merchandisingSnapShotRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.UserId == userId,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<IList<AppMerchandisingSnapShotModel>>(result);

            return modelResult;
        }

        public async Task<IList<AppMerchandisingSnapShotLogModel>> GetAppMerchandisingSnapShotListByCurrentUser(int dealerId)
        {
            var userId = AppIdentity.AppUser.UserId;
            var result = await _merchandisingSnapShotRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.UserId == userId && x.DealerId == dealerId,
                                x => x.OrderByDescending(o => o.CreatedTime),
                                x => x.Include(i => i.MerchandisingSnapShotCategory),
                                true
                            );

            var modelResult = _mapper.Map<IList<AppMerchandisingSnapShotLogModel>>(result);

            return modelResult;
        }

        public async Task<SaveMerchandisingSnapShotModel> GetMerchandisingSnapShotByDealerIdAsync(int id)
        {
            //var result = await _merchandisingSnapShotRepository.GetFirstOrDefaultIncludeAsync(
            //                    x => x,
            //                    x => x.DealerId == id,
            //                    x => x.OrderByDescending(o => o.CreatedTime),
            //                    x => x.Include(i => i.MerchandisingSnapShotCategory),
            //                    true
            //                );

            var modelResult = new SaveMerchandisingSnapShotModel();
            modelResult.DealerId = id;

            //var companyList = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.MerchandisingSnapShot);

            return modelResult;
        }

        public async Task<IList<SaveMerchandisingSnapShotModel>> GetMerchandisingSnapShotListByDealerIdsAsync(IList<int> ids)
        {
            var modelResults = new List<SaveMerchandisingSnapShotModel>();

            //var companyList = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.SwappingCompetition);

            foreach (var id in ids)
            {

                //var result = await _merchandisingSnapShotRepository.GetFirstOrDefaultIncludeAsync(
                //                    x => x,
                //                    x => x.DealerId == id,
                //                    x => x.OrderByDescending(o => o.CreatedTime),
                //                    x => x.Include(i => i.MerchandisingSnapShotCategory),
                //                    true
                //                );

                var modelResult = new SaveMerchandisingSnapShotModel();
                modelResult.DealerId = id;

                modelResults.Add(modelResult);
            }

            return modelResults;
        }

        public async Task<MerchandisingSnapShotModel> GetByIdAsync(int id)
        {
            var result = await _merchandisingSnapShotRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.User).Include(i => i.Dealer)
                                        .Include(i => i.MerchandisingSnapShotCategory),
                                true
                            );

            var modelResult = _mapper.Map<MerchandisingSnapShotModel>(result);

            return modelResult;
        }
    }
}
