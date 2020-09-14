using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;

namespace BergerMsfaApi.Services.Users.Interfaces
{
    public interface IDelegationService
    {
        Task<IEnumerable<DelegationModel>> GetDelegationsAsync();
        Task<IEnumerable<DelegationModel>> GetNewDelegationsAsync();
        Task<IEnumerable<DelegationModel>> GetPastDelegationsAsync();
        Task<DelegationModel> GetDelegationAsync(int id);
        Task<DelegationModel> SaveAsync(DelegationModel model);
        Task<DelegationModel> UpdateAsync(DelegationModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(int id);

    }
}
