using System.Collections.Generic;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface ICollectionDataService
    {
        Task<decimal> GetTotalCollectionValue(IList<int> dealerIds);
    }
}
