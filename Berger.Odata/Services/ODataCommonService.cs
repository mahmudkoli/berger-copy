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
        private readonly IODataRepository<CreditControlArea> _creditControlAreaRepository;
        private readonly IMapper _mapper;

        public ODataCommonService(
            IODataRepository<CreditControlArea> creditControlAreaRepository,
            IMapper mapper)
        {
            _creditControlAreaRepository = creditControlAreaRepository;
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
    }
}
