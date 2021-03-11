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
                                x => new { x.Id, x.MaterialCode, x.MaterialDescription },
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
                    default: break;
                }
            }

            if(findAll.Any())
                await _brandInfoRepository.UpdateListAsync(findAll);

            // update Brand info status log
            foreach (var brandInfoItem in findAll)
            {
                // var existLog = (await _brandInfoStatusLogRepository.FindAsync(log => log.BrandInfoId == brandInfoItem.Id));


                var brandStatusLog = new BrandInfoStatusLog()
                {
                    //CreatedBy = (existLog != null) ? existLog.CreatedBy : userId,
                    //CreatedTime = (existLog != null) ? existLog.CreatedTime : DateTime.Now,
                    //ModifiedBy = userId,
                    //ModifiedTime = DateTime.Now,
                    //Status = Status.Active,
                    UserId = userId,
                    BrandInfoId = brandInfoItem.Id,
                    PropertyValue = GetPropertyValue(brandStatus.PropertyName, brandInfoItem),
                    PropertyName = brandStatus.PropertyName

                };
                await _brandInfoStatusLogRepository.CreateAsync(brandStatusLog);
            }

            return true;
        }
        private string GetPropertyValue(string propertyName,BrandInfo brandInfo)
        {
            string value="";
            switch (propertyName)
            {
                case "IsCBInstalled": value = (brandInfo.IsCBInstalled == true? "CBI" : "NonCBI"); break;
                case "IsMTS": value = (brandInfo.IsMTS == true ? "MTS" : "NonMTS"); break;
                case "IsPremium": value = (brandInfo.IsPremium == true ? "PREMIUM" : "NonPREMIUM"); break;
                default: break;
            }
            return value;
        }
        
    }
}


