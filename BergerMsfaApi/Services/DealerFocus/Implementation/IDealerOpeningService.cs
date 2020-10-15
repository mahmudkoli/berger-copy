using BergerMsfaApi.Controllers.DealerFocus;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IDealerOpeningService
    {
        Task<IEnumerable<DealerOpeningModel>> GetDealerOpeningListAsync();
        Task<bool> IsExistAsync(int Id);
        Task<DealerOpeningModel> CreateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> files);
        Task<DealerOpeningModel> UpdateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> files);
        Task<int> DeleteDealerOpeningAsync(int DealerId);


    }
}
