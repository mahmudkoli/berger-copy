using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface IReportDataService
    {
        Task<IList<TargetReportResultModel>> MyTarget(MyTargetSearchModel model,IList<int> dealerIds);
    }
}
