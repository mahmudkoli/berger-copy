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
        private readonly IMapper _mapper;

        public BrandService(
            IRepository<BrandInfo> brandInfoRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<DealerInfo> dealerInfoRepository,
            IMapper mapper
            )
        {
            _brandInfoRepository = brandInfoRepository;
            _userInfoRepository = userInfoRepository;
            _dealerInfoRepository = dealerInfoRepository;
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
       
        public async Task<bool> BrandStatusUpdate(BrandStatusModel brandStatus)
        {
            var find = await _brandInfoRepository.FindAsync(f => f.MaterialCode.ToLower() == brandStatus.MaterialCode.ToLower());
            if (find == null) return false;

            switch (brandStatus.PropertyName)
            {
                case "IsCBInstalled": find.IsCBInstalled = !find.IsCBInstalled; break;
                case "IsMTS": find.IsMTS = !find.IsMTS; break;
                case "IsPremium": find.IsPremium = !find.IsPremium; break;
                default: break;
            }
            
            await _brandInfoRepository.UpdateAsync(find);
            return true;
        }
    }
}
