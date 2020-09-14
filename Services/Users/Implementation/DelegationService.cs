using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Domain.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Users.Interfaces;

namespace BergerMsfaApi.Services.Users.Implementation
{
    public class DelegationService : IDelegationService
    {
        private readonly IRepository<Delegation> _delegation;


        public DelegationService(IRepository<Delegation> delegation)
        {
            _delegation = delegation;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _delegation.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsCodeExistAsync( int id)
        {
            var result = id <= 0
                ? await _delegation.IsExistAsync(s => s.Id == id)
                : await _delegation.IsExistAsync(s => s.Id == id);

            return result;
        }
        public async Task<DelegationModel> GetDelegationAsync(int id)
        {
            var result = await _delegation.FindAsync(s => s.Id == id);
            return result.ToMap<Delegation, DelegationModel>();
        }

        public async Task<IEnumerable<DelegationModel>> GetDelegationsAsync()
        {
            var result = await _delegation.GetAllAsync();
            return result.ToMap<Delegation, DelegationModel>();
        }
        public async Task<IEnumerable<DelegationModel>> GetNewDelegationsAsync()
        {
            //var result = await _delegation.GetAllAsync();
            var result = await _delegation.ExecuteQueryAsyc<DelegationModel>("SELECT d.*,uf.[Name] AS [DeligatedFromUserName],ut.[Name] AS " +
                                                                             "[DeligatedToUserName] FROM[dbo].[Delegations] AS d " +
                                                                             "INNER JOIN[dbo].[UserInfos] AS uf ON d.[DeligatedFromUserId] = uf.[Id] " +
                                                                             "INNER JOIN[dbo].[UserInfos] AS ut ON d.[DeligatedToUserId] = ut.[Id]");
            var delegations = result as DelegationModel[] ?? result.ToArray();
            var nResult = delegations.Where(d => d.ToDate >= DateTime.Now.Date);
            return nResult;
        }
        public async Task<IEnumerable<DelegationModel>> GetPastDelegationsAsync()
        {
            //var result = await _delegation.GetAllAsync();
            var result = await _delegation.ExecuteQueryAsyc<DelegationModel>("SELECT d.*,uf.[Name] AS [DeligatedFromUserName],ut.[Name] AS " +
                                                                             "[DeligatedToUserName] FROM[dbo].[Delegations] AS d " +
                                                                             "INNER JOIN[dbo].[UserInfos] AS uf ON d.[DeligatedFromUserId] = uf.[Id] " +
                                                                             "INNER JOIN[dbo].[UserInfos] AS ut ON d.[DeligatedToUserId] = ut.[Id]");
            //var delegations = result as Delegation[] ?? result.ToArray();
            var delegations = result as DelegationModel[] ?? result.ToArray();
            var nResult = delegations.Where(d => d.ToDate < DateTime.Now.Date);
            return nResult;//.ToMap<Delegation, DelegationModel>();
        }
        

        public async Task<DelegationModel> SaveAsync(DelegationModel model)
        {
            var example = model.ToMap<DelegationModel, Delegation>();
            var result = await _delegation.CreateOrUpdateAsync(example);
            return result.ToMap<Delegation, DelegationModel>();
        }

        public async Task<DelegationModel> UpdateAsync(DelegationModel model)
        {
            var example = model.ToMap<DelegationModel, Delegation>();
            var result = await _delegation.UpdateAsync(example);
            return result.ToMap<Delegation, DelegationModel>();
        }

    }
}
