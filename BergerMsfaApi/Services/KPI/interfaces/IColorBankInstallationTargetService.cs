using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Report;

namespace BergerMsfaApi.Services.KPI.interfaces
{
    public interface IColorBankInstallationTargetService
    {
        Task<IList<ColorBankInstallationTargetSaveModel>> GetFyYearData(ColorBankTargetSetupSearchModel query);
        Task<int> SaveOrUpdate(IList<ColorBankInstallationTargetSaveModel> model);
    }
}
