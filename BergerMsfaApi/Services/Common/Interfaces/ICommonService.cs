using BergerMsfaApi.Models.Dealer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<DealerInfoModel>> GetDealerInfoList();
        Task<IEnumerable<DealerInfoModel>> GeApptDealerInfoList(string territory);
    }
}
