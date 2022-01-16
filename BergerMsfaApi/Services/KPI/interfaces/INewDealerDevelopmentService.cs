using Berger.Data.MsfaEntity.KPI;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Scheme;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.interfaces
{
    public interface INewDealerDevelopmentService
    {
        Task<IList<NewDealerDevelopment>> GetNewDealerDevelopmentByIdAsync(SearchNewDealerDevelopment query);
        Task<IList<NewDealerDevelopmentSaveModel>> GetDealerConversionByYearAsync(SearchNewDealerDevelopment query);
        Task<IList<NewDealerDevelopmentModel>> GetNewDealerDevelopmentReport(SearchNewDealerDevelopment query);
        Task<int> AddNewDealerDevelopmentAsync(IList<NewDealerDevelopment> model);
        Task<bool> AddDealerConversionAsync(IList<NewDealerDevelopmentSaveModel> model);
        Task<IList<DealerConversionModel>> GetDealerConversionReport(SearchNewDealerDevelopment query);
    }
}
