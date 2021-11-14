using Berger.Data.MsfaEntity.Users;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Users.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetByIdAsync(Guid id);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task<Guid> AddAsync(RefreshToken entity);
        Task<Guid> UpdateAsync(RefreshToken entity);
    }
}
