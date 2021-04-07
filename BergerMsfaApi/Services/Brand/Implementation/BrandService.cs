using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Brand;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Brand.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using AutoMapper;
using BergerMsfaApi.Models.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BergerMsfaApi.Services.Brand.Implementation
{
    public class BrandService:IBrandService
    {
        private readonly IRepository<BrandInfo> _brandInfoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<DealerInfo> _dealerInfoRepository;
        private readonly IRepository<BrandInfoStatusLog> _brandInfoStatusLogRepository;
        private readonly IMapper _mapper;

        public BrandService(
            IRepository<BrandInfo> brandInfoRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<DealerInfo> dealerInfoRepository,
            IRepository<BrandInfoStatusLog> brandInfoStatusLogRepository,
            IMapper mapper
            )
        {
            _brandInfoRepository = brandInfoRepository;
            _userInfoRepository = userInfoRepository;
            _dealerInfoRepository = dealerInfoRepository;
            _brandInfoStatusLogRepository = brandInfoStatusLogRepository;
            _mapper = mapper;
        }

        public async Task<BrandInfoModel> GetBrandById(int id)
        {
            var result = await _brandInfoRepository.FindAsync(f => f.Id == id);
            return _mapper.Map<BrandInfoModel>(result);
        }

        public async Task<QueryResultModel<BrandInfoModel>> GetBrandsAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<BrandInfo, object>>>()
            {
                ["materialCode"] = v => v.MaterialCode,
                ["materialDescription"] = v => v.MaterialDescription,
                ["materialGroupOrBrand"] = v => v.MaterialGroupOrBrand,
            };

            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.MaterialGroupOrBrand.Contains(query.GlobalSearchValue) || x.MaterialCode.Contains(query.GlobalSearchValue) || x.MaterialDescription.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                null,
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<BrandInfoModel>>(result.Items);

            var queryResult = new QueryResultModel<BrandInfoModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }
       
        public async Task<object> GetBrandsAsync(AppBrandSearchModel query)
        {
            query.PageNo = query.PageNo ?? 1;
            query.PageSize = query.PageSize ?? int.MaxValue;
            query.MaterialDescription = query.MaterialDescription ?? string.Empty;

            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => new { x.Id, x.MaterialCode, x.MaterialDescription, x.MaterialGroupOrBrand },
                                x => (string.IsNullOrEmpty(query.MaterialDescription) || x.MaterialCode.Contains(query.MaterialDescription) || x.MaterialDescription.Contains(query.MaterialDescription)),
                                x => x.OrderBy(o => o.MaterialDescription),
                                null,
                                query.PageNo.Value,
                                query.PageSize.Value,
                                true
                            );

            return result.Items;
        }
       
        public async Task<bool> BrandStatusUpdate(BrandStatusModel brandStatus)
        {
            var userId = AppIdentity.AppUser.UserId;

            var columnsMap = new Dictionary<string, Expression<Func<BrandInfo, bool>>>()
            {
                ["IsCBInstalled"] = f => f.MaterialCode.ToLower() == brandStatus.MaterialOrBrandCode.ToLower(),
                ["IsMTS"] = f => f.MaterialGroupOrBrand.ToLower() == brandStatus.MaterialOrBrandCode.ToLower(),
                ["IsPremium"] = f => f.MaterialGroupOrBrand.ToLower() == brandStatus.MaterialOrBrandCode.ToLower(),
                ["isEnamel"] = f => f.MaterialGroupOrBrand.ToLower() == brandStatus.MaterialOrBrandCode.ToLower(),
            };

            var findAll = (await _brandInfoRepository.FindAllAsync(columnsMap[brandStatus.PropertyName])).ToList();
            if (findAll == null || !findAll.Any()) return false;

            foreach (var find in findAll)
            {

                switch (brandStatus.PropertyName)
                {
                    case "IsCBInstalled": find.IsCBInstalled = !find.IsCBInstalled; break;
                    case "IsMTS": find.IsMTS = !find.IsMTS; break;
                    case "IsPremium": find.IsPremium = !find.IsPremium; break;
                    case "isEnamel": find.IsEnamel = !find.IsEnamel; break;
                    default: break;
                }
            }
            
            if (findAll.Any())
                await _brandInfoRepository.UpdateListAsync(findAll);

            // Create Brand info status log
            await CreateBrandInfoStatusLog(brandStatus, userId, findAll);

            return true;
        }


        
        private async Task CreateBrandInfoStatusLog(BrandStatusModel brandStatus, int userId, List<BrandInfo> findAll)
        {
            
            foreach (var brandInfoItem in findAll)
            {

                var brandStatusLog = new BrandInfoStatusLog()
                {
                    UserId = userId,
                    BrandInfoId = brandInfoItem.Id,
                    PropertyValue = GetPropertyValue(brandStatus.PropertyName, brandInfoItem),
                    PropertyName = brandStatus.PropertyName.Remove(0,2) // ISMTS -> MTS

                };
                
                await _brandInfoStatusLogRepository.CreateAsync(brandStatusLog);
                
            }
        }

        private string GetPropertyValue(string propertyName,BrandInfo brandInfo)
        {
            string value="";
            switch (propertyName)
            {
                case "IsCBInstalled": value = (brandInfo.IsCBInstalled ? "Yes" : "No"); break;
                case "IsMTS": value = (brandInfo.IsMTS ? "Yes" : "No"); break;
                case "IsPremium": value = (brandInfo.IsPremium ? "Yes" : "No"); break;
                case "isEnamel": value = (brandInfo.IsEnamel ? "Yes" : "No"); break;
                default: break;
            }
            return value;
        }

        public async Task<IEnumerable<BrandInfoStatusLogModel>> GetBrandInfoStatusLog(int brandInfoId)
        {

            var result = await _brandInfoStatusLogRepository.GetAllIncludeAsync(
                        brand => brand,
                        brand => brand.BrandInfoId == brandInfoId,
                        brand => brand.OrderByDescending(b => b.CreatedTime),
                        brand => brand.Include(i => i.BrandInfo).Include(i => i.User),
                        true
                );
            var modelResult = _mapper.Map<IEnumerable<BrandInfoStatusLogModel>>(result);
            return modelResult;
        }

    }
}


