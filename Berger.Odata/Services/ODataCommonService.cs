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
using Berger.Data.MsfaEntity.Master;

namespace Berger.Odata.Services
{
    public class ODataCommonService : IODataCommonService
    {
        private readonly IODataApplicationRepository<CreditControlArea> _creditControlAreaRepository;
        private readonly IODataApplicationRepository<RPRSPolicy> _rPRSPolicyRepository;
        private readonly IODataApplicationRepository<Depot> _depotRepository;
        private readonly IODataApplicationRepository<Division> _divisionRepository;
        private readonly IMapper _mapper;

        public ODataCommonService(
           IODataApplicationRepository<CreditControlArea> creditControlAreaRepository,
           IODataApplicationRepository<RPRSPolicy> rPRSPolicyRepository,
           IODataApplicationRepository<Depot> depotRepository,
           IODataApplicationRepository<Division> divisionRepository,
            IMapper mapper)
        {
            _creditControlAreaRepository = creditControlAreaRepository;
            _rPRSPolicyRepository = rPRSPolicyRepository;
            _depotRepository = depotRepository;
            _divisionRepository = divisionRepository;
            _mapper = mapper;
        }

        public async Task<IList<CreditControlArea>> GetAllCreditControlAreasAsync()
        {
            var result = await _creditControlAreaRepository.GetAllIncludeAsync(
                                x => x,
                                null,
                                null,
                                null,
                                true
                            );

            return result;
        }

        public async Task<IList<RPRSPolicy>> GetAllRPRSPoliciesAsync()
        {
            var result = await _rPRSPolicyRepository.GetAllIncludeAsync(
                                x => x,
                                null,
                                null,
                                null,
                                true
                            );

            return result;
        }

        public async Task<IList<(string Code, string Name)>> GetAllDepotsAsync(Expression<Func<Depot, bool>> predicate)
        {
            var result = await _depotRepository.GetAllIncludeAsync(
                                x => new ValueTuple<string, string>(x.Werks, x.Name1),
                                predicate,
                                null,
                                null,
                                true
                            );

            return result;
        }

        public async Task<IList<Division>> GetAllDivisionsAsync()
        {
            var result = await _divisionRepository.GetAllIncludeAsync(
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
