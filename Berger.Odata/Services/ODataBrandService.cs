using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using AutoMapper;
using System.Linq.Expressions;
using Berger.Odata.Repositories;

namespace Berger.Odata.Services
{
    public class ODataBrandService : IODataBrandService
    {
        private readonly IODataRepository<BrandInfo> _brandInfoRepository;
        private readonly IODataRepository<BrandFamilyInfo> _brandFamilyInfoRepository;
        private readonly IMapper _mapper;

        public ODataBrandService(
            IODataRepository<BrandInfo> brandInfoRepository,
            IODataRepository<BrandFamilyInfo> brandFamilyInfoRepository,
            IMapper mapper
            )
        {
            _brandInfoRepository = brandInfoRepository;
            _brandFamilyInfoRepository = brandFamilyInfoRepository;
            _mapper = mapper;
        }

        public async Task<IList<string>> GetCBMaterialCodesAsync()
        {
            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => x.MaterialCode,
                                x => x.IsCBInstalled,
                                null,
                                null,
                                true
                            );

            return result.Distinct().ToList();
        }

        public async Task<IList<string>> GetMTSBrandCodesAsync()
        {
            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => x.MaterialGroupOrBrand,
                                x => x.IsMTS,
                                null,
                                null,
                                true
                            );

            return result.Distinct().ToList();
        }

        public async Task<IList<string>> GetPremiumBrandCodesAsync()
        {
            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => x.MaterialGroupOrBrand,
                                x => x.IsPremium,
                                null,
                                null,
                                true
                            );

            return result.Distinct().ToList();
        }

        public async Task<IList<string>> GetEnamelBrandCodesAsync()
        {
            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => x.MaterialGroupOrBrand,
                                x => x.IsEnamel,
                                null,
                                null,
                                true
                            );

            return result.Distinct().ToList();
        }

        public async Task<IList<string>> GetPowderBrandCodesAsync()
        {
            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => x.MaterialGroupOrBrand,
                                x => x.IsPowder,
                                null,
                                null,
                                true
                            );

            return result.Distinct().ToList();
        }

        public async Task<IList<string>> GetLiquidBrandCodesAsync()
        {
            var result = await _brandInfoRepository.GetAllIncludeAsync(
                                x => x.MaterialGroupOrBrand,
                                x => x.IsLiquid,
                                null,
                                null,
                                true
                            );

            return result.Distinct().ToList();
        }

        public async Task<IList<BrandFamilyInfo>> GetBrandFamilyInfosAsync()
        {
            var result = await _brandFamilyInfoRepository.GetAllIncludeAsync(
                                x => x,
                                null,
                                null,
                                null,
                                true
                            );

            return result;
        }
    }
}
