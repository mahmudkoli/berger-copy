﻿using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.Brand;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using AutoMapper;
using BergerMsfaApi.Models.Common;
using System.Linq.Expressions;
using BergerMsfaApi.Services.OData.Interfaces;
using Berger.Odata.Services;
using Berger.Odata.Model;

namespace BergerMsfaApi.Services.OData.Implementation
{
    public class ODataService : IODataService
    {
        private readonly IRepository<BrandInfo> _brandInfoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<DealerInfo> _dealerInfoRepository;
        private readonly IMTSDataService _mTSDataService;
        private readonly IMapper _mapper;

        public ODataService(
            IRepository<BrandInfo> brandInfoRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<DealerInfo> dealerInfoRepository,
            IMTSDataService mTSDataService,
            IMapper mapper
            )
        {
            _brandInfoRepository = brandInfoRepository;
            _userInfoRepository = userInfoRepository;
            _dealerInfoRepository = dealerInfoRepository;
            _mTSDataService = mTSDataService;
            _mapper = mapper;
        }

        public async Task<IList<MTSResultModel>> GetMTSBrandsVolumeAsync(MTSSearchModel model)
        {
            var result = await _mTSDataService.GetMTSBrandsVolume(model);
            var matGroups = result.Select(y => y.MatarialGroupOrBrand).ToList();
            var brands = (await _brandInfoRepository.FindAllAsync(x => matGroups.Contains(x.MatarialGroupOrBrand))).ToList();

            foreach (var item in result)
            {
                item.MatarialGroupOrBrand = string.Join(", ", brands.Where(x => x.MatarialGroupOrBrand.ToLower() == item.MatarialGroupOrBrand.ToLower()).Select(x => x.MatarialGroupOrBrand).Distinct().ToList());
            }

            return result;
        }
    }
}