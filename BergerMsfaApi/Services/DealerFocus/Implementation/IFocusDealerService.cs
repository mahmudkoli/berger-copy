using BergerMsfaApi.Models.FocusDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DealerFocus.Implementation
{
    public interface IFocusDealerService
    {
        Task<IEnumerable<FocusDealerModel>> GetFocusDealerList();
        Task<FocusDealerModel> GetFocusDealerById(int id);


        Task<FocusDealerModel> CreateAsync(FocusDealerModel model);
        Task<FocusDealerModel> UpdateAsync(FocusDealerModel model);
        Task<int> DeleteAsync(int id);


        Task<bool> IsExistAsync(int id);

    }
}
